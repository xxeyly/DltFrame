using System;
using System.Collections.Generic;
using System.IO;
#if HybridCLR
using HybridCLR.Editor.Commands;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XFramework;

public class AssetBundleManager : BaseEditor
{
#if UNITY_EDITOR

    [LabelText("打包存放地址")] [FolderPath] public string buildSavePath;
    [LabelText("打包压缩方式")] public BuildAssetBundleOptions targetBuildAssetBundleOptions = BuildAssetBundleOptions.None;
    [LabelText("Assembly打包")] public bool AssemblyParticipatePackaging;
    [LabelText("MetaAssembly打包")] public bool MetaAssemblyParticipatePackaging;

    /*[LabelText("Assembly-路径可不填")] [TableList]
    public List<BundleFileConfig> AssemblyAssetDirectoryConfig;
    [LabelText("元数据")] [TableList] public List<BundleDirectoryConfig> MetaAssembly;*/

    [LabelText("框架")] [TableList] public List<BundleFileConfig> GameRootStartBundleDirectoryConfig;


    [HorizontalGroup("打包", width: 100)]
    [Button("清空打包勾选")]
    public void ClearAll()
    {
        ClearBundleFileConfigPackaging(GameRootStartBundleDirectoryConfig);
        // ClearBundleDirectoryConfigPackaging(MetaAssembly);
        AssetBundle.UnloadAllAssetBundles(true);
    }


    [HorizontalGroup("打包", width: 100)]
    [Button("全选打包勾选")]
    public void SelectAll()
    {
        AllSelectBundleFileConfigPackaging(GameRootStartBundleDirectoryConfig);
        // AllSelectBundleDirectoryConfigPackaging(MetaAssembly);
        AssetBundle.UnloadAllAssetBundles(true);
    }

    private void ClearBundleDirectoryConfigPackaging(List<BundleDirectoryConfig> bundleDirectoryConfigs)
    {
        foreach (BundleDirectoryConfig bundleDirectoryConfig in bundleDirectoryConfigs)
        {
            bundleDirectoryConfig.participatePackaging = false;
        }
    }

    private void AllSelectBundleDirectoryConfigPackaging(List<BundleDirectoryConfig> bundleDirectoryConfigs)
    {
        foreach (BundleDirectoryConfig bundleDirectoryConfig in bundleDirectoryConfigs)
        {
            bundleDirectoryConfig.participatePackaging = true;
        }
    }

    private void ClearBundleFileConfigPackaging(List<BundleFileConfig> bundleFileConfigs)
    {
        foreach (BundleFileConfig bundleFileConfig in bundleFileConfigs)
        {
            bundleFileConfig.participatePackaging = false;
        }
    }

    private void AllSelectBundleFileConfigPackaging(List<BundleFileConfig> bundleFileConfigs)
    {
        foreach (BundleFileConfig bundleFileConfig in bundleFileConfigs)
        {
            bundleFileConfig.participatePackaging = true;
        }
    }


    [HorizontalGroup("打包")]
    [Button("生成文件配置信息")]
    public void GenerateFileConfig()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        OnSaveConfig();
        //热更文件
        if (AssemblyParticipatePackaging)
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            string platformName = string.Empty;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    platformName = "StandaloneWindows64";
                    break;
                case BuildTarget.WSAPlayer:
                    platformName = "WSAPlayer";
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
            hotFixAssemblyConfig.Md5 = FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes");
            hotFixAssemblyConfig.Name = "Assembly-CSharp.dll.bytes";
            hotFixAssemblyConfig.Size = FileOperation.GetFileSize(Application.streamingAssetsPath + "/HotFixRuntime/Assembly/" + "Assembly-CSharp.dll.bytes").ToString();
            hotFixAssemblyConfig.Path = "HotFixRuntime/Assembly/";
            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFixRuntime/AssemblyConfig", "AssemblyConfig.json", JsonMapper.ToJson(hotFixAssemblyConfig));
            Debug.Log("移动完毕");
        }

        //框架
        foreach (BundleFileConfig bundleFileConfig in GameRootStartBundleDirectoryConfig)
        {
            if (!bundleFileConfig.participatePackaging)
            {
                continue;
            }

            if (!Directory.Exists("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFixRuntime/GameRootStartAssetBundle");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            AssetImporter gameRootStartImporter = null;
            gameRootStartImporter = AssetImporter.GetAtPath(bundleFileConfig.filePath);
            gameRootStartImporter.assetBundleName = "GameRootStartAssetBundle/GameRootStart";
            BuildPipeline.BuildAssetBundles(buildSavePath, targetBuildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
            DataFrameComponent.RemoveAllAssetBundleName();
            File.Delete(buildSavePath + "/" + "GameRootStartAssetBundle/gamerootstart.manifest");
            UnityEditor.AssetDatabase.Refresh();

            HotFixRuntimeDownConfig hotFixGameRootStartConfig = new HotFixRuntimeDownConfig();
            hotFixGameRootStartConfig.Md5 = FileOperation.GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart");
            hotFixGameRootStartConfig.Name = "gamerootstart";
            hotFixGameRootStartConfig.Size = FileOperation.GetFileSize(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundle/" + "gamerootstart").ToString();
            hotFixGameRootStartConfig.Path = "HotFixRuntime/GameRootStartAssetBundle/";
            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFixRuntime/GameRootStartAssetBundleConfig", "GameRootStartConfig.json", JsonMapper.ToJson(hotFixGameRootStartConfig));
        }

        //元数据
        if (MetaAssemblyParticipatePackaging)
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
                    case BuildTarget.WSAPlayer:
                        platformName = "WSAPlayer";
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
                    Name = DataFrameComponent.GetPathFileNameDontContainFileType(path) + ".bytes",
                    Md5 = FileOperation.GetMD5HashFromFile(path),
                    Path = "HotFix/Metadata/",
                    Size = FileOperation.GetFileSize(path).ToString()
                });
            }

            FileOperation.SaveTextToLoad(Application.streamingAssetsPath + "/HotFix/MetadataConfig", "MetadataConfig.json", JsonMapper.ToJson(hotFixMetaAssemblyConfigs));
        }

        OnLoadConfig();
        AssetDatabase.Refresh();
        DataFrameComponent.RemoveAllAssetBundleName();
    }

    private void OnTextureCompressByPath(List<string> texturePath)
    {
        foreach (string path in texturePath)
        {
            OnTextureCompressByPath(path);
        }
    }

    private void OnTextureCompressByPath(string texturePath)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (textureImporter != null && (textureImporter.textureType == TextureImporterType.NormalMap ||
                                        textureImporter.textureType == TextureImporterType.Sprite ||
                                        textureImporter.textureType == TextureImporterType.Default))
        {
            textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings()
            {
                maxTextureSize = 1024,
                compressionQuality = 50,
                name = "Standalone",
                overridden = true,
                format = TextureImporterFormat.DXT5Crunched
            });

            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            AssetDatabase.ImportAsset(texturePath);
        }
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
        FileOperation.SaveTextToLoad(Application.dataPath + "/Config/BuildAssetBundleConfig.json", JsonMapper.ToJson(this));
    }

    public override void OnLoadConfig()
    {
        if (File.Exists(Application.dataPath + "/Config/BuildAssetBundleConfig.json"))
        {
            AssetBundleManager loadAssetBundleManager = JsonMapper.ToObject<AssetBundleManager>(FileOperation.GetTextToLoad(Application.dataPath + "/Config/BuildAssetBundleConfig.json"));
            buildSavePath = loadAssetBundleManager.buildSavePath;
            targetBuildAssetBundleOptions = loadAssetBundleManager.targetBuildAssetBundleOptions;
            GameRootStartBundleDirectoryConfig = loadAssetBundleManager.GameRootStartBundleDirectoryConfig;
        }
    }

    public override void OnInit()
    {
    }
   
}


[Serializable]
public class BundleDirectoryConfig
{
    [HorizontalGroup("文件夹路径")] [HideLabel] [FolderPath]
    public string folderPath;

    [HorizontalGroup("打包", Width = 20)] [HideLabel] [TableColumnWidth(width: 40, resizable: false)]
    public bool participatePackaging;

    [HorizontalGroup("资源优化")] [HideLabel] [TableColumnWidth(width: 100, resizable: false)]
    public bool compress = false;
}

[Serializable]
public class BundleFileConfig
{
    [HorizontalGroup("文件路径")] [HideLabel] [Sirenix.OdinInspector.FilePath]
    public string filePath;

    [HorizontalGroup("打包", Width = 20)] [HideLabel] [TableColumnWidth(width: 40, resizable: false)]
    public bool participatePackaging;

    [HorizontalGroup("资源优化")] [HideLabel] [TableColumnWidth(width: 100, resizable: false)]
    public bool compress = false;
}
#endif