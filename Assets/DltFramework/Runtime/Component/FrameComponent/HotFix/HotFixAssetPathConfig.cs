using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DltFramework;

public class HotFixAssetPathConfig : MonoBehaviour
{
    [LabelText("生成路径")] public string generateHierarchyPath;
    [LabelText("预制体路径")] public string prefabPath;
    [LabelText("Ab包路径")] public string assetBundlePath;
    private string _hotFixPrefabsPath;
#if UNITY_EDITOR
    [Button("生成路径并应用预制体", ButtonSizes.Medium)]
    [GUIColor(0, 1, 0)]
    public void SetPathAndApplyPrefab()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        _hotFixPrefabsPath = "Assets/HotFixPrefabs/Scene/" + sceneName + "/" + GetHotFixAssetType();
        if (!Directory.Exists(_hotFixPrefabsPath))
        {
            Directory.CreateDirectory(_hotFixPrefabsPath);
        }

        string assetBundleDirectory = "Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle/" + sceneName + "/" + GetHotFixAssetType() + "/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        AssetDatabase.Refresh();
        generateHierarchyPath = DataFrameComponent.Hierarchy_GetTransformHierarchy(transform, false);
        if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        }
        else
        {
            prefabPath = _hotFixPrefabsPath + "/" + gameObject.name + ".prefab";
        }

        assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/" + GetHotFixAssetType() + "/" + DataFrameComponent.String_AllCharToLower(gameObject.name);
        // Debug.Log("Ab包路径:" + assetBundlePath);
        ApplyPrefab();
    }

    private string GetHotFixAssetType()
    {
        string hotFixAssetType;
        if (gameObject.GetComponent<BaseWindow>())
        {
            hotFixAssetType = "UI";
        }
        else if (gameObject.GetComponent<SceneComponent>())
        {
            hotFixAssetType = "SceneComponent";
        }
        else if (gameObject.GetComponent<SceneComponentInit>())
        {
            hotFixAssetType = "SceneComponentInit";
        }
        else if (gameObject.GetComponent<EntityItem>())
        {
            hotFixAssetType = "Entity";
        }
        else
        {
            hotFixAssetType = "Env";
        }

        return hotFixAssetType;
    }

    private void ApplyPrefab()
    {
        if (!File.Exists(prefabPath))
        {
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject) || PrefabUtility.IsPrefabAssetMissing(gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.AutomatedAction);
        }
        else
        {
            // PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.AutomatedAction);
            // PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
        }

        AgainCheckPath();
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// 再次检查路径
    /// </summary>
    private void AgainCheckPath()
    {
        GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        HotFixAssetPathConfig hotFixAssetPathConfig = prefabObj.GetComponent<HotFixAssetPathConfig>();
        if (hotFixAssetPathConfig != null)
        {
            hotFixAssetPathConfig.generateHierarchyPath = generateHierarchyPath;
            hotFixAssetPathConfig.assetBundlePath = assetBundlePath;
            hotFixAssetPathConfig.prefabPath = prefabPath;
        }

        EditorUtility.SetDirty(hotFixAssetPathConfig);
        EditorUtility.SetDirty(prefabObj);
        prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        hotFixAssetPathConfig = prefabObj.GetComponent<HotFixAssetPathConfig>();
        if (hotFixAssetPathConfig.generateHierarchyPath != generateHierarchyPath || hotFixAssetPathConfig.assetBundlePath != assetBundlePath || hotFixAssetPathConfig.prefabPath != prefabPath)
        {
            AgainCheckPath();
        }
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