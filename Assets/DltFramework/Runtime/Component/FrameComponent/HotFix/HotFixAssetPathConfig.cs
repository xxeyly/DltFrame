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
#if UNITY_EDITOR
    [Button("生成路径", ButtonSizes.Medium)]
    [GUIColor(0, 1, 0)]
    public void SetPathAndApplyPrefab()
    {
        generateHierarchyPath = DataFrameComponent.Hierarchy_GetTransformHierarchy(transform, false);
        prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        string prefabPathDirectory = DataFrameComponent.String_BuilderString("Assets/HotFixPrefabs/Scene/", SceneManager.GetActiveScene().name, "/", GetHotFixAssetType());
        if (prefabPath == string.Empty)
        {
            if (!Directory.Exists(prefabPathDirectory))
            {
                Directory.CreateDirectory(prefabPathDirectory);
                AssetDatabase.Refresh();
            }
        }

        assetBundlePath = DataFrameComponent.String_BuilderString("HotFixRuntime/HotFixAssetBundle", DataFrameComponent.String_AllCharToLower(prefabPath.Replace("Assets/HotFixPrefabs/Scene", "").Replace(".prefab", "")));
        prefabPath = DataFrameComponent.String_BuilderString(prefabPathDirectory, "/", gameObject.name, ".prefab");
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

    [Button("保存预制体")]
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

        GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        var thisTransform = transform;
        prefabObj.transform.localPosition = thisTransform.localPosition;
        prefabObj.transform.localEulerAngles = thisTransform.localEulerAngles;
        prefabObj.transform.localScale = thisTransform.localScale;
        AssetDatabase.SaveAssets();
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