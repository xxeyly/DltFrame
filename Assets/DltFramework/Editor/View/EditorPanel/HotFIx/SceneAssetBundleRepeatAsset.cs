using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DltFramework
{
    [Serializable]
    public class SceneAssetBundleRepeatAsset : SerializedScriptableObject
    {
        [LabelText("ab包名称")] public string assetBundleName;
        [LabelText("资源包含路径")] public List<string> assetBundleContainPath;
    }
}