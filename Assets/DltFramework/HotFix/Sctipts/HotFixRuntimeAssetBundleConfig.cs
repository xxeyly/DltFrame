using System;
using Sirenix.OdinInspector;

[Serializable]
public class HotFixRuntimeAssetBundleConfig
{
    [LabelText("AssetBundle名称")] public string assetBundleName;
    [LabelText("AssetBundle路径")] public string assetBundlePath;
    [LabelText("AssetBundle实例化路径")] public string assetBundleInstantiatePath;
    [LabelText("AssetBundleMd5码")] public string md5;
    [LabelText("AssetBundle大小")] public string assetBundleSize;
}
