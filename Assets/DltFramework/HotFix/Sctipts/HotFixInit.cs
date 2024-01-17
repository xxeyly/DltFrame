using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        HotFixDebug.Log("初始化");
        SceneManager.sceneLoaded += SceneLoadOverCallBack;
        SceneManager.LoadScene("HotFix");
    }

    private static void SceneLoadOverCallBack(Scene arg0, LoadSceneMode arg1)
    {
        bool localIsUpdate = false;
        string hotFixDownPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "LocalIsUpdate.txt";

        if (!File.Exists(hotFixDownPath))
        {
            localIsUpdate = true;
        }
        else
        {
            localIsUpdate = bool.Parse(HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix", "LocalIsUpdate.txt"));
        }

        if (localIsUpdate)
        {
#if UNITY_EDITOR
            GameObject hotFixView = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/HotFixView.prefab");

#else
        GameObject hotFixView = AssetBundle.LoadFromFile(HotFixGlobal.GetDeviceStoragePath() + "/" + "HotFix/HotFixView/hotfixview").LoadAsset<GameObject>("HotFixView");
#endif
            Object.Instantiate(hotFixView);
            HotFixDebug.Log("HotFixView加载完毕");
        }
        else
        {
            HotFixOver.Over();
        }


        SceneManager.sceneLoaded -= SceneLoadOverCallBack;
    }
}