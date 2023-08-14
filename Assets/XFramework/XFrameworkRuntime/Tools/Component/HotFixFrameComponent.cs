using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public override void FrameEndComponent()
        {
        }

        #region 热更

        /// <summary>
        /// 初始化资源到临时位置
        /// </summary>
        public void InstantiateTempHotFixAssetBundle()
        {
            GameObject sceneLoadComponent = transform.Find("SceneLoadComponent/SceneHotFixTemp").gameObject;
            string fontAssetBundlePath = hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundlePath;
            string fontAssetBundleName = hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundleName;
            string localFontPath = Application.streamingAssetsPath + "/" + fontAssetBundlePath + fontAssetBundleName;
            //加载字体
            AssetBundle fontAssetBundle = AssetBundle.LoadFromFile(localFontPath);
            //加载内容
            for (int i = 0; i < hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                string assetBundlePath = hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundlePath;
                string assetBundleName = hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName;
                string assetBundleInstantiatePath = hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath;
                AssetBundle tempHotFixAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetBundlePath + assetBundleName);
                GameObject hotFixObject = tempHotFixAssetBundle.LoadAsset<GameObject>(assetBundleName);
                GameObject tempHotFixObject = Instantiate(hotFixObject, sceneLoadComponent.transform, false);
                if (!hotFixAssetAssetBundleTempPath.ContainsKey(assetBundleInstantiatePath))
                {
                    hotFixAssetAssetBundleTempPath.Add(assetBundleInstantiatePath, new List<GameObject>());
                }

                hotFixAssetAssetBundleTempPath[assetBundleInstantiatePath].Add(tempHotFixObject);
                currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
            }

            foreach (AssetBundle assetBundle in currentSceneAllAssetBundle)
            {
                assetBundle.Unload(false);
            }

            currentSceneAllAssetBundle.Clear();
            fontAssetBundle.Unload(false);
            Debug.Log("初始化完毕");
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
        public void LoadHotFixSceneConfig(string sceneName)
        {
            string hotFixAssetConfig = FileOperation.GetTextToLoad(Application.streamingAssetsPath + "/HotFix/HotFixConfig", sceneName + ".json");
            hotFixAssetAssetBundleSceneConfigs = JsonMapper.ToObject<HotFixAssetAssetBundleSceneConfig>(hotFixAssetConfig);
        }

        /// <summary>
        /// 加载AssetBundle场景到系统中
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadAssetBundleSceneToSystem(string sceneName)
        {
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/HotFix/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName);
            }
        }

        #endregion
    }
}