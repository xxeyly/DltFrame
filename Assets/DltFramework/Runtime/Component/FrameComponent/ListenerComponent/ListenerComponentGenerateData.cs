﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public class ListenerComponentGenerateData
    {
        [LabelText("生成脚本内容")] private Dictionary<string, Dictionary<string, List<List<string>>>> _callDic = new Dictionary<string, Dictionary<string, List<List<string>>>>();

        [LabelText("生成脚本内容")] private Dictionary<string, Dictionary<string, List<List<string>>>> _returnCallDic = new Dictionary<string, Dictionary<string, List<List<string>>>>();

        [LabelText("所有脚本内容")] private Dictionary<string, string> _allScriptsContentDic;

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void OnGenerate()
        {
            string listenerComponentDataPath = GenerateGeneral.GetPath("ListenerComponentData");
            if (listenerComponentDataPath == null)
            {
                Debug.LogWarning("ListenerComponentData脚本未创建");
                return;
            }

            #region 加载本地脚本

            _callDic = new Dictionary<string, Dictionary<string, List<List<string>>>>();
            _returnCallDic = new Dictionary<string, Dictionary<string, List<List<string>>>>();
            _allScriptsContentDic = new Dictionary<string, string>();
            List<string> classPath = DataFrameComponent.Path_GetGetSpecifyPathInAllType("Assets", "cs");

            for (int i = 0; i < classPath.Count; i++)
            {
                if (!classPath[i].Contains("DltFramework") && !_allScriptsContentDic.ContainsKey(DataFrameComponent.Path_GetPathFileNameDontContainFileType(classPath[i])))
                {
                    _allScriptsContentDic.Add(DataFrameComponent.Path_GetPathFileNameDontContainFileType(classPath[i]), FileOperationComponent.GetTextToLoad(classPath[i]));
                }
            }

            #endregion

            #region 读取监听内容

            foreach (KeyValuePair<string, string> pair in _allScriptsContentDic)
            {
                if (pair.Value.Contains("AddListenerEvent"))
                {
                    int index = 0;
                    string functionName = String.Empty;

                    Dictionary<string, List<List<string>>> funGroup = new Dictionary<string, List<List<string>>>();
                    while ((index = pair.Value.IndexOf("AddListenerEvent", index, StringComparison.Ordinal)) != -1)
                    {
                        string parameter = String.Empty;
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
                        if (parameter.Length > 1 && parameter[0].ToString() == "<" &&
                            parameter[parameter.Length - 1].ToString() == ">")
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

                        // Debug.Log(functionName);

                        // Debug.Log("Event事件名称:" + functionName);
                        List<string> parameterList = ParameterSplit(parameter);

                        if (!funGroup.ContainsKey(functionName))
                        {
                            funGroup.Add(functionName, new List<List<string>>() { parameterList });
                        }
                        else
                        {
                            funGroup[functionName].Add(parameterList);
                        }

                        functionName = String.Empty;
                    }

                    _callDic.Add(pair.Key, funGroup);
                }
                else
                {
                }
            }

            foreach (KeyValuePair<string, string> pair in _allScriptsContentDic)
            {
                if (pair.Value.Contains("AddReturnListenerEvent"))
                {
                    int index = 0;
                    string functionName = String.Empty;

                    Dictionary<string, List<List<string>>> funGroup = new Dictionary<string, List<List<string>>>();
                    while ((index = pair.Value.IndexOf("AddReturnListenerEvent", index, StringComparison.Ordinal)) != -1)
                    {
                        string parameter = String.Empty;
                        index = index + "AddReturnListenerEvent".Length;
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
                        if (parameter.Length > 1 && parameter[0].ToString() == "<" &&
                            parameter[parameter.Length - 1].ToString() == ">")
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

                        // Debug.Log(functionName);

                        // Debug.Log("Event事件名称:" + functionName);
                        List<string> parameterList = ParameterSplit(parameter);

                        if (!funGroup.ContainsKey(functionName))
                        {
                            funGroup.Add(functionName, new List<List<string>>() { parameterList });
                        }
                        else
                        {
                            funGroup[functionName].Add(parameterList);
                        }

                        functionName = String.Empty;
                    }

                    _returnCallDic.Add(pair.Key, funGroup);
                }
                else
                {
                }
            }

            #endregion

            #region 写入生成内容

            string oldContent = GenerateGeneral.GetOldScriptsContent("ListenerComponentData");
            oldContent =
                GenerateGeneral.ReplaceScriptContent(oldContent, GenerationMethod(_callDic, _returnCallDic), "//监听生成开始",
                    "//监听生成结束");
            FileOperationComponent.SaveTextToLoad(GenerateGeneral.GetPath("ListenerComponentData"), oldContent);

            #endregion

            Debug.Log("代码生成完毕");
        }

        /// <summary>
        /// 生成方法
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="returnMethod">返回值方法</param>
        /// <returns></returns>
        private string GenerationMethod(Dictionary<string, Dictionary<string, List<List<string>>>> method,
            Dictionary<string, Dictionary<string, List<List<string>>>> returnMethod)
        {
            //类方法组
            Dictionary<string, List<string>> classMethodGroup = new Dictionary<string, List<string>>();
            //生成类
            string generateClassContent = String.Empty;
            string methodName;

            #region 无返回值

            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> pair in method)
            {
                List<string> classMethod = new List<string>();
                foreach (KeyValuePair<string, List<List<string>>> valuePair in pair.Value)
                {
                    for (int i = 0; i < valuePair.Value.Count; i++)
                    {
                        //方法名称
                        //方法属性
                        string methodParameter = String.Empty;
                        //监听属性
                        string listenePparameter;
                        methodName = GenerateGeneral.Indents(12) + "public void " + valuePair.Key + "(";
                        for (int j = 0; j < valuePair.Value[i].Count; j++)
                        {
                            if (j == valuePair.Value[i].Count - 1)
                            {
                                methodParameter += valuePair.Value[i][j] + " " + "arg" + j;
                            }
                            else
                            {
                                methodParameter += valuePair.Value[i][j] + " " + "arg" + j + ",";
                            }
                        }

                        methodName += methodParameter + ")" + GenerateGeneral.LineFeed + GenerateGeneral.Indents(12) +
                                      "{" +
                                      GenerateGeneral.LineFeed;
                        listenePparameter = String.Empty;

                        for (int j = 0; j < valuePair.Value[i].Count; j++)
                        {
                            if (j == valuePair.Value[i].Count - 1)
                            {
                                listenePparameter += "arg" + j;
                            }
                            else
                            {
                                listenePparameter += "arg" + j + ",";
                            }
                        }

                        string temp;
                        if (listenePparameter == String.Empty)
                        {
                            temp = String.Empty;
                        }
                        else
                        {
                            temp = ",";
                        }

                        methodName += GenerateGeneral.Indents(16) + "Instance.ExecuteEvent(\"" + pair.Key + "-" +
                                      valuePair.Key + "\"" + temp + listenePparameter + ");" + GenerateGeneral.LineFeed;
                        methodName += GenerateGeneral.Indents(12) + "}" + GenerateGeneral.LineFeed;
                        classMethod.Add(methodName);
                    }
                }

                if (!classMethodGroup.ContainsKey(pair.Key))
                {
                    classMethodGroup.Add(pair.Key, classMethod);
                }
                else
                {
                    for (int i = 0; i < classMethod.Count; i++)
                    {
                        classMethodGroup[pair.Key].Add(classMethod[i]);
                    }
                }
            }

            #endregion

            #region 有返回值

            foreach (KeyValuePair<string, Dictionary<string, List<List<string>>>> pair in returnMethod)
            {
                List<string> classMethod = new List<string>();
                foreach (KeyValuePair<string, List<List<string>>> valuePair in pair.Value)
                {
                    for (int i = 0; i < valuePair.Value.Count; i++)
                    {
                        //方法名称
                        //方法属性
                        string methodParameter = String.Empty;
                        //监听属性
                        string listenePparameter;
                        methodName = GenerateGeneral.Indents(12) + "public ";
                        string variable = "<";
                        for (int j = 0; j < valuePair.Value[i].Count; j++)
                        {
                            if (j == valuePair.Value[i].Count - 1)
                            {
                                methodName += valuePair.Value[i][j] + " ";
                                variable += valuePair.Value[i][j] + ">";
                            }
                            else
                            {
                                variable += valuePair.Value[i][j] + ",";
                            }
                        }

                        methodName += valuePair.Key + "(";
                        for (int j = 0; j < valuePair.Value[i].Count - 1; j++)
                        {
                            if (j == valuePair.Value[i].Count - 2)
                            {
                                methodParameter += valuePair.Value[i][j] + " " + "arg" + j;
                            }
                            else
                            {
                                methodParameter += valuePair.Value[i][j] + " " + "arg" + j + ",";
                            }
                        }

                        methodName += methodParameter + ")" + GenerateGeneral.LineFeed + GenerateGeneral.Indents(12) +
                                      "{" +
                                      GenerateGeneral.LineFeed;
                        listenePparameter = String.Empty;

                        for (int j = 0; j < valuePair.Value[i].Count - 1; j++)
                        {
                            if (j == valuePair.Value[i].Count - 2)
                            {
                                listenePparameter += "arg" + j;
                            }
                            else
                            {
                                listenePparameter += "arg" + j + ",";
                            }
                        }

                        string temp;
                        if (listenePparameter == String.Empty)
                        {
                            temp = String.Empty;
                        }
                        else
                        {
                            temp = ",";
                        }

                        methodName += GenerateGeneral.Indents(16) + "return Instance.ExecuteReturnEvent" + variable +
                                      "(\"" + pair.Key + "-" +
                                      valuePair.Key + "\"" + temp + listenePparameter + ");" + GenerateGeneral.LineFeed;
                        methodName += GenerateGeneral.Indents(12) + "}" + GenerateGeneral.LineFeed;
                        classMethod.Add(methodName);
                        // Debug.Log(methodName);
                    }
                }

                if (!classMethodGroup.ContainsKey(pair.Key))
                {
                    classMethodGroup.Add(pair.Key, classMethod);
                }
                else
                {
                    for (int i = 0; i < classMethod.Count; i++)
                    {
                        classMethodGroup[pair.Key].Add(classMethod[i]);
                    }
                }
            }

            #endregion

            //生成类
            foreach (KeyValuePair<string, List<string>> pair in classMethodGroup)
            {
                generateClassContent += GenerateGeneral.Indents(8) + "[HideInInspector] public " + pair.Key + " " + DataFrameComponent.String_FirstCharToLower(pair.Key) + " = new " + pair.Key + "()" +
                                        ";" +
                                        GenerateGeneral.LineFeed;
            }

            //生成类方法
            foreach (KeyValuePair<string, List<string>> pair in classMethodGroup)
            {
                generateClassContent += GenerateGeneral.Indents(8) + "public class " + pair.Key + GenerateGeneral.LineFeed;
                generateClassContent += GenerateGeneral.Indents(8) + "{" + GenerateGeneral.LineFeed;
                foreach (string moth in pair.Value)
                {
                    generateClassContent += moth;
                }

                generateClassContent += GenerateGeneral.Indents(8) + "}" + GenerateGeneral.LineFeed;
            }


            return generateClassContent;
        }

        /// <summary>
        /// 属性分割
        /// </summary>
        /// <param name="paramete">属性</param>
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