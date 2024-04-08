using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
#if HybridCLR
using HybridCLR.Editor.Commands;
#endif
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DltFramework
{
    [Serializable]
    public class PlatformPath
    {
        public string path;
        public BuildTarget buildTarget;
    }

    [Serializable]
    public class SceneRepeatAssets
    {
        public string assetName;
        public List<string> repeatAssets = new List<string>();
    }

    public class HotFixCollect : BaseEditor
    {
        #region 属性

        [LabelText("打包压缩方式")] [LabelWidth(120)] [EnumPaging]
        public BuildAssetBundleOptions targetBuildAssetBundleOptions;

        [LabelText("更新数据下载地址")] [LabelWidth(120)] [ShowInInspector]
        private string hotFixDownPath;

        [LabelText("平台下载地址")] [HideInInspector]
        public List<PlatformPath> hotFixDownPathData = new List<PlatformPath>();

        [LabelText("本地是否开启更新")] public bool localIsUpdate;

        [InfoBox("打包后会拷贝所有资源到当前路径下,并且还会复制一份以当前版本号的为名的备份文件夹")] [LabelText("资源外部拷贝路径")] [LabelWidth(120)] [FolderPath(AbsolutePath = true)] [ShowInInspector]
        private string targetOutPath;

        [LabelText("平台拷贝地址")] [HideInInspector]
        public List<PlatformPath> targetOutPathData = new List<PlatformPath>();

        [InfoBox("当勾选版本号自增,会默认勾选所有资源,并且资源版本号+1")] [LabelText("版本号自增")] [LabelWidth(120)]
        public bool isUpdateVersion;

        [InfoBox("版本号分3个等级,\n等级1是特大改动(需要手动更改),\n等级2每次需要重新出包时变动(需要手动更改),\n等级3是每次小更新时变动(勾选版本自增每次打包会自增1)")] [LabelText("当前资源版本号")] [LabelWidth(120)]
        public Vector3 targetResourceVersion;

        [LabelText("打包后移除AssetBundle信息")] public bool removeAssetBundleName;

        #region HotFixView

        [ToggleGroup("buildHotFixView", "HotFixView")]
        public bool buildHotFixView;

        [ToggleGroup("buildHotFixView")] [LabelText("HotFixView预制体")] [AssetSelector]
        public GameObject HotFixViewPrefab;

        [ToggleGroup("buildHotFixView")] [LabelText("HotFixView预制体路径")] [HideInInspector]
        public string HotFixViewPrePath;

        #endregion

        #region HotFixCode

        [ToggleGroup("buildHotFixCode", "HotFixCode")]
        public bool buildHotFixCode;

        #endregion

        #region 元数据

        [ToggleGroup("buildMetaAssemblyParticipatePackaging", "MetaAssembly")]
        public bool buildMetaAssemblyParticipatePackaging;

        [ToggleGroup("buildAssemblyParticipatePackaging", "Assembly")]
        public bool buildAssemblyParticipatePackaging;

        #endregion

        #region GameRootStart

        [ToggleGroup("buildGameRootStart", "GameRootStart")]
        public bool buildGameRootStart;

        [ToggleGroup("buildGameRootStart")] [LabelText("GameRootStart预制体")] [AssetSelector]
        public GameObject GameRootStartPrefab;

        [ToggleGroup("buildGameRootStart")] [LabelText("GameRootStart预制体路径")] [Sirenix.OdinInspector.FilePath] [HideInInspector]
        public string GameRootStartPath;

        #endregion

        #region 场景打包

        [ToggleGroup("BuildScene", "场景打包")] public bool BuildScene;

        [ToggleGroup("BuildScene")] [LabelText("拷贝场景配置")] [AssetList]
        public List<CopySceneAssetBundleAsset> CopySceneAssetBundleAsset = new List<CopySceneAssetBundleAsset>();

        [HideInInspector] [LabelText("CopySceneAssetBundleAsset路径")]
        public List<string> CopySceneAssetBundleAssetPath = new List<string>();

        [ToggleGroup("BuildScene")] [LabelText("正常场景配置")] [AssetList]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssets = new List<NormalSceneAssetBundleAsset>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAsset")]
        public List<string> NormalSceneAssetBundleAssetsPath = new List<string>();

        [LabelText("当前打开场景名称")] [HideInInspector]
        public string currentOpenSceneName;

        [LabelText("重复利用资源")] private List<SceneRepeatAssets> sceneRepeatAssets = new List<SceneRepeatAssets>();

        [LabelText("场景预制体关联文件")] public Dictionary<string, List<string>> scenePrefabAssetPath = new Dictionary<string, List<string>>();
        [LabelText("所有场景资源路径")] private List<string> sceneAllBuildAssetPath = new List<string>();

        #endregion

        #endregion

        #region 打包入口

        [GUIColor(0, 1, 0)]
        [Button("打包", ButtonSizes.Large)]
        [LabelText("图集列表")]
        public void OnBuild()
        {
            sceneRepeatAssets.Clear();
            OnSaveConfig();

            #region 全局打包

            if (isUpdateVersion)
            {
                targetResourceVersion += new Vector3(0, 0, 1);
                OnSaveConfig();
                HotFixViewBuild();
                HotFixCodeBuild();
                AssemblyBuild();
                MetaAssemblyBuild();
                GameRootStartBuild();
                // CopySceneBuild();
                NormalSceneBuild();
            }

            #endregion

            #region 局部打包

            else
            {
                if (buildHotFixView)
                {
                    HotFixViewBuild();
                }

                if (buildHotFixCode)
                {
                    HotFixCodeBuild();
                }

                if (buildAssemblyParticipatePackaging)
                {
                    AssemblyBuild();
                }

                if (buildMetaAssemblyParticipatePackaging)
                {
                    MetaAssemblyBuild();
                }

                if (buildGameRootStart)
                {
                    GameRootStartBuild();
                }

                if (BuildScene)
                {
                    CopySceneBuild();
                    NormalSceneBuild();
                }
            }

            #endregion

            #region 打包后拷贝

            //这里保存两份是因为,一份是在StreamingAssets下,一份是在UnStreamingAssets下,这样可以保证在打包时,引用StreamingAssets路径下的
            //编辑器模式下,引用UnStreamingAssets路径下的,打包后StreamingAssets路径下只有HotFixDownPath.txt和localIsUpdate.txt两个文件
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/HotFix/", "HotFixDownPath.txt", hotFixDownPath);
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFix/", "HotFixDownPath.txt", hotFixDownPath);
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/HotFix/", "localIsUpdate.txt", localIsUpdate.ToString());
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFix/", "localIsUpdate.txt", localIsUpdate.ToString());

            List<string> hotFixServerResources = new List<string>();
            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
            {
                hotFixServerResources.Add(normalSceneAssetBundleAsset.name);
            }

            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFixRuntime/", "HotFixServerResourcesCount.json", JsonMapper.ToJson(hotFixServerResources));

            if (File.Exists("Assets/UnStreamingAssets/HotFixRuntime/HotFixRuntime"))
            {
                File.Delete("Assets/UnStreamingAssets/HotFixRuntime/HotFixRuntime");
            }

            if (File.Exists("Assets/UnStreamingAssets/HotFixRuntime/HotFixRuntime.manifest"))
            {
                File.Delete("Assets/UnStreamingAssets/HotFixRuntime/HotFixRuntime.manifest");
            }

            AssetDatabase.Refresh();
            if (Directory.Exists(targetOutPath))
            {
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixCode", targetOutPath + "/HotFix/HotFixCode");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixCodeConfig", targetOutPath + "/HotFix/HotFixCodeConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixView", targetOutPath + "/HotFix/HotFixView");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixViewConfig", targetOutPath + "/HotFix/HotFixViewConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/Metadata", targetOutPath + "/HotFix/Metadata");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/MetadataConfig", targetOutPath + "/HotFix/MetadataConfig");

                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/Assembly", targetOutPath + "/HotFixRuntime/Assembly");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/AssemblyConfig", targetOutPath + "/HotFixRuntime/AssemblyConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundle", targetOutPath + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig", targetOutPath + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle", targetOutPath + "/HotFixRuntime/HotFixAssetBundle");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundleConfig", targetOutPath + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperationComponent.CopyFile("Assets/UnStreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + "/HotFixRuntime/HotFixServerResourcesCount.json");
                //备份
                string backupVersion = "/Backups_" + targetResourceVersion.x + "." + targetResourceVersion.y + "." + targetResourceVersion.z;
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixCode", targetOutPath + backupVersion + "/HotFix/HotFixCode");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixCodeConfig", targetOutPath + backupVersion + "/HotFix/HotFixCodeConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixView", targetOutPath + backupVersion + "/HotFix/HotFixView");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/HotFixViewConfig", targetOutPath + backupVersion + "/HotFix/HotFixViewConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/Metadata", targetOutPath + backupVersion + "/HotFix/Metadata");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFix/MetadataConfig", targetOutPath + backupVersion + "/HotFix/MetadataConfig");

                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/Assembly", targetOutPath + backupVersion + "/HotFixRuntime/Assembly");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/AssemblyConfig", targetOutPath + backupVersion + "/HotFixRuntime/AssemblyConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundle", targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig", targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle", targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundle");
                FileOperationComponent.Copy("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundleConfig", targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperationComponent.CopyFile("Assets/UnStreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + backupVersion + "/HotFixRuntime/HotFixServerResourcesCount.json");
            }

            #endregion

            OnLoadConfig();
            OpenScene(this.currentOpenSceneName);
            Debug.Log("资源打包完毕");
        }

        #endregion

        #region 打包HotFixView

        [LabelText("打包HotFixView")]
        private void HotFixViewBuild()
        {
            if (HotFixViewPrefab == null)
            {
                Debug.Log("配置信息错误");
                return;
            }

            if (!Directory.Exists("Assets/UnStreamingAssets/HotFix/HotFixView"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFix/HotFixView");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixViewPrefab));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/hotfixview";
            BuildPipeline.BuildAssetBundles("Assets/UnStreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            DataFrameComponent.RemoveAllAssetBundleName();
            AssetDatabase.Refresh();

            string filePath = "Assets/UnStreamingAssets/HotFix/HotFixView/" + "hotfixview";
            File.Delete(filePath + ".manifest");
            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "hotfixview"; //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixView/";
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFix/HotFixViewConfig/" + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            Debug.Log("HotFixView配置输出");

            OnLoadConfig();
        }

        #endregion

        #region HotFixCode

        private void HotFixCodeBuild()
        {
            //热更新打包
#if HybridCLR
            CompileDllCommand.CompileDllActiveBuildTarget();
#endif
            string platformName = EditorUserBuildSettings.activeBuildTarget.ToString();
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFix/HotFixCode"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFix/HotFixCode");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            File.Copy(DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/HotFixCode.dll",
                RuntimeGlobal.GetDeviceStoragePath() + "/HotFix/HotFixCode/" + "HotFixCode.dll.bytes", true);
            string path = "Assets/UnStreamingAssets/HotFix/HotFixCode/HotFixCode.dll.bytes";

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "HotFixCode.dll.bytes";
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(path);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(path).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixCode/";
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFix/HotFixCodeConfig/" + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            Debug.Log("HotFixCode配置输出");
            OnLoadConfig();
        }

        #endregion

        #region Assembly

        private void AssemblyBuild()
        {
#if HybridCLR
            CompileDllCommand.CompileDllActiveBuildTarget();
#endif
            string platformName = EditorUserBuildSettings.activeBuildTarget.ToString();
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFixRuntime/Assembly"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFixRuntime/Assembly");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            if (!Directory.Exists("Assets/UnStreamingAssets/HotFixRuntime/AssemblyConfig"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFixRuntime/AssemblyConfig");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            File.Copy(DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/Assembly-CSharp.dll",
                RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes", true);
            HotFixRuntimeDownConfig hotFixAssemblyConfig = new HotFixRuntimeDownConfig();
            hotFixAssemblyConfig.md5 = FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes");
            hotFixAssemblyConfig.name = "Assembly-CSharp.dll.bytes";
            hotFixAssemblyConfig.size = FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes").ToString();
            hotFixAssemblyConfig.path = "HotFixRuntime/Assembly/";
            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/AssemblyConfig", "AssemblyConfig.json", JsonMapper.ToJson(hotFixAssemblyConfig));
            Debug.Log("移动完毕");
        }

        #endregion

        #region MetaAssembly

        private void MetaAssemblyBuild()
        {
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFix/Metadata"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFix/Metadata");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            if (!Directory.Exists("Assets/UnStreamingAssets/HotFix/MetadataConfig"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFix/MetadataConfig");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }
#if HybridCLR
            //移动元文件
            foreach (string metadataName in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                string platformName = EditorUserBuildSettings.activeBuildTarget.ToString();
                File.Copy(DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/AssembliesPostIl2CppStrip/" + platformName + "/" + metadataName,
                    RuntimeGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/" + metadataName + ".bytes", true);
            }
#endif
            List<string> buildPath = DataFrameComponent.Path_GetGetSpecifyPathInAllType("Assets/UnStreamingAssets/HotFix/Metadata", "bytes");
            List<HotFixRuntimeDownConfig> hotFixMetaAssemblyConfigs = new List<HotFixRuntimeDownConfig>();
            foreach (string path in buildPath)
            {
                hotFixMetaAssemblyConfigs.Add(new HotFixRuntimeDownConfig()
                {
                    name = DataFrameComponent.Path_GetPathFileNameDontContainFileType(path) + ".bytes",
                    md5 = FileOperationComponent.GetMD5HashFromFile(path),
                    path = "HotFix/Metadata/",
                    size = FileOperationComponent.GetFileSize(path).ToString()
                });
            }


            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.GetDeviceStoragePath() + "/HotFix/MetadataConfig", "MetadataConfig.json", JsonMapper.ToJson(hotFixMetaAssemblyConfigs));
        }

        #endregion

        #region GameRootStart

        private void GameRootStartBuild()
        {
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundle"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFixRuntime/GameRootStartAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            if (GameRootStartPrefab == null)
            {
                Debug.LogError("GameRootStartPrefab为空");
                return;
            }

            AssetImporter gameRootStartImporter;
            gameRootStartImporter = AssetImporter.GetAtPath(GameRootStartPath);
            gameRootStartImporter.assetBundleName = "GameRootStartAssetBundle/GameRootStart";
            BuildPipeline.BuildAssetBundles("Assets/UnStreamingAssets/HotFixRuntime", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            //移除所有AssetBundle的名称
            DataFrameComponent.RemoveAllAssetBundleName();
            File.Delete("Assets/UnStreamingAssets/HotFixRuntime" + "/" + "GameRootStartAssetBundle/gamerootstart.manifest");
            AssetDatabase.Refresh();

            HotFixRuntimeDownConfig hotFixGameRootStartConfig = new HotFixRuntimeDownConfig();
            hotFixGameRootStartConfig.md5 = FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart");
            hotFixGameRootStartConfig.name = "gamerootstart";
            hotFixGameRootStartConfig.size = FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart").ToString();
            hotFixGameRootStartConfig.path = "HotFixRuntime/GameRootStartAssetBundle/";
            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/GameRootStartAssetBundleConfig", "GameRootStartConfig.json", JsonMapper.ToJson(hotFixGameRootStartConfig));
        }

        #endregion

        #region 拷贝场景打包

        private void CopySceneBuild()
        {
            for (int i = 0; i < CopySceneAssetBundleAsset.Count; i++)
            {
                OpenScene(CopySceneAssetBundleAsset[i].name.Replace("Copy", ""));
                // BundleSceneDataAndGenerateBuildConfig(CopySceneAssetBundleAsset[i]);
            }
        }

        [BoxGroup("打包")]
        [GUIColor(0, 1, 0)]
        public void BundleSceneDataAndGenerateBuildConfig(CopySceneAssetBundleAsset copySceneAssetBundleAsset)
        {
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            //查找场景中所有热更配置
            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            copySceneAssetBundleAsset.scenePrefabPaths.Clear();
            //应用预制体并记录路径
            for (int i = 0; i < hotFixAssetPathConfigs.Count; i++)
            {
                hotFixAssetPathConfigs[i].SetPathAndApplyPrefab();
                copySceneAssetBundleAsset.scenePrefabPaths.Add(hotFixAssetPathConfigs[i].prefabPath);
            }

            //移除当前场景中的HotFixAssetPathConfig
            foreach (HotFixAssetPathConfig hotFixAssetSceneHierarchyPath in hotFixAssetPathConfigs)
            {
                GameObject.DestroyImmediate(hotFixAssetSceneHierarchyPath.gameObject);
            }

            //删除manifest列表
            List<string> deleteManifestPath = new List<string>();
            //打包场景其他数据
            foreach (string scenePrefabPath in copySceneAssetBundleAsset.scenePrefabPaths)
            {
                //不管该Prefab是否参与打包,字体肯定要重新打包
                foreach (string assetDependency in GetAssetDependencies(scenePrefabPath))
                {
                    if (assetDependency.Contains("TMP_SDF.shader") || assetDependency.Contains(".otf") || assetDependency.Contains(".ttf") || assetDependency.Contains(".asset"))
                    {
                        AssetImporter fontAssetImporter = UnityEditor.AssetImporter.GetAtPath(assetDependency);
                        fontAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/Font/" + SceneManager.GetActiveScene().name + "Font";
                        deleteManifestPath.Add("Assets/UnStreamingAssets/" + fontAssetImporter.assetBundleName + ".manifest");
                    }
                }

                AssetImporter assetImporter = AssetImporter.GetAtPath(scenePrefabPath);
                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
                deleteManifestPath.Add("Assets/UnStreamingAssets/" + assetImporter.assetBundleName + ".manifest");
            }


            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.BuildPipeline.BuildAssetBundles("Assets/UnStreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            UnityEditor.AssetDatabase.Refresh();
            //删除manifest
            foreach (string path in deleteManifestPath)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            //显示场景物体
            ShowSceneObject(copySceneAssetBundleAsset.scenePrefabPaths);
            //生成场景配置表
            GenerateBuildConfig(copySceneAssetBundleAsset);
            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }
        }

        #endregion

        #region 普通场景打包

        //普通场景打包
        private void NormalSceneBuild()
        {
            for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
                //打开对应的场景文件
                OpenScene(NormalSceneAssetBundleAssets[i].name);
                BundleSceneDataAndGenerateBuildConfig(NormalSceneAssetBundleAssets[i]);
            }
        }

        [BoxGroup("打包")]
        [GUIColor(0, 1, 0)]
        public void BundleSceneDataAndGenerateBuildConfig(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            //需要打包的数量
            if (!Directory.Exists("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle"))
            {
                Directory.CreateDirectory("Assets/UnStreamingAssets/HotFixRuntime/HotFixAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            scenePrefabAssetPath.Clear();
            //视图重新排序并应用
            FrameMenu.ViewSortSiblingIndex();
            //场景中所有热更配置
            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            normalSceneAssetBundleAsset.scenePrefabPaths.Clear();
            //应用热更配置并记录路径
            for (int i = 0; i < hotFixAssetPathConfigs.Count; i++)
            {
                hotFixAssetPathConfigs[i].SetPathAndApplyPrefab();
                normalSceneAssetBundleAsset.scenePrefabPaths.Add(hotFixAssetPathConfigs[i].prefabPath);
            }

            //移除当前场景中的HotFixAssetPathConfig
            foreach (HotFixAssetPathConfig hotFixAssetSceneHierarchyPath in hotFixAssetPathConfigs)
            {
                GameObject.DestroyImmediate(hotFixAssetSceneHierarchyPath.gameObject);
            }

            //保存场景
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            //删除manifest列表
            List<string> deleteManifestPath = new List<string>();
            //打包场景其他数据
            foreach (string scenePrefabPath in normalSceneAssetBundleAsset.scenePrefabPaths)
            {
                //当前预制体Md5
                string scenePrefabMd5 = FileOperationComponent.GetMD5HashFromFile(scenePrefabPath);

                //旧的场景Md5
                string oldScenePrefabMd5 = string.Empty;

                if (normalSceneAssetBundleAsset.scenePrefabMd5.ContainsKey(scenePrefabPath))
                {
                    oldScenePrefabMd5 = normalSceneAssetBundleAsset.scenePrefabMd5[scenePrefabPath];
                }

                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                //当前预制体的Md5和旧的Md5不一样,或者AB包不存在,重新打包
                if (scenePrefabMd5 != oldScenePrefabMd5 || !File.Exists("Assets/UnStreamingAssets/" + hotFixAssetPathConfig.assetBundlePath))
                {
                    Debug.Log(scenePrefabPath + "重新打包");
                    //只有当场景预制体发生变化时,才会重新打包
                    AssetImporter assetImporter = AssetImporter.GetAtPath(scenePrefabPath);
                    assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
                    scenePrefabAssetPath.Add(hotFixAssetPathConfig.assetBundlePath, GetAssetDependencies(scenePrefabPath));
                    deleteManifestPath.Add("Assets/UnStreamingAssets/" + assetImporter.assetBundleName + ".manifest");
                }
                else
                {
                    // Debug.Log(scenePrefabPath + "不参与打包");
                }

                //AB包所有依赖
                List<string> scenePrefabDependencies = new List<string>(GetAssetDependencies(scenePrefabPath));
                //AB包所有依赖中的重复资源
                foreach (string scenePrefabDependency in scenePrefabDependencies)
                {
                    //是否是重复利用资源
                    string repeatPath = GetRepeatPath(scenePrefabDependency, normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets);
                    if (repeatPath != string.Empty)
                    {
                        //添加到重复资源列表
                        AddRepeatAsset(repeatPath, scenePrefabDependency);
                    }
                }
            }


            #region 场景

            string sceneMd5 = FileOperationComponent.GetMD5HashFromFile(SceneManager.GetActiveScene().path);
            string oldSceneMd5 = normalSceneAssetBundleAsset.sceneMd5;
            string sceneAssetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/scene/" + SceneManager.GetActiveScene().name;
            if (sceneMd5 != oldSceneMd5 || !File.Exists("Assets/UnStreamingAssets/" + sceneAssetBundleName))
            {
                Debug.Log(SceneManager.GetActiveScene().name + "重新打包");
                //打包当前场景
                AssetImporter sceneAssetImporter = AssetImporter.GetAtPath(SceneManager.GetActiveScene().path);
                sceneAssetImporter.assetBundleName = sceneAssetBundleName;
                deleteManifestPath.Add("Assets/UnStreamingAssets/" + sceneAssetBundleName + ".manifest");
            }

            //所有场景的资源
            sceneAllBuildAssetPath = new List<string>(GetAssetDependencies(SceneManager.GetActiveScene().path));
            for (int i = 0; i < sceneAllBuildAssetPath.Count; i++)
            {
                string repeatPath = GetRepeatPath(sceneAllBuildAssetPath[i], normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets);
                if (repeatPath != string.Empty)
                {
                    AddRepeatAsset(repeatPath, sceneAllBuildAssetPath[i]);
                }
            }

            #endregion

            #region 重复资源

            //打包重复资源

            foreach (SceneRepeatAssets sceneRepeatAsset in sceneRepeatAssets)
            {
                //相同
                bool isSame = true;
                //没有包含当前重复资源
                if (normalSceneAssetBundleAsset.sceneRepeatMd5.ContainsKey(sceneRepeatAsset.assetName))
                {
                    List<string> oldRepeatMd5List = normalSceneAssetBundleAsset.sceneRepeatMd5[sceneRepeatAsset.assetName];
                    if (oldRepeatMd5List.Count == sceneRepeatAsset.repeatAssets.Count)
                    {
                        foreach (string repeatAsset in sceneRepeatAsset.repeatAssets)
                        {
                            if (!oldRepeatMd5List.Contains(FileOperationComponent.GetMD5HashFromFile(repeatAsset)))
                            {
                                isSame = false;
                            }
                        }
                    }
                    else
                    {
                        isSame = false;
                    }
                }
                else
                {
                    isSame = false;
                }

                //需要打包的资源包含了当前重复资源
                foreach (KeyValuePair<string, List<string>> pair in scenePrefabAssetPath)
                {
                    foreach (string s in pair.Value)
                    {
                        if (sceneRepeatAsset.repeatAssets.Contains(s))
                        {
                            isSame = false;
                        }
                    }
                }

                //文件是否存在
                string repeatAssetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/repeat/" + sceneRepeatAsset.assetName;
                if (!File.Exists("Assets/UnStreamingAssets/" + repeatAssetBundleName))
                {
                    isSame = false;
                }

                //资源相同跳过打包
                if (isSame)
                {
                    continue;
                }

                Debug.Log(sceneRepeatAsset.assetName + "重新打包");
                foreach (string path in sceneRepeatAsset.repeatAssets)
                {
                    AssetImporter assetImporter = AssetImporter.GetAtPath(path);
                    assetImporter.assetBundleName = repeatAssetBundleName;
                    deleteManifestPath.Add("Assets/UnStreamingAssets/" + assetImporter.assetBundleName + ".manifest");
                }
            }

            #endregion

            AssetDatabase.Refresh();
            UnityEditor.BuildPipeline.BuildAssetBundles("Assets/UnStreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            //删除manifest
            foreach (string path in deleteManifestPath)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }


            //生成场景配置表
            GenerateBuildConfig(normalSceneAssetBundleAsset);
            //显示场景物体
            ShowSceneObject(normalSceneAssetBundleAsset.scenePrefabPaths);
            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }
        }

        #endregion

        #region 下载路径

        //是否包含下载路径
        private bool IsContainHotFixDownPathData()
        {
            foreach (PlatformPath platformPath in hotFixDownPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    return true;
                }
            }

            return false;
        }

        //更新下载路径
        private void UpdateHotFixDownPathData(string path)
        {
            foreach (PlatformPath platformPath in hotFixDownPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    platformPath.path = path;
                    return;
                }
            }
        }

        //添加下载路径
        private void AddHotFixDownPathData(string path)
        {
            PlatformPath platformPath = new PlatformPath();
            platformPath.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            platformPath.path = path;
            hotFixDownPathData.Add(platformPath);
        }

        //获得下载路径
        private string GetHotFixDownPathData()
        {
            foreach (PlatformPath platformPath in hotFixDownPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    return platformPath.path;
                }
            }

            return string.Empty;
        }

        #endregion

        #region 输出路径

        //输出路径是否包含
        private bool IsContainTargetOutPathData()
        {
            foreach (PlatformPath platformPath in targetOutPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    return true;
                }
            }

            return false;
        }

        //更新输出路径
        private void UpdateTargetOutPathData(string path)
        {
            foreach (PlatformPath platformPath in targetOutPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    platformPath.path = path;
                    return;
                }
            }
        }

        //添加目标输出路径
        private void AddTargetOutPathData(string path)
        {
            PlatformPath platformPath = new PlatformPath();
            platformPath.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            platformPath.path = path;
            targetOutPathData.Add(platformPath);
        }

        //获得目标输出路径
        private string GetTargetOutPathData()
        {
            foreach (PlatformPath platformPath in targetOutPathData)
            {
                if (platformPath.buildTarget == EditorUserBuildSettings.activeBuildTarget)
                {
                    return platformPath.path;
                }
            }

            return string.Empty;
        }

        #endregion

        #region 重复资源

        public bool IsRepeatAssetType(string assetName)
        {
            foreach (SceneRepeatAssets sceneRepeatAsset in sceneRepeatAssets)
            {
                if (sceneRepeatAsset.assetName == assetName)
                {
                    return true;
                }
            }

            return false;
        }

        public SceneRepeatAssets GetRepeatAsset(string assetName)
        {
            foreach (SceneRepeatAssets sceneRepeatAsset in sceneRepeatAssets)
            {
                if (sceneRepeatAsset.assetName == assetName)
                {
                    return sceneRepeatAsset;
                }
            }

            return null;
        }

        public void AddRepeatAsset(string assetName, List<string> assetPath)
        {
            foreach (string path in assetPath)
            {
                AddRepeatAsset(assetName, path);
            }
        }

        public void AddRepeatAsset(string assetName, string assetPath)
        {
            SceneRepeatAssets sceneRepeatAsset = GetRepeatAsset(assetName);
            if (sceneRepeatAsset == null)
            {
                sceneRepeatAsset = new SceneRepeatAssets();
                sceneRepeatAsset.assetName = assetName;
                sceneRepeatAssets.Add(sceneRepeatAsset);
            }

            if (!sceneRepeatAsset.repeatAssets.Contains(assetPath))
            {
                sceneRepeatAsset.repeatAssets.Add(assetPath);
            }
        }

        [LabelText("是否是重复资源路径")]
        private string GetRepeatPath(string path, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets)
        {
            foreach (SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset in sceneAssetBundleRepeatAssets)
            {
                foreach (string ContainPath in sceneAssetBundleRepeatAsset.assetBundleContainPath)
                {
                    if (path.Contains(ContainPath))
                    {
                        return sceneAssetBundleRepeatAsset.assetBundleName;
                    }
                }
            }

            return string.Empty;
        }

        #endregion

        #region 获得资源路径的所有资源依赖

        [LabelText("获得资源路径的所有资源依赖")]
        private static List<string> GetAssetDependencies(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                return null;
            }

            List<string> dependecies = new List<string>(UnityEditor.AssetDatabase.GetDependencies(assetPath));
            List<string> csDependecies = new List<string>();

            for (int i = 0; i < csDependecies.Count; i++)
            {
                dependecies.Remove(csDependecies[i]);
            }

            return dependecies;
        }

        #endregion

        #region 打开场景

        //打开场景
        private void OpenScene(string sceneName)
        {
            List<string> allScenePath = DataFrameComponent.Path_GetSpecifyTypeOnlyInAssets("unity");
            for (int i = 0; i < allScenePath.Count; i++)
            {
                if (DataFrameComponent.Path_GetPathFileNameDontContainFileType(allScenePath[i]) == sceneName)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(allScenePath[i]);
                }
            }
        }

        #endregion

        #region 显示场景资源

        [LabelText("显示场景资源")]
        private void ShowSceneObject(List<string> scenePrefabPaths)
        {
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

        #endregion

        #region 视图重新排序

        private void ViewSort()
        {
            List<BaseWindow> sceneAllBaseWindow = DataFrameComponent.Hierarchy_GetAllObjectsInScene<BaseWindow>();
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

        #endregion

        #region 生成配置信息

        private void GenerateBuildConfig(CopySceneAssetBundleAsset copySceneAssetBundleAsset)
        {
            string sceneName = DataFrameComponent.String_AllCharToLower(SceneManager.GetActiveScene().name);
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //场景AssetBundle
            foreach (string scenePrefabPath in copySceneAssetBundleAsset.scenePrefabPaths)
            {
                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                HotFixRuntimeAssetBundleConfig hot = new HotFixRuntimeAssetBundleConfig();
                hot.assetBundleName = DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name);
                hot.assetBundlePath = hotFixAssetPathConfig.assetBundlePath.Replace(DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name), "");
                hot.assetBundleInstantiatePath = hotFixAssetPathConfig.GetHierarchyGeneratePath();
                string adPath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hot.assetBundlePath + hot.assetBundleName;
                hot.assetBundleSize = FileOperationComponent.GetFileSize(adPath).ToString();
                hot.md5 = FileOperationComponent.GetMD5HashFromFile(adPath);
                hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hot);
            }

            string hotFixAssetBundleConfigPath = RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + DataFrameComponent.String_AllCharToLower(SceneManager.GetActiveScene().name) + ".json";
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                File.Delete(hotFixAssetBundleConfigPath);
            }

            FileOperationComponent.SaveTextToLoad(hotFixAssetBundleConfigPath, JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
            Debug.Log("打包 配置信息完成");
        }

        [LabelText("生成配置信息")]
        private void GenerateBuildConfig(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            string sceneName = DataFrameComponent.String_AllCharToLower(SceneManager.GetActiveScene().name);
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //场景数据
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleName = sceneName;
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/";
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleSize =
                FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName).ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.md5 =
                FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName);
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleInstantiatePath = "";
            //场景旧的Md5
            normalSceneAssetBundleAsset.sceneMd5 = FileOperationComponent.GetMD5HashFromFile(SceneManager.GetActiveScene().path);

            //场景重复资源Md5
            normalSceneAssetBundleAsset.sceneRepeatMd5.Clear();
            foreach (SceneRepeatAssets sceneRepeatAsset in sceneRepeatAssets)
            {
                normalSceneAssetBundleAsset.sceneRepeatMd5.Add(sceneRepeatAsset.assetName, new List<string>());
                foreach (string repeatAsset in sceneRepeatAsset.repeatAssets)
                {
                    normalSceneAssetBundleAsset.sceneRepeatMd5[sceneRepeatAsset.assetName].Add(FileOperationComponent.GetMD5HashFromFile(repeatAsset));
                }
            }

            //重复资源
            foreach (SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset in normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets)
            {
                HotFixRuntimeAssetBundleConfig hotFixRuntimeAssetBundleConfig = new HotFixRuntimeAssetBundleConfig();
                hotFixRuntimeAssetBundleConfig.assetBundleName = sceneAssetBundleRepeatAsset.assetBundleName;
                hotFixRuntimeAssetBundleConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/repeat/";
                hotFixRuntimeAssetBundleConfig.assetBundleSize =
                    FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/repeat/" + sceneAssetBundleRepeatAsset.assetBundleName).ToString();
                hotFixRuntimeAssetBundleConfig.md5 =
                    FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/repeat/" + sceneAssetBundleRepeatAsset.assetBundleName);
                hotFixRuntimeAssetBundleConfig.assetBundleInstantiatePath = "";
                hotFixRuntimeSceneAssetBundleConfig.repeatSceneFixRuntimeAssetConfig.Add(hotFixRuntimeAssetBundleConfig);
            }

            normalSceneAssetBundleAsset.scenePrefabMd5.Clear();
            //场景AssetBundle旧的Md5
            foreach (string scenePrefabPath in normalSceneAssetBundleAsset.scenePrefabPaths)
            {
                normalSceneAssetBundleAsset.scenePrefabMd5.Add(scenePrefabPath, FileOperationComponent.GetMD5HashFromFile(scenePrefabPath));
            }

            //场景AssetBundle
            foreach (string scenePrefabPath in normalSceneAssetBundleAsset.scenePrefabPaths)
            {
                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                HotFixRuntimeAssetBundleConfig hot = new HotFixRuntimeAssetBundleConfig();
                hot.assetBundleName = DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name);
                hot.assetBundlePath = hotFixAssetPathConfig.assetBundlePath.Replace(DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name), "");
                hot.assetBundleInstantiatePath = hotFixAssetPathConfig.GetHierarchyGeneratePath();
                string adPath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hot.assetBundlePath + hot.assetBundleName;
                hot.assetBundleSize = FileOperationComponent.GetFileSize(adPath).ToString();
                hot.md5 = FileOperationComponent.GetMD5HashFromFile(adPath);
                hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hot);
            }

            string hotFixAssetBundleConfigPath = RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + DataFrameComponent.String_AllCharToLower(SceneManager.GetActiveScene().name) + ".json";
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                File.Delete(hotFixAssetBundleConfigPath);
            }

            FileOperationComponent.SaveTextToLoad(hotFixAssetBundleConfigPath, JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
        }

        #endregion

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
            currentOpenSceneName = SceneManager.GetActiveScene().name;

            if (IsContainHotFixDownPathData())
            {
                UpdateHotFixDownPathData(hotFixDownPath);
            }
            else
            {
                AddHotFixDownPathData(hotFixDownPath);
            }

            if (IsContainTargetOutPathData())
            {
                UpdateTargetOutPathData(targetOutPath);
            }
            else
            {
                AddTargetOutPathData(targetOutPath);
            }


            HotFixViewPrePath = AssetDatabase.GetAssetPath(HotFixViewPrefab);
            GameRootStartPath = AssetDatabase.GetAssetPath(GameRootStartPrefab);
            CopySceneAssetBundleAssetPath.Clear();
            foreach (CopySceneAssetBundleAsset copySceneAssetBundleAsset in CopySceneAssetBundleAsset)
            {
                CopySceneAssetBundleAssetPath.Add(AssetDatabase.GetAssetPath(copySceneAssetBundleAsset));
            }

            NormalSceneAssetBundleAssetsPath.Clear();
            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
            {
                NormalSceneAssetBundleAssetsPath.Add(AssetDatabase.GetAssetPath(normalSceneAssetBundleAsset));
            }

            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json", JsonUtility.ToJson(this));
        }

        public override void OnLoadConfig()
        {
            if (!File.Exists(RuntimeGlobal.assetRootPath + "HotFixCollect.json"))
            {
                return;
            }

            HotFixCollect hotFixCollect = JsonUtil.FromJson<HotFixCollect>(FileOperationComponent.GetTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json"));
            this.localIsUpdate = hotFixCollect.localIsUpdate;
            this.hotFixDownPathData = hotFixCollect.hotFixDownPathData;
            if (IsContainHotFixDownPathData())
            {
                this.hotFixDownPath = GetHotFixDownPathData();
            }

            this.targetOutPathData = hotFixCollect.targetOutPathData;
            this.targetResourceVersion = hotFixCollect.targetResourceVersion;
            this.targetBuildAssetBundleOptions = hotFixCollect.targetBuildAssetBundleOptions;
            if (IsContainTargetOutPathData())
            {
                this.targetOutPath = GetTargetOutPathData();
            }

            this.targetResourceVersion = hotFixCollect.targetResourceVersion;
            this.isUpdateVersion = hotFixCollect.isUpdateVersion;
            this.buildHotFixView = hotFixCollect.buildHotFixView;
            this.HotFixViewPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollect.HotFixViewPrePath);
            this.HotFixViewPrePath = hotFixCollect.HotFixViewPrePath;
            this.buildHotFixCode = hotFixCollect.buildHotFixCode;
            this.buildAssemblyParticipatePackaging = hotFixCollect.buildAssemblyParticipatePackaging;
            this.buildMetaAssemblyParticipatePackaging = hotFixCollect.buildMetaAssemblyParticipatePackaging;
            this.buildGameRootStart = hotFixCollect.buildGameRootStart;
            this.GameRootStartPath = hotFixCollect.GameRootStartPath;
            this.GameRootStartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollect.GameRootStartPath);
            this.BuildScene = hotFixCollect.BuildScene;
            this.currentOpenSceneName = hotFixCollect.currentOpenSceneName;
            CopySceneAssetBundleAsset.Clear();
            NormalSceneAssetBundleAssets.Clear();
            for (int i = 0; i < hotFixCollect.CopySceneAssetBundleAssetPath.Count; i++)
            {
                CopySceneAssetBundleAsset.Add(AssetDatabase.LoadAssetAtPath<CopySceneAssetBundleAsset>(hotFixCollect.CopySceneAssetBundleAssetPath[i]));
            }

            for (int i = 0; i < hotFixCollect.NormalSceneAssetBundleAssetsPath.Count; i++)
            {
                NormalSceneAssetBundleAssets.Add(AssetDatabase.LoadAssetAtPath<NormalSceneAssetBundleAsset>(hotFixCollect.NormalSceneAssetBundleAssetsPath[i]));
            }
        }

        public override void OnInit()
        {
        }
    }
}