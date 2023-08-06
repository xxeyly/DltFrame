using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFramework;

public class HotFixAssetPathConfig : SerializedMonoBehaviour
{
    [LabelText("生成路径")] public string generateHierarchyPath;
    [LabelText("预制体路径")] public string prefabPath;
    [LabelText("Ab包路径")] public string assetBundlePath;
#if UNITY_EDITOR
    [Button("生成路径", ButtonSizes.Medium)]
    [LabelText("重命名")]
    [GUIColor(0, 1, 0)]
    public void SetPath()
    {
        generateHierarchyPath = DataFrameComponent.GetComponentPath(transform, false);
        prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        assetBundlePath = "HotFix/HotFixAssetBundle" + DataFrameComponent.AllCharToLower(prefabPath.Replace("Assets/HotFixPrefabs/Scene", "").Replace(".prefab", ""));
    }

    [Button("保存预制体")]
    public GameObject ApplyPrefab()
    {
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
        GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return prefab;
    }

    /// <summary>
    /// 生成路径
    /// </summary>
    /// <returns></returns>
    public string GetHierarchyGeneratePath()
    {
        return generateHierarchyPath;
    }
#endif
}