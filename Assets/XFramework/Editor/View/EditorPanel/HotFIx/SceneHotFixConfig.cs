using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    [Title("一个场景只能选择一种资源类型,拷贝资源类型不会产于主动打包,只会在正常场景打包时包含进去")]
    public class SceneHotFixConfig : BaseEditor
    {
        [LabelText("场景资源文件是否存在")] private bool isExistsNormalSceneAssetBundleAsset;

        [ShowIf("isExistsNormalSceneAssetBundleAsset")] [HorizontalGroup("NormalSceneAssetBundleAsset")] [LabelText("正常场景配置")] [InlineEditor()] [OnValueChanged("OnNormalSceneAssetBundleAssetChanged")] [AssetList]
        public NormalSceneAssetBundleAsset NormalSceneAssetBundleAsset;

        [ShowIf("@this.isExistsNormalSceneAssetBundleAsset")]
        [EnableIf("@this.NormalSceneAssetBundleAsset!=null")]
        [GUIColor(1, 0, 0)]
        [Button("删除", ButtonSizes.Medium)]
        public void RemoveNormalSceneAssetBundleAsset()
        {
            if (NormalSceneAssetBundleAsset != null)
            {
                UnityEditor.AssetDatabase.DeleteAsset("Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }
        }

        [LabelText("创建场景资源文件")] private bool isCreateNormalSceneAssetBundleAsset;

        [Button("创建场景热更配置")]
        [InfoBox("正常场景类型为常见的常见类型,可设置是否包含拷贝资源类型,如果包含拷贝资源类型,那么就会使用拷贝资源类型的资源,如果不包含,那么就会使用正常资源类型的资源")]
        [ShowIf("@isCreateNormalSceneAssetBundleAsset && isExistsCopySceneAssetBundleAsset==false")]
        public void CreateSceneAssetBundleAsset()
        {
            if (!Directory.Exists("Assets/Config/SceneHotfixAsset/"))
            {
                Directory.CreateDirectory("Assets/Config/SceneHotfixAsset/");
                UnityEditor.AssetDatabase.Refresh();
            }

            UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NormalSceneAssetBundleAsset>(), "Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        private void OnNormalSceneAssetBundleAssetChanged()
        {
            if (NormalSceneAssetBundleAsset != null)
            {
                if (NormalSceneAssetBundleAsset.name != SceneManager.GetActiveScene().name)
                {
                    List<string> allScenePath = DataFrameComponent.GetSpecifyTypeOnlyInAssetsPath("unity");
                    for (int i = 0; i < allScenePath.Count; i++)
                    {
                        if (DataFrameComponent.GetPathFileNameDontContainFileType(allScenePath[i]) == NormalSceneAssetBundleAsset.name)
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(allScenePath[i]);
                        }
                    }
                }
            }
        }

        [LabelText("场景资源文件是否存在")] private bool isExistsCopySceneAssetBundleAsset;

        [ShowIf("@this.isExistsCopySceneAssetBundleAsset")] [LabelText("拷贝场景资源文件")] [InlineEditor()] [OnValueChanged("OnCreateCopySceneAssetBundleAssetChanged")] [AssetList]
        public CopySceneAssetBundleAsset CopySceneAssetBundleAsset;

        [ShowIf("@this.isExistsCopySceneAssetBundleAsset")]
        [EnableIf("@this.CopySceneAssetBundleAsset!=null")]
        [GUIColor(1, 0, 0)]
        [Button("删除", ButtonSizes.Medium)]
        public void RemoveCopySceneAssetBundleAsset()
        {
            if (CopySceneAssetBundleAsset != null)
            {
                UnityEditor.AssetDatabase.DeleteAsset("Assets/Config/SceneHotfixAsset/" + "Copy" + SceneManager.GetActiveScene().name + ".asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }

            GetSceneAssetBundleAsset();
        }

        [LabelText("创建场景资源文件")] private bool isCreateCopySceneAssetBundleAsset;

        [HorizontalGroup]
        [Button("创建拷贝场景热更配置")]
        [InfoBox("当多个场景中用到了同一份资源,如UI或资源是多个场景共同使用的,场景之间不同的就是场景中环境不一样,创建拷贝类型就可以应对这种情况," +
                 "拷贝的节点要名称统一为(当前场景名称)+Root,举例,当前拷贝场景为'Main',那么拷贝的节点就应该为'MainRoot'", InfoMessageType.Warning)]
        [ShowIf("@isCreateCopySceneAssetBundleAsset && isExistsNormalSceneAssetBundleAsset==false")]
        public void CreateCopySceneAssetBundleAsset()
        {
            if (!Directory.Exists("Assets/Config/SceneHotfixAsset/"))
            {
                Directory.CreateDirectory("Assets/Config/SceneHotfixAsset/");
                UnityEditor.AssetDatabase.Refresh();
            }

            UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CopySceneAssetBundleAsset>(), "Assets/Config/SceneHotfixAsset/" + "Copy" + SceneManager.GetActiveScene().name + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        private void OnCreateCopySceneAssetBundleAssetChanged()
        {
            if (CopySceneAssetBundleAsset != null)
            {
                if (CopySceneAssetBundleAsset.name != SceneManager.GetActiveScene().name)
                {
                    List<string> allScenePath = DataFrameComponent.GetSpecifyTypeOnlyInAssetsPath("unity");
                    for (int i = 0; i < allScenePath.Count; i++)
                    {
                        if (DataFrameComponent.GetPathFileNameDontContainFileType(allScenePath[i]) == NormalSceneAssetBundleAsset.name)
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(allScenePath[i]);
                        }
                    }
                }
            }

            GetCopySceneAssetBundleAsset();
        }


        public void Update()
        {
        }

        [LabelText("获得当前场景热更配置")]
        public void GetSceneAssetBundleAsset()
        {
            NormalSceneAssetBundleAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<NormalSceneAssetBundleAsset>("Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
            if (NormalSceneAssetBundleAsset == null)
            {
                isExistsNormalSceneAssetBundleAsset = false;
                isCreateNormalSceneAssetBundleAsset = true;
            }
            else
            {
                isExistsNormalSceneAssetBundleAsset = true;
                isCreateNormalSceneAssetBundleAsset = false;
            }
        }

        [LabelText("获得当前场景热更配置")]
        public void GetCopySceneAssetBundleAsset()
        {
            CopySceneAssetBundleAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<CopySceneAssetBundleAsset>("Assets/Config/SceneHotfixAsset/" + "Copy" + SceneManager.GetActiveScene().name + ".asset");
            if (CopySceneAssetBundleAsset == null)
            {
                isExistsCopySceneAssetBundleAsset = false;
                isCreateCopySceneAssetBundleAsset = true;
                return;
            }
            else
            {
                isExistsCopySceneAssetBundleAsset = true;
                isCreateCopySceneAssetBundleAsset = false;
            }
        }


        #region 节点操作

        [BoxGroup("节点操作")]
        [ShowIf("@this.CopySceneAssetBundleAsset !=null")]
        [GUIColor(0, 1, 0)]
        [Button("生成节点Root", ButtonSizes.Medium)]
        public void GenerateEmptyRoot()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!GameObject.Find(sceneName + "Root"))
            {
                Debug.LogError("未找到当前场景的根Root");
                return;
            }

            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.GetAllObjectsInScene<HotFixAssetPathConfig>();
            CopySceneAssetBundleAsset.scenePrefabPaths.Clear();
            //应用预制体,并且保存路径
            foreach (HotFixAssetPathConfig hotFixAssetPathConfig in hotFixAssetPathConfigs)
            {
                if (!PrefabUtility.IsPartOfPrefabAsset(hotFixAssetPathConfig.gameObject))
                {
                    hotFixAssetPathConfig.SetPathAndApplyPrefab();
                }

                CopySceneAssetBundleAsset.scenePrefabPaths.Add(hotFixAssetPathConfig.prefabPath);
            }


            //移除当前场景中的HotFixAssetPathConfig
            foreach (HotFixAssetPathConfig hotFixAssetSceneHierarchyPath in hotFixAssetPathConfigs)
            {
                GameObject.DestroyImmediate(hotFixAssetSceneHierarchyPath.gameObject);
            }

            GameObject emptyRoot = GameObject.Find(sceneName + "Root");
            if (!Directory.Exists("Assets/HotFixPrefabs/Scene/" + sceneName))
            {
                Directory.CreateDirectory("Assets/HotFixPrefabs/Scene/" + sceneName);
                UnityEditor.AssetDatabase.Refresh();
            }

            //存储根点信息
#pragma warning disable 0618
            string tootPrefabPath = "Assets/HotFixPrefabs/Scene/" + sceneName + "/" + emptyRoot.name + ".prefab";
            PrefabUtility.CreatePrefab(tootPrefabPath, emptyRoot, ReplacePrefabOptions.ConnectToPrefab);
            CopySceneAssetBundleAsset.rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(tootPrefabPath);
#pragma warning restore 0618
            // NormalSceneAssetBundleAsset.rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/HotFixPrefabs/Scene/" + sceneName + "/" + emptyRoot.name + ".prefab");
            PrefabUtility.UnpackPrefabInstance(emptyRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            //显示场景物体
            ShowSceneObject();
        }

        [BoxGroup("节点操作")]
        [ShowIf("@this.NormalSceneAssetBundleAsset !=null && this.NormalSceneAssetBundleAsset.copySceneAssetBundleAsset !=null && this.NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab != null")]
        [GUIColor(0, 1, 0)]
        [Button("加载节点Root", ButtonSizes.Medium)]
        public void LoadEmptyRoot()
        {
            RemoveEmptyRoot();
            GameObject root = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab, null);

            UnityEditor.PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            //显示场景物体
            ShowSceneObject();
        }

        #endregion

        private void RemoveEmptyRoot()
        {
            //查找到相同节点,先删除,后添加
            if (GameObject.Find(NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab.name))
            {
                GameObject.DestroyImmediate(GameObject.Find(NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.rootPrefab.name));
            }
        }

        [LabelText("显示场景资源")]
        private void ShowSceneObject()
        {
            List<string> scenePrefabPaths = new List<string>();
            if (NormalSceneAssetBundleAsset != null)
            {
                for (int i = 0; i < NormalSceneAssetBundleAsset.scenePrefabPaths.Count; i++)
                {
                    scenePrefabPaths.Add(NormalSceneAssetBundleAsset.scenePrefabPaths[i]);
                }

                if (NormalSceneAssetBundleAsset.copySceneAssetBundleAsset != null)
                {
                    for (int i = 0; i < NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.scenePrefabPaths.Count; i++)
                    {
                        scenePrefabPaths.Add(NormalSceneAssetBundleAsset.copySceneAssetBundleAsset.scenePrefabPaths[i]);
                    }
                }
            }
            else if (CopySceneAssetBundleAsset != null)
            {
                for (int i = 0; i < CopySceneAssetBundleAsset.scenePrefabPaths.Count; i++)
                {
                    scenePrefabPaths.Add(CopySceneAssetBundleAsset.scenePrefabPaths[i]);
                }
            }

            for (int i = 0; i < scenePrefabPaths.Count; i++)
            {
                //路径信息
                HotFixAssetPathConfig hotFixAssetSceneHierarchyPath = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPaths[i]);
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
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
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

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
            GetSceneAssetBundleAsset();
            GetCopySceneAssetBundleAsset();
        }
    }
}