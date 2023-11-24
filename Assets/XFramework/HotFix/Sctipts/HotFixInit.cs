using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization;
#if HybridCLR
using HybridCLR;
#endif

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
        GameObject hotFixView = AssetBundle.LoadFromFile(HotFixGlobal.GetDeviceStoragePath() + "/" + "HotFix/HotFixView/hotfixview").LoadAsset<GameObject>("HotFixView");
        GameObject.Instantiate(hotFixView);
        SceneManager.sceneLoaded -= SceneLoadOverCallBack;
    }

    //首先加载这个是因为网络下载的时候需要使用这个元数据
    private static void LoadMscorlibMetadataForAOTAssemblies()
    {
        if (!Directory.Exists(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/"))
        {
            Directory.CreateDirectory(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/");
        }

        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll"
        };

        foreach (var aotDllName in aotDllList)
        {
            string aotDllPath = HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/" + aotDllName + ".bytes";
            if (!File.Exists(aotDllPath))
            {
                Debug.Log(aotDllName + "拷贝");
                Debug.Log(aotDllPath);

                File.Copy(Application.streamingAssetsPath + "/HotFix/Metadata/" + aotDllName + ".bytes", aotDllPath, true);
            }

            byte[] dllBytes = File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/{"HotFix/Metadata/" + aotDllName}.bytes");
#if HybridCLR
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
#endif
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
        };
        if (!Directory.Exists(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/"))
        {
            Directory.CreateDirectory(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/");
        }

        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/{"HotFix/Metadata/" + aotDllName}.bytes");
#if HybridCLR
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
#endif
        }
    }

    public static void Over()
    {
        Debug.Log("元数据加载");
        //加载元数据
        LoadMetadataForAOTAssemblies();
        LoadHotFixCode();
        GameObject GameRootStart = AssetBundle.LoadFromFile(HotFixGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/GameRootStartAssetBundle/gamerootstart").LoadAsset<GameObject>("GameRootStart");
        GameObject.Instantiate(GameRootStart);
    }

    //加载XFrameworkHotFix数据
    private static void LoadHotFixCode()
    {
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。  
#if !UNITY_EDITOR
        Assembly hotFix = Assembly.Load(File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/HotFixRuntime/Assembly/Assembly-CSharp.dll.bytes"));
#else

#endif
    }
}