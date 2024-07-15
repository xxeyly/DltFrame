using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    public class SceneAssetBundleRepeatAssetManager : BaseEditor
    {
        [BoxGroup("创建内容")] [LabelText("ab包名称")]
        public string assetBundleName;

        [BoxGroup("创建内容")] [LabelText("资源包含路径")]
        public List<string> assetBundleContainPath;

        [AssetList(AutoPopulate = true)] public List<SceneAssetBundleRepeatAsset> SceneAssetBundleRepeatAssets = new List<SceneAssetBundleRepeatAsset> { };

        [Button("创建场景重复资源配置")]
        public void CreateSceneAssetBundleRepeatAsset()
        {
            if (!Directory.Exists("Assets/Config/SceneAssetBundleRepeatAsset/"))
            {
                Directory.CreateDirectory("Assets/Config/SceneAssetBundleRepeatAsset/");
                UnityEditor.AssetDatabase.Refresh();
            }

            SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset = ScriptableObject.CreateInstance<SceneAssetBundleRepeatAsset>();
            sceneAssetBundleRepeatAsset.assetBundleName = assetBundleName;
            sceneAssetBundleRepeatAsset.assetBundleContainPath = assetBundleContainPath;
            UnityEditor.AssetDatabase.CreateAsset(sceneAssetBundleRepeatAsset, "Assets/Config/SceneAssetBundleRepeatAsset/" + assetBundleName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
        }
    }
}