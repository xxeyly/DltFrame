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
        [LabelText("热更AssetBundle临时路径")] public Dictionary<string, List<GameObject>> HotFixAssetAssetBundleTempPath = new Dictionary<string, List<GameObject>>();
        private List<AssetBundle> _currentSceneAllAssetBundle = new List<AssetBundle>();

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

        public void InstantiateTempHotFixAssetBundle()
        {
            if (!GameRootStart.Instance.hotFixLoad)
            {
                return;
            }

            GameObject sceneLoadComponent = transform.Find("SceneLoadComponent/SceneHotFixTemp").gameObject;
            string localFontPath = Application.streamingAssetsPath + "/" + hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundlePath +
                                   hotFixAssetAssetBundleSceneConfigs.sceneFontFixAssetConfig.assetBundleName;
            //加载字体
            AssetBundle fontAssetBundle = AssetBundle.LoadFromFile(localFontPath);
            //加载内容
            for (int i = 0; i < hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                AssetBundle tempHotFixAssetBundle =
                    AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundlePath +
                                             DataFrameComponent.AllCharToLower(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName));
                _currentSceneAllAssetBundle.Add(tempHotFixAssetBundle);
                GameObject hotFixObject = tempHotFixAssetBundle.LoadAsset<GameObject>(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleName);

                GameObject tempHotFixObject = Instantiate(hotFixObject, sceneLoadComponent.transform, false);

                if (!HotFixAssetAssetBundleTempPath.ContainsKey(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath))
                {
                    HotFixAssetAssetBundleTempPath.Add(hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath, new List<GameObject>());
                }

                HotFixAssetAssetBundleTempPath[hotFixAssetAssetBundleSceneConfigs.assetBundleHotFixAssetAssetBundleAssetConfigs[i].assetBundleInstantiatePath].Add(tempHotFixObject);
            }

            foreach (AssetBundle assetBundle in _currentSceneAllAssetBundle)
            {
                assetBundle.Unload(false);
            }

            _currentSceneAllAssetBundle.Clear();
            fontAssetBundle.Unload(false);
            Debug.Log("初始化完毕");
        }

        public void ReleaseTempHotFixAssetBundle()
        {
            foreach (KeyValuePair<string, List<GameObject>> pair in HotFixAssetAssetBundleTempPath)
            {
                foreach (GameObject hotFixObj in pair.Value)
                {
                    hotFixObj.transform.SetParent(GameObject.Find(pair.Key).transform, false);
                }
            }

            HotFixAssetAssetBundleTempPath.Clear();
        }

        public void LoadHotFixSceneConfig(string sceneName)
        {
            string hotFixAssetConfig = FileOperation.GetTextToLoad(Application.streamingAssetsPath + "/HotFix/HotFixConfig", sceneName + ".json");
            hotFixAssetAssetBundleSceneConfigs = JsonMapper.ToObject<HotFixAssetAssetBundleSceneConfig>(hotFixAssetConfig);
        }
        
        public void SceneAssetBundleUnload()
        {
            if (!GameRootStart.Instance.hotFixLoad)
            {
                return;
            }

            foreach (AssetBundle assetBundle in _currentSceneAllAssetBundle)
            {
                if (assetBundle != null)
                {
                    assetBundle.Unload(false);
                }
            }
        }


        #endregion
    }
}