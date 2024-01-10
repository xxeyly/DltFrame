using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class CopySceneAssetBundleAsset : SerializedScriptableObject
    {
        [LabelText("空的节点信息")] public GameObject rootPrefab;
        [LabelText("场景预制体资源")] public List<string> scenePrefabPaths = new List<string>();
    }
}