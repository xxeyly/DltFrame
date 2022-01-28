using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace XFramework
{
    public class CreateTemplateScript : EndNameEditAction
    {
        private GenerateBaseWindowData _generateBaseWindowData;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var className = Path.GetFileNameWithoutExtension(pathName);
            _generateBaseWindowData =
                AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            className = className.Replace(" ", "");


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

            if (resourceFile == General.BaseWindowTemplatePath)
            {
                text = text.Replace("BaseWindowTemplate", className);
                text = text.Replace("StartCustomAttributesStart", _generateBaseWindowData.startCustomAttributesStart);
                text = text.Replace("EndCustomAttributesStart", _generateBaseWindowData.endCustomAttributesStart);
            }

            if (resourceFile == General.ChildBaseWindowTemplatePath)
            {
                text = text.Replace("ChildBaseWindowTemplate", className);
            }

            if (resourceFile == General.CircuitBaseDataTemplatePath)
            {
                text = text.Replace("CircuitBaseDataTemplate", className);
            }

            if (resourceFile == General.ListenerComponentDataTemplatePath)
            {
                text = text.Replace("ListenerComponentDataTemplate", "ListenerComponent");
            }

            if (resourceFile == General.SceneComponentTemplatePath)
            {
                text = text.Replace("SceneComponentTemplate", className);
            }

            if (resourceFile == General.AnimatorControllerParameterDataTemplatePath)
            {
                text = text.Replace("AnimatorControllerParameterDataTemplate", "AnimatorControllerData");
            }


            //utf8
            var encoding = new UTF8Encoding(true, false);

            File.WriteAllText(pathName, text, encoding);

            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }
}