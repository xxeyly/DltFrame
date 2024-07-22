using System;
using System.Collections.Generic;
using System.IO;
using Aot;
using Cysharp.Threading.Tasks;
using HotFix;
#if HybridCLR
using HybridCLR.Editor.Commands;
#endif
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
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
    public class SceneRepeatAsset
    {
        public string assetName;
        public List<string> repeatAssets = new List<string>();
    }

    [Serializable]
    [LabelText("场景打开状态")]
    public class SceneBuildState
    {
        [HideLabel] [HorizontalGroup("场景名称")] public string sceneName;

        [HideLabel] [HorizontalGroup("打包状态")] public bool isBuild;
    }

    public class HotFixCollect : BaseEditor
    {
        #region 属性

        [LabelText("打包压缩方式")] [LabelWidth(120)] [EnumPaging] [OnValueChanged("OnSaveConfig")]
        public BuildAssetBundleOptions targetBuildAssetBundleOptions;

        [LabelText("平台")] private string platformName;

        [LabelText("更新数据下载地址")] [LabelWidth(120)] [ShowInInspector] [OnValueChanged("OnSaveConfig")]
        private string hotFixDownPath;

        [LabelText("平台下载地址")] [HideInInspector]
        public List<PlatformPath> hotFixDownPathData = new List<PlatformPath>();

        [LabelText("本地是否开启更新")] [OnValueChanged("OnSaveConfig")]
        public bool localIsUpdate;

        [OnValueChanged("OnSaveConfig")] [InfoBox("打包后会拷贝所有资源到当前路径下,并且还会复制一份以当前版本号的为名的备份文件夹")] [LabelText("资源外部拷贝路径")] [LabelWidth(120)] [FolderPath(AbsolutePath = true)] [ShowInInspector] [OnValueChanged("OnSaveConfig")]
        private string targetOutPath;

        [LabelText("平台拷贝地址")] [HideInInspector]
        public List<PlatformPath> targetOutPathData = new List<PlatformPath>();

        [InfoBox("当勾选版本号自增,会默认勾选所有资源,并且资源版本号+1")] [LabelText("版本号自增")] [LabelWidth(120)] [OnValueChanged("OnSaveConfig")]
        public bool isUpdateVersion;

        [InfoBox("版本号分3个等级,\n等级1是特大改动(需要手动更改),\n等级2每次需要重新出包时变动(需要手动更改),\n等级3是每次小更新时变动(勾选版本自增每次打包会自增1)")] [LabelText("当前资源版本号")] [LabelWidth(120)] [OnValueChanged("OnSaveConfig")]
        public Vector3 targetResourceVersion;

        [LabelText("打包后移除AssetBundle信息")] [OnValueChanged("OnSaveConfig")]
        public bool removeAssetBundleName;

        #region 打包路径

        private string UnStreamingAssetsPath;
        private string HotFixPath;
        private string HotFixCodePath;
        private string HotFixCodeConfigPath;
        private string HotFixViewPath;
        private string HotFixViewConfigPath;
        private string MetadataPath;
        private string MetadataConfigPath;

        private string HotFixRuntimePath;
        private string AssemblyPath;
        private string AssemblyConfigPath;
        private string GameRootStartAssetBundlePath;
        private string GameRootStartAssetBundleConfigPath;
        private string HotFixAssetBundlePath;
        private string HotFixAssetBundleConfigPath;

        #endregion

        public override void OnInit()
        {
            UnStreamingAssetsPath = "Assets/UnStreamingAssets/";
            if (!Directory.Exists(UnStreamingAssetsPath))
            {
                Directory.CreateDirectory(UnStreamingAssetsPath);
            }

            #region HotFix Directory

            HotFixPath = UnStreamingAssetsPath + "HotFix/";
            if (!Directory.Exists(UnStreamingAssetsPath + HotFixPath))
            {
                Directory.CreateDirectory(HotFixPath);
            }

            HotFixCodePath = HotFixPath + "HotFixCode/";
            if (!Directory.Exists(HotFixCodePath))
            {
                Directory.CreateDirectory(HotFixCodePath);
            }

            HotFixCodeConfigPath = HotFixPath + "HotFixCodeConfig/";
            if (!Directory.Exists(HotFixCodeConfigPath))
            {
                Directory.CreateDirectory(HotFixCodeConfigPath);
            }

            HotFixViewPath = HotFixPath + "HotFixView/";
            if (!Directory.Exists(HotFixViewPath))
            {
                Directory.CreateDirectory(HotFixViewPath);
            }

            HotFixViewConfigPath = HotFixPath + "HotFixViewConfig/";
            if (!Directory.Exists(HotFixViewConfigPath))
            {
                Directory.CreateDirectory(HotFixViewConfigPath);
            }

            MetadataPath = HotFixPath + "Metadata/";
            if (!Directory.Exists(MetadataPath))
            {
                Directory.CreateDirectory(MetadataPath);
            }

            MetadataConfigPath = HotFixPath + "MetadataConfig/";
            if (!Directory.Exists(MetadataConfigPath))
            {
                Directory.CreateDirectory(MetadataConfigPath);
            }

            #endregion

            #region HotFixRuntime Directory

            HotFixRuntimePath = UnStreamingAssetsPath + "HotFixRuntime/";
            if (!Directory.Exists(HotFixRuntimePath))
            {
                Directory.CreateDirectory(HotFixRuntimePath);
            }

            AssemblyPath = HotFixRuntimePath + "Assembly/";
            if (!Directory.Exists(AssemblyPath))
            {
                Directory.CreateDirectory(AssemblyPath);
            }

            AssemblyConfigPath = HotFixRuntimePath + "AssemblyConfig/";
            if (!Directory.Exists(AssemblyConfigPath))
            {
                Directory.CreateDirectory(AssemblyConfigPath);
            }

            GameRootStartAssetBundlePath = HotFixRuntimePath + "GameRootStartAssetBundle/";
            if (!Directory.Exists(GameRootStartAssetBundlePath))
            {
                Directory.CreateDirectory(GameRootStartAssetBundlePath);
            }

            GameRootStartAssetBundleConfigPath = HotFixRuntimePath + "GameRootStartAssetBundleConfig/";
            if (!Directory.Exists(GameRootStartAssetBundleConfigPath))
            {
                Directory.CreateDirectory(GameRootStartAssetBundleConfigPath);
            }

            HotFixAssetBundlePath = HotFixRuntimePath + "HotFixAssetBundle/";
            if (!Directory.Exists(HotFixAssetBundlePath))
            {
                Directory.CreateDirectory(HotFixAssetBundlePath);
            }

            HotFixAssetBundleConfigPath = HotFixRuntimePath + "HotFixAssetBundleConfig/";
            if (!Directory.Exists(HotFixAssetBundleConfigPath))
            {
                Directory.CreateDirectory(HotFixAssetBundleConfigPath);
            }

            #endregion

            AssetDatabase.Refresh();

            platformName = EditorUserBuildSettings.activeBuildTarget.ToString();

            Debug.Log("初始化");
        }

        #region HotFixView

        [ToggleGroup("buildHotFixView", "HotFixView")] [OnValueChanged("OnSaveConfig")]
        public bool buildHotFixView;

        [ToggleGroup("buildHotFixView")] [LabelText("HotFixView预制体")] [AssetSelector] [OnValueChanged("OnSaveConfig")]
        public GameObject HotFixViewPrefab;

        [ToggleGroup("buildHotFixView")] [LabelText("HotFixView预制体路径")] [HideInInspector] [OnValueChanged("OnSaveConfig")]
        public string HotFixViewPrePath;

        #endregion

        #region HotFixCode

        [ToggleGroup("buildHotFixCode", "HotFixCode")] [OnValueChanged("OnSaveConfig")]
        public bool buildHotFixCode;

        #endregion

        #region 元数据

        [ToggleGroup("buildMetaAssemblyParticipatePackaging", "MetaAssembly")] [OnValueChanged("OnSaveConfig")]
        public bool buildMetaAssemblyParticipatePackaging;

        [ToggleGroup("buildAssemblyParticipatePackaging", "Assembly")] [OnValueChanged("OnSaveConfig")]
        public bool buildAssemblyParticipatePackaging;

        #endregion

        #region GameRootStart

        [ToggleGroup("buildGameRootStart", "GameRootStart")] [OnValueChanged("OnSaveConfig")]
        public bool buildGameRootStart;

        [ToggleGroup("buildGameRootStart")] [LabelText("GameRootStart预制体")] [AssetSelector] [OnValueChanged("OnSaveConfig")]
        public GameObject GameRootStartPrefab;

        [ToggleGroup("buildGameRootStart")] [LabelText("GameRootStart预制体路径")] [Sirenix.OdinInspector.FilePath] [HideInInspector]
        public string GameRootStartPath;

        #endregion

        #region 场景打包

        [ToggleGroup("SceneBuildSwitch", "场景打包")] [OnValueChanged("OnSaveConfig")] [SerializeField]
        public bool SceneBuildSwitch;

        [ToggleGroup("SceneBuildSwitch")] [LabelText("正常场景配置")] [AssetList] [InlineEditor()] [OnValueChanged("OnSaveConfig")]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssets = new List<NormalSceneAssetBundleAsset>();

        [ToggleGroup("SceneBuildSwitch")] [LabelText("正常场景生成表")] [AssetList] [InlineEditor()] [OnValueChanged("OnSaveConfig")]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssetConfig = new List<NormalSceneAssetBundleAsset>();

        [ToggleGroup("SceneBuildSwitch")] [LabelText("场景打开状态")]
        private List<string> sceneOpenState = new List<string>();

        [ToggleGroup("SceneBuildSwitch")] [LabelText("场景打包状态")] [TableList]
        public List<SceneBuildState> SceneBuildStates = new List<SceneBuildState>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAsset")]
        public List<string> NormalSceneAssetBundleAssetsPath = new List<string>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAssetConfig")]
        public List<string> NormalSceneAssetBundleAssetConfigsPath = new List<string>();

        [ToggleGroup("SceneBuildSwitch")] [LabelText("当前打开场景名称")]
        private string currentOpenSceneName;

        [ToggleGroup("SceneBuild")] [LabelText("重复利用资源")] [Tooltip("每个场景可能都不一样")]
        private List<SceneRepeatAsset> sceneRepeatAssets = new List<SceneRepeatAsset>();

        #endregion

        #endregion

        #region 打包入口

        [GUIColor(0, 1, 0)]
        [Button("打包", ButtonSizes.Large)]
        [LabelText("图集列表")]
        public async void StartBuild()
        {
            await OnBuild();
        }


        private async UniTask OnBuild()
        {
            currentOpenSceneName = SceneManager.GetActiveScene().name;
            sceneRepeatAssets.Clear();
            sceneOpenState.Clear();
            SceneBuildStates.Clear();

            #region 全局打包

            if (isUpdateVersion)
            {
                targetResourceVersion += new Vector3(0, 0, 1);
            }

            #endregion

            #region 局部打包

            else
            {
                DataFrameComponent.RemoveAllAssetBundleName();

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

                if (SceneBuildSwitch)
                {
                    for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
                    {
                        SceneBuildStates.Add(new SceneBuildState()
                        {
                            sceneName = NormalSceneAssetBundleAssets[i].name,
                            isBuild = false
                        });
                    }

                    //这里没有UniTask是因为发现,打包场景的AssetBundle的时候,会执行不下去
                    for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
                    {
                        Debug.Log("打包场景:" + NormalSceneAssetBundleAssets[i].name);
                        NormalSceneBuild(NormalSceneAssetBundleAssets[i]);
                    }

                    sceneOpenState.Clear();

                    for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
                    {
                        await GenerateBuildConfig(NormalSceneAssetBundleAssets[i]);
                    }

                    Debug.Log("生成配置信息结束");
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
            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssetConfig)
            {
                hotFixServerResources.Add(normalSceneAssetBundleAsset.name);
            }

            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/HotFixRuntime/", "HotFixServerResourcesCount.json", JsonMapper.ToJson(hotFixServerResources));

            AssetDatabase.Refresh();
            if (Directory.Exists(targetOutPath))
            {
                FileOperationComponent.Copy(HotFixCodePath, targetOutPath + "/HotFix/HotFixCode");
                FileOperationComponent.Copy(HotFixCodeConfigPath, targetOutPath + "/HotFix/HotFixCodeConfig");
                FileOperationComponent.Copy(HotFixViewPath, targetOutPath + "/HotFix/HotFixView");
                FileOperationComponent.Copy(HotFixViewConfigPath, targetOutPath + "/HotFix/HotFixViewConfig");
                FileOperationComponent.Copy(MetadataPath, targetOutPath + "/HotFix/Metadata");
                FileOperationComponent.Copy(MetadataConfigPath, targetOutPath + "/HotFix/MetadataConfig");

                FileOperationComponent.Copy(AssemblyPath, targetOutPath + "/HotFixRuntime/Assembly");
                FileOperationComponent.Copy(AssemblyConfigPath, targetOutPath + "/HotFixRuntime/AssemblyConfig");
                FileOperationComponent.Copy(GameRootStartAssetBundlePath, targetOutPath + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperationComponent.Copy(GameRootStartAssetBundleConfigPath, targetOutPath + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperationComponent.Copy(HotFixAssetBundlePath, targetOutPath + "/HotFixRuntime/HotFixAssetBundle");
                FileOperationComponent.Copy(HotFixAssetBundleConfigPath, targetOutPath + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperationComponent.CopyFile("Assets/UnStreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + "/HotFixRuntime/HotFixServerResourcesCount.json");

                //额外数据
                foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
                {
                    foreach (string sceneExceptionAssetPath in normalSceneAssetBundleAsset.sceneExceptionAsset)
                    {
                        FileOperationComponent.CopyFile(sceneExceptionAssetPath, targetOutPath + "/" + sceneExceptionAssetPath.Replace("Assets/UnStreamingAssets/", ""));
                    }
                }

                //备份
                string backupVersion = "/Backups_" + targetResourceVersion.x + "." + targetResourceVersion.y + "." + targetResourceVersion.z;
                FileOperationComponent.Copy(HotFixCodePath, targetOutPath + backupVersion + "/HotFix/HotFixCode");
                FileOperationComponent.Copy(HotFixCodeConfigPath, targetOutPath + backupVersion + "/HotFix/HotFixCodeConfig");
                FileOperationComponent.Copy(HotFixViewPath, targetOutPath + backupVersion + "/HotFix/HotFixView");
                FileOperationComponent.Copy(HotFixViewConfigPath, targetOutPath + backupVersion + "/HotFix/HotFixViewConfig");
                FileOperationComponent.Copy(MetadataPath, targetOutPath + backupVersion + "/HotFix/Metadata");
                FileOperationComponent.Copy(MetadataConfigPath, targetOutPath + backupVersion + "/HotFix/MetadataConfig");

                FileOperationComponent.Copy(AssemblyPath, targetOutPath + backupVersion + "/HotFixRuntime/Assembly");
                FileOperationComponent.Copy(AssemblyConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/AssemblyConfig");
                FileOperationComponent.Copy(GameRootStartAssetBundlePath, targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperationComponent.Copy(GameRootStartAssetBundleConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperationComponent.Copy(HotFixAssetBundlePath, targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundle");
                FileOperationComponent.Copy(HotFixAssetBundleConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperationComponent.CopyFile("Assets/UnStreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + backupVersion + "/HotFixRuntime/HotFixServerResourcesCount.json");

                //额外数据
                foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
                {
                    foreach (string sceneExceptionAssetPath in normalSceneAssetBundleAsset.sceneExceptionAsset)
                    {
                        FileOperationComponent.CopyFile(sceneExceptionAssetPath, targetOutPath + backupVersion + "/" + sceneExceptionAssetPath.Replace("Assets/UnStreamingAssets/", ""));
                    }
                }
            }

            #endregion

            OnLoadConfig();
            sceneOpenState.Clear();
            Debug.Log("资源打包完毕");
            OpenScene(this.currentOpenSceneName);
        }

        #endregion

        #region 打包HotFixView

        [LabelText("打包HotFixView")]
        private void HotFixViewBuild()
        {
            if (HotFixViewPrefab == null)
            {
                Debug.Log("配置信息错误");
            }

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixViewPrefab));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/" + DataFrameComponent.String_AllCharToLower("HotFixView");
            BuildPipeline.BuildAssetBundles(UnStreamingAssetsPath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }

            string filePath = HotFixViewPath + DataFrameComponent.String_AllCharToLower("HotFixView");
            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = DataFrameComponent.String_AllCharToLower("HotFixView"); //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixView/";
            FileOperationComponent.SaveTextToLoad(HotFixViewConfigPath + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            AssetDatabase.Refresh();
            Debug.Log("HotFixView打包完毕");
        }

        #endregion

        #region HotFixCode

        private void HotFixCodeBuild()
        {
            //热更新打包
#if HybridCLR
            CompileDllCommand.CompileDllActiveBuildTarget();
#endif
            File.Copy(DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/HotFixCode.dll",
                RuntimeGlobal.GetDeviceStoragePath() + "/HotFix/HotFixCode/" + "HotFixCode.dll.bytes", true);
            string path = HotFixCodePath + "HotFixCode.dll.bytes";

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "HotFixCode.dll.bytes";
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(path);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(path).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixCode/";
            FileOperationComponent.SaveTextToLoad(HotFixCodeConfigPath + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            AssetDatabase.Refresh();
        }

        #endregion

        #region MetaAssembly

        private void MetaAssemblyBuild()
        {
#if HybridCLR
            //生成元数据
            StripAOTDllCommand.GenerateStripedAOTDlls();
            //移动元文件
            foreach (string metadataName in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                string metaDataDllPath = DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/AssembliesPostIl2CppStrip/" + platformName + "/" + metadataName;
                if (!File.Exists(metaDataDllPath))
                {
                    Debug.LogError("当前元数据" + metadataName + "未生成");
                }
                else
                {
                    File.Copy(metaDataDllPath, MetadataPath + metadataName + ".bytes", true);
                }
            }
#endif
            List<string> buildPath = DataFrameComponent.Path_GetGetSpecifyPathInAllType(MetadataPath, "bytes");
            Debug.Log(buildPath.Count);
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


            FileOperationComponent.SaveTextToLoad(MetadataConfigPath, "MetadataConfig.json", JsonMapper.ToJson(hotFixMetaAssemblyConfigs));
        }

        #endregion

        #region Assembly

        private void AssemblyBuild()
        {
#if HybridCLR
            CompileDllCommand.CompileDllActiveBuildTarget();
#endif
            string assemblyDllPath = DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/Assembly-CSharp.dll";
            File.Copy(assemblyDllPath, AssemblyPath + "Assembly-CSharp.dll.bytes", true);
            HotFixRuntimeDownConfig hotFixAssemblyConfig = new HotFixRuntimeDownConfig();
            hotFixAssemblyConfig.md5 = FileOperationComponent.GetMD5HashFromFile(AssemblyPath + "Assembly-CSharp.dll.bytes");
            hotFixAssemblyConfig.name = "Assembly-CSharp.dll.bytes";
            hotFixAssemblyConfig.size = FileOperationComponent.GetFileSize(AssemblyPath + "Assembly-CSharp.dll.bytes").ToString();
            hotFixAssemblyConfig.path = "HotFixRuntime/Assembly/";
            FileOperationComponent.SaveTextToLoad(AssemblyConfigPath, "AssemblyConfig.json", JsonMapper.ToJson(hotFixAssemblyConfig));
        }

        #endregion

        #region GameRootStart

        private void GameRootStartBuild()
        {
            if (GameRootStartPrefab == null)
            {
                Debug.LogError("GameRootStartPrefab为空");
            }

            AssetImporter gameRootStartImporter = AssetImporter.GetAtPath(GameRootStartPath);
            gameRootStartImporter.assetBundleName = "HotFixRuntime/GameRootStartAssetBundle/" + DataFrameComponent.String_AllCharToLower("GameRootStart");
            BuildPipeline.BuildAssetBundles(UnStreamingAssetsPath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);

            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }


            string filePath = GameRootStartAssetBundlePath + DataFrameComponent.String_AllCharToLower("GameRootStart");
            File.Delete(filePath + ".manifest");
            HotFixRuntimeDownConfig hotFixGameRootStartConfig = new HotFixRuntimeDownConfig();
            hotFixGameRootStartConfig.name = DataFrameComponent.String_AllCharToLower("GameRootStart");
            hotFixGameRootStartConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixGameRootStartConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixGameRootStartConfig.path = "HotFixRuntime/GameRootStartAssetBundle/";

            FileOperationComponent.SaveTextToLoad(GameRootStartAssetBundleConfigPath, "GameRootStartConfig.json", JsonMapper.ToJson(hotFixGameRootStartConfig));
            Debug.Log("GameRootStart打包完毕");
        }

        #endregion

        #region 场景打包

        private void NormalSceneBuild(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            Debug.Log(normalSceneAssetBundleAsset.name);
            DataFrameComponent.RemoveAllAssetBundleName();
            AssetDatabase.Refresh();
            sceneRepeatAssets.Clear();

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            // Debug.Log("保存场景:" + SceneManager.GetActiveScene().name);
            //打开对应的场景文件
            OpenScene(normalSceneAssetBundleAsset.name);
            while (!sceneOpenState.Contains(normalSceneAssetBundleAsset.name))
            {
            }

            Debug.Log("打开场景:" + normalSceneAssetBundleAsset.name);


            #region 创建文件夹

            string sceneAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "Scene" + "/";
            if (!Directory.Exists(sceneAssetBundleDirectory))
            {
                Directory.CreateDirectory(sceneAssetBundleDirectory);
            }

            string entityAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "Entity" + "/";
            if (!Directory.Exists(entityAssetBundleDirectory))
            {
                Directory.CreateDirectory(entityAssetBundleDirectory);
            }

            string repeatAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "Repeat" + "/";
            if (!Directory.Exists(repeatAssetBundleDirectory))
            {
                Directory.CreateDirectory(repeatAssetBundleDirectory);
            }

            string UIAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "UI" + "/";
            if (!Directory.Exists(UIAssetBundleDirectory))
            {
                Directory.CreateDirectory(UIAssetBundleDirectory);
            }

            string SceneComponentAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "SceneComponent" + "/";
            if (!Directory.Exists(SceneComponentAssetBundleDirectory))
            {
                Directory.CreateDirectory(SceneComponentAssetBundleDirectory);
            }

            string SceneComponentInitAssetBundleDirectory = HotFixAssetBundlePath + normalSceneAssetBundleAsset.name + "/" + "SceneComponentInit" + "/";
            if (!Directory.Exists(SceneComponentInitAssetBundleDirectory))
            {
                Directory.CreateDirectory(SceneComponentInitAssetBundleDirectory);
            }

            #endregion

            #region HotFixAssetPathConfig

            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            //应用热更配置并记录路径
            for (int i = 0; i < hotFixAssetPathConfigs.Count; i++)
            {
                //应用预制体
                hotFixAssetPathConfigs[i].SetPathAndApplyPrefab();
                //设置AssetBundle名称
                AssetImporter assetImporter = AssetImporter.GetAtPath(hotFixAssetPathConfigs[i].prefabPath);
                assetImporter.assetBundleName = hotFixAssetPathConfigs[i].assetBundlePath;
                //预制体添加到重复场景资源
                PrefabAddRepeatAsset(hotFixAssetPathConfigs[i].prefabPath, normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets);
            }

            //保存场景
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            // await UniTask.DelayFrame(buildDelayFrame);

            #endregion


            #region 场景

            string sceneAssetBundleName = "HotFixRuntime/HotFixAssetBundle/" + normalSceneAssetBundleAsset.name + "/Scene/" + normalSceneAssetBundleAsset.name;
            //打包当前场景
            AssetImporter sceneAssetImporter = AssetImporter.GetAtPath(SceneManager.GetActiveScene().path);
            //场景设置AssetBundle
            sceneAssetImporter.assetBundleName = sceneAssetBundleName;
            //场景添加到重复场景资源
            SceneAddRepeatAsset(SceneManager.GetActiveScene().path, normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets);

            #endregion

            #region 重复资源

            //打包重复资源
            foreach (SceneRepeatAsset sceneRepeatAsset in sceneRepeatAssets)
            {
                //文件是否存在
                string repeatAssetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/Repeat/" + sceneRepeatAsset.assetName;
                Debug.Log("重复资源" + sceneRepeatAsset.assetName + "打包");
                foreach (string path in sceneRepeatAsset.repeatAssets)
                {
                    //重复资源设置AssetBundle
                    AssetImporter assetImporter = AssetImporter.GetAtPath(path);
                    assetImporter.assetBundleName = repeatAssetBundleName;
                }
            }

            #endregion

            BuildPipeline.BuildAssetBundles(UnStreamingAssetsPath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            //设置打包状态
            for (int i = 0; i < SceneBuildStates.Count; i++)
            {
                if (SceneBuildStates[i].sceneName == normalSceneAssetBundleAsset.name)
                {
                    SceneBuildStates[i].isBuild = true;
                }
            }

            Debug.Log(normalSceneAssetBundleAsset.name + "打包完毕");

            //生成场景配置表
            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }
        }

        [LabelText("预制体添加重复资源")]
        private void PrefabAddRepeatAsset(string prefabPath, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets)
        {
            //AB包所有依赖
            List<string> prefabDependencies = new List<string>(GetAssetDependencies(prefabPath, ".cs"));
            //AB包所有依赖中的重复资源
            foreach (string PrefabDependency in prefabDependencies)
            {
                //添加到重复资源
                AddSceneRepeatAsset(PrefabDependency, sceneAssetBundleRepeatAssets);
            }
        }

        [LabelText("场景添加到重复资源")]
        private void SceneAddRepeatAsset(string prefabPath, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets)
        {
            //AB包所有依赖
            List<string> sceneDependencies = new List<string>(GetAssetDependencies(prefabPath, ".cs"));
            //AB包所有依赖中的重复资源
            foreach (string sceneDependency in sceneDependencies)
            {
                //添加到重复资源
                AddSceneRepeatAsset(sceneDependency, sceneAssetBundleRepeatAssets);
            }
        }

        //添加场景重复资源
        private void AddSceneRepeatAsset(string abName, string path)
        {
            if (!SceneRepeatAssetPathIsContains(path))
            {
                SceneRepeatAsset sceneRepeatAsset = GetSceneRepeatAssetByAbName(abName);
                AddPathToSceneRepeatAsset(sceneRepeatAsset, path);
            }
        }

        //获得场景重复资源
        private SceneRepeatAsset GetSceneRepeatAssetByAbName(string abName)
        {
            SceneRepeatAsset tempSceneRepeatAsset = null;
            foreach (SceneRepeatAsset sceneRepeatAsset in sceneRepeatAssets)
            {
                if (sceneRepeatAsset.assetName == abName)
                {
                    tempSceneRepeatAsset = sceneRepeatAsset;
                }
            }

            if (tempSceneRepeatAsset == null)
            {
                tempSceneRepeatAsset = new SceneRepeatAsset();
                tempSceneRepeatAsset.assetName = abName;
                sceneRepeatAssets.Add(tempSceneRepeatAsset);
            }

            return tempSceneRepeatAsset;
        }

        //路径是否已经被包含
        private bool SceneRepeatAssetPathIsContains(string path)
        {
            foreach (SceneRepeatAsset sceneRepeatAsset in sceneRepeatAssets)
            {
                foreach (string repeatAssetPath in sceneRepeatAsset.repeatAssets)
                {
                    if (repeatAssetPath == path)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //添加到重复列表中
        private void AddPathToSceneRepeatAsset(SceneRepeatAsset sceneRepeatAsset, string path)
        {
            sceneRepeatAsset.repeatAssets.Add(path);
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

        [LabelText("是否是重复资源路径")]
        private void AddSceneRepeatAsset(string path, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets)
        {
            foreach (SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset in sceneAssetBundleRepeatAssets)
            {
                foreach (string ContainPath in sceneAssetBundleRepeatAsset.assetBundleContainPath)
                {
                    if (path.Contains(ContainPath))
                    {
                        AddSceneRepeatAsset(sceneAssetBundleRepeatAsset.assetBundleName, path);
                    }
                }
            }
        }

        #endregion

        #region 获得资源路径的所有资源依赖

        [LabelText("获得资源路径的所有资源依赖")]
        private static List<string> GetAssetDependencies(string assetPath, string excludeFileName)
        {
            if (!File.Exists(assetPath))
            {
                return null;
            }

            List<string> dependecies = new List<string>(AssetDatabase.GetDependencies(assetPath));
            List<string> tempDependecies = new List<string>();
            for (int i = 0; i < dependecies.Count; i++)
            {
                if (!dependecies[i].Contains(excludeFileName))
                {
                    tempDependecies.Add(dependecies[i]);
                }
            }

            return tempDependecies;
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
                    EditorSceneManager.sceneOpened += EditorSceneOpened;
                    EditorSceneManager.OpenScene(allScenePath[i], OpenSceneMode.Single);
                }
            }
        }

        private void EditorSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (!sceneOpenState.Contains(scene.name))
            {
                sceneOpenState.Add(scene.name);
            }

            EditorSceneManager.sceneOpened -= EditorSceneOpened;
        }

        #endregion

        #region 生成场景配置信息

        private async UniTask GenerateBuildConfig(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            OpenScene(normalSceneAssetBundleAsset.name);
            await UniTask.WaitUntil(() => sceneOpenState.Contains(normalSceneAssetBundleAsset.name));
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            GenerateBuildConfig(normalSceneAssetBundleAsset.name, hotFixAssetPathConfigs, normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets, normalSceneAssetBundleAsset.sceneExceptionAsset);
        }

        [LabelText("生成配置信息")]
        private void GenerateBuildConfig(string sceneName, List<HotFixAssetPathConfig> hotFixAssetPathConfigs, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets, List<string> sceneExceptionAsset)
        {
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //场景数据
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetConfig.assetName = DataFrameComponent.String_AllCharToLower(sceneName);
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetConfig.assetPath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/";
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetConfig.assetSize =
                FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/" + DataFrameComponent.String_AllCharToLower(sceneName)).ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetConfig.assetMd5 =
                FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/" + DataFrameComponent.String_AllCharToLower(sceneName));

            //重复资源
            foreach (SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset in sceneAssetBundleRepeatAssets)
            {
                HotFixRuntimeAssetConfig hotFixRuntimeAssetConfig = new HotFixRuntimeAssetConfig();
                hotFixRuntimeAssetConfig.assetName = DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName);
                hotFixRuntimeAssetConfig.assetPath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/";
                hotFixRuntimeAssetConfig.assetSize =
                    FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/" +
                                                       DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName)).ToString();
                hotFixRuntimeAssetConfig.assetMd5 =
                    FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/" +
                                                              DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName));
                hotFixRuntimeSceneAssetBundleConfig.repeatSceneFixRuntimeAssetConfig.Add(hotFixRuntimeAssetConfig);
            }

            //场景AssetBundle
            foreach (HotFixAssetPathConfig hotFixAssetPathConfig in hotFixAssetPathConfigs)
            {
                HotFixRuntimeAssetConfig hotFixRuntimeAssetConfig = new HotFixRuntimeAssetConfig();
                hotFixRuntimeAssetConfig.assetName = DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name);
                hotFixRuntimeAssetConfig.assetPath = hotFixAssetPathConfig.assetBundlePath.Replace(DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name), "");
                string adPath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hotFixRuntimeAssetConfig.assetPath + hotFixRuntimeAssetConfig.assetName;
                hotFixRuntimeAssetConfig.assetSize = FileOperationComponent.GetFileSize(adPath).ToString();
                hotFixRuntimeAssetConfig.assetMd5 = FileOperationComponent.GetMD5HashFromFile(adPath);
                hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Add(hotFixRuntimeAssetConfig);
            }

            //额外数据
            foreach (string sceneExceptionAssetPath in sceneExceptionAsset)
            {
                Debug.Log(sceneExceptionAssetPath);
                HotFixRuntimeAssetConfig hotFixRuntimeSceneExceptConfig = new HotFixRuntimeAssetConfig();
                hotFixRuntimeSceneExceptConfig.assetName = DataFrameComponent.Path_GetPathFileName(sceneExceptionAssetPath);
                string assetPath = DataFrameComponent.Path_GetPathDontContainFileName(sceneExceptionAssetPath.Replace("Assets/UnStreamingAssets/", ""));
                if (assetPath != string.Empty)
                {
                    assetPath += "/";
                }

                hotFixRuntimeSceneExceptConfig.assetPath = assetPath;
                hotFixRuntimeSceneExceptConfig.assetSize = FileOperationComponent.GetFileSize(sceneExceptionAssetPath).ToString();
                hotFixRuntimeSceneExceptConfig.assetMd5 = FileOperationComponent.GetMD5HashFromFile(sceneExceptionAssetPath);
                hotFixRuntimeSceneAssetBundleConfig.sceneExceptConfigs.Add(hotFixRuntimeSceneExceptConfig);
            }


            string hotFixAssetBundleConfigPath = RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + DataFrameComponent.String_AllCharToLower(SceneManager.GetActiveScene().name) + ".json";
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                File.Delete(hotFixAssetBundleConfigPath);
            }

            FileOperationComponent.SaveTextToLoad(hotFixAssetBundleConfigPath, JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
        }

        #endregion

        #region 热更配置信息

        [LabelText("保存配置")]
        public override void OnSaveConfig()
        {
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
            NormalSceneAssetBundleAssetsPath.Clear();
            NormalSceneAssetBundleAssetConfigsPath.Clear();
            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
            {
                NormalSceneAssetBundleAssetsPath.Add(AssetDatabase.GetAssetPath(normalSceneAssetBundleAsset));
            }

            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssetConfig)
            {
                NormalSceneAssetBundleAssetConfigsPath.Add(AssetDatabase.GetAssetPath(normalSceneAssetBundleAsset));
            }

            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json", JsonUtility.ToJson(this));
        }

        [LabelText("加载配置")]
        public override void OnLoadConfig()
        {
            if (!File.Exists(RuntimeGlobal.assetRootPath + "HotFixCollect.json"))
            {
                return;
            }

            HotFixCollect hotFixCollectConfig = JsonUtil.FromJson<HotFixCollect>(FileOperationComponent.GetTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json"));
            this.removeAssetBundleName = hotFixCollectConfig.removeAssetBundleName;
            this.localIsUpdate = hotFixCollectConfig.localIsUpdate;
            this.hotFixDownPathData = hotFixCollectConfig.hotFixDownPathData;
            if (IsContainHotFixDownPathData())
            {
                this.hotFixDownPath = GetHotFixDownPathData();
            }

            this.targetOutPathData = hotFixCollectConfig.targetOutPathData;
            this.targetResourceVersion = hotFixCollectConfig.targetResourceVersion;
            this.targetBuildAssetBundleOptions = hotFixCollectConfig.targetBuildAssetBundleOptions;
            if (IsContainTargetOutPathData())
            {
                this.targetOutPath = GetTargetOutPathData();
            }

            this.targetResourceVersion = hotFixCollectConfig.targetResourceVersion;
            this.isUpdateVersion = hotFixCollectConfig.isUpdateVersion;
            this.buildHotFixView = hotFixCollectConfig.buildHotFixView;
            this.HotFixViewPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollectConfig.HotFixViewPrePath);
            this.HotFixViewPrePath = hotFixCollectConfig.HotFixViewPrePath;
            this.buildHotFixCode = hotFixCollectConfig.buildHotFixCode;
            this.buildAssemblyParticipatePackaging = hotFixCollectConfig.buildAssemblyParticipatePackaging;
            this.buildMetaAssemblyParticipatePackaging = hotFixCollectConfig.buildMetaAssemblyParticipatePackaging;
            this.buildGameRootStart = hotFixCollectConfig.buildGameRootStart;
            this.GameRootStartPath = hotFixCollectConfig.GameRootStartPath;
            this.GameRootStartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollectConfig.GameRootStartPath);
            this.SceneBuildSwitch = hotFixCollectConfig.SceneBuildSwitch;
            NormalSceneAssetBundleAssets.Clear();
            for (int i = 0; i < hotFixCollectConfig.NormalSceneAssetBundleAssetsPath.Count; i++)
            {
                NormalSceneAssetBundleAssets.Add(AssetDatabase.LoadAssetAtPath<NormalSceneAssetBundleAsset>(hotFixCollectConfig.NormalSceneAssetBundleAssetsPath[i]));
            }
            NormalSceneAssetBundleAssetConfig.Clear();
            for (int i = 0; i < hotFixCollectConfig.NormalSceneAssetBundleAssetConfigsPath.Count; i++)
            {
                NormalSceneAssetBundleAssetConfig.Add(AssetDatabase.LoadAssetAtPath<NormalSceneAssetBundleAsset>(hotFixCollectConfig.NormalSceneAssetBundleAssetConfigsPath[i]));
            }
        }

        #endregion

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public void Update()
        {
        }
    }
}