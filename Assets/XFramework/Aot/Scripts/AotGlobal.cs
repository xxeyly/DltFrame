using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AotGlobal
{
    //获得设备存储路径
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
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                path = Application.persistentDataPath;
                break;
        }

        return path;
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

    //获得文件大小
    public static long GetFileSize(string fileName)
    {
        if (File.Exists(fileName))
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            long size = file.Length;
            file.Close();
            file.Dispose();
            return size;
        }

        return 0;
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

    //获得文件地址的MD5值
    public static string GetMD5HashFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            file.Dispose();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }

        return null;
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="sourceDirName"></param>
    /// <param name="destDirName"></param>
    public static void Copy(string sourceDirName, string destDirName)
    {
        if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
        {
            sourceDirName = sourceDirName + "\\";
        }

        if (destDirName.Substring(destDirName.Length - 1) != "\\")
        {
            destDirName = destDirName + "\\";
        }

        if (Directory.Exists(sourceDirName))
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (string item in Directory.GetFiles(sourceDirName))
            {
                if (item.Contains("meta"))
                {
                    continue;
                }

                File.Copy(item, destDirName + Path.GetFileName(item), true);
            }

            foreach (string item in Directory.GetDirectories(sourceDirName))
            {
                Copy(item, destDirName + item.Substring(item.LastIndexOf("\\", StringComparison.Ordinal) + 1));
            }
        }
    }
}