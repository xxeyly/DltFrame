using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    public class BuildSceneAssetBundleData : ScriptableObject
    {
#if UNITY_EDITOR

        [LabelText("场景Build文件")] public List<SceneAssetBundleInfo> sceneAssetBundleInfos;

        [Serializable]
        public class SceneAssetBundleInfo
        {
            [LabelText("场景文件")] public SceneAsset sceneAsset;
            [LabelText("文件Md5")] public string Md5;
        }
#endif
    }
}