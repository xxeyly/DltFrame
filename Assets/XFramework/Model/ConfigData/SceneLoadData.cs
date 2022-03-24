using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class SceneLoadData : ScriptableObject
    {
        [LabelText("场景信息")] public SceneFile sceneFile = new SceneFile();

        /// <summary>
        /// 下载文件
        /// </summary>
        [Serializable]
        public class SceneFile
        {
            /// <summary>
            /// 场景文件列表
            /// </summary>
            [TableList] [LabelText("版本信息列表")] public List<SceneInfo> sceneInfoList;

            /// <summary>
            /// 场景文件信息
            /// </summary>
            [Serializable]
            public struct SceneInfo
            {
                [HorizontalGroup("场景名称")] [HideLabel] public string sceneName;

                [HorizontalGroup("场景加载方式")] [HideLabel]
                public SceneLoadType sceneLoadType;
            }

            public enum SceneLoadType
            {
                不加载,
                同步,
                异步,
                下载同步,
                下载异步
            }
        }
    }
}