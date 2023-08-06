#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace XFramework
{
    public class CreateTemplateScript : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);
            var className = Path.GetFileNameWithoutExtension(pathName);
            className = className.Replace(" ", "");
            text = text.Replace("#region StartUsing", GenerateBaseWindowData.startUsing);
            text = text.Replace("#endregion EndUsing", GenerateBaseWindowData.endUsing);
            text = text.Replace("#region StartUIVariable", GenerateBaseWindowData.startUiVariable);
            text = text.Replace("#endregion EndUIVariable", GenerateBaseWindowData.endUiVariable);
            text = text.Replace("#region StartVariableBindPath", GenerateBaseWindowData.startVariableBindPath);
            text = text.Replace("#endregion EndVariableBindPath", GenerateBaseWindowData.endVariableBindPath);
            text = text.Replace("#region StartVariableBindListener", GenerateBaseWindowData.startVariableBindListener);
            text = text.Replace("#endregion EndVariableBindListener", GenerateBaseWindowData.endVariableBindListener);
            text = text.Replace("#region StartVariableBindEvent", GenerateBaseWindowData.startVariableBindEvent);
            text = text.Replace("#endregion EndVariableBindEvent", GenerateBaseWindowData.endVariableBindEvent);

            if (resourceFile == General.BaseWindowTemplatePath)
            {
                text = text.Replace("BaseWindowTemplate", className);
                text = text.Replace("#region StartCustomAttributesStart", GenerateBaseWindowData.startCustomAttributesStart);
                text = text.Replace("#endregion EndCustomAttributesStart", GenerateBaseWindowData.endCustomAttributesStart);
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

            if (resourceFile == General.SceneComponentInitTemplatePath)
            {
                text = text.Replace("SceneComponentInitTemplate", className);
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
#endif