using System;
using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor.Commands;
using LitJson;
using Sirenix.OdinInspector;
using Unity.Plastic.Antlr3.Runtime.Misc;
using Unity.VisualScripting.IonicZip;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;


namespace XFramework
{
    public class HotFixCollect : BaseEditor
    {
        [LabelText("打包压缩方式")] [LabelWidth(120)] [EnumPaging]
        public BuildAssetBundleOptions targetBuildAssetBundleOptions;

        [LabelText("更新数据下载地址")] [LabelWidth(120)]
        public string HotFixDownPath;

        [LabelText("本地是否开启更新")] public bool localIsUpdate = false;

        [InfoBox("打包后会拷贝所有资源到当前路径下,并且还会复制一份以当前版本号的为名的备份文件夹")] [LabelText("资源外部拷贝路径")] [LabelWidth(120)] [FolderPath(AbsolutePath = true)]
        public string targetOutPath;

        [InfoBox("当勾选版本号自增,会默认勾选所有资源,并且资源版本号+1")] [LabelText("版本号自增")] [LabelWidth(120)]
        public bool isUpdateVersion = false;

        [InfoBox("版本号分3个等级,\n等级1是特大改动(需要手动更改),\n等级2每次需要重新出包时变动(需要手动更改),\n等级3是每次小更新时变动(勾选版本自增每次打包会自增1)")] [LabelText("当前资源版本号")] [LabelWidth(120)]
        public Vector3 targetResourceVersion;

        [ToggleGroup("HotFixView")] public bool HotFixView = false;

        [ToggleGroup("HotFixView")] [LabelText("HotFixView预制体")]
        public GameObject HotFixViewPrefab;

        [ToggleGroup("HotFixView")] [LabelText("HotFixView预制体路径")] [HideInInspector]
        public string HotFixViewPrePath;

        [ToggleGroup("HotFixCode")] public bool HotFixCode = false;

        [ToggleGroup("MetaAssemblyParticipatePackaging", "MetaAssembly")]
        public bool MetaAssemblyParticipatePackaging;

        [ToggleGroup("AssemblyParticipatePackaging", "Assembly")]
        public bool AssemblyParticipatePackaging;


        [ToggleGroup("GameRootStart")] public bool GameRootStart;

        [ToggleGroup("GameRootStart")] [LabelText("GameRootStart预制体")]
        public GameObject GameRootStartPrefab;

        [ToggleGroup("GameRootStart")] [LabelText("GameRootStart预制体路径")] [Sirenix.OdinInspector.FilePath] [HideInInspector]
        public string GameRootStartPath;

        [ToggleGroup("Scene", "场景打包")] public bool Scene;

        [ToggleGroup("Scene")] [LabelText("拷贝场景配置")]
        public List<CopySceneAssetBundleAsset> CopySceneAssetBundleAsset = new List<CopySceneAssetBundleAsset>();

        [HideInInspector] [LabelText("CopySceneAssetBundleAsset路径")]
        public List<string> CopySceneAssetBundleAssetPath = new List<string>();

        [ToggleGroup("Scene")] [LabelText("正常场景配置")]
        public List<NormalSceneAssetBundleAsset> NormalSceneAssetBundleAssets = new List<NormalSceneAssetBundleAsset>();

        [HideInInspector] [LabelText("NormalSceneAssetBundleAsset")]
        public List<string> NormalSceneAssetBundleAssetsPath = new List<string>();

        [GUIColor(0, 1, 0)]
        [Button("打包", ButtonSizes.Large)]
        public void OnBuild()
        {
            OnSaveConfig();
            if (isUpdateVersion)
            {
                targetResourceVersion += new Vector3(0, 0, 1);
                OnSaveConfig();
                HotFixViewBuild();
                HotFixCodeBuild();
                AssemblyBuild();
                MetaAssemblyBuild();
                GameRootStartBuild();
                CopySceneBuild();
                NormalSceneBuild();
            }
            else
            {
                if (HotFixView)
                {
                    HotFixViewBuild();
                }

                if (HotFixCode)
                {
                    HotFixCodeBuild();
                }

                if (AssemblyParticipatePackaging)
                {
                    AssemblyBuild();
                }

                if (MetaAssemblyParticipatePackaging)
                {
                    MetaAssemblyBuild();
                }

                if (GameRootStart)
                {
                    GameRootStartBuild();
                }

                if (Scene)
                {
                    CopySceneBuild();
                    NormalSceneBuild();
                }
            }

            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/", "HotFixDownPath.txt", HotFixDownPath);
            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/", "localIsUpdate.txt", localIsUpdate.ToString());

            List<string> hotFixServerResources = new List<string>();
            foreach (NormalSceneAssetBundleAsset normalSceneAssetBundleAsset in NormalSceneAssetBundleAssets)
            {
                hotFixServerResources.Add(normalSceneAssetBundleAsset.name);
            }

            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFixRuntime/", "HotFixServerResourcesCount.json", JsonMapper.ToJson(hotFixServerResources));

            if (File.Exists("Assets/StreamingAssets/HotFixRuntime/HotFixRuntime"))
            {
                File.Delete("Assets/StreamingAssets/HotFixRuntime/HotFixRuntime");
            }

            if (File.Exists("Assets/StreamingAssets/HotFixRuntime/HotFixRuntime.manifest"))
            {
                File.Delete("Assets/StreamingAssets/HotFixRuntime/HotFixRuntime.manifest");
            }

            AssetDatabase.Refresh();
            if (Directory.Exists(targetOutPath))
            {
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixCode", targetOutPath + "/HotFix/HotFixCode");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixCodeConfig", targetOutPath + "/HotFix/HotFixCodeConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixView", targetOutPath + "/HotFix/HotFixView");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixViewConfig", targetOutPath + "/HotFix/HotFixViewConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/Metadata", targetOutPath + "/HotFix/Metadata");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/MetadataConfig", targetOutPath + "/HotFix/MetadataConfig");

                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/Assembly", targetOutPath + "/HotFixRuntime/Assembly");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/AssemblyConfig", targetOutPath + "/HotFixRuntime/AssemblyConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle", targetOutPath + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig", targetOutPath + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle", targetOutPath + "/HotFixRuntime/HotFixAssetBundle");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundleConfig", targetOutPath + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperation.CopyFile("Assets/StreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + "/HotFixRuntime/HotFixServerResourcesCount.json");
                //备份
                string backupVersion = "/Backups_" + targetResourceVersion.x + "." + targetResourceVersion.y + "." + targetResourceVersion.z;
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixCode", targetOutPath + backupVersion + "/HotFix/HotFixCode");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixCodeConfig", targetOutPath + backupVersion + "/HotFix/HotFixCodeConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixView", targetOutPath + backupVersion + "/HotFix/HotFixView");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/HotFixViewConfig", targetOutPath + backupVersion + "/HotFix/HotFixViewConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/Metadata", targetOutPath + backupVersion + "/HotFix/Metadata");
                FileOperation.Copy("Assets/StreamingAssets/HotFix/MetadataConfig", targetOutPath + backupVersion + "/HotFix/MetadataConfig");

                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/Assembly", targetOutPath + backupVersion + "/HotFixRuntime/Assembly");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/AssemblyConfig", targetOutPath + backupVersion + "/HotFixRuntime/AssemblyConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle", targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundle");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig", targetOutPath + backupVersion + "/HotFixRuntime/GameRootStartAssetBundleConfig");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle", targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundle");
                FileOperation.Copy("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundleConfig", targetOutPath + backupVersion + "/HotFixRuntime/HotFixAssetBundleConfig");
                FileOperation.CopyFile("Assets/StreamingAssets/HotFixRuntime/HotFixServerResourcesCount.json", targetOutPath + backupVersion + "/HotFixRuntime/HotFixServerResourcesCount.json");
            }

            OnLoadConfig();
            Debug.Log("资源打包完毕");
        }

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
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

            FileOperation.SaveTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json", JsonUtility.ToJson(this));
        }

        public override void OnLoadConfig()
        {
            if (!File.Exists(RuntimeGlobal.assetRootPath + "HotFixCollect.json"))
            {
                return;
            }

            HotFixCollect hotFixCollect = JsonMapper.ToObject<HotFixCollect>(FileOperation.GetTextToLoad(RuntimeGlobal.assetRootPath, "HotFixCollect.json"));

            this.localIsUpdate = hotFixCollect.localIsUpdate;
            this.HotFixDownPath = hotFixCollect.HotFixDownPath;
            this.targetResourceVersion = hotFixCollect.targetResourceVersion;
            this.targetBuildAssetBundleOptions = hotFixCollect.targetBuildAssetBundleOptions;
            this.targetOutPath = hotFixCollect.targetOutPath;
            this.targetResourceVersion = hotFixCollect.targetResourceVersion;
            this.isUpdateVersion = hotFixCollect.isUpdateVersion;
            this.HotFixView = hotFixCollect.HotFixView;
            this.HotFixViewPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollect.HotFixViewPrePath);
            this.HotFixViewPrePath = hotFixCollect.HotFixViewPrePath;
            this.HotFixCode = hotFixCollect.HotFixCode;
            this.AssemblyParticipatePackaging = hotFixCollect.AssemblyParticipatePackaging;
            this.MetaAssemblyParticipatePackaging = hotFixCollect.MetaAssemblyParticipatePackaging;
            this.GameRootStart = hotFixCollect.GameRootStart;
            this.GameRootStartPath = hotFixCollect.GameRootStartPath;
            this.GameRootStartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(hotFixCollect.GameRootStartPath);
            this.Scene = hotFixCollect.Scene;

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

        [LabelText("打包HotFixView")]
        private void HotFixViewBuild()
        {
            if (HotFixViewPrefab == null)
            {
                Debug.Log("配置信息错误");
                return;
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFix/HotFixView"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFix/HotFixView");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixViewPrefab));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/hotfixview";
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            DataFrameComponent.RemoveAllAssetBundleName();
            UnityEditor.AssetDatabase.Refresh();

            string filePath = "Assets/StreamingAssets/HotFix/HotFixView/" + "hotfixview";
            File.Delete(filePath + ".manifest");
            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "hotfixview"; //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperation.GetFileSize(filePath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixView/";
            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixViewConfig/" + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            Debug.Log("HotFixView配置输出");

            OnLoadConfig();
        }

        private void HotFixCodeBuild()
        {
            //热更新打包
            CompileDllCommand.CompileDllActiveBuildTarget();
            string platformName = string.Empty;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    platformName = "StandaloneWindows";
                    break;
                case BuildTarget.StandaloneWindows64:
                    platformName = "StandaloneWindows64";
                    break;
                case BuildTarget.WSAPlayer:
                    platformName = "WSAPlayer";
                    break;
                case BuildTarget.Android:
                    platformName = "Android";
                    break;
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFix/HotFixCode"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFix/HotFixCode");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            File.Copy(DataFrameComponent.GetCombine(Application.dataPath, 0) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/HotFixCode.dll",
                Application.streamingAssetsPath + "/HotFix/HotFixCode/" + "HotFixCode.dll.bytes", true);
            string path = "Assets/StreamingAssets/HotFix/HotFixCode/HotFixCode.dll.bytes";

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "HotFixCode.dll.bytes";
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(path);
            hotFixAssetConfig.size = FileOperation.GetFileSize(path).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixCode/";
            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixCodeConfig/" + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            Debug.Log("HotFixCode配置输出");
            OnLoadConfig();
        }

        private void AssemblyBuild()
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            string platformName = string.Empty;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    platformName = "StandaloneWindows64";
                    break;
                case BuildTarget.StandaloneWindows64:
                    platformName = "StandaloneWindows64";
                    break;
                case BuildTarget.WSAPlayer:
                    platformName = "WSAPlayer";
                    break;
                case BuildTarget.Android:
                    platformName = "Android";
                    break;
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/Assembly"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/Assembly");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/AssemblyConfig"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/AssemblyConfig");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            File.Copy(DataFrameComponent.GetCombine(Application.dataPath, 0) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/Assembly-CSharp.dll",
                Application.streamingAssetsPath + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes", true);
            HotFixRuntimeDownConfig hotFixAssemblyConfig = new HotFixRuntimeDownConfig();
            hotFixAssemblyConfig.md5 = FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes");
            hotFixAssemblyConfig.name = "Assembly-CSharp.dll.bytes";
            hotFixAssemblyConfig.size = FileOperation.GetFileSize(Application.streamingAssetsPath + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes").ToString();
            hotFixAssemblyConfig.path = "HotFixRuntime/Assembly/";
            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFixRuntime/AssemblyConfig", "AssemblyConfig.json", JsonMapper.ToJson(hotFixAssemblyConfig));
            Debug.Log("移动完毕");
        }

        private void MetaAssemblyBuild()
        {
            if (!Directory.Exists("Assets/StreamingAssets/HotFix/Metadata"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFix/Metadata");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFix/MetadataConfig"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFix/MetadataConfig");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            //移动元文件
            foreach (string metadataName in AOTGenericReferences.PatchedAOTAssemblyList)
            {
                string platformName = string.Empty;
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.StandaloneWindows:
                        platformName = "StandaloneWindows64";
                        break;
                    case BuildTarget.StandaloneWindows64:
                        platformName = "StandaloneWindows64";
                        break;
                    case BuildTarget.WSAPlayer:
                        platformName = "WSAPlayer";
                        break;
                    case BuildTarget.Android:
                        platformName = "Android";
                        break;
                }

                File.Copy(DataFrameComponent.GetCombine(Application.dataPath, 0) + "/HybridCLRData/AssembliesPostIl2CppStrip/" + platformName + "/" + metadataName,
                    Application.streamingAssetsPath + "/HotFix/Metadata/" + metadataName + ".bytes", true);
            }

            List<string> buildPath = DataFrameComponent.GetGetSpecifyPathInAllTypePath("Assets/StreamingAssets/HotFix/Metadata", "bytes");
            List<HotFixRuntimeDownConfig> hotFixMetaAssemblyConfigs = new List<HotFixRuntimeDownConfig>();
            foreach (string path in buildPath)
            {
                hotFixMetaAssemblyConfigs.Add(new HotFixRuntimeDownConfig()
                {
                    name = DataFrameComponent.GetPathFileNameDontContainFileType(path) + ".bytes",
                    md5 = FileOperation.GetMD5HashFromFile(path),
                    path = "HotFix/Metadata/",
                    size = FileOperation.GetFileSize(path).ToString()
                });
            }


            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFix/MetadataConfig", "MetadataConfig.json", JsonMapper.ToJson(hotFixMetaAssemblyConfigs));
        }

        private void GameRootStartBuild()
        {
            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            AssetImporter gameRootStartImporter = null;
            gameRootStartImporter = AssetImporter.GetAtPath(GameRootStartPath);
            gameRootStartImporter.assetBundleName = "GameRootStartAssetBundle/GameRootStart";
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/HotFixRuntime", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);

            DataFrameComponent.RemoveAllAssetBundleName();
            File.Delete("Assets/StreamingAssets/HotFixRuntime" + "/" + "GameRootStartAssetBundle/gamerootstart.manifest");
            UnityEditor.AssetDatabase.Refresh();

            HotFixRuntimeDownConfig hotFixGameRootStartConfig = new HotFixRuntimeDownConfig();
            hotFixGameRootStartConfig.md5 = FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart");
            hotFixGameRootStartConfig.name = "gamerootstart";
            hotFixGameRootStartConfig.size = FileOperation.GetFileSize(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart").ToString();
            hotFixGameRootStartConfig.path = "HotFixRuntime/GameRootStartAssetBundle/";
            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundleConfig", "GameRootStartConfig.json", JsonMapper.ToJson(hotFixGameRootStartConfig));
        }

        private void CopySceneBuild()
        {
            for (int i = 0; i < CopySceneAssetBundleAsset.Count; i++)
            {
                OpenScene(CopySceneAssetBundleAsset[i].name.Replace("Copy", ""));
                BundleSceneDataAndGenerateBuildConfig(CopySceneAssetBundleAsset[i]);
            }
        }

        private void NormalSceneBuild()
        {
            for (int i = 0; i < NormalSceneAssetBundleAssets.Count; i++)
            {
                //打开对应的场景文件
                OpenScene(NormalSceneAssetBundleAssets[i].name);
                BundleSceneDataAndGenerateBuildConfig(NormalSceneAssetBundleAssets[i]);
            }
        }

        //打开场景
        private void OpenScene(string sceneName)
        {
            List<string> allScenePath = DataFrameComponent.GetSpecifyTypeOnlyInAssetsPath("unity");
            for (int i = 0; i < allScenePath.Count; i++)
            {
                if (DataFrameComponent.GetPathFileNameDontContainFileType(allScenePath[i]) == sceneName)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(allScenePath[i]);
                }
            }
        }

        [BoxGroup("打包")]
        [GUIColor(0, 1, 0)]
        public void BundleSceneDataAndGenerateBuildConfig(CopySceneAssetBundleAsset copySceneAssetBundleAsset)
        {
            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.GetAllObjectsInScene<HotFixAssetPathConfig>();
            copySceneAssetBundleAsset.scenePrefabPaths.Clear();
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
            List<string> deleteManifestPath = new ListStack<string>();
            //打包场景其他数据
            foreach (string scenePrefabPath in copySceneAssetBundleAsset.scenePrefabPaths)
            {
                //不管该Prefab是否参与打包,字体肯定要重更新打包
                foreach (string assetDependency in GetAssetDependencies(scenePrefabPath))
                {
                    if (assetDependency.Contains("TMP_SDF.shader") || assetDependency.Contains(".otf") || assetDependency.Contains(".ttf") || assetDependency.Contains(".asset"))
                    {
                        AssetImporter fontAssetImporter = UnityEditor.AssetImporter.GetAtPath(assetDependency);
                        fontAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/Font/" + SceneManager.GetActiveScene().name + "Font";
                        deleteManifestPath.Add("Assets/StreamingAssets/" + fontAssetImporter.assetBundleName + ".manifest");
                    }
                }

                AssetImporter assetImporter = AssetImporter.GetAtPath(scenePrefabPath);
                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
                deleteManifestPath.Add("Assets/StreamingAssets/" + assetImporter.assetBundleName + ".manifest");
            }


            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
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
            DataFrameComponent.RemoveAllAssetBundleName();
        }

        [BoxGroup("打包")]
        [GUIColor(0, 1, 0)]
        public void BundleSceneDataAndGenerateBuildConfig(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/HotFixAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            List<HotFixAssetPathConfig> hotFixAssetPathConfigs = DataFrameComponent.GetAllObjectsInScene<HotFixAssetPathConfig>();
            normalSceneAssetBundleAsset.scenePrefabPaths.Clear();
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

            //删除manifest列表
            List<string> deleteManifestPath = new ListStack<string>();

            //打包当前场景
            //打包场景
            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            UnityEditor.AssetImporter sceneAssetImporter = UnityEditor.AssetImporter.GetAtPath(SceneManager.GetActiveScene().path);
            sceneAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/scene/" + SceneManager.GetActiveScene().name;
            deleteManifestPath.Add("Assets/StreamingAssets/" + sceneAssetImporter.assetBundleName + ".manifest");
            //打包场景其他数据
            foreach (string scenePrefabPath in normalSceneAssetBundleAsset.scenePrefabPaths)
            {
                //不管该Prefab是否参与打包,字体肯定要重更新打包
                foreach (string assetDependency in GetAssetDependencies(scenePrefabPath))
                {
                    if (assetDependency.Contains("TMP_SDF.shader") || assetDependency.Contains(".otf") || assetDependency.Contains(".ttf") || assetDependency.Contains(".asset"))
                    {
                        AssetImporter fontAssetImporter = UnityEditor.AssetImporter.GetAtPath(assetDependency);
                        fontAssetImporter.assetBundleName = "HotFixRuntime/HotFixAssetBundle/" + SceneManager.GetActiveScene().name + "/Font/" + SceneManager.GetActiveScene().name + "Font";
                        deleteManifestPath.Add("Assets/StreamingAssets/" + fontAssetImporter.assetBundleName + ".manifest");
                    }
                }

                AssetImporter assetImporter = AssetImporter.GetAtPath(scenePrefabPath);
                HotFixAssetPathConfig hotFixAssetPathConfig = AssetDatabase.LoadAssetAtPath<HotFixAssetPathConfig>(scenePrefabPath);
                assetImporter.assetBundleName = hotFixAssetPathConfig.assetBundlePath;
                deleteManifestPath.Add("Assets/StreamingAssets/" + assetImporter.assetBundleName + ".manifest");
            }


            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
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
            ShowSceneObject(normalSceneAssetBundleAsset.scenePrefabPaths);
            //生成场景配置表
            GenerateBuildConfig(normalSceneAssetBundleAsset);
            DataFrameComponent.RemoveAllAssetBundleName();
        }

        [LabelText("获得资源路径的所有资源依赖")]
        private static string[] GetAssetDependencies(string assetPath)
        {
            if (!File.Exists(assetPath))
            {
                return null;
            }

            string[] dependecies = UnityEditor.AssetDatabase.GetDependencies(assetPath);
            return dependecies;
        }

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

        private void GenerateBuildConfig(CopySceneAssetBundleAsset copySceneAssetBundleAsset)
        {
            string sceneName = DataFrameComponent.AllCharToLower(SceneManager.GetActiveScene().name);
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //字体
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


            //场景AssetBundle
            foreach (string scenePrefabPath in copySceneAssetBundleAsset.scenePrefabPaths)
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

            string hotFixAssetBundleConfigPath = Application.streamingAssetsPath + "/HotFixRuntime/HotFixAssetBundleConfig/" + DataFrameComponent.AllCharToLower(SceneManager.GetActiveScene().name) + ".json";
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                File.Delete(hotFixAssetBundleConfigPath);
            }

            FileOperation.SaveTextToLoad(hotFixAssetBundleConfigPath, JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
            Debug.Log("打包 配置信息完成");
        }

        [LabelText("生成配置信息")]
        private void GenerateBuildConfig(NormalSceneAssetBundleAsset normalSceneAssetBundleAsset)
        {
            string sceneName = DataFrameComponent.AllCharToLower(SceneManager.GetActiveScene().name);
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = new HotFixRuntimeSceneAssetBundleConfig();
            //场景数据
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleName = sceneName;
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/";
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleSize =
                FileOperation.GetFileSize(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName).ToString();
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.md5 =
                FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + sceneName + "/scene/" + sceneName);
            hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleInstantiatePath = "";
            //字体
            if (normalSceneAssetBundleAsset.copySceneAssetBundleAsset == null)
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
                string copySceneName = DataFrameComponent.AllCharToLower(normalSceneAssetBundleAsset.copySceneAssetBundleAsset.name.Replace("Copy", ""));
                //场景字体
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleName = copySceneName + "font";
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundlePath = "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/";
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.assetBundleSize =
                    FileOperation.GetFileSize(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/" + copySceneName + "font").ToString();
                hotFixRuntimeSceneAssetBundleConfig.sceneFontFixRuntimeAssetConfig.md5 =
                    FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/HotFixAssetBundle/" + copySceneName + "/font/" + copySceneName + "font");

                //场景AssetBundle
                foreach (string scenePrefabPath in normalSceneAssetBundleAsset.copySceneAssetBundleAsset.scenePrefabPaths)
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
            }


            //场景AssetBundle
            foreach (string scenePrefabPath in normalSceneAssetBundleAsset.scenePrefabPaths)
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

            string hotFixAssetBundleConfigPath = Application.streamingAssetsPath + "/HotFixRuntime/HotFixAssetBundleConfig/" + DataFrameComponent.AllCharToLower(SceneManager.GetActiveScene().name) + ".json";
            if (File.Exists(hotFixAssetBundleConfigPath))
            {
                File.Delete(hotFixAssetBundleConfigPath);
            }

            FileOperation.SaveTextToLoad(hotFixAssetBundleConfigPath, JsonUtility.ToJson(hotFixRuntimeSceneAssetBundleConfig));
        }
    }
}