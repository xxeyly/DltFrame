using System.Collections;
using System.Collections.Generic;
using System.IO;
using HybridCLR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotFixInit
{
    public static void Init()
    {
        Debug.Log("初始化");
        SceneManager.sceneLoaded += SceneLoadOverCallBack;
        SceneManager.LoadScene("HotFix");
    }

    private static void SceneLoadOverCallBack(Scene arg0, LoadSceneMode arg1)
    {
        LoadMetadataForAOTAssemblies();
        GameObject hotFixView = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "HotFix/HotFixAsset/hotfixview").LoadAsset<GameObject>("HotFixView");
        GameObject.Instantiate(hotFixView);
        SceneManager.sceneLoaded -= SceneLoadOverCallBack;
    }

    private static void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // 如果使用了Linq，需要这个
            // "Newtonsoft.Json.dll", 
            // "protobuf-net.dll",
        };

        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{"HotFix/Metadata/" + aotDllName}.bytes");
            LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }
}