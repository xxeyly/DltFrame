using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotFixInit
{
    public static void Init()
    {
        Debug.Log("初始化");
        LoadMscorlibMetadataForAOTAssemblies();
        SceneManager.sceneLoaded += SceneLoadOverCallBack;
        SceneManager.LoadScene("HotFix");
    }

    private static void SceneLoadOverCallBack(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("HotFix");
        GameObject hotFixView = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "HotFix/HotFixView/hotfixview").LoadAsset<GameObject>("HotFixView");
        GameObject.Instantiate(hotFixView);
        SceneManager.sceneLoaded -= SceneLoadOverCallBack;
    }

    private static void LoadMscorlibMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll"
        };

        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{"HotFix/Metadata/" + aotDllName}.bytes");
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }

    private static void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "StompyRobot.SRF.dll",
            "System.Core.dll",
            "System.dll",
            "UnityEngine.AssetBundleModule.dll",
            "UnityEngine.CoreModule.dll",
            "UnityEngine.JSONSerializeModule.dll",
            "mscorlib.dll",
        };

        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{"HotFix/Metadata/" + aotDllName}.bytes");
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }

    public static void Over()
    {
        Debug.Log("元数据加载");
        //加载元数据
        LoadMetadataForAOTAssemblies();
        LoadHotFixCode();
        GameObject GameRootStart = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "HotFixRuntime/GameRootStartAssetBundle/gamerootstart").LoadAsset<GameObject>("GameRootStart");
        GameObject.Instantiate(GameRootStart);
    }

    //加载XFrameworkHotFix数据
    private static void LoadHotFixCode()
    {
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。  
#if !UNITY_EDITOR
        Assembly hotFix = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotFixRuntime/Assembly/Assembly-CSharp.dll.bytes"));
#else

#endif
    }
}