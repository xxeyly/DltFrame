using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class SceneAssetBundleAsset : SerializedScriptableObject
{
    [LabelText("打包场景")] public bool sceneBuildScene = false;
    [LabelText("拷贝资源文件")] public SceneAssetBundleAsset copySceneAssetBundleAsset;
    [LabelText("场景预制体资源")] [TableList] public List<ScenePrefabConfig> ScenePrefabConfigs = new List<ScenePrefabConfig>();
    [LabelText("空的节点信息")] public GameObject rootPrefab;
}

[LabelText("场景预制体信息")]
public class ScenePrefabConfig
{
    [HorizontalGroup("预制体路径")] [HideLabel] public string prefabPath;

    [HorizontalGroup("预制体Md5")] [HideLabel] [TableColumnWidth(width: 260, resizable: false)]
    public string prefabMd5;

    [HorizontalGroup("资源更新")] [HideLabel] [TableColumnWidth(width: 60, resizable: false)]
    public bool resourceUpdate;
}

[Serializable]
public enum AssetType
{
    Scene,
    Asset
}