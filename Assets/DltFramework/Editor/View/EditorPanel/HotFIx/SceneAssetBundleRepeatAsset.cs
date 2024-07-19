using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DltFramework
{
    [Serializable]
    public class SceneAssetBundleRepeatAsset : SerializedScriptableObject
    {
        [HorizontalGroup("AssetBundle包名称")] [HideLabel]
        public string assetBundleName;

        [HorizontalGroup("资源包含路径")] [HideLabel]
        public List<string> assetBundleContainPath;
    }
}