using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;

namespace XxSlitFrame.Tools
{
    public class DoCreateScriptAsset : EndNameEditAction
    {
        private GenerateBaseWindowData _generateBaseWindowData;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var className = Path.GetFileNameWithoutExtension(pathName);
            _generateBaseWindowData =
                AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            className = className.Replace(" ", "");

            if (resourceFile == General.BaseWindowTemplatePath)
            {
                text = text.Replace("BaseWindowTemplate", className);
            }

            if (resourceFile == General.ChildBaseWindowTemplatePath)
            {
                text = text.Replace("ChildBaseWindowTemplate", className);
            }

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