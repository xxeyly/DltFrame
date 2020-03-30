using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using XxSlitFrame.ConfigData;

namespace XxSlitFrame.Tools.Editor
{
    public class CreateDialogueData : UnityEditor.Editor
    {
        // 在菜单栏创建对话数据
        // [MenuItem("xxslit/创建对话数据")]
        static void CreateData()
        {
            ScriptableObject dialogueData = ScriptableObject.CreateInstance<DialogueData>();

            // 自定义资源保存路径
            string path = Application.dataPath + "XxSlitFrame/Resources";

            // 如果项目总不包含该路径，创建一个
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //拼接保存自定义资源（.asset） 路径
            path = $"Assets/XxSlitFrame/Resources/{"DialogueData"}.asset";

            // 生成自定义资源到指定路径
            AssetDatabase.CreateAsset(dialogueData, path);
        }
    }
}