using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using HotFix;
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
        [LabelText("热更加载接口")] private List<IHotFixAssetBundleLoadProgress> iHotFixAssetBundleLoadProgresses = new List<IHotFixAssetBundleLoadProgress>();
        [LabelText("热更资源数量")] public float hotfixAssetBundleCount;
        [LabelText("当前加载的热更数量")] public float currentLoadHotfixAssetBundleCount = 0;

        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameSceneInitComponent()
        {
            UnloadAssetBundle();
            iHotFixAssetBundleLoadProgresses = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IHotFixAssetBundleLoadProgress>();
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
        public async UniTask InstantiateHotFixAssetBundle()
        {
            //热更数量:重复资源+通用资源+场景
            //本地重复资源
            for (int i = 0; i < hotFixRuntimeSceneAssetBundleConfigs.repeatSceneFixRuntimeAssetConfig.Count; i++)
            {
                HotFixRuntimeAssetConfig hotFixRuntimeAssetConfig = hotFixRuntimeSceneAssetBundleConfigs.repeatSceneFixRuntimeAssetConfig[i];
                string localRepeatPath = DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(), "/" + hotFixRuntimeAssetConfig.assetPath, hotFixRuntimeAssetConfig.assetName);
                AssetBundle repeatAssetBundle = await AssetBundle.LoadFromFileAsync(localRepeatPath);
                currentSceneAllAssetBundle.Add(repeatAssetBundle);
                currentLoadHotfixAssetBundleCount += 1;
                UpdateLoadHotFixAssetBundleProgress();
                // Debug.Log("加载:" + hotFixRuntimeSceneAssetBundleConfigs.repeatSceneFixRuntimeAssetConfig[i].assetName + ":" + currentLoadHotfixAssetBundleCount);
            }


            //加载内容
            for (int i = 0; i < hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                string assetBundlePath = DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(), "/", hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetPath);
                string assetBundleName = DataFrameComponent.String_AllCharToLower(hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetName);

                AssetBundle tempHotFixAssetBundle = await AssetBundle.LoadFromFileAsync(assetBundlePath + assetBundleName);
                GameObject hotFixObject = (GameObject)await tempHotFixAssetBundle.LoadAssetAsync<GameObject>(hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetName);
                currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
                currentLoadHotfixAssetBundleCount += 1;
                UpdateLoadHotFixAssetBundleProgress();
                // Debug.Log("加载:" + hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetName + ":" + currentLoadHotfixAssetBundleCount);
            }
        }

        private void UpdateLoadHotFixAssetBundleProgress()
        {
            foreach (IHotFixAssetBundleLoadProgress iHotFixAssetBundleLoadProgress in iHotFixAssetBundleLoadProgresses)
            {
                float progress = float.Parse((currentLoadHotfixAssetBundleCount / hotfixAssetBundleCount).ToString("F"));
                iHotFixAssetBundleLoadProgress.AssetBundleLoadProgress(progress);
            }
        }

        public void UnloadAssetBundle()
        {
            foreach (AssetBundle assetBundle in currentSceneAllAssetBundle)
            {
                assetBundle.Unload(false);
            }

            currentSceneAllAssetBundle.Clear();
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
            hotfixAssetBundleCount = hotFixRuntimeSceneAssetBundleConfigs.repeatSceneFixRuntimeAssetConfig.Count + hotFixRuntimeSceneAssetBundleConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count + 1;
            currentLoadHotfixAssetBundleCount = 0;
            return String.Empty;
        }

        /// <summary>
        /// 加载AssetBundle场景到系统中
        /// </summary>
        /// <param name="sceneName"></param>
        public async UniTask LoadAssetBundleSceneToSystem(string sceneName)
        {
            //如果没加载过当前场景
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.Log("加载场景:" + sceneName);
                //加载场景
                await AssetBundle.LoadFromFileAsync(DataFrameComponent.String_BuilderString(RuntimeGlobal.GetDeviceStoragePath(), "/HotFixRuntime/HotFixAssetBundle/", sceneName, "/scene/", sceneName));
                // await UniTask.WaitUntil(() => Application.CanStreamedLevelBeLoaded(sceneName));
            }

            currentLoadHotfixAssetBundleCount += 1;
            UpdateLoadHotFixAssetBundleProgress();
        }

        #endregion

        [Button("应用场景热更配置")]
        public void ApplyScenePrefab()
        {
#if UNITY_EDITOR

            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            //应用热更配置并记录路径
            for (int i = 0; i < hotFixAssetPathConfigs.Count; i++)
            {
                hotFixAssetPathConfigs[i].SetPathAndApplyPrefab();
            }
#endif
        }
    }
}