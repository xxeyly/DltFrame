using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
[InfoBox("场景内所有AssetBundle资源信息")]
public class HotFixRuntimeSceneAssetBundleConfig
{
    [LabelText("场景配置")] public HotFixRuntimeAssetBundleConfig sceneHotFixRuntimeAssetBundleConfig = new HotFixRuntimeAssetBundleConfig();

    [LabelText("重复资源")] public List<HotFixRuntimeAssetBundleConfig> repeatSceneFixRuntimeAssetConfig = new List<HotFixRuntimeAssetBundleConfig>();
    [LabelText("场景AssetBundle")] public List<HotFixRuntimeAssetBundleConfig> assetBundleHotFixAssetAssetBundleAssetConfigs = new List<HotFixRuntimeAssetBundleConfig>();
}