using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class HotFixViewEditor : BaseEditor
    {
        [LabelText("HotFixCode路径-保存读取用")] [HideInInspector]
        public string HotFixCodeFilePath;

        [TabGroup("HotFix", "HotFixView")] [LabelText("HotFixViewPrefab")]
        public Object HotFixView;

        [LabelText("HotFixCode路径-保存读取用")] [HideInInspector]
        public string HotFixViewFilePath;


        [TabGroup("HotFix", "HotFixCode")]
        [Button("HotFixCode配置输出")]
        public void HotFixCodeConfigOut()
        {
            Debug.Log("HotFixCode配置输出");
            File.Copy(DataFrameComponent.GetCombine(Application.dataPath, 0) + "/HybridCLRData/HotUpdateDlls/StandaloneWindows64/XFrameworkHotFix.dll",
                Application.streamingAssetsPath + "/HotFix/HotFixCode/" + "XFrameworkHotFix.dll.bytes", true);
            string path = "Assets/StreamingAssets/HotFix/HotFixCode/XFrameworkHotFix.dll.bytes";

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "XFrameworkHotFix.dll.bytes";
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(path);
            hotFixAssetConfig.size = FileOperation.GetFileSize(path).ToString();
            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixCodeConfig/" + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
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

            AssetImporter hotFixViewImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(HotFixView));
            hotFixViewImporter.assetBundleName = "HotFix/HotFixView/hotfixview";
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
            DataFrameComponent.RemoveAllAssetBundleName();
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("HotFixView配置输出");

            string filePath = "Assets/StreamingAssets/HotFix/HotFixView/" + "hotfixview";
            File.Delete(filePath + ".manifest");
            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "hotfixview"; //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(filePath);
            hotFixAssetConfig.size = FileOperation.GetFileSize(filePath).ToString();
            FileOperation.SaveTextToLoad("Assets/StreamingAssets/HotFix/HotFixViewConfig/" + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
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
            FileOperation.SaveTextToLoad(General.assetRootPath, "HotFixView.json", JsonUtility.ToJson(this));
        }

        public override void OnLoadConfig()
        {
            HotFixViewEditor hotFixViewEditor = JsonUtility.FromJson<HotFixViewEditor>(FileOperation.GetTextToLoad(General.assetRootPath, "HotFixView.json"));
            this.HotFixView = AssetDatabase.LoadAssetAtPath<Object>(hotFixViewEditor.HotFixViewFilePath);
        }

        public override void OnInit()
        {
        }
    }
}