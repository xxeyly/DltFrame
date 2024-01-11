using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HotFixGlobal
{
    //获得设备存储路径
    public static string GetDeviceStoragePath(bool unityWebRequestPath = false)
    {
        string path = String.Empty;

        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                path = Application.dataPath + "/UnStreamingAssets";
                break;
            case RuntimePlatform.WindowsPlayer:
                path = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.WSAPlayerX64:
            case RuntimePlatform.WSAPlayerX86:
            case RuntimePlatform.WSAPlayerARM:
                path = Application.persistentDataPath;
                break;
            case RuntimePlatform.Android:
                if (unityWebRequestPath)
                {
                    path = "file://" + Application.persistentDataPath;
                }
                else
                {
                    path = Application.persistentDataPath;
                }

                break;
            case RuntimePlatform.IPhonePlayer:
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

    /// <summary>
    /// 读取本地文件信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static string GetTextToLoad(string path, string fileName)
    {
//            UnityEngine.Debug.Log(Path + "/" + FileName);
        if (Directory.Exists(path))
        {
        }
        else
        {
            Debug.LogError("文件不存在:" + path + "/" + fileName);
        }

        FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Open);
        StreamReader sr = new StreamReader(aFile);
        var textData = sr.ReadToEnd();
        sr.Close();
        return textData;
    }

    //字节长度转换单位
    public static string FileSizeString(double length)
    {
        int byteConversion = 1024;
        double bytes = Convert.ToDouble(length);

        // 超过EB的单位已经没有实际转换意义了, 太大了, 忽略不用
        if (bytes >= Math.Pow(byteConversion, 6)) // EB
        {
            return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 6), 2), " EB");
        }

        if (bytes >= Math.Pow(byteConversion, 5)) // PB
        {
            return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 5), 2), " PB");
        }
        else if (bytes >= Math.Pow(byteConversion, 4)) // TB
        {
            return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 4), 2), " TB");
        }
        else if (bytes >= Math.Pow(byteConversion, 3)) // GB
        {
            return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
        }
        else if (bytes >= Math.Pow(byteConversion, 2)) // MB
        {
            return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
        }
        else if (bytes >= byteConversion) // KB
        {
            return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
        }
        else // Bytes
        {
            return string.Concat(bytes, " Bytes");
        }
    }

    /// <summary>
    /// 获得文件地址的MD5值
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
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

    //获得文件大小
    public static long GetFileSize(string fileName)
    {
        if (File.Exists(fileName))
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            long size = file.Length;
            file.Dispose();
            return size;
        }

        return 0;
    }

    //获得数据的MD5值
    public static string GetMD5HashByte(byte[] fileByte)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(fileByte);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }

        return sb.ToString();
    }
}