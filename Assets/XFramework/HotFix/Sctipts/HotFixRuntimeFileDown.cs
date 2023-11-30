using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


//开始下载
public delegate void HotFixRuntimeDownStart();

//下载速度
public delegate void HotFixRuntimeDownSpeed(float downSpeed);

//下载量
public delegate void HotFixRuntimeDownloadValue(double current, double total);

//开始结束
public delegate void HotFixRuntimeDownOver();

public class HotFixRuntimeFileDown : MonoBehaviour
{
    [LabelText("下载地址")] public string hotFixPath = "http://127.0.0.1/";
    [LabelText("缓存更改路径")] public List<string> replaceCacheFile = new List<string>();
    [LabelText("当前检测时间")] [SerializeField] private float currentCheckTime;
    [LabelText("检测时间")] [SerializeField] private float checkTime = 1;
    [LabelText("下载流")] private FileStream _hotFixFileStream;
    [LabelText("下载请求")] private UnityWebRequest _hotFixUnityWebRequest;
    [LabelText("上一次下载字节长度")] public int oldDownByteLength;
    [LabelText("当前下载量数据")] public double currentDownloadValue;
    [LabelText("总的下载量数据")] public double totalDownloadValue;
    [LabelText("下载开始")] public static HotFixRuntimeDownStart HotFixRuntimeDownStart;
    [LabelText("下载完毕")] public static HotFixRuntimeDownOver HotFixRuntimeDownOver;
    [LabelText("下载速度")] public static HotFixRuntimeDownSpeed HotFixRuntimeDownSpeed;
    [LabelText("当前下载量")] public static HotFixRuntimeDownloadValue HotFixRuntimeDownloadValue;

    [LabelText("HotFixRuntimeDownConfig下载完毕")]
    public bool hotFixRuntimeDownConfigOver;

    //替换缓存文件
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

    public void DownHotFixRuntimeDownConfig(List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig, string hotFixPath)
    {
        HotFixRuntimeDownStart?.Invoke();
        this.hotFixPath = hotFixPath;
        foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in needDownHotFixRuntimeDownConfig)
        {
            totalDownloadValue += double.Parse(hotFixRuntimeDownConfig.size);
        }

        HotFixRuntimeDownloadValue?.Invoke(currentDownloadValue, totalDownloadValue);
        StartCoroutine(StartDownHotFixRuntimeDownConfig(needDownHotFixRuntimeDownConfig));
    }

    IEnumerator StartDownHotFixRuntimeDownConfig(List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig)
    {
        for (int i = 0; i < needDownHotFixRuntimeDownConfig.Count; i++)
        {
            hotFixRuntimeDownConfigOver = false;
            Debug.Log("下载:" + needDownHotFixRuntimeDownConfig[i].name);
            StartCoroutine(HotFixRuntimeDownConfigLocalCacheCheck(needDownHotFixRuntimeDownConfig[i]));
            yield return new WaitUntil(() => hotFixRuntimeDownConfigOver);
        }

        Debug.Log("所有文件下载完毕");
        ReplaceCacheFile();
        yield return new WaitForSeconds(1f);
        HotFixRuntimeDownOver?.Invoke();
        HotFIxOver.Over();
    }

    IEnumerator HotFixRuntimeDownConfigLocalCacheCheck(HotFixRuntimeDownConfig hotFixAssetConfig)
    {
        //下载路径
        string downFileUrl = hotFixPath + hotFixAssetConfig.path + hotFixAssetConfig.name;
        //本地路径文件夹
        string localPathDirectory = HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixAssetConfig.path;
        //文件夹不存在,创建文件夹
        if (!Directory.Exists(localPathDirectory))
        {
            Directory.CreateDirectory(localPathDirectory);
        }

        //下载文件缓存路径
        string downFileCachePath = localPathDirectory + hotFixAssetConfig.name + ".Cache";
        bool isCache = File.Exists(downFileCachePath);

        if (isCache)
        {
            //本地缓存文件的Md5
            string localCacheMd5 = HotFixGlobal.GetMD5HashFromFile(downFileCachePath);
            Debug.Log("存在缓存文件:" + downFileCachePath + ":" + localCacheMd5);
            //当前下载量加上已经下载的缓存量
            currentDownloadValue += HotFixGlobal.GetFileSize(downFileCachePath);
            //缓存文件的Md5和服务器的Md5相同,表示已经下载完毕
            if (localCacheMd5 == hotFixAssetConfig.md5)
            {
                hotFixRuntimeDownConfigOver = true;
                replaceCacheFile.Add(downFileCachePath);
            }
            else
            {
                //下载请求
                _hotFixUnityWebRequest = UnityWebRequest.Get(downFileUrl);
                //更新下载数据
                HotFixRuntimeDownloadValue?.Invoke(currentDownloadValue, totalDownloadValue);
                //使用断点续传下载
                _hotFixUnityWebRequest.SetRequestHeader("Range", "bytes=" + HotFixGlobal.GetFileSize(downFileCachePath) + "-");
                StartCoroutine(DownHotFixRuntimeDownConfig(downFileCachePath, hotFixAssetConfig));
            }
        }
        else
        {
            //下载请求
            _hotFixUnityWebRequest = UnityWebRequest.Get(downFileUrl);
            StartCoroutine(DownHotFixRuntimeDownConfig(downFileCachePath, hotFixAssetConfig));
        }

        yield return null;
    }

    //下载HotFixAssetConfig
    IEnumerator DownHotFixRuntimeDownConfig(string downFileCachePath, HotFixRuntimeDownConfig hotFixAssetConfig)
    {
        //文件流
        _hotFixFileStream = new FileStream(downFileCachePath, FileMode.OpenOrCreate, FileAccess.Write);
        //开启下载
        yield return _hotFixUnityWebRequest.SendWebRequest();
        //重置检测时间
        currentCheckTime = 0;
        //下载流程完毕,直接写入文件
        WriteContent(_hotFixFileStream);
        //重置上一次下载字节长度
        oldDownByteLength = 0;
        //关闭下载流
        _hotFixFileStream.Close();
        _hotFixFileStream.Dispose();
        _hotFixFileStream = null;
        //检测下载完后的文件的Md5
        string localCacheMd5 = HotFixGlobal.GetMD5HashFromFile(downFileCachePath);
        if (_hotFixUnityWebRequest.responseCode != 200 && _hotFixUnityWebRequest.responseCode != 206)
        {
            //下载出错,发起下次下载请求
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(HotFixRuntimeDownConfigLocalCacheCheck(hotFixAssetConfig));
        }
        else
        {
            if (localCacheMd5 != hotFixAssetConfig.md5)
            {
                Debug.LogError("Md5不匹配,删除文件重新下载:" + _hotFixUnityWebRequest.url);
                Debug.Log("本地下载的Md5:" + localCacheMd5);
                Debug.Log("服务器的Md5:" + hotFixAssetConfig.md5);
                //重置上一次下载字节长度
                oldDownByteLength = 0;
                //清除已经下载的大小
                currentDownloadValue -= HotFixGlobal.GetFileSize(downFileCachePath);
                //关闭下载流
                _hotFixUnityWebRequest = null;
                //删除文件
                if (File.Exists(downFileCachePath))
                {
                    File.Delete(downFileCachePath);
                }

                //再次发起下载请求
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(HotFixRuntimeDownConfigLocalCacheCheck(hotFixAssetConfig));
            }
            else
            {
                Debug.Log("下载完毕:" + hotFixAssetConfig.name);
                _hotFixUnityWebRequest = null;
                hotFixRuntimeDownConfigOver = true;
                replaceCacheFile.Add(downFileCachePath);
            }
        }
    }

    private void Update()
    {
        currentCheckTime += Time.deltaTime;
        if (currentCheckTime >= checkTime)
        {
            currentCheckTime = 0;
            // UpdateHotFixViewDownProgress();
            if (_hotFixFileStream != null && _hotFixUnityWebRequest != null)
            {
                WriteContent(_hotFixFileStream);
            }
        }
    }

    [LabelText("写入内容")]
    //下载文件完毕或者下载文件过大,需要分段下载,写入文件
    private void WriteContent(FileStream fileStream)
    {
        if (fileStream != null && _hotFixUnityWebRequest != null && _hotFixUnityWebRequest.downloadHandler != null && _hotFixUnityWebRequest.downloadHandler.data != null)
        {
            // Debug.Log("本地大小:" + fileStream.Length);
            //下载文件大小
            int downSize = _hotFixUnityWebRequest.downloadHandler.data.Length;
            // Debug.Log("当前下载大小:" + downSize);
            //写入文件长度
            int newDownSize = downSize - oldDownByteLength;
            // Debug.Log("写入大小:" + newDownSize);
            // Debug.Log("有下载更新:" + newDownSize);
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);

            if (newDownSize > 0)
            {
                // Debug.Log(oldDownByteLength + ";" + newDownSize);
                fileStream.Write(_hotFixUnityWebRequest.downloadHandler.data, oldDownByteLength, newDownSize);
                HotFixRuntimeDownSpeed?.Invoke(newDownSize);
                currentDownloadValue += newDownSize;
                HotFixRuntimeDownloadValue?.Invoke(currentDownloadValue, totalDownloadValue);
                oldDownByteLength = downSize;
                // Debug.Log("写入后大小:" + fileStream.Length);
            }
            else
            {
                // Debug.Log("无更新内容");
            }
        }
    }
}