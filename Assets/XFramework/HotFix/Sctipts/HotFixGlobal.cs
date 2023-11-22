using System;
using UnityEngine;

public class HotFixGlobal
{
    public static string GetDeviceStoragePath()
    {
        string path = String.Empty;

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                path = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.WSAPlayerX64:
            case RuntimePlatform.WSAPlayerX86:
            case RuntimePlatform.WSAPlayerARM:
                path = Application.persistentDataPath;
                break;
        }

        return path;
    }
}