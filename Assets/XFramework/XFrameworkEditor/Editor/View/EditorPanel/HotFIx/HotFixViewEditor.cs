using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class HotFixViewEditor : BaseEditor
    {
        [TabGroup("HotFix", "HotFixCode")] [LabelText("HotFixCode")]
        public Object HotFixCode;

        [TabGroup("HotFix", "HotFixView")] [LabelText("HotFixViewPrefab")]
        public Object HotFixView;

        [TabGroup("HotFix", "HotFixCode")] [LabelText("HotFixCode配置输出路径")] [FolderPath]
        public string HotFixCodeConfigOutPath;

        [TabGroup("HotFix", "HotFixView")] [LabelText("HotFixView配置输出路径")] [FolderPath]
        public string HotFixViewConfigOutPath;

        [TabGroup("HotFix", "HotFixCode")]
        [Button("HotFixCode配置输出")]
        public void HotFixCodeConfigOut()
        {
            Debug.Log("HotFixCode配置输出");
            if (HotFixCode == null || !Directory.Exists(HotFixCodeConfigOutPath))
            {
                Debug.Log("配置信息错误");
                return;
            }

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "XFrameworkHotFix.dll.bytes";
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(HotFixCode));
            hotFixAssetConfig.size = FileOperation.GetFileSize(AssetDatabase.GetAssetPath(HotFixCode)).ToString();
            FileOperation.SaveTextToLoad(HotFixCodeConfigOutPath + "/" + "HotFixCodeConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
        }

        [TabGroup("HotFix", "HotFixView")]
        [Button("HotFixView配置输出")]
        public void HotFixViewConfigOut()
        {
            if (HotFixView == null || !Directory.Exists(HotFixViewConfigOutPath))
            {
                Debug.Log("配置信息错误");
                return;
            }

            Debug.Log("HotFixView配置输出");

            HotFixAssetConfig hotFixAssetConfig = new HotFixAssetConfig();
            hotFixAssetConfig.name = "hotfixview"; //ab包打包后自带转换成小写
            hotFixAssetConfig.md5 = FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(HotFixView));
            hotFixAssetConfig.size = FileOperation.GetFileSize(AssetDatabase.GetAssetPath(HotFixView)).ToString();
            FileOperation.SaveTextToLoad(HotFixViewConfigOutPath + "/" + "HotFixViewConfig.json", JsonUtility.ToJson(hotFixAssetConfig));
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
        }
    }
}