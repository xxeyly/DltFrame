using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DltFramework
{
    [Serializable]
    public class NormalSceneAssetBundleAsset : SerializedScriptableObject
    {
        [LabelText("拷贝资源文件")] public CopySceneAssetBundleAsset copySceneAssetBundleAsset;
        [LabelText("场景预制体资源")] public List<string> scenePrefabPaths = new List<string>();

        [LabelText("场景重复资源")] public List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets = new List<SceneAssetBundleRepeatAsset>();

        [BoxGroup("Md5")] [LabelText("场景Md5")] public string sceneMd5;

        [BoxGroup("Md5")] [LabelText("场景预制体Md5")]
        public Dictionary<string, string> scenePrefabMd5 = new Dictionary<string, string>();

        [BoxGroup("Md5")] [LabelText("场景重复Md5")]
        public Dictionary<string, List<string>> sceneRepeatMd5 = new Dictionary<string, List<string>>();
    }

    [Serializable]
    public class SceneAssetBundleRepeatAsset
    {
        [LabelText("ab包名称")] public string assetBundleName;
        [LabelText("资源包含路径")] public List<string> assetBundleContainPath;
    }
}