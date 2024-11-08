using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;

namespace DltFramework
{
    public class GenerateGeneral
    {
        [LabelText("换行")] public static string LineFeed = "\n";

        /// <summary>
        /// 缩进
        /// </summary>
        /// <param name="number">缩进数量</param>
        /// <returns></returns>
        public static string Indents(int number)
        {
            string temp = String.Empty;
            for (int i = 0; i < number; i++)
            {
                temp = DataFrameComponent.String_BuilderString(temp, " ");
            }

            return temp;
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="scriptsContent">原脚本内容</param>
        /// <param name="insertContent">要插入的内容</param>
        /// <param name="insertStartMark">插入开始标记</param>
        /// <param name="insertEndMark">插入结束标记</param>
        /// <returns></returns>
        public static string ReplaceScriptContent(string scriptsContent, string insertContent, string insertStartMark, string insertEndMark)
        {
            if (scriptsContent.Contains(insertStartMark) && scriptsContent.Contains(insertEndMark))
            {
                //开始位置 
                int usingStartIndex = scriptsContent.IndexOf(insertStartMark, StringComparison.Ordinal);
                //结束位置
                int usingEndIndex = scriptsContent.IndexOf(insertEndMark, StringComparison.Ordinal);
                //移除多余空格
                while (scriptsContent[usingEndIndex - 1] == ' ')
                {
                    usingEndIndex -= 1;
                }

                //查找要被替换的内容
                string scriptUsingContent;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < scriptsContent.Length; i++)
                {
                    if (i >= usingStartIndex && i < usingEndIndex)
                    {
                        stringBuilder.Append(scriptsContent[i]);
                    }
                }

                scriptUsingContent = stringBuilder.ToString();

                string tempInsertContent = String.Empty;

                tempInsertContent = DataFrameComponent.String_BuilderString(tempInsertContent, insertContent);
                tempInsertContent = DataFrameComponent.String_BuilderString(tempInsertContent, insertStartMark, "\n", tempInsertContent, "\n");
                //替换新内容
                return scriptsContent.Replace(scriptUsingContent, tempInsertContent);
            }
            else
            {
                return scriptsContent;
            }
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="scriptsContent">原脚本内容</param>
        /// <param name="insertContent">要插入的内容</param>
        /// <param name="insertStartMark">插入开始标记</param>
        /// <param name="insertEndMark">插入结束标记</param>
        /// <returns></returns>
        public static string ReplaceScriptContent(string scriptsContent, List<string> insertContent, string insertStartMark, string insertEndMark)
        {
            string temp = String.Empty;
            for (int i = 0; i < insertContent.Count; i++)
            {
                temp += insertContent[i];
            }

            return ReplaceScriptContent(scriptsContent, temp, insertStartMark, insertEndMark);
        }

        /// <summary>
        /// 获得旧的脚本内容
        /// </summary>
        /// <param name="scriptsName">脚本名称</param>
        /// <returns></returns>
        public static string GetOldScriptsContent(string scriptsName)
        {
            string scriptPath = GetPath(scriptsName);

            string oldScriptContent = FileOperationComponent.GetTextToLoad(scriptPath);
            return oldScriptContent;
        }

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <param name="scriptName">脚本名称</param>
        /// <returns></returns>
        public static string GetPath(string scriptName)
        {
#if UNITY_EDITOR

            string[] path = UnityEditor.AssetDatabase.FindAssets(scriptName);

            for (int i = 0; i < path.Length; i++)
            {
                if (UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]).Contains("Assets") &&
                    UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]).Contains(scriptName + ".cs"))
                {
                    return UnityEditor.AssetDatabase.GUIDToAssetPath(path[i]);
                }
            }

#endif
            return null;
        }
    }
}