#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DltFramework;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

[InitializeOnLoad]
public static class PackageLister
{
    public static ListRequest list;
    public static Dictionary<string, bool> oldContainDic = new Dictionary<string, bool>();

    static PackageLister()
    {
        GetInstalledPackages();
    }


    public static async void GetInstalledPackages()
    {
        UnityWebRequest www = UnityWebRequest.Get(Application.dataPath + "/../Packages/manifest.json");
        await www.SendWebRequest();
        EditorMacro(www.downloadHandler.text, "HybridCLR");
        EditorMacro(www.downloadHandler.text, "Obfuz4HybridCLR");
        GetInstalledPackages();
    }

    private static void EditorMacro(string allPackageName, string macroName)
    {
        if (allPackageName.Contains(DataFrameComponent.String_AllCharToLower(macroName)))
        {
            if (!oldContainDic.ContainsKey(macroName))
            {
                oldContainDic.Add(macroName, true);
                AddMacro(macroName);
            }

            if (!oldContainDic[macroName])
            {
                AddMacro(macroName);
                oldContainDic[macroName] = true;
            }
        }
        else
        {
            if (!oldContainDic.ContainsKey(macroName))
            {
                oldContainDic.Add(macroName, false);
                RemoveMacro(macroName);
            }

            if (oldContainDic[macroName])
            {
                RemoveMacro(macroName);
                oldContainDic[macroName] = false;
            }
        }
    }

    //添加宏定义
    [Obsolete("Obsolete")]
    private static void AddMacro(string macroName)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        //获取当前平台已有的宏定义
        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        //添加想要的宏定义
        var symbolsList = symbols.Split(';').ToList();
        if (symbolsList.Contains(macroName))
        {
            return;
        }

        symbolsList.Add(macroName);
        symbols = string.Join(";", symbolsList);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
    }

    //删除宏定义
    [Obsolete("Obsolete")]
    private static void RemoveMacro(string macroName)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        //获取当前平台已有的宏定义
        var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        //添加想要的宏定义
        var symbolsList = symbols.Split(';').ToList();
        if (!symbolsList.Contains(macroName))
        {
            return;
        }

        symbolsList.Remove(macroName);
        symbols = string.Join(";", symbolsList);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
    }
}
#endif