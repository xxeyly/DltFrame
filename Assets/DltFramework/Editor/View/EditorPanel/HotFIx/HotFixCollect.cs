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

        [LabelText("仅资源版本对比")] [OnValueChanged("OnSaveConfig")]
        public bool onlyResourcesVersionContrast;

        [OnValueChanged("OnSaveConfig")]
        [InfoBox("打包后会拷贝所有资源到当前路径下,并且还会复制一份以当前版本号的为名的备份文件夹")]
        [LabelText("资源外部拷贝路径")]
        [LabelWidth(120)]
        [FolderPath(AbsolutePath = true)]
        [ShowInInspector]
        [OnValueChanged("OnSaveConfig")]
        private string targetOutPath;

        [LabelText("平台拷贝地址")] [HideInInspector]
        public List<PlatformPath> targetOutPathData = new List<PlatformPath>();

        [BoxGroup("版本号")] [LabelText("主版本号")] public int majorVersion = 1;

        [BoxGroup("版本号")] [LabelText("次版本号")] public int minorVersion;

        [BoxGroup("版本号")] [LabelText("修订版本号(每次打包自增1)")]
        public int revisionVersion;

        [BoxGroup("AssetBundle信息")] [LabelText("打包后移除AssetBundle数据")] [OnValueChanged("OnSaveConfig")]
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

            #endregion

            #region HotFixRuntime Directory

            HotFixRuntimePath = UnStreamingAssetsPath + "HotFixRuntime/";
            MetadataPath = HotFixRuntimePath + "Metadata/";
            if (!Directory.Exists(MetadataPath))
            {
                Directory.CreateDirectory(MetadataPath);
            }

            MetadataConfigPath = HotFixRuntimePath + "MetadataConfig/";
            if (!Directory.Exists(MetadataConfigPath))
            {
                Directory.CreateDirectory(MetadataConfigPath);
            }

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

            // Debug.Log("初始化");
        }

        #region 热更元素

        #region HotFixView

        [BoxGroup("HotFixView")] [LabelText("打包")] [OnValueChanged("OnSaveConfig")]
        public bool buildHotFixView;

        [BoxGroup("HotFixView")] [LabelText("HotFixView预制体")] [AssetSelector] [OnValueChanged("OnSaveConfig")] [EnableIf("buildHotFixView")]
        public GameObject HotFixViewPrefab;

        [BoxGroup("buildHotFixView")] [LabelText("HotFixView预制体路径")] [HideInInspector] [OnValueChanged("OnSaveConfig")]
        public string HotFixViewPrePath;

        #endregion

        #region HotFixCode

        [BoxGroup("HotFixCode")] [LabelText("打包")] [OnValueChanged("OnSaveConfig")]
        public bool buildHotFixCode;

        #endregion

        #region 元数据

        [BoxGroup("MetaAssembly")] [LabelText("打包")] [OnValueChanged("OnSaveConfig")]
        public bool buildMetaAssemblyParticipatePackaging;

        [BoxGroup("Assembly")] [LabelText("打包")] [OnValueChanged("OnSaveConfig")]
        public bool buildAssemblyParticipatePackaging;

        #endregion

        #region GameRootStart

        [BoxGroup("GameRootStart")] [LabelText("打包")] [OnValueChanged("OnSaveConfig")]
        public bool buildGameRootStart;

        [BoxGroup("GameRootStart")] [LabelText("GameRootStart预制体")] [AssetSelector] [OnValueChanged("OnSaveConfig")] [EnableIf("buildGameRootStart")]
        public GameObject GameRootStartPrefab;

        [BoxGroup("GameRootStart")] [LabelText("GameRootStart预制体路径")] [Sirenix.OdinInspector.FilePath] [HideInInspector]
        public string GameRootStartPath;

        #endregion

        #region SceneBuild

        [FoldoutGroup("Scene")] [LabelText("打包")] [OnValueChanged("OnSaveConfig", true)] [SerializeField]
        public bool Scene;

        [FoldoutGroup("Scene")] [LabelText("正常场景配置")] [AssetList] [InlineEditor()] [OnValueChanged("OnSaveConfig")] [EnableIf("Scene")]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssets = new List<NormalSceneAssetBundleAsset>();

        [FoldoutGroup("Scene")] [LabelText("正常场景生成表")] [AssetList] [InlineEditor()] [OnValueChanged("OnSaveConfig")] [EnableIf("Scene")]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssetConfig = new List<NormalSceneAssetBundleAsset>();

        [FoldoutGroup("Scene")] [LabelText("场景打开状态")]
        private List<string> sceneOpenState = new List<string>();

        [FoldoutGroup("Scene")] [LabelText("场景打包状态")] [TableList]
        public List<SceneBuildState> SceneBuildStates = new List<SceneBuildState>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAsset")]
        public List<string> NormalSceneAssetBundleAssetsPath = new List<string>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAssetConfig")]
        public List<string> NormalSceneAssetBundleAssetConfigsPath = new List<string>();

        [ToggleGroup("SceneBuildSwitch")] [LabelText("当前打开场景名称")]
        private string currentOpenSceneName;

        [LabelText("重复利用资源")] [Tooltip("每个场景可能都不一样")]
        private List<SceneRepeatAsset> sceneRepeatAssets = new List<SceneRepeatAsset>();

        #endregion

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


        private UniTask OnBuild()
        {
            currentOpenSceneName = SceneManager.GetActiveScene().name;
            sceneRepeatAssets.Clear();
            sceneOpenState.Clear();
            SceneBuildStates.Clear();
            newBuildHotFixData.Clear();
            //增加版本号
            DataFrameComponent.RemoveAllAssetBundleName();

            if ((buildHotFixView && HotFixViewPrefab != null) || buildHotFixCode || buildAssemblyParticipatePackaging || buildMetaAssemblyParticipatePackaging || buildGameRootStart ||
                (Scene && NormalSceneAssetBundleAssets.Count > 0))
            {
                revisionVersion += 1;
                OnSaveConfig();
            }

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

            if (Scene)
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

                /*for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
                {
                    await GenerateBuildConfig(NormalSceneAssetBundleAssets[i]);
                }*/

                Debug.Log("生成配置信息结束");
            }

            #endregion

            #region 打包后拷贝

            //这里保存两份是因为,一份是在StreamingAssets下,一份是在UnStreamingAssets下,这样可以保证在打包时,引用StreamingAssets路径下的
            //编辑器模式下,引用UnStreamingAssets路径下的,打包后StreamingAssets路径下只有HotFixDownPath.txt和localIsUpdate.txt两个文件
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/Config/", "HotFixDownPath.txt", hotFixDownPath);
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/Config/", "HotFixDownPath.txt", hotFixDownPath);
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/Config/", "localIsUpdate.txt", localIsUpdate.ToString());
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/Config/", "localIsUpdate.txt", localIsUpdate.ToString());
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/Config/", "OnlyResourcesVersionContrast.txt", onlyResourcesVersionContrast.ToString());
            FileOperationComponent.SaveTextToLoad("Assets/UnStreamingAssets/Config/", "OnlyResourcesVersionContrast.txt", onlyResourcesVersionContrast.ToString());

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
                FileOperationComponent.Copy(MetadataPath, targetOutPath + "/HotFixRuntime/Metadata");
                FileOperationComponent.Copy(MetadataConfigPath, targetOutPath + "/HotFixRuntime/MetadataConfig");
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
                string backupVersion = "/Backups_" + majorVersion + "." + minorVersion + "." + revisionVersion;
                FileOperationComponent.Copy(HotFixCodePath, targetOutPath + backupVersion + "/HotFix/HotFixCode");
                FileOperationComponent.Copy(HotFixCodeConfigPath, targetOutPath + backupVersion + "/HotFix/HotFixCodeConfig");
                FileOperationComponent.Copy(HotFixViewPath, targetOutPath + backupVersion + "/HotFix/HotFixView");
                FileOperationComponent.Copy(HotFixViewConfigPath, targetOutPath + backupVersion + "/HotFix/HotFixViewConfig");
                FileOperationComponent.Copy(MetadataPath, targetOutPath + backupVersion + "/HotFixRuntime/Metadata");
                FileOperationComponent.Copy(MetadataConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/MetadataConfig");

                FileOperationComponent.Copy(AssemblyPath, targetOutPath + backupVersion + "/HotFixRuntime/Assembly");
                FileOperationComponent.Copy(AssemblyConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/AssemblyConfig");
                FileOperationComponent.Copy(GameRootStartAssetBundlePath, targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperationComponent.Copy(GameRootStartAssetBundleConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperationComponent.Copy(HotFixAssetBundlePath, targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundle");
                FileOperationComponent.Copy(HotFixAssetBundleConfigPath, targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperationComponent.CopyFile("Assets/UnStreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json",
                    targetOutPath + backupVersion + "/HotFixRuntime/HotFixServerResourcesCount.json");

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
            return UniTask.CompletedTask;
        }

        #region 打包HotFixView

        [LabelText("打包HotFixView")]
        private void HotFixViewBuild()
        {
            if (HotFixViewPrefab == null)
            {
                Debug.Log("配置信息错误");
            }

            //旧版打包文件配置表
            HotFixAssetConfig oldHotFixAssetConfig = new HotFixAssetConfig();
            string filePath = HotFixViewPath + DataFrameComponent.String_AllCharToLower("HotFixView");
            string fileConfigPath = HotFixViewConfigPath + "HotFixViewConfig.json";
            if (File.Exists(fileConfigPath))
            {
                oldHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(FileOperationComponent.GetTextToLoad(fileConfigPath));
            }

            #region 打包AB包

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixViewPrefab));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/" + DataFrameComponent.String_AllCharToLower("HotFixView");
            BuildPipeline.BuildAssetBundles(UnStreamingAssetsPath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }

            #endregion

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = DataFrameComponent.String_AllCharToLower("HotFixView"); //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixView/";

            if (hotFixAssetConfig.md5 != oldHotFixAssetConfig.md5)
            {
                oldHotFixAssetConfig.version += 1;
                newBuildHotFixData.Add(filePath);
            }

            hotFixAssetConfig.version = oldHotFixAssetConfig.version;

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
            string hotfixCodeDllPath = HotFixCodePath + "HotFixCode.dll.bytes";
            string hotfixCodeDllConfigPath = HotFixCodeConfigPath + "HotFixCodeConfig.json";
            HotFixAssetConfig oldHotFixAssetConfig = new HotFixAssetConfig();
            if (File.Exists(hotfixCodeDllConfigPath))
            {
                oldHotFixAssetConfig = JsonUtil.FromJson<HotFixAssetConfig>(FileOperationComponent.GetTextToLoad(hotfixCodeDllConfigPath));
            }

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "HotFixCode.dll.bytes";
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(hotfixCodeDllPath);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(hotfixCodeDllPath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixCode/";

            if (hotFixAssetConfig.md5 != oldHotFixAssetConfig.md5)
            {
                oldHotFixAssetConfig.version += 1;
                newBuildHotFixData.Add(hotfixCodeDllPath);
            }

            hotFixAssetConfig.version = oldHotFixAssetConfig.version;
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
            string hotFixMetaAssemblyConfigPath = MetadataConfigPath + "MetadataConfig.json";
            List<HotFixRuntimeDownConfig> oldHotFixMetaAssemblyConfigs = new List<HotFixRuntimeDownConfig>();
            if (File.Exists(hotFixMetaAssemblyConfigPath))
            {
                oldHotFixMetaAssemblyConfigs = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(FileOperationComponent.GetTextToLoad(hotFixMetaAssemblyConfigPath));
            }

            List<HotFixRuntimeDownConfig> hotFixMetaAssemblyConfigs = new List<HotFixRuntimeDownConfig>();

            foreach (string metadataName in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                HotFixRuntimeDownConfig hotFixMetaAssemblyConfig = new HotFixRuntimeDownConfig();

                hotFixMetaAssemblyConfig.name = metadataName + ".bytes";
                hotFixMetaAssemblyConfig.md5 = FileOperationComponent.GetMD5HashFromFile(MetadataPath + metadataName + ".bytes");
                hotFixMetaAssemblyConfig.path = "HotFixRuntime/Metadata/";
                hotFixMetaAssemblyConfig.size = FileOperationComponent.GetFileSize(MetadataPath + metadataName + ".bytes").ToString();

                HotFixRuntimeDownConfig contrastHotFixRuntimeDownConfig = FindHotFixRuntimeDownConfigByName(hotFixMetaAssemblyConfig.name, oldHotFixMetaAssemblyConfigs);

                if (contrastHotFixRuntimeDownConfig.md5 != hotFixMetaAssemblyConfig.md5)
                {
                    newBuildHotFixData.Add(MetadataPath + metadataName + ".bytes");
                    contrastHotFixRuntimeDownConfig.version += 1;
                }

                hotFixMetaAssemblyConfig.version = contrastHotFixRuntimeDownConfig.version;
                hotFixMetaAssemblyConfigs.Add(hotFixMetaAssemblyConfig);
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

            string configPath = AssemblyConfigPath + "AssemblyConfig.json";
            HotFixRuntimeDownConfig oldHotFixAssemblyConfigs = new HotFixRuntimeDownConfig();
            if (File.Exists(configPath))
            {
                oldHotFixAssemblyConfigs = JsonUtil.FromJson<HotFixRuntimeDownConfig>(FileOperationComponent.GetTextToLoad(configPath));
            }

            HotFixRuntimeDownConfig hotFixAssemblyConfig = new HotFixRuntimeDownConfig();
            hotFixAssemblyConfig.md5 = FileOperationComponent.GetMD5HashFromFile(AssemblyPath + "Assembly-CSharp.dll.bytes");
            hotFixAssemblyConfig.name = "Assembly-CSharp.dll.bytes";
            hotFixAssemblyConfig.size = FileOperationComponent.GetFileSize(AssemblyPath + "Assembly-CSharp.dll.bytes").ToString();
            hotFixAssemblyConfig.path = "HotFixRuntime/Assembly/";
            if (hotFixAssemblyConfig.md5 != oldHotFixAssemblyConfigs.md5)
            {
                oldHotFixAssemblyConfigs.version += 1;
                newBuildHotFixData.Add("Assembly-CSharp.dll.bytes");
            }

            hotFixAssemblyConfig.version = oldHotFixAssemblyConfigs.version;
            FileOperationComponent.SaveTextToLoad(AssemblyConfigPath, "AssemblyConfig.json", JsonUtil.ToJson(hotFixAssemblyConfig));
        }

        #endregion

        #region GameRootStart

        private void GameRootStartBuild()
        {
            if (GameRootStartPrefab == null)
            {
                Debug.LogError("GameRootStartPrefab为空");
            }

            string filePath = GameRootStartAssetBundlePath + DataFrameComponent.String_AllCharToLower("GameRootStart");
            string configPath = GameRootStartAssetBundleConfigPath + "GameRootStartConfig.json";
            HotFixRuntimeDownConfig oldHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();
            if (File.Exists(configPath))
            {
                oldHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(FileOperationComponent.GetTextToLoad(configPath));
            }

            #region 打包

            AssetImporter gameRootStartImporter = AssetImporter.GetAtPath(GameRootStartPath);
            gameRootStartImporter.assetBundleName = "HotFixRuntime/GameRootStartAssetBundle/" + DataFrameComponent.String_AllCharToLower("GameRootStart");
            BuildPipeline.BuildAssetBundles(UnStreamingAssetsPath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);

            if (removeAssetBundleName)
            {
                DataFrameComponent.RemoveAllAssetBundleName();
            }

            #endregion

            HotFixRuntimeDownConfig hotFixGameRootStartConfig = new HotFixRuntimeDownConfig();
            hotFixGameRootStartConfig.name = DataFrameComponent.String_AllCharToLower("GameRootStart");
            hotFixGameRootStartConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixGameRootStartConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixGameRootStartConfig.path = "HotFixRuntime/GameRootStartAssetBundle/";

            if (hotFixGameRootStartConfig.md5 != oldHotFixRuntimeDownConfig.md5)
            {
                oldHotFixRuntimeDownConfig.version += 1;
                newBuildHotFixData.Add("GameRootStart");
            }

            hotFixGameRootStartConfig.version = oldHotFixRuntimeDownConfig.version;
            FileOperationComponent.SaveTextToLoad(GameRootStartAssetBundleConfigPath, "GameRootStartConfig.json", JsonMapper.ToJson(hotFixGameRootStartConfig));

            Debug.Log("GameRootStart打包完毕");
        }

        #endregion

        #region 场景打包

        private void NormalSceneBuild(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
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

            #region 场景中热更文件打包

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
            hotFixAssetPathConfigs = DataFrameComponent.Hierarchy_GetAllObjectsInScene<HotFixAssetPathConfig>();
            GenerateBuildConfig(normalSceneAssetBundleAsset.name, hotFixAssetPathConfigs, normalSceneAssetBundleAsset.sceneAssetBundleRepeatAssets, normalSceneAssetBundleAsset.sceneExceptionAsset);

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
        private void GenerateBuildConfig(string sceneName, List<HotFixAssetPathConfig> hotFixAssetPathConfigs, List<SceneAssetBundleRepeatAsset> sceneAssetBundleRepeatAssets,
            List<string> sceneExceptionAsset)
        {
            //场景配置表路径

            #region 读取旧的配置信息

            string hotFixAssetBundleConfigPath = RuntimeGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + SceneManager.GetActiveScene().name + ".json";
            HotFixRuntimeSceneAssetBundleConfig oldHotFixRuntimeSceneAssetBundleConfig;
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                //存在读取
                oldHotFixRuntimeSceneAssetBundleConfig = JsonUtility.FromJson<HotFixRuntimeSceneAssetBundleConfig>(FileOperationComponent.GetTextToLoad(hotFixAssetBundleConfigPath));
            }
            else
            {
                //不存在新建
                oldHotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            }

            List<HotFixRuntimeDownConfig> oldSceneHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();
            oldSceneHotFixRuntimeDownConfigs.Add(oldHotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig);

            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in oldHotFixRuntimeSceneAssetBundleConfig.repeatSceneHotFixRuntimeDownConfigs)
            {
                oldSceneHotFixRuntimeDownConfigs.Add(hotFixRuntimeDownConfig);
            }

            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in oldHotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixRuntimeDownConfigs)
            {
                oldSceneHotFixRuntimeDownConfigs.Add(hotFixRuntimeDownConfig);
            }

            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in oldHotFixRuntimeSceneAssetBundleConfig.sceneExceptConfigsHotFixRuntimeDownConfigs)
            {
                oldSceneHotFixRuntimeDownConfigs.Add(hotFixRuntimeDownConfig);
            }

            #endregion

            #region 场景

            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //场景数据
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.name = DataFrameComponent.String_AllCharToLower(sceneName);
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.path = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/";
            string sceneAssetSize = FileOperationComponent
                .GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/" + DataFrameComponent.String_AllCharToLower(sceneName))
                .ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.size = sceneAssetSize;
            string sceneAssetMd5 = FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Scene/" +
                                                                             DataFrameComponent.String_AllCharToLower(sceneName));
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.md5 = sceneAssetMd5;
            string scenePath = UnStreamingAssetsPath + hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.path + sceneName;

            HotFixRuntimeDownConfig oldSceneHotFixRuntimeDownConfig =
                FindHotFixRuntimeDownConfigByName(hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.name, oldSceneHotFixRuntimeDownConfigs);
            if (oldSceneHotFixRuntimeDownConfig.md5 != sceneAssetMd5)
            {
                oldSceneHotFixRuntimeDownConfig.version += 1;
                newBuildHotFixData.Add(scenePath);
            }

            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.version = oldSceneHotFixRuntimeDownConfig.version;

            #endregion


            //重复资源
            foreach (SceneAssetBundleRepeatAsset sceneAssetBundleRepeatAsset in sceneAssetBundleRepeatAssets)
            {
                //获取旧的配置
                HotFixRuntimeDownConfig oldHotFixRuntimeDownConfig = FindHotFixRuntimeDownConfigByName(sceneAssetBundleRepeatAsset.assetBundleName, oldSceneHotFixRuntimeDownConfigs);

                HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();
                hotFixRuntimeDownConfig.name = DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName);
                hotFixRuntimeDownConfig.path = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/";
                hotFixRuntimeDownConfig.size =
                    FileOperationComponent.GetFileSize(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/" +
                                                       DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName)).ToString();
                hotFixRuntimeDownConfig.md5 =
                    FileOperationComponent.GetMD5HashFromFile(RuntimeGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/Repeat/" +
                                                              DataFrameComponent.String_AllCharToLower(sceneAssetBundleRepeatAsset.assetBundleName));
                hotFixRuntimeSceneAssetBundleConfig.repeatSceneHotFixRuntimeDownConfigs.Add(hotFixRuntimeDownConfig);
                string sceneAssetPath = UnStreamingAssetsPath + hotFixRuntimeDownConfig.path + hotFixRuntimeDownConfig.name;

                if (oldHotFixRuntimeDownConfig.md5 != hotFixRuntimeDownConfig.md5)
                {
                    oldHotFixRuntimeDownConfig.version += 1;
                    newBuildHotFixData.Add(sceneAssetPath);
                }

                hotFixRuntimeDownConfig.version = oldHotFixRuntimeDownConfig.version;
            }

            //场景AssetBundle
            foreach (HotFixAssetPathConfig hotFixAssetPathConfig in hotFixAssetPathConfigs)
            {
                //旧的数据
                HotFixRuntimeDownConfig oldHotFixRuntimeDownConfig =
                    FindHotFixRuntimeDownConfigByName(DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name), oldSceneHotFixRuntimeDownConfigs);

                HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();
                hotFixRuntimeDownConfig.name = DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name);
                hotFixRuntimeDownConfig.path = hotFixAssetPathConfig.assetBundlePath.Replace(DataFrameComponent.String_AllCharToLower(hotFixAssetPathConfig.name), "");
                string adPath = RuntimeGlobal.GetDeviceStoragePath() + "/" + hotFixRuntimeDownConfig.path + hotFixRuntimeDownConfig.name;
                hotFixRuntimeDownConfig.size = FileOperationComponent.GetFileSize(adPath).ToString();
                hotFixRuntimeDownConfig.md5 = FileOperationComponent.GetMD5HashFromFile(adPath);
                hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixRuntimeDownConfigs.Add(hotFixRuntimeDownConfig);
                string sceneAssetPath = UnStreamingAssetsPath + hotFixRuntimeDownConfig.path + hotFixRuntimeDownConfig.name;

                if (oldHotFixRuntimeDownConfig.md5 != hotFixRuntimeDownConfig.md5)
                {
                    oldHotFixRuntimeDownConfig.version += 1;
                    newBuildHotFixData.Add(sceneAssetPath);
                }

                hotFixRuntimeDownConfig.version = oldHotFixRuntimeDownConfig.version;
            }

            //额外数据
            foreach (string sceneExceptionAssetPath in sceneExceptionAsset)
            {
                HotFixRuntimeDownConfig oldHotFixRuntimeDownConfig =
                    FindHotFixRuntimeDownConfigByName(DataFrameComponent.Path_GetPathFileName(sceneExceptionAssetPath), oldSceneHotFixRuntimeDownConfigs);
                HotFixRuntimeDownConfig hotFixRuntimeSceneExceptConfig = new HotFixRuntimeDownConfig();
                hotFixRuntimeSceneExceptConfig.name = DataFrameComponent.Path_GetPathFileName(sceneExceptionAssetPath);
                string assetPath = DataFrameComponent.Path_GetPathDontContainFileName(sceneExceptionAssetPath.Replace("Assets/UnStreamingAssets/", ""));
                if (assetPath != string.Empty)
                {
                    assetPath += "/";
                }

                hotFixRuntimeSceneExceptConfig.path = assetPath;
                hotFixRuntimeSceneExceptConfig.size = FileOperationComponent.GetFileSize(sceneExceptionAssetPath).ToString();
                hotFixRuntimeSceneExceptConfig.md5 = FileOperationComponent.GetMD5HashFromFile(sceneExceptionAssetPath);
                hotFixRuntimeSceneAssetBundleConfig.sceneExceptConfigsHotFixRuntimeDownConfigs.Add(hotFixRuntimeSceneExceptConfig);

                if (oldHotFixRuntimeDownConfig.md5 != hotFixRuntimeSceneExceptConfig.md5)
                {
                    oldHotFixRuntimeDownConfig.version += 1;
                    newBuildHotFixData.Add(hotFixRuntimeSceneExceptConfig.path);
                }

                hotFixRuntimeSceneExceptConfig.version = oldHotFixRuntimeDownConfig.version;
            }


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
            removeAssetBundleName = hotFixCollectConfig.removeAssetBundleName;
            localIsUpdate = hotFixCollectConfig.localIsUpdate;
            onlyResourcesVersionContrast = hotFixCollectConfig.onlyResourcesVersionContrast;
            hotFixDownPathData = hotFixCollectConfig.hotFixDownPathData;
            if (IsContainHotFixDownPathData())
            {
                hotFixDownPath = GetHotFixDownPathData();
            }

            targetOutPathData = hotFixCollectConfig.targetOutPathData;
            targetBuildAssetBundleOptions = hotFixCollectConfig.targetBuildAssetBundleOptions;
            if (IsContainTargetOutPathData())
            {
                targetOutPath = GetTargetOutPathData();
            }

            majorVersion = hotFixCollectConfig.majorVersion;
            minorVersion = hotFixCollectConfig.minorVersion;
            revisionVersion = hotFixCollectConfig.revisionVersion;
            buildHotFixView = hotFixCollectConfig.buildHotFixView;
            HotFixViewPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollectConfig.HotFixViewPrePath);
            HotFixViewPrePath = hotFixCollectConfig.HotFixViewPrePath;
            buildHotFixCode = hotFixCollectConfig.buildHotFixCode;
            buildAssemblyParticipatePackaging = hotFixCollectConfig.buildAssemblyParticipatePackaging;
            buildMetaAssemblyParticipatePackaging = hotFixCollectConfig.buildMetaAssemblyParticipatePackaging;
            buildGameRootStart = hotFixCollectConfig.buildGameRootStart;
            GameRootStartPath = hotFixCollectConfig.GameRootStartPath;
            GameRootStartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollectConfig.GameRootStartPath);
            Scene = hotFixCollectConfig.Scene;
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

        #region 打包后数据

        [LabelText("新打包后数据")] public List<string> newBuildHotFixData = new List<string>();

        #endregion

        private HotFixRuntimeDownConfig FindHotFixRuntimeDownConfigByName(string name, List<HotFixRuntimeDownConfig> hotFixRuntimeDownConfigs)
        {
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in hotFixRuntimeDownConfigs)
            {
                if (hotFixRuntimeDownConfig.name == name)
                {
                    return hotFixRuntimeDownConfig;
                }
            }

            return new HotFixRuntimeDownConfig();
        }


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