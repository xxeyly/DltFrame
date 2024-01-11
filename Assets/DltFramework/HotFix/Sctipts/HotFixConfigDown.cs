using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HotFixConfigDown : MonoBehaviour
{
    public static HotFixConfigDown Instance;

    private void Awake()
    {
        Instance = this;
    }

    [LabelText("下载地址")] public string downPath = "http://127.0.0.1/";
    [LabelText("下载UI进度条")] public Slider progress;
    [LabelText("下载进度文本")] public Text progressPercentage;
    [LabelText("总的下载量")] public Text totalDownload;
    [LabelText("当前下载速度")] public Text currentDownSpeed;
    [LabelText("下载流")] private FileStream _hotFixFileStream;
    [LabelText("下载请求")] private UnityWebRequest _hotFixUnityWebRequest;
    [LabelText("总的下载量数据")] public double totalDownloadValue;
    [LabelText("当前下载量数据")] public double currentDownloadValue;
    public HotFixRuntimeDownConfig currentHotFixRuntimeDownConfig;
    [LabelText("下载大小")] public int hotFixAssetConfigDownSize;
    [LabelText("缓存更改路径")] public List<string> replaceCacheFile = new List<string>();
    
    private float time;
    private float timer = 1;

    /// <summary>
    /// 转换字节大小、长度, 根据字节大小范围返回KB, MB, GB自适长度
    /// </summary>
    /// <param name="length">传入字节大小</param>
    /// <returns></returns>
    private string FileSizeString(double length)
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

    //更新UI
    private void UpdateView()
    {
        progress.value = (float)(currentDownloadValue / totalDownloadValue);
        progressPercentage.text = (int)(currentDownloadValue / totalDownloadValue * 100) + "/" + 100;
    }

    [LabelText("写入内容")]
    private void WriteContent(FileStream fileStream)
    {
        if (fileStream != null && _hotFixUnityWebRequest != null && _hotFixUnityWebRequest.downloadHandler != null && _hotFixUnityWebRequest.downloadHandler.data != null)
        {
            int index = (int)fileStream.Length;
            // Debug.Log("本地文件大小:" + index);
            int downSize = _hotFixUnityWebRequest.downloadHandler.data.Length;
            // Debug.Log("下载大小:" + downSize);
            int newDownSize = (downSize - hotFixAssetConfigDownSize);
            // Debug.Log("新内容大小:" + newDownSize);
            fileStream.Seek(index, SeekOrigin.Begin);
            // Debug.Log("有下载更新:" + newDownSize);
            if (newDownSize > 0)
            {
                fileStream.Write(_hotFixUnityWebRequest.downloadHandler.data, hotFixAssetConfigDownSize, newDownSize);
                hotFixAssetConfigDownSize = downSize;
                currentDownSpeed.text = FileSizeString(newDownSize);
                currentDownloadValue += newDownSize;
                totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
                UpdateView();
            }
            else
            {
                // Debug.Log("无更新内容");
            }
        }
        else
        {
            hotFixAssetConfigDownSize = 0;
        }
    }

    private static long GetFileSize(string fileName)
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

    private static string GetMD5HashFromFile(string fileName)
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

    private void ReplaceCacheFile()
    {
        foreach (string cachePath in replaceCacheFile)
        {
            string replacePath = cachePath.Replace(".Cache", "");
            if (File.Exists(replacePath))
            {
                File.Delete(replacePath);
            }

            if (File.Exists(cachePath))
            {
                File.Move(cachePath, replacePath);
            }
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= timer)
        {
            time = 0;
            // UpdateHotFixViewDownProgress();
            if (_hotFixFileStream != null && _hotFixUnityWebRequest != null)
            {
                WriteContent(_hotFixFileStream);
            }
        }
    }
}