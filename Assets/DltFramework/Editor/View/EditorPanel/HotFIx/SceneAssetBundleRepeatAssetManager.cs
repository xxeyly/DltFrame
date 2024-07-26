using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    public class SceneAssetBundleRepeatAssetManager : BaseEditor
    {
        [BoxGroup("创建内容")] [LabelText("AssetBundle包名称")]
        public string assetBundleName;

        [BoxGroup("创建内容")] [LabelText("资源包含路径")]
        public List<string> assetBundleContainPath;

        [BoxGroup("创建内容")]
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
            for (int i = 0; i < assetBundleContainPath.Count; i++)
            {
                sceneAssetBundleRepeatAsset.assetBundleContainPath.Add(assetBundleContainPath[i]);
            }
            UnityEditor.AssetDatabase.CreateAsset(sceneAssetBundleRepeatAsset, "Assets/Config/SceneAssetBundleRepeatAsset/" + assetBundleName + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            assetBundleName = String.Empty;
            assetBundleContainPath.Clear();
        }

        [InlineEditor()] [LabelText("场景重复资源列表")]
        public List<SceneAssetBundleRepeatAsset> SceneAssetBundleRepeatAssets = new List<SceneAssetBundleRepeatAsset> { };


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