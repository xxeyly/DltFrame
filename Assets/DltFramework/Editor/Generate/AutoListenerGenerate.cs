using System;
using System.IO;
using DltFramework;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class AutoListenerGenerate
{
    private static long oldTime;
    private static TimeSpan st1;
    private static AutoGenerateListenerConfig autoGenerateListenerConfig;

    static AutoListenerGenerate()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        st1 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);

        if (Convert.ToInt64(st1.TotalSeconds) - oldTime >= 1)
        {
            if (!File.Exists("Assets/Config/" + "AutoGenerateListenerConfig.asset"))
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AutoGenerateListenerConfig>(), "Assets/Config/" + "AutoGenerateListenerConfig.asset");
            }

            if (autoGenerateListenerConfig == null)
            {
                autoGenerateListenerConfig = AssetDatabase.LoadAssetAtPath<AutoGenerateListenerConfig>("Assets/Config/" + "AutoGenerateListenerConfig.asset");
                autoGenerateListenerConfig.isAuto = true;
                EditorUtility.SetDirty(autoGenerateListenerConfig);
                Debug.Log("自动生成开启");

            }


            if (!autoGenerateListenerConfig.isAuto)
            {
                return;
            }

            oldTime = Convert.ToInt64(st1.TotalSeconds);
            GenerateListenerComponent.GenerateListener();
        }
    }
}