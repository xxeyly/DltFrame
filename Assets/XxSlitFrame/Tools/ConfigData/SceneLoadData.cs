using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    public class SceneLoadData : ScriptableObject
    {
#if UNITY_EDITOR

        [LabelText("场景加载配置")] public List<SceneInfo> sceneInfos;

        [FolderPath] [LabelText("场景AssetBundle存放位置")]
        public string sceneAssetBundlePath;

        [Serializable]
        public struct SceneInfo
        {
            [HideLabel] [HorizontalGroup()] public SceneAsset sceneAsset;

            [LabelWidth(60)] [HorizontalGroup()] [EnumToggleButtons] [HideLabel]
            public SceneLoadType sceneLoadType;
        }

        public enum SceneLoadType
        {
            不加载,
            同步,
            异步
        }
#endif
    }
}