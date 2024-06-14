using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

public class FrameLogic
{
    [LabelText("反射数据")] private static Dictionary<string, List<MethodInfoData>> _requestCodes = new Dictionary<string, List<MethodInfoData>>();

    public static void Init()
    {
        ReflectionRequestCode();
    }

    #region 反射请求数据

    /// <summary>
    /// 反射请求数据
    /// </summary>
    private static void ReflectionRequestCode()
    {
        Assembly _assembly = Assembly.Load("Assembly-CSharp");
        foreach (Type type in _assembly.GetTypes())
        {
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
                {
                    if (customAttribute is AddFrameDataLogicAttribute)
                    {
                        string frameDataType = ((AddFrameDataLogicAttribute)customAttribute).FrameDataType;
                        //参数长度
                        int parameterTypeLength = methodInfo.GetParameters().Length;
                        //参数类型个数
                        if (parameterTypeLength != 1)
                        {
                            continue;
                        }

                        //参数类型
                        if (methodInfo.GetParameters()[0].ParameterType != typeof(byte[]))
                        {
                            continue;
                        }


                        if (!_requestCodes.ContainsKey(frameDataType))
                        {
                            _requestCodes.Add(frameDataType, new List<MethodInfoData>());
                        }

                        _requestCodes[frameDataType].Add(new MethodInfoData()
                        {
                            obj = Activator.CreateInstance(type), methodInfo = methodInfo
                        });
                    }
                }
            }
        }
    }

    #endregion

    public static void ExecuteReflection(string frameDataType, byte[] data)
    {
        if (_requestCodes.ContainsKey(frameDataType))
        {
            foreach (MethodInfoData methodInfoData in _requestCodes[frameDataType])
            {
                try
                {
                    methodInfoData.methodInfo.Invoke(methodInfoData.obj, new object[] { data });
                }
                catch (Exception e)
                {
                    Debug.LogError("请求码:" + frameDataType + "异常" + methodInfoData.obj.ToString() + methodInfoData.methodInfo.Name + ":" + e);
                }
            }
        }
        else
        {
            Debug.LogError("未找到对应的FrameDataType:" + frameDataType);
        }
    }
}