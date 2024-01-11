using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SceneAssetBundleAsset : SerializedScriptableObject
{
    [LabelText("打包场景")] public bool sceneBuildScene = false;
    [LabelText("拷贝资源文件")] public SceneAssetBundleAsset copySceneAssetBundleAsset;

    [LabelText("场景预制体资源")] [TableList] public List<string> ScenePrefabConfigs = new List<string>();
    [LabelText("空的节点信息")] public GameObject rootPrefab;
}