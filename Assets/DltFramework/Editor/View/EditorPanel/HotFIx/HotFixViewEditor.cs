#if HybridCLR
using System;
using System.IO;
using Aot;
using HybridCLR.Editor.Commands;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DltFramework
{
    public class HotFixViewEditor : BaseEditor
    {
        [TabGroup("HotFix", "HotFixView")] [LabelText("HotFixViewPrefab")]
        public Object HotFixView;

        [LabelText("HotFixCode路径-保存读取用")] [HideInInspector]
        public string HotFixViewFilePath;


        [TabGroup("HotFix", "HotFixCode")]
        [Button("HotFixCode配置输出")]
        public void HotFixCodeConfigOut()
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

            if (!Directory.Exists("Assets/StreamingAssets/HotFix/HotFixCode"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets/HotFix/HotFixCode");
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            File.Copy(DataFrameComponent.Path_GetParentDirectory(Application.dataPath, 1) + "/HybridCLRData/HotUpdateDlls/" + platformName + "/HotFixCode.dll",
                Application.streamingAssetsPath + "/HotFix/HotFixCode/" + "HotFixCode.dll.bytes", true);
            string path = "Assets/StreamingAssets/HotFix/HotFixCode/HotFixCode.dll.bytes";

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "HotFixCode.dll.bytes";
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(path);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(path).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixCode/";
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixCodeConfig/" + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            Debug.Log("HotFixCode配置输出");

            OnLoadConfig();
        }

        [TabGroup("HotFix", "HotFixView")]
        [Button("HotFixView配置输出")]
        public void HotFixViewConfigOut()
        {

            if (HotFixView == null)
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

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixView));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/hotfixview";
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            DataFrameComponent.RemoveAllAssetBundleName();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("HotFixView配置输出");

            string filePath = "Assets/StreamingAssets/HotFix/HotFixView/" + "hotfixview";
            File.Delete(filePath + ".manifest");
            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "hotfixview"; //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperationComponent.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperationComponent.GetFileSize(filePath).ToString();
            hotFixAssetConfig.path = "HotFix/HotFixView/";
            FileOperationComponent.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixViewConfig/" + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
            OnLoadConfig();
        }

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
            HotFixViewFilePath = AssetDatabase.GetAssetPath(HotFixView);
            FileOperationComponent.SaveTextToLoad(RuntimeGlobal.assetRootPath, "HotFixView.json", JsonUtility.ToJson(this));
        }

        public override void OnLoadConfig()
        {
            if (!File.Exists(RuntimeGlobal.assetRootPath + "HotFixView.json"))
            {
                return;
            }

            HotFixViewEditor hotFixViewEditor = JsonMapper.ToObject<HotFixViewEditor>(FileOperationComponent.GetTextToLoad(RuntimeGlobal.assetRootPath, "HotFixView.json"));
            this.HotFixView = AssetDatabase.LoadAssetAtPath<Object>(hotFixViewEditor.HotFixViewFilePath);
        }

        public override void OnInit()
        {
        }
    }
}
#endif