using System;
using Sirenix.OdinInspector;

namespace HotFix
{
    [Serializable]
    public class HotFixRuntimeAssetConfig
    {
        [LabelText("Asset名称")] public string assetName;
        [LabelText("Asset路径")] public string assetPath;
        [LabelText("AssetMd5码")] public string assetMd5;
        [LabelText("Asset大小")] public string assetSize;
        [LabelText("Asset版本")] public string version;
    }
}