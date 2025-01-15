#if UNITY_EDITOR

using System;
using System.Collections.Generic;
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
            List<string> readme = new List<string>(File.ReadAllLines(Application.dataPath + "/../README.md"));
            for (int i = 0; i < readme.Count; i++)
            {
                if (readme[i].Contains("当前版本: "))
                {
                    ExportValue = readme[i].Replace("当前版本: ", "").Replace(" ", "");
                }
            }
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}
#endif