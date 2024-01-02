using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class AotGlobal
{
    //获得设备存储路径
    public static string GetDeviceStoragePath(bool unityWebRequestPath = false)
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

    public static string GetTextToLoad(string path, string fileName)
    {
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

    public static void SaveTextToLoad(string path, string information)
    {
        FileStream aFile = new FileStream(path, FileMode.Create);
        //得到字符串的UTF8 数据流
        information = Regex.Unescape(information);
        byte[] bts = System.Text.Encoding.UTF8.GetBytes(information);
        // StreamWriter sw = new StreamWriter(aFile, Encoding.UTF8);
        // sw.WriteLine(information);
        // sw.Close();
        aFile.Write(bts, 0, bts.Length);
        if (aFile != null)
        {
            //清空缓存
            aFile.Flush();
            // 关闭流
            aFile.Close();
            //销毁资源
            aFile.Dispose();
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
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

                Debug.Log("拷贝文件" + item);
                File.Copy(item, destDirName + Path.GetFileName(item), true);
            }

            foreach (string item in Directory.GetDirectories(sourceDirName))
            {
                Copy(item, destDirName + item.Substring(item.LastIndexOf("\\", StringComparison.Ordinal) + 1));
            }
        }
        else
        {
            Debug.Log(sourceDirName + "不存在");
        }
    }

    public static void CopyFile(string sourcePath, string destinationPath)
    {
        byte[] fileData = null;
        // 从 StreamingAssets 文件夹读取文件数据
        UnityWebRequest www = UnityWebRequest.Get(sourcePath);
        www.SendWebRequest();
        fileData = www.downloadHandler.data;

        // 创建目标文件夹（如果不存在）
        string destinationFolder = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }

        // 将文件数据写入目标文件
        File.WriteAllBytes(destinationPath, fileData);
    }
}