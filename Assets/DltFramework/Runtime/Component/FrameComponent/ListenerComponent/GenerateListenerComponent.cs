using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using DltFramework;

public abstract class GenerateListenerComponent
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static List<GenerateClassData> _generateClassDataList = new List<GenerateClassData>();
    private static Assembly _assembly;
    public static string listenerComponentDataPath = "";

    public static void GenerateListener()
    {
        if (listenerComponentDataPath == string.Empty)
        {
            listenerComponentDataPath = GenerateGeneral.GetPath("ListenerComponentData");
        }

        if (listenerComponentDataPath == null)
        {
            // DebugFrameComponent.LogWarning("ListenerComponentData脚本未创建 Create > DltFramework > C# ListenerFrameComponentData");
            return;
        }

        _generateClassDataList.Clear();
        _assembly = Assembly.Load("Assembly-CSharp");
        foreach (Type type in _assembly.GetTypes())
        {
            GenerateClassData tempGenerateClassData = new GenerateClassData();
            tempGenerateClassData.className = type.Name;
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
                {
                    if (customAttribute is AddListenerEventAttribute)
                    {
                        GenerateMethodData tempGenerateMethodData = new GenerateMethodData();
                        tempGenerateMethodData.methodName = methodInfo.Name;
                        tempGenerateMethodData.returnType = methodInfo.ReturnType;
                        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                        {
                            tempGenerateMethodData.parameterType.Add(parameterInfo.ParameterType);
                        }

                        tempGenerateClassData.generateMethodData.Add(tempGenerateMethodData);
                    }
                }
            }

            if (tempGenerateClassData.generateMethodData.Count > 0)
            {
                _generateClassDataList.Add(tempGenerateClassData);
            }
        }


        string oldContent = GenerateGeneral.GetOldScriptsContent("ListenerComponentData");

        string generateClassContent = String.Empty;
        foreach (GenerateClassData generateClassData in _generateClassDataList)
        {
            generateClassContent = DataFrameComponent.String_BuilderString(
                generateClassContent, GenerateGeneral.Indents(8), "[HideInInspector] public ", generateClassData.className, GenerateGeneral.Indents(1),
                DataFrameComponent.String_FirstCharToLower(generateClassData.className), " = new ", generateClassData.className, "();",
                GenerateGeneral.LineFeed);
        }

        foreach (GenerateClassData generateClassData in _generateClassDataList)
        {
            //类
            generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent, GenerateGeneral.Indents(8), "public class ", generateClassData.className, GenerateGeneral.LineFeed);
            generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent, GenerateGeneral.Indents(8), "{", GenerateGeneral.LineFeed);

            foreach (GenerateMethodData generateMethodData in generateClassData.generateMethodData)
            {
                string parameterTypeContent = String.Empty;

                for (int i = 0; i < generateMethodData.parameterType.Count; i++)
                {
                    parameterTypeContent = DataFrameComponent.String_BuilderString(parameterTypeContent, CommonTypeConversion(generateMethodData.parameterType[i]), GenerateGeneral.Indents(1), "arg", i.ToString());
                    if (i < generateMethodData.parameterType.Count - 1)
                    {
                        parameterTypeContent = DataFrameComponent.String_BuilderString(parameterTypeContent, ",");
                    }
                }

                generateClassContent = DataFrameComponent.String_BuilderString(
                    generateClassContent, GenerateGeneral.Indents(12), "public", GenerateGeneral.Indents(1), CommonTypeConversion(generateMethodData.returnType),
                    GenerateGeneral.Indents(1), generateMethodData.methodName, "(", parameterTypeContent, ")", GenerateGeneral.LineFeed);
                generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent, GenerateGeneral.Indents(12), "{", GenerateGeneral.LineFeed);
                string inputParameterTypeContent = String.Empty;

                string executeEventContent = string.Empty;
                if (generateMethodData.returnType == typeof(void))
                {
                    executeEventContent = "Instance.ExecuteEvent";
                }
                else
                {
                    if (generateMethodData.parameterType.Count == 0)
                    {
                        executeEventContent = DataFrameComponent.String_BuilderString(executeEventContent, "return Instance.ExecuteReturnEvent", "<", CommonTypeConversion(generateMethodData.returnType), ">");
                    }
                    else
                    {
                        string returnParameterTypeContent = String.Empty;
                        for (int i = 0; i < generateMethodData.parameterType.Count; i++)
                        {
                            returnParameterTypeContent = DataFrameComponent.String_BuilderString(returnParameterTypeContent, CommonTypeConversion(generateMethodData.parameterType[i]), ",");
                        }

                        executeEventContent = DataFrameComponent.String_BuilderString(executeEventContent, "return Instance.ExecuteReturnEvent<", returnParameterTypeContent, CommonTypeConversion(generateMethodData.returnType), ">");
                    }
                }

                if (generateMethodData.parameterType.Count == 0)
                {
                    generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent,
                        GenerateGeneral.Indents(16), executeEventContent, "(\"", generateClassData.className, "\"", ",", "\"", generateMethodData.methodName, "\"", ");", GenerateGeneral.LineFeed);
                }
                else
                {
                    for (int i = 0; i < generateMethodData.parameterType.Count; i++)
                    {
                        inputParameterTypeContent = DataFrameComponent.String_BuilderString(inputParameterTypeContent, "arg", i.ToString());
                        if (i < generateMethodData.parameterType.Count - 1)
                        {
                            inputParameterTypeContent = DataFrameComponent.String_BuilderString(inputParameterTypeContent, ",");
                        }
                    }

                    generateClassContent = DataFrameComponent.String_BuilderString(
                        generateClassContent, GenerateGeneral.Indents(16), executeEventContent, "(\"", generateClassData.className, "\"", ",", "\"",
                        generateMethodData.methodName, "\"", ",", inputParameterTypeContent, ");",
                        GenerateGeneral.LineFeed);
                }

                generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent, GenerateGeneral.Indents(12), "}", GenerateGeneral.LineFeed);
            }

            generateClassContent = DataFrameComponent.String_BuilderString(generateClassContent, GenerateGeneral.Indents(8), "}", GenerateGeneral.LineFeed);
        }

        string newCon = ReplaceScriptContent(oldContent, generateClassContent, "//监听生成开始", "//监听生成结束");
        if (newCon != oldContent)
        {
            FileOperationComponent.SaveTextToLoad(GenerateGeneral.GetPath("ListenerComponentData"), newCon);
            DebugFrameComponent.Log("监听生成成功");
        }
    }

    /// <summary>
    /// 替换内容
    /// </summary>
    private static string ReplaceScriptContent(string scriptsContent, string insertContent, string insertStartMark, string insertEndMark)
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
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < scriptsContent.Length; i++)
            {
                if (i >= usingStartIndex && i < usingEndIndex)
                {
                    stringBuilder.Append(scriptsContent[i]);
                }
            }

            var scriptUsingContent = stringBuilder.ToString();

            string tempInsertContent = String.Empty;
            tempInsertContent = DataFrameComponent.String_BuilderString(tempInsertContent, insertContent);
            tempInsertContent = DataFrameComponent.String_BuilderString(insertStartMark, "\n", tempInsertContent, "\n");
            //替换新内容
            return scriptsContent.Replace(scriptUsingContent, tempInsertContent);
        }
        else
        {
            return scriptsContent;
        }
    }

    [LabelText("Type转换")]
    private static string CommonTypeConversion(Type type)
    {
        if (type.Name == nameof(Boolean))
        {
            return "bool";
        }

        if (type.Name == nameof(Byte))
        {
            return "byte";
        }

        if (type.Name == nameof(Char))
        {
            return "char";
        }

        if (type.Name == nameof(Decimal))
        {
            return "decimal";
        }

        if (type.Name == nameof(Double))
        {
            return "double";
        }

        if (type.Name == nameof(Single))
        {
            return "float";
        }

        if (type.Name == nameof(Int32))
        {
            return "int";
        }

        if (type.Name == nameof(Int64))
        {
            return "long";
        }

        if (type.Name == nameof(SByte))
        {
            return "sbyte";
        }

        if (type.Name == nameof(Int16))
        {
            return "short";
        }

        if (type.Name == nameof(UInt32))
        {
            return "uint";
        }

        if (type.Name == nameof(UInt64))
        {
            return "ulong";
        }

        if (type.Name == nameof(UInt16))
        {
            return "ushort";
        }

        if (type.Name == nameof(String))
        {
            return "string";
        }

        if (type.Name == typeof(void).Name)
        {
            return "void";
        }

        if (type.Name == typeof(List<>).Name)
        {
            return DataFrameComponent.String_BuilderString("List<", CommonTypeConversion(type.GetGenericArguments()[0]), ">");
        }

        if (type.Name == typeof(UniTask<>).Name)
        {
            return CommonTypeConversion(type.GetGenericArguments()[0]);
        }

        if (type.Name == typeof(Dictionary<,>).Name)
        {
            return DataFrameComponent.String_BuilderString("Dictionary<", CommonTypeConversion(type.GetGenericArguments()[0]), ",", CommonTypeConversion(type.GetGenericArguments()[1]), ">");
        }

        return type.Name;
    }


    [Serializable]
    [LabelText("生成类数据")]
    public class GenerateClassData
    {
        [LabelText("类名字")] public string className;
        public List<GenerateMethodData> generateMethodData = new List<GenerateMethodData>();
    }

    [Serializable]
    [LabelText("方法类数据")]
    public class GenerateMethodData
    {
        [LabelText("方法名字")] public string methodName;
        [LabelText("返回类型")] public Type returnType;
        [LabelText("参数类型")] public List<Type> parameterType = new List<Type>();
    }
}