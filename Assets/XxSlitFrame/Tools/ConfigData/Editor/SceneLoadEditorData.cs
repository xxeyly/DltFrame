using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    /// <summary>
    /// 场景打包配置文件
    /// </summary>
    [Serializable]
    public class SceneLoadEditorData : ScriptableObject
    {
        [LabelText("场景加载配置")] public List<SceneInfo> sceneInfos;

        [LabelText("场景AssetBundle存放位置")] [FolderPath]
        public string sceneAssetBundlePath;

        [Serializable]
        public class SceneInfo
        {
            [HorizontalGroup] [HideLabel] public SceneAsset sceneAsset;

            [HorizontalGroup] [HideLabel] [EnumToggleButtons] [LabelWidth(60)]
            public SceneLoadType sceneLoadType;

            [HideInInspector] public string Md5;
        }

        public enum SceneLoadType
        {
            不加载,
            同步,
            异步
        }
    }
}