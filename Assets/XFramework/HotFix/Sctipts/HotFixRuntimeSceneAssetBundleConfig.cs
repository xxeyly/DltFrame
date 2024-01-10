using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
[InfoBox("场景内所有AssetBundle资源信息")]
public class HotFixRuntimeSceneAssetBundleConfig
{
    [LabelText("场景配置")] public HotFixRuntimeAssetBundleConfig sceneHotFixRuntimeAssetBundleConfig = new HotFixRuntimeAssetBundleConfig();

    //TODO 这里有个问题,当拷贝场景字体和正常场景字体不一致时,并且都存在时,会出现问题
    [LabelText("场景字体")] public HotFixRuntimeAssetBundleConfig sceneFontFixRuntimeAssetConfig = new HotFixRuntimeAssetBundleConfig();
    [LabelText("场景AssetBundle")] public List<HotFixRuntimeAssetBundleConfig> assetBundleHotFixAssetAssetBundleAssetConfigs = new List<HotFixRuntimeAssetBundleConfig>();
}