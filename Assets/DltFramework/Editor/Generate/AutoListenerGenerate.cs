using System;
using UnityEditor;


[InitializeOnLoad]
public class AutoListenerGenerate
{
    private static long oldTime;
    private static TimeSpan st1;

    static AutoListenerGenerate()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        st1 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        if (Convert.ToInt64(st1.TotalSeconds) - oldTime >= 1)
        {
            oldTime = Convert.ToInt64(st1.TotalSeconds);
            GenerateListenerComponent.GenerateListener();
        }
    }
}