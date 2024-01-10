using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFramework;

#if HybridCLR
public class SceneHotfixAssetManager : BaseEditor
{
#if UNITY_EDITOR
    [LabelText("场景热更资源文件-切换场景后会自动更换配置文件,无需手动切换")] [AssetList] [InlineEditor()]
    public SceneAssetBundleAsset SceneAssetBundleAsset;

    [LabelText("当前编辑场景")] private Scene currentrScene;


    public void Update()
    {
        Scene tempCurrentScene = SceneManager.GetActiveScene();
        if (tempCurrentScene != currentrScene)
        {
            currentrScene = tempCurrentScene;
            UpdateCurrenScene();
        }
    }

    private void UpdateCurrenScene()
    {
        GetCurrentSceneAssetBundleAsset();
    }


    [LabelText("显示场景资源")]
    private void ShowSceneObject()
    {
        for (int i = 0; i < SceneAssetBundleAsset.ScenePrefabConfigs.Count; i++)
        {
            //路径信息
            HotFixAssetPathConfig hotFixAssetSceneHierarchyPath = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(SceneAssetBundleAsset.ScenePrefabConfigs[i]);
            Transform parent = null;
            if (hotFixAssetSceneHierarchyPath.GetHierarchyGeneratePath() != string.Empty)
            {
                if (GameObject.Find(hotFixAssetSceneHierarchyPath.GetHierarchyGeneratePath()))
                {
                    parent = GameObject.Find(hotFixAssetSceneHierarchyPath.GetHierarchyGeneratePath()).transform;
                }
            }

            //实例化
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(hotFixAssetSceneHierarchyPath.gameObject);
            GameObject temp = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath), parent);
        }

        //UI层排序
        ViewSort();
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(currentrScene);
    }


    private bool ParentConHotFixAssetPathConfig(Transform current)
    {
        if (current.parent != null)
        {
            if (current.parent.GetComponent<HotFixAssetPathConfig>())
            {
                return true;
            }
            else
            {
                return ParentConHotFixAssetPathConfig(current.parent);
            }
        }

        return false;
    }

    [Button("创建当前场景热更配置")]
    [EnableIf("@this.SceneAssetBundleAsset == null")]
    public void CreateCurrentSceneAssetBundleAsset()
    {
        if (!Directory.Exists("Assets/Config/SceneHotfixAsset/"))
        {
            Directory.CreateDirectory("Assets/Config/SceneHotfixAsset/");
            UnityEditor.AssetDatabase.Refresh();
        }

        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SceneAssetBundleAsset>(), "Assets/Config/SceneHotfixAsset/" + currentrScene.name + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        GetCurrentSceneAssetBundleAsset();
    }

    [LabelText("获得当前场景热更配置")]
    public void GetCurrentSceneAssetBundleAsset()
    {
        SceneAssetBundleAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<SceneAssetBundleAsset>("Assets/Config/SceneHotfixAsset/" + currentrScene.name + ".asset");
        if (SceneAssetBundleAsset == null)
        {
            return;
        }

        if (SceneAssetBundleAsset.copySceneAssetBundleAsset != null)
        {
            SceneAssetBundleAsset.ScenePrefabConfigs = SceneAssetBundleAsset.copySceneAssetBundleAsset.ScenePrefabConfigs;
            SceneAssetBundleAsset.sceneBuildScene = true;
            SceneAssetBundleAsset.rootPrefab = null;
        }
    }

    //视图重新排序
    private void ViewSort()
    {
        List<BaseWindow> sceneAllBaseWindow = DataFrameComponent.GetAllObjectsInScene<BaseWindow>();
        List<BaseWindow> sortBaseWindow = new List<BaseWindow>();

        for (int i = 0; i < sceneAllBaseWindow.Count; i++)
        {
            foreach (BaseWindow baseWindow in sceneAllBaseWindow)
            {
                if (baseWindow.GetSceneLayerIndex() == i)
                {
                    sortBaseWindow.Add(baseWindow);
                }
            }
        }

        //UI层排序
        foreach (BaseWindow baseWindow in sortBaseWindow)
        {
            if (!baseWindow.GetComponent<ChildBaseWindow>())
            {
                baseWindow.SetSetSiblingIndex();
            }
        }
    }

    [LabelText("生成配置信息")]
    private void GenerateBuildConfig()
    {
        string sceneName = DataFrameComponent.AllCharToLower(SceneManager.GetActiveScene().name);
        HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
        if (SceneAssetBundleAsset.sceneBuildScene)
        {
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleName = sceneName;
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/";
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleSize =
                FileOperation.GetFileSize(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName).ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.md5 =
                FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName);
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleInstantiatePath = "";
        }

        if (SceneAssetBundleAsset.copySceneAssetBundleAsset == null)
        {
            //场景中可能不包含字体,不包含,设为空
            if (!File.Exists(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/font/" + sceneName + "font"))
            {
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig = null;
            }
            else
            {
                //场景字体
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleName = sceneName + "font";
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/font/";
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleSize =
                    FileOperation.GetFileSize(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/font/" + sceneName + "font").ToString();
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.md5 =
                    FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/font/" + sceneName + "font");
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleInstantiatePath = "";
            }
        }
        else
        {
            string copySceneName = DataFrameComponent.AllCharToLower(SceneAssetBundleAsset.copySceneAssetBundleAsset.name);
            //场景字体
            hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleName = copySceneName + "font";
            hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/";
            hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleSize =
                FileOperation.GetFileSize(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/" + copySceneName + "font").ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.md5 =
                FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/" + copySceneName + "font");
        }


        //场景AssetBundle
        foreach (string scenePrefabPath in SceneAssetBundleAsset.ScenePrefabConfigs)
        {
            HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
            UnityEditor.AssetImporter assetImporter = UnityEditor.AssetImporter.GetAtPath(hotFixAssetPathConfig.prefabPath);
            assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
            HotFixRuntimeAssetBundleConfig hot = new HotFixRuntimeAssetBundleConfig();

            hot.assetBundleName = DataFrameComponent.AllCharToLower(hotFixAssetPathConfig.name);
            hot.assetBundlePath = hotFixAssetPathConfig.assetBundlePath.Replace(DataFrameComponent.AllCharToLower(hotFixAssetPathConfig.name), "");
            hot.assetBundleInstantiatePath = hotFixAssetPathConfig.GetHierarchyGeneratePath();
            string adPath = Application.streamingAssetsPath + "/" + hot.assetBundlePath + hot.assetBundleName;
            hot.assetBundleSize = FileOperation.GetFileSize(adPath).ToString();
            hot.md5 = FileOperation.GetMD5HashFromFile(adPath);
            hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hot);
        }

        FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFixRuntime/HotFixAssetBundleConfig", SceneManager.GetActiveScene().name + ".json", JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
        Debug.Log("打包 配置信息完成");
    }

    [BoxGroup("节点操作")]
    [EnableIf("@this.SceneAssetBundleAsset !=null && this.SceneAssetBundleAsset.copySceneAssetBundleAsset == null")]
    [GUIColor(0, 1, 0)]
    [Button("生成节点Root", ButtonSizes.Medium)]
    public void GenerateEmptyRoot()
    {
        List<HotFixAssetPathConfig> hotFixAssetSceneHierarchyPaths = DataFrameComponent.GetAllObjectsInScene<HotFixAssetPathConfig>();
        //移除当前场景中的HotFixAssetPathConfig
        foreach (HotFixAssetPathConfig hotFixAssetSceneHierarchyPath in hotFixAssetSceneHierarchyPaths)
        {
            GameObject.DestroyImmediate(hotFixAssetSceneHierarchyPath.gameObject);
        }

        string sceneName = currentrScene.name;
        GameObject emptyRoot = GameObject.Find(sceneName + "Root");
        if (!Directory.Exists("Assets/HotFixPrefabs/Scene/" + sceneName))
        {
            Directory.CreateDirectory("Assets/HotFixPrefabs/Scene/" + sceneName);
            UnityEditor.AssetDatabase.Refresh();
        }

        //存储根点信息
#pragma warning disable 0618
        PrefabUtility.CreatePrefab("Assets/HotFixPrefabs/Scene/" + sceneName + "/" + emptyRoot.name + ".prefab", emptyRoot, ReplacePrefabOptions.ConnectToPrefab);
#pragma warning restore 0618
        SceneAssetBundleAsset.rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/HotFixPrefabs/Scene/" + sceneName + "/" + emptyRoot.name + ".prefab");
        PrefabUtility.UnpackPrefabInstance(emptyRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        //显示场景物体
        ShowSceneObject();
    }

    [BoxGroup("节点操作")]
    [EnableIf("@this.SceneAssetBundleAsset !=null && this.SceneAssetBundleAsset.copySceneAssetBundleAsset !=null && this.SceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab != null")]
    [GUIColor(0, 1, 0)]
    [Button("加载节点Root", ButtonSizes.Medium)]
    public void LoadEmptyRoot()
    {
        RemoveEmptyRoot();
        GameObject root = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(SceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab, null);

        UnityEditor.PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        //显示场景物体
        ShowSceneObject();
    }

    private void RemoveEmptyRoot()
    {
        //查找到相同节点,先删除,后添加
        if (GameObject.Find(SceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab.name))
        {
            GameObject.DestroyImmediate(GameObject.Find(SceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab.name));
        }
    }

    [BoxGroup("打包")]
    [GUIColor(0, 1, 0)]
    [Button("清空资源AssetBundle信息", ButtonSizes.Medium)]
    public void ClearAssetBundleData()
    {
        DataFrameComponent.RemoveAllAssetBundleName();
    }

    [BoxGroup("打包")]
    [GUIColor(0, 1, 0)]
    [Button("打包AssetBundle并生成配置表", ButtonSizes.Medium)]
    public void BundleSceneDataAndGenerateBuildConfig()
    {
        if (SceneAssetBundleAsset == null)
        {
            Debug.Log("无配置文件");
            return;
        }

        if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle"))
        {
            Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle");
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        //检查场景中热更资源配置是否正确
        //HotFixAssetPathConfig 只能作为父节点存在,上面任何父物体不能再次包含HotFixAssetPathConfig,保证唯一性
        //设置场景中所有路径配置信息
        List<HotFixAssetPathConfig> hotFixAssetSceneHierarchyPaths = DataFrameComponent.GetAllObjectsInScene<HotFixAssetPathConfig>();
        //是否有配置信息错误
        bool HotFixAssetPathConfigNodeCheck = false;
        foreach (HotFixAssetPathConfig hotFixAssetPathConfig in hotFixAssetSceneHierarchyPaths)
        {
            if (ParentConHotFixAssetPathConfig(hotFixAssetPathConfig.transform))
            {
                Debug.Log(hotFixAssetPathConfig.name + "该物体配置错误");
                HotFixAssetPathConfigNodeCheck = true;
            }
        }


        if (!HotFixAssetPathConfigNodeCheck)
        {
            //应用预制体
            foreach (HotFixAssetPathConfig hotFixAssetPathConfig in hotFixAssetSceneHierarchyPaths)
            {
                hotFixAssetPathConfig.SetPathAndApplyPrefab();
            }

            //移除当前场景中的HotFixAssetPathConfig
            foreach (HotFixAssetPathConfig hotFixAssetSceneHierarchyPath in hotFixAssetSceneHierarchyPaths)
            {
                GameObject.DestroyImmediate(hotFixAssetSceneHierarchyPath.gameObject);
            }
            //打包当前场景

            string sceneName = String.Empty;
            //打包场景
            sceneName = SceneManager.GetActiveScene().name;
            if (SceneAssetBundleAsset.sceneBuildScene)
            {
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                UnityEditor.AssetImporter sceneAssetImporter = UnityEditor.AssetImporter.GetAtPath(SceneManager.GetActiveScene().path);
                sceneAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName;
                Debug.Log(sceneName);
            }

            //如果是拷贝场景数据,信息不参与打包
            if (SceneAssetBundleAsset.copySceneAssetBundleAsset == null)
            {
                foreach (string scenePrefabPath in SceneAssetBundleAsset.ScenePrefabConfigs)
                {
                    //不管该Prefab是否参与打包,字体肯定要重更新打包
                    foreach (string assetDependency in GetAssetDependencies(scenePrefabPath))
                    {
                        if (assetDependency.Contains("TMP_SDF.shader") || assetDependency.Contains(".otf") || assetDependency.Contains(".ttf") || assetDependency.Contains(".asset"))
                        {
                            AssetImporter fontAssetImporter = UnityEditor.AssetImporter.GetAtPath(assetDependency);
                            fontAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Font/" + sceneName + "Font";
                        }
                    }

                    AssetImporter assetImporter = AssetImporter.GetAtPath(scenePrefabPath);
                    HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                    assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
                }
            }


            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression /*| BuildAssetBundleOptions.DisableWriteTypeTree*/, EditorUserBuildSettings.activeBuildTarget);

            UnityEditor.AssetDatabase.Refresh();

            //显示场景物体
            ShowSceneObject();
            //生成场景配置表
            GenerateBuildConfig();
            DataFrameComponent.RemoveAllAssetBundleName();
        }
    }

    [BoxGroup("配置表")]
    [GUIColor(0, 1, 0)]
    [Button("生成所有场景配置表", ButtonSizes.Medium)]
    public void GenerateAllSceneConfig()
    {
        List<string> HotFixAssetAssetBundleSceneConfig = new List<string>();
        List<string> HotFixAssetAssetBundleSceneConfigPath = DataFrameComponent.GetGetSpecifyPathInAllTypePath("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundleConfig", "json");

        for (int i = 0; i < HotFixAssetAssetBundleSceneConfigPath.Count; i++)
        {
            HotFixAssetAssetBundleSceneConfig.Add(DataFrameComponent.GetPathFileNameDontContainFileType(HotFixAssetAssetBundleSceneConfigPath[i]));
        }

        FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFixRuntime", "HotFixServerResourcesCount.json", JsonMapper.ToJson(HotFixAssetAssetBundleSceneConfig));
    }

    public void Save()
    {
        if (SceneAssetBundleAsset == null)
        {
            return;
        }

        UnityEditor.EditorUtility.SetDirty(SceneAssetBundleAsset);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }

    private static string[] GetAssetDependencies(string assetPath)
    {
        if (!File.Exists(assetPath))
        {
            return null;
        }

        string[] dependecies = UnityEditor.AssetDatabase.GetDependencies(assetPath);
        return dependecies;
    }
#endif
    public override void OnDisable()
    {
    }

    public override void OnCreateConfig()
    {
    }

    public override void OnSaveConfig()
    {
        Save();
    }

    public override void OnLoadConfig()
    {
    }

    private void OnActiveBuildTargetChanged()
    {
    }

    public override void OnInit()
    {
    }
}
#endif