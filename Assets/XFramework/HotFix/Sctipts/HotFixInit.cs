using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.Serialization;
using UnityEditor;
#if HybridCLR
using HybridCLR;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
#if UNITY_EDITOR
        GameObject hotFixView = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/HotFixView.prefab");

#else
        GameObject hotFixView = AssetBundle.LoadFromFile(HotFixGlobal.GetDeviceStoragePath() + "/" + "HotFix/HotFixView/hotfixview").LoadAsset<GameObject>("HotFixView");

#endif
        Object.Instantiate(hotFixView);
        Debug.Log("HotFixView加载完毕");
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
                File.Copy(Application.streamingAssetsPath + "/HotFix/Metadata/" + aotDllName + ".bytes", aotDllPath, true);
            }

            byte[] dllBytes = File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/{"HotFix/Metadata/" + aotDllName}.bytes");
#if HybridCLR
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
#endif
        }
    }
}