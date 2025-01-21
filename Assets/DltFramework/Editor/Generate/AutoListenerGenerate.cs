using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;


[InitializeOnLoad]
public class AutoListenerGenerate
{
    private static AutoGenerateListenerConfig autoGenerateListenerConfig;
    public static bool isAssemblyError;

    static AutoListenerGenerate()
    {
        // 监听编译完成的事件
        CompilationPipeline.assemblyCompilationFinished += OnAssemblyCompilationFinished;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    private static void OnAfterAssemblyReload()
    {
        if (!isAssemblyError)
        {
            GenerateCode();
        }
    }

    private static async void OnAssemblyCompilationFinished(string assemblyPath, CompilerMessage[] compilerMessages)
    {
        // 如果有编译错误，停止代码生成
        bool hasError = false;
        foreach (var message in compilerMessages)
        {
            if (message.type == CompilerMessageType.Error)
            {
                hasError = true;
                break;
            }
        }

        isAssemblyError = hasError;
    }


    static void GenerateCode()
    {
        if (!Directory.Exists("Assets/Config/"))
        {
            Directory.CreateDirectory("Assets/Config/");
            AssetDatabase.Refresh();
        }

        if (!File.Exists("Assets/Config/" + "AutoGenerateListenerConfig.asset"))
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AutoGenerateListenerConfig>(), "Assets/Config/" + "AutoGenerateListenerConfig.asset");
        }

        if (autoGenerateListenerConfig == null)
        {
            autoGenerateListenerConfig = AssetDatabase.LoadAssetAtPath<AutoGenerateListenerConfig>("Assets/Config/" + "AutoGenerateListenerConfig.asset");
            autoGenerateListenerConfig.isAuto = true;
            EditorUtility.SetDirty(autoGenerateListenerConfig);
        }


        if (!autoGenerateListenerConfig.isAuto)
        {
            return;
        }

        GenerateListenerComponent.GenerateListener();
    }
}