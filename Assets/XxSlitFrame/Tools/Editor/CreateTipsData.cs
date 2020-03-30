using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using XxSlitFrame.ConfigData;

namespace XxSlitFrame.Tools.Editor
{
    public class CreateTipsData : UnityEditor.Editor
    {
        [MenuItem("xxslit/创建提示数据")]
        static void CreateData()
        {
            ScriptableObject tipData = ScriptableObject.CreateInstance<TipsData>();

            // 自定义资源保存路径
            string path = Application.dataPath + "XxSlitFrame/Resources";

            // 如果项目总不包含该路径，创建一个
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //将类名 Bullet 转换为字符串
            //拼接保存自定义资源（.asset） 路径
            path = $"Assets/XxSlitFrame/Resources/{"TipsData"}.asset";

            // 生成自定义资源到指定路径
            AssetDatabase.CreateAsset(tipData, path);
        }
    }
}