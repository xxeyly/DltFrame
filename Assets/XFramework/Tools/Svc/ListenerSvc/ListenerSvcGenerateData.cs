using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class ListenerSvcGenerateData : MonoBehaviour
    {
        private Dictionary<string, Dictionary<string, List<string>>> _callDic = new Dictionary<string, Dictionary<string, List<string>>>();
        private Dictionary<string, string> _allScriptsContentDic;
        private List<string> _allScriptsPath;

        private string GenerationMethod(Dictionary<string, Dictionary<string, List<string>>> method2)
        {
            Dictionary<string, List<string>> group = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> pair in method2)
            {
                List<string> group2 = new List<string>();
                foreach (KeyValuePair<string, List<string>> valuePair in pair.Value)
                {
                    string methodName = String.Empty;
                    string parameter = String.Empty;
                    string parameter2 = String.Empty;
                    parameter = String.Empty;
                    methodName = GenerateGeneral.Indents(12) + "public void " + valuePair.Key + "(";
                    for (int i = 0; i < valuePair.Value.Count; i++)
                    {
                        if (i == valuePair.Value.Count - 1)
                        {
                            parameter += valuePair.Value[i] + " " + "arg" + i;
                        }
                        else
                        {
                            parameter += valuePair.Value[i] + " " + "arg" + i + ",";
                        }
                    }

                    methodName += parameter + ")" + GenerateGeneral.LineFeed + GenerateGeneral.Indents(12) + "{" + GenerateGeneral.LineFeed;
                    parameter2 = String.Empty;
                    for (int i = 0; i < valuePair.Value.Count; i++)
                    {
                        if (i == valuePair.Value.Count - 1)
                        {
                            parameter2 += "arg" + i;
                        }
                        else
                        {
                            parameter2 += "arg" + i + ",";
                        }
                    }

                    string temp3 = String.Empty;
                    if (parameter2 == String.Empty)
                    {
                        temp3 = String.Empty;
                    }
                    else
                    {
                        temp3 = ",";
                    }

                    methodName += GenerateGeneral.Indents(16) + "Instance.ExecuteEvent(\"" + valuePair.Key + "\"" + temp3 + parameter2 + ");" + GenerateGeneral.LineFeed;
                    methodName += GenerateGeneral.Indents(12) + "}" + GenerateGeneral.LineFeed;
                    group2.Add(methodName);
                }


                group.Add(pair.Key, group2);
            }

            string tt = String.Empty;
            foreach (KeyValuePair<string, List<string>> pair in group)
            {
                tt += GenerateGeneral.Indents(8) + "public " + pair.Key + " " + DataSvc.FirstCharToLower(pair.Key) + " = new " + pair.Key + "()" + ";" + GenerateGeneral.LineFeed;
            }

            foreach (KeyValuePair<string, List<string>> pair in group)
            {
                tt += GenerateGeneral.Indents(8) + "public class " + pair.Key + GenerateGeneral.LineFeed;
                tt += GenerateGeneral.Indents(8) + "{" + GenerateGeneral.LineFeed;
                foreach (string moth in pair.Value)
                {
                    tt += moth;
                }

                tt += GenerateGeneral.Indents(8) + "}" + GenerateGeneral.LineFeed;
            }


            return tt;
        }


        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void OnGenerate()
        {
            string listenerSvcDataPath = GenerateGeneral.GetPath("ListenerSvcData");
            if (listenerSvcDataPath == null)
            {
                Debug.LogWarning("ListenerSvcData脚本未创建");
                return;
            }

            _callDic = new Dictionary<string, Dictionary<string, List<string>>>();
            string path = string.Format("{0}", Application.dataPath + "/Scripts");

            _allScriptsContentDic = new Dictionary<string, string>();
            _allScriptsPath = new List<string>();
            //获取指定路径下面的所有资源文件  
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    //忽略关联文件
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }

                    _allScriptsPath.Add(files[i].Name);
                    _allScriptsContentDic.Add(files[i].Name.Replace(".cs", ""), FileOperation.GetTextToLoad(FileOperation.ConvertToLocalPath(files[i].FullName)));
                }
            }

            foreach (KeyValuePair<string, string> pair in _allScriptsContentDic)
            {
                if (pair.Value.Contains("AddListenerEvent"))
                {
                    int index = 0;
                    int count = 0;
                    string functionName = String.Empty;

                    Dictionary<string, List<string>> funGroup = new Dictionary<string, List<string>>();
                    while ((index = pair.Value.IndexOf("AddListenerEvent", index, StringComparison.Ordinal)) != -1)
                    {
                        string parameter = String.Empty;
                        count++;
                        index = index + "AddListenerEvent".Length;
                        int firstBrackets = index;
                        // Debug.Log(pair.Value);
                        for (int i = index; i < pair.Value.Length; i++)
                        {
                            if (pair.Value[i].ToString() == "(")
                            {
                                break;
                            }

                            firstBrackets++;
                        }

                        for (int i = index; i < firstBrackets; i++)
                        {
                            parameter += pair.Value[i];
                        }

                        // Debug.Log("属性:" + parameter);
                        //去掉多余<>
                        if (parameter.Length > 1 && parameter[0].ToString() == "<" && parameter[parameter.Length - 1].ToString() == ">")
                        {
                            parameter = parameter.Remove(0, 1);
                            parameter = parameter.Remove(parameter.Length - 1, 1);
                        }

                        // Debug.Log("属性:" + parameter);
                        int eventNameStart = firstBrackets + ("(" + "\"").Length;
                        // Debug.Log(eventNameStart);
                        int eventNameEnd = eventNameStart;
                        for (int i = eventNameStart; i < pair.Value.Length; i++)
                        {
                            if (pair.Value[i].ToString() == "\"")
                            {
                                break;
                            }

                            eventNameEnd++;
                        }

                        // Debug.Log(eventNameEnd);
                        for (int i = eventNameStart; i < eventNameEnd; i++)
                        {
                            functionName += pair.Value[i];
                        }

                        // Debug.Log("Event事件名称:" + functionName);
                        List<string> parameterList = ParameterSplit(parameter);
                        foreach (string s in parameterList)
                        {
                            // Debug.Log(s);
                        }

                        funGroup.Add(functionName, parameterList);
                        functionName = String.Empty;
                    }

                    _callDic.Add(pair.Key, funGroup);
                }
            }

            string oldContent = GenerateGeneral.GetOldScriptsContent("ListenerSvcData");
            oldContent = GenerateGeneral.ReplaceScriptContent(oldContent, GenerationMethod(_callDic), "//监听生成开始", "//监听生成结束");
            FileOperation.SaveTextToLoad(GenerateGeneral.GetPath("ListenerSvcData"), oldContent);
        }

        /// <summary>
        /// 属性分割
        /// </summary>
        /// <param name="paramete"></param>
        /// <returns></returns>
        private List<string> ParameterSplit(string paramete)
        {
            int leftBrackets = 0;
            List<string> parameterList = new List<string>();
            string singleParameter = String.Empty;
            for (int i = 0; i < paramete.Length; i++)
            {
                if (paramete[i].ToString() == "<")
                {
                    leftBrackets += 1;
                    singleParameter += paramete[i];
                }
                else if (paramete[i].ToString() == ">")
                {
                    leftBrackets -= 1;
                    singleParameter += paramete[i];
                    if (leftBrackets == 0)
                    {
                        parameterList.Add(singleParameter);
                        singleParameter = String.Empty;
                    }
                }
                else if (paramete[i].ToString() == ",")
                {
                    if (leftBrackets == 0)
                    {
                        if (singleParameter.Length > 0)
                        {
                            parameterList.Add(singleParameter);
                        }

                        singleParameter = String.Empty;
                    }
                    else
                    {
                        singleParameter += paramete[i];
                    }
                }
                else if (i == paramete.Length - 1)
                {
                    singleParameter += paramete[i];
                    if (singleParameter.Length > 0)
                    {
                        parameterList.Add(singleParameter);
                    }

                    singleParameter = String.Empty;
                }
                else
                {
                    singleParameter += paramete[i];
                }
            }

            return parameterList;
        }
    }
}