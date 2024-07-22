using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace HotFix
{
    [Serializable]
    [InfoBox("场景内所有AssetBundle资源信息")]
    public class HotFixRuntimeSceneAssetBundleConfig
    {
        [LabelText("场景配置")] public HotFixRuntimeAssetConfig sceneHotFixRuntimeAssetConfig = new HotFixRuntimeAssetConfig();
        [LabelText("重复资源")] public List<HotFixRuntimeAssetConfig> repeatSceneFixRuntimeAssetConfig = new List<HotFixRuntimeAssetConfig>();
        [LabelText("场景AssetBundle")] public List<HotFixRuntimeAssetConfig> assetBundleHotFixAssetAssetBundleAssetConfigs = new List<HotFixRuntimeAssetConfig>();
        [LabelText("额外数据")] public List<HotFixRuntimeAssetConfig> sceneExceptConfigs = new List<HotFixRuntimeAssetConfig>();
    }
}