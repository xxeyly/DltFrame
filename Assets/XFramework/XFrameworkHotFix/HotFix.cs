using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class HotFix : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_EDITOR
	        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotFixRuntime/Assembly/Assembly-CSharp.dll.bytes"));
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集  
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Assembly-CSharp");
#endif
        Type type = hotUpdateAss.GetType("HotFixRuntimeInit");
        type.GetMethod("Init").Invoke(null, null);
    }
}