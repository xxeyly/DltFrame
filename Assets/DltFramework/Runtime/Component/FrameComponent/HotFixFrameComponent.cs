using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace DltFramework
{
    public class HotFixFrameComponent : FrameComponent
    {
        public static HotFixFrameComponent Instance;

        [LabelText("热修复包AssetBundle配置")] public HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfigs = new HotFixRuntimeSceneAssetBundleConfig();
        [LabelText("热更AssetBundle临时路径")] public Dictionary<string, List<GameObject>> hotFixAssetAssetBundleTempPath = new Dictionary<string, List<GameObject>>();
        [LabelText("场景所有AssetBundle")] public List<AssetBundle> currentSceneAllAssetBundle = new List<AssetBundle>();

        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }

        #region 热更

        /// <summary>
        /// 初始化资源到临时位置
        /// </summary>
        public async UniTask<string> InstantiateHotFixAssetBundle()
        {
            //本地字体路径
            string localFontPath = DataFrameComponent.String_BuilderString(
                RuntimeGlobal.GetDeviceStoragePath(), "/" + hotFixRuntimeSceneAssetBundleConfigs.sceneFontFixRuntimeAssetConfig.assetBundlePath, hotFixRuntimeSceneAssetBundleConfigs.sceneFontFixRuntimeAssetConfig.assetBundleName);
            AssetBundle fontAssetBundle = null;
            if (File.Exists(localFontPath))
            {
                //加载字体
                fontAssetBundle = await AssetBundle.LoadFromFileAsync(localFontPath);
            }

            //加载内容
            for (int i = 0; i < hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                string assetBundlePath = DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(), "/", hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundlePath);
                string assetBundleName = DataFrameComponent.String_AllCharToLower(hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);

                AssetBundle tempHotFixAssetBundle = await AssetBundle.LoadFromFileAsync(assetBundlePath + assetBundleName);
                currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
                GameObject hotFixObject = (GameObject)await tempHotFixAssetBundle.LoadAssetAsync<GameObject>(hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);
                if (hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath == string.Empty)
                {
                    Instantiate(hotFixObject, null, false);
                }
                else
                {
                    Instantiate(hotFixObject, GameObject.Find(hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath).transform, false);
                }
            }

            foreach (AssetBundle assetBundle in currentSceneAllAssetBundle)
            {
                assetBundle.Unload(false);
            }

            currentSceneAllAssetBundle.Clear();
            if (File.Exists(localFontPath))
            {
                fontAssetBundle?.Unload(false);
            }

            return string.Empty;
        }


        /// <summary>
        /// 释放临时资源到指定位置
        /// </summary>
        public void ReleaseTempHotFixAssetBundle()
        {
            foreach (KeyValuePair<string, List<GameObject>> pair in hotFixAssetAssetBundleTempPath)
            {
                foreach (GameObject hotFixObj in pair.Value)
                {
                    hotFixObj.transform.SetParent(GameObject.Find(pair.Key).transform, false);
                }
            }

            hotFixAssetAssetBundleTempPath.Clear();
        }

        /// <summary>
        /// 加载热更配置表
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask<string> LoadHotFixSceneConfig(string sceneName)
        {
            UnityWebRequest request = UnityWebRequest.Get(DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(true), "/HotFixRuntime/HotFixAssetBundleConfig/", sceneName, ".json"));
            await request.SendWebRequest();
            string hotFixAssetConfig = request.downloadHandler.text;
            hotFixRuntimeSceneAssetBundleConfigs = JsonUtility.FromJson<HotFixRuntimeSceneAssetBundleConfig>(hotFixAssetConfig);
            return String.Empty;
        }

        /// <summary>
        /// 加载AssetBundle场景到系统中
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask<string> LoadAssetBundleSceneToSystem(string sceneName)
        {
            //如果没加载过当前场景
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                //加载场景
                await AssetBundle.LoadFromFileAsync(DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(), "/HotFixRuntime/HotFixAssetBundle/", sceneName, "/scene/", sceneName));
            }

            return string.Empty;
        }

        #endregion
    }
}