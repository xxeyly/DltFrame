using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace XFramework
{
    [Serializable]
    public class NormalSceneAssetBundleAsset : SerializedScriptableObject
    {
        
        [LabelText("拷贝资源文件")] public CopySceneAssetBundleAsset copySceneAssetBundleAsset;
        [LabelText("场景预制体资源")] public List<string> scenePrefabPaths = new List<string>();
    }
}