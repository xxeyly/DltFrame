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
    }

    [Serializable]
    public class SceneAssetBundleRepeatAsset
    {
        [LabelText("ab包名称")] public string assetBundleName;
        [LabelText("资源包含路径")] public List<string> assetBundleContainPath;
    }
}