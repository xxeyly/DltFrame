using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DltFramework;


public class HotFixAssetPathConfig : MonoBehaviour
{
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

        AssetDatabase.Refresh();
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

    /// <summary>
    /// 获取热更资源类型
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 应用预制体
    /// </summary>
    private void ApplyPrefab()
    {
        if (!File.Exists(prefabPath))
        {
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject) || PrefabUtility.IsPrefabAssetMissing(gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }

            try
            {
                PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.AutomatedAction);
            }
            catch (Exception e)
            {
                Debug.Log(gameObject.name + e);
            }
        }
        else
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, prefabPath, InteractionMode.AutomatedAction);
        }
#if UNITY_2021_1_OR_NEWER
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }
#endif

        AgainCheckPath();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
            hotFixAssetPathConfig.assetBundlePath = assetBundlePath;
            hotFixAssetPathConfig.prefabPath = prefabPath;
        }

        EditorUtility.SetDirty(hotFixAssetPathConfig);
        EditorUtility.SetDirty(prefabObj);
        prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        hotFixAssetPathConfig = prefabObj.GetComponent<HotFixAssetPathConfig>();
        if (hotFixAssetPathConfig.assetBundlePath != assetBundlePath || hotFixAssetPathConfig.prefabPath != prefabPath)
        {
            AgainCheckPath();
        }
    }


#endif
}