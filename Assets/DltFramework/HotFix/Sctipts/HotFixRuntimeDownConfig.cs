using System;
using Sirenix.OdinInspector;

namespace HotFix
{
    [Serializable]
    public class HotFixRuntimeDownConfig
    {
        [LabelText("Asset名称")] public string name;
        [LabelText("Asset路径")] public string path;
        [LabelText("Asset大小")] public string size;
        [LabelText("AssetMD5")] public string md5;
        [LabelText("Asset版本")] public int version = 0;
    }
}