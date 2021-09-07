using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;

public class ListenerSvcGenerateData : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, List<string>>> callDic2 = new Dictionary<string, Dictionary<string, List<string>>>();
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
        // callDic = new Dictionary<string, List<string>>();
        callDic2 = new Dictionary<string, Dictionary<string, List<string>>>();
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
                _allScriptsContentDic.Add(files[i].Name.Replace(".cs", ""), ResSvc.FileOperation.GetTextToLoad(ResSvc.FileOperation.ConvertToLocalPath(files[i].FullName)));
            }
        }

        foreach (KeyValuePair<string, string> pair in _allScriptsContentDic)
        {
            if (pair.Value.Contains("AddListenerEvent"))
            {
                int index = 0;
                int count = 0;
                int Length = 0;
                string parameter = String.Empty;
                string functionName = String.Empty;

                Dictionary<string, List<string>> funGroup = new Dictionary<string, List<string>>();
                while ((index = pair.Value.IndexOf("AddListenerEvent", index, StringComparison.Ordinal)) != -1)
                {
                    count++;
                    index = index + "AddListenerEvent".Length;
                    Length = 0;
                    if (pair.Value[index].ToString() == "(")
                    {
                        parameter = String.Empty;
                        int Length2 = -2;
                        for (int j = index + 1; j < pair.Value.Length; j++)
                        {
                            if (pair.Value[j].ToString() == ",")
                            {
                                break;
                            }
                            else
                            {
                                Length2++;
                            }
                        }

                        functionName = pair.Value.Substring(index + Length + 2, Length2);
                    }

                    else if (pair.Value[index].ToString() == "<")
                    {
                        for (int j = index; j < pair.Value.Length; j++)
                        {
                            Length++;
                            if (pair.Value[j].ToString() == ">")
                            {
                                break;
                            }
                        }

                        parameter = pair.Value.Substring(index + 1, Length - 2);
                        int lenght2 = -2;
                        for (int j = index + Length; j < pair.Value.Length; j++)
                        {
                            if (pair.Value[j].ToString() == ",")
                            {
                                break;
                            }
                            else
                            {
                                lenght2++;
                            }
                        }

                        functionName = pair.Value.Substring(index + Length + 2, lenght2 - 1);
                    }


                    if (parameter.Length <= 0)
                    {
                        funGroup.Add(functionName, new List<string>());
                        // callDic.Add(functionName, new List<string>());
                    }
                    else
                    {
                        funGroup.Add(functionName, new List<string>(parameter.Split(',')));
                        // callDic.Add(functionName, new List<string>(parameter.Split(',')));
                    }
                }

                callDic2.Add(pair.Key, funGroup);
            }
        }

        string oldContent = GenerateGeneral.GetOldScriptsContent("ListenerSvcData");
        oldContent = GenerateGeneral.ReplaceScriptContent(oldContent, GenerationMethod(callDic2), "//监听生成开始", "//监听生成结束");
        ResSvc.FileOperation.SaveTextToLoad(GenerateGeneral.GetPath("ListenerSvcData"), oldContent);
    }
}