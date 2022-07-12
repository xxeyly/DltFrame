#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 场景打包配置文件
    /// </summary>
    [Serializable]
    public class SceneLoadEditorData : ScriptableObject
    {
        [LabelText("场景加载配置")] public List<SceneInfo> sceneInfos = new List<SceneInfo>();

        [LabelText("场景AssetBundle存放位置")] [FolderPath]
        public string sceneAssetBundlePath;

        [LabelText("当前打包方式:")] public General.BuildTargetPlatform buildTargetPlatform;

        [Serializable]
        public class SceneInfo
        {
            [HorizontalGroup] [HideLabel] public SceneAsset sceneAsset;

            [HorizontalGroup] [HideLabel] /*[EnumToggleButtons]*/ [LabelWidth(60)][EnumPaging]
            public SceneLoad.SceneLoadType sceneLoadType;

            [HideInInspector] public string Md5;
        }
    }
}
#endif