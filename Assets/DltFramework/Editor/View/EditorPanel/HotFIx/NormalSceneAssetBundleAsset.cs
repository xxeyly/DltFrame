using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [Serializable]
    public class NormalSceneAssetBundleAsset : SerializedScriptableObject
    {
        [BoxGroup("例外资源-路径限制UnStreamingAssets文件夹内")] [LabelText("场景例外资源")] [Tooltip("该资源会放到StreamingAssets中进行加载")] [FilePath]
        public List<string> sceneExceptionAsset = new List<string>();

        [LabelText("场景重复资源")] [AssetList] [InlineEditor()]
        public List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets = new List<SceneAssetBundleRepeatAsset>();
    }
}