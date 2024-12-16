#if UNITY_EDITOR

using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    public class GameRootEditor : BaseEditor
    {
        [BoxGroup("Export")] [LabelText("导出路径")] [FolderPath(AbsolutePath = true)]
        public string ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        [BoxGroup("Export")] [LabelText("导出版本")]
        public string ExportValue;

        [BoxGroup("Export")]
        [Button("导出框架", ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void ExportFrameToDesktop()
        {
            AssetDatabase.ExportPackage("Assets/" + "DltFramework", string.Format(ExportPath + "/DltFramework{0}.unitypackage", ExportValue), ExportPackageOptions.Recurse);
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
        }

        public override void OnLoadConfig()
        {
            ExportValue = File.ReadAllLines(Application.dataPath + "/DltFramework/README.md")[0].Replace("当前版本", "");
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}
#endif