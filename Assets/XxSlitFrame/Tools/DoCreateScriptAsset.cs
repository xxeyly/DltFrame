using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;

namespace XxSlitFrame.Tools.Editor
{
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEditor.ProjectWindowCallback;
    using System;

    public class DoCreateScriptAsset : EndNameEditAction
    {
        private GenerateBaseWindowData _generateBaseWindowData;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var className = Path.GetFileNameWithoutExtension(pathName);
            //清除空格
            CustomScriptableObject customScriptableObject = new CustomScriptableObject();
            _generateBaseWindowData =
                AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(customScriptableObject.generateBaseWindowPath);
            className = className.Replace(" ", "");

            text = text.Replace("BaseWindowTemplate", className);
            text = text.Replace("StartUsing", _generateBaseWindowData.startUsing);
            text = text.Replace("EndUsing", _generateBaseWindowData.endUsing);
            text = text.Replace("StartUIVariable", _generateBaseWindowData.startUiVariable);
            text = text.Replace("EndUIVariable", _generateBaseWindowData.endUiVariable);
            text = text.Replace("StartVariableBindPath", _generateBaseWindowData.startVariableBindPath);
            text = text.Replace("EndVariableBindPath", _generateBaseWindowData.endVariableBindPath);
            text = text.Replace("StartVariableBindListener", _generateBaseWindowData.startVariableBindListener);
            text = text.Replace("EndVariableBindListener", _generateBaseWindowData.endVariableBindListener);
            text = text.Replace("StartVariableBindEvent", _generateBaseWindowData.startVariableBindEvent);
            text = text.Replace("EndVariableBindEvent", _generateBaseWindowData.endVariableBindEvent);

            //utf8
            var encoding = new UTF8Encoding(true, false);

            File.WriteAllText(pathName, text, encoding);

            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }
}