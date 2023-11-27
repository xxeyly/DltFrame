using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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

    /// <summary>
    /// 保存文本信息到本地
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="fileName">文件名称</param>
    /// <param name="information">保存信息</param>
    public static void SaveTextToLoad(string path, string fileName, string information)
    {
        if (Directory.Exists(path))
        {
        }
        else
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllText(path + "/" + fileName, information, new System.Text.UTF8Encoding(false));
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public static string GetMD5HashFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }

        return null;
    }
}