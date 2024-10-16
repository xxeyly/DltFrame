using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace HotFix
{
    [Serializable]
    [InfoBox("场景内所有AssetBundle资源信息")]
    public class HotFixRuntimeSceneAssetBundleConfig
    {
        [LabelText("场景配置")] public HotFixRuntimeDownConfig sceneHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();
        [LabelText("重复资源")] public List<HotFixRuntimeDownConfig> repeatSceneHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();
        [LabelText("场景AssetBundle")] public List<HotFixRuntimeDownConfig> assetBundleHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();
        [LabelText("额外数据")] public List<HotFixRuntimeDownConfig> sceneExceptConfigsHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();
    }
}