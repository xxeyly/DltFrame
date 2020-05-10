using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildSceneData", menuName = "配置文件/打包场景数据", order = 1)]
    public class BuildSceneData : ScriptableObject
    {

        public void OnGUI()
        {
            if (GUILayout.Button("Select"))
            {
            }
        }
    }

    [Serializable]
    public struct SceneData
    {
        [Header("打包名称")] public string sceneName;
        [Header("打包物体")] public GameObject buildDataObj;
    }

    [Serializable]
    public struct SceneDataList
    {
        [Header("打包数据")] public List<SceneData> sceneDatas;
    }
}