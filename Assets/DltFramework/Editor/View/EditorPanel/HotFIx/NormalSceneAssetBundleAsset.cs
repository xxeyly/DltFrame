using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DltFramework
{
    [Serializable]
    public class NormalSceneAssetBundleAsset : SerializedScriptableObject
    {
        [LabelText("场景重复资源")] [AssetList] [InlineEditor()] public List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets = new List<SceneAssetBundleRepeatAsset>();
    }
}