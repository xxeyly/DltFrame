using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework
{
    public class HotFixFrameComponent : FrameComponent
    {
        public static HotFixFrameComponent Instance;

        [LabelText("热修复包AssetBundle配置")] public HotFixAssetAssetBundleSceneConfig hotFixAssetAssetBundleSceneConfigs = new HotFixAssetAssetBundleSceneConfig();
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
            string localFontPath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundlePath + hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundleName;
            //加载字体
            AssetBundle fontAssetBundle = await AssetBundle.LoadFromFileAsync(localFontPath);
            //加载内容
            for (int i = 0; i < hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                string assetBundlePath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundlePath;
                string assetBundleName = DataFrameComponent.AllCharToLower(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);

                AssetBundle tempHotFixAssetBundle = await AssetBundle.LoadFromFileAsync(assetBundlePath + assetBundleName);
                currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
                GameObject hotFixObject = (GameObject)await tempHotFixAssetBundle.LoadAssetAsync<GameObject>(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);
                if (hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath == string.Empty)
                {
                    Instantiate(hotFixObject, null, false);
                }
                else
                {
                    Instantiate(hotFixObject, GameObject.Find(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath).transform, false);
                }
            }

            foreach (AssetBundle assetBundle in currentSceneAllAssetBundle)
            {
                assetBundle.Unload(false);
            }

            currentSceneAllAssetBundle.Clear();
            fontAssetBundle.Unload(false);
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
            UnityWebRequest request = UnityWebRequest.Get(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + sceneName + ".json");
            await request.SendWebRequest();
            string hotFixAssetConfig = request.downloadHandler.text;
            hotFixAssetAssetBundleSceneConfigs = JsonUtility.FromJson<HotFixAssetAssetBundleSceneConfig>(hotFixAssetConfig);
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
                await AssetBundle.LoadFromFileAsync(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName);
            }

            return string.Empty;
        }

        #endregion
    }
}