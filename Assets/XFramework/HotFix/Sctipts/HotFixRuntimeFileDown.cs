using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HotFixRuntimeFileDown : MonoBehaviour
{
    public static void DownHotFixRuntimeDownConfig(List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig)
    {
        /*Debug.Log("加载完毕");

        if (needDownHotFixRuntimeDownConfig.Count == 0)
        {
            ReplaceCacheFile();
            HotFixInit.Over();
        }
        else
        {
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in needDownHotFixRuntimeDownConfig)
            {
                totalDownloadValue += double.Parse(hotFixRuntimeDownConfig.Size);
            }

            StartCoroutine(HotFixDown(needDownHotFixRuntimeDownConfig, 0, () =>
            {
                ReplaceCacheFile();
                HotFixInit.Over();
            }));
        }*/
    }

    /*IEnumerator HotFixDown(List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig, int index, Action action)
    {
        if (index <= needDownHotFixRuntimeDownConfig.Count - 1)
        {
            HotFixRuntimeDownConfig hotFixRuntimeDownConfig = null;
            hotFixRuntimeDownConfig = needDownHotFixRuntimeDownConfig[index];
            Debug.Log("开始下载:" + hotFixRuntimeDownConfig.Name);
            currentHotFixRuntimeDownConfig = hotFixRuntimeDownConfig;
            string url = downPath + "/" + hotFixRuntimeDownConfig.Path + hotFixRuntimeDownConfig.Name;
            if (!Directory.Exists(HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixRuntimeDownConfig.Path))
            {
                Directory.CreateDirectory(HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixRuntimeDownConfig.Path);
            }

            //本地路径
            string downFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixRuntimeDownConfig.Path + hotFixRuntimeDownConfig.Name;
            string downFileCachePath = downFilePath + ".Cache";


            //检查本地缓存文件
            string localMd5 = String.Empty;
            if (File.Exists(downFileCachePath))
            {
                byte[] localCache = File.ReadAllBytes(downFileCachePath);
                localMd5 = GetMD5HashByte(localCache);
            }

            //文件流
            _hotFixFileStream = new FileStream(downFileCachePath, FileMode.OpenOrCreate, FileAccess.Write);
            //下载请求
            _hotFixUnityWebRequest = UnityWebRequest.Get(url);
            hotFixAssetConfigDownSize = 0;

            if (localMd5 == hotFixRuntimeDownConfig.Md5)
            {
                _hotFixFileStream.Close();
                _hotFixFileStream.Dispose();
                _hotFixFileStream = null;
                _hotFixUnityWebRequest = null;
                replaceCacheFile.Add(downFileCachePath);
                index += 1;
                StartCoroutine(HotFixDown(needDownHotFixRuntimeDownConfig, index, action));
            }
            else
            {
                //请求地址
                //有缓存文件
                if (_hotFixFileStream.Length > 0)
                {
                    Debug.Log("已有缓存文件,继续下载:" + hotFixRuntimeDownConfig.Name);
                    currentDownloadValue += _hotFixFileStream.Length;
                    totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
                    _hotFixUnityWebRequest.SetRequestHeader("Range", "bytes=" + _hotFixFileStream.Length + "-");
                }
                else
                {
                    Debug.Log("新文件,重新下载:" + hotFixRuntimeDownConfig.Name);
                }

                yield return _hotFixUnityWebRequest.SendWebRequest();
                WriteContent(_hotFixFileStream);
                _hotFixFileStream.Close();
                _hotFixFileStream.Dispose();
                _hotFixFileStream = null;
                hotFixAssetConfigDownSize = (int)GetFileSize(downFileCachePath);
                string localFileMd5 = GetMD5HashFromFile(downFileCachePath);
                // Debug.Log(_hotFixUnityWebRequest.responseCode);
                if (_hotFixUnityWebRequest.responseCode != 200 && _hotFixUnityWebRequest.responseCode != 206)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(HotFixDown(needDownHotFixRuntimeDownConfig, index, action));
                }
                else
                {
                    hotFixAssetConfigDownSize = 0;
                    if (localFileMd5 != hotFixRuntimeDownConfig.Md5)
                    {
                        Debug.LogError("Md5不匹配,删除文件重新下载:" + _hotFixUnityWebRequest.url);
                        Debug.Log("本地下载的Md5:" + localFileMd5);
                        Debug.Log("服务器的Md5:" + hotFixRuntimeDownConfig.Md5);
                        //下载错误,移除错误数据大小
                        currentDownloadValue -= _hotFixUnityWebRequest.downloadHandler.data.Length;
                        totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
                        _hotFixUnityWebRequest = null;
                        if (File.Exists(downFileCachePath))
                        {
                            File.Delete(downFileCachePath);
                        }

                        yield return new WaitForSeconds(0.2f);
                        StartCoroutine(HotFixDown(needDownHotFixRuntimeDownConfig, index, action));
                    }
                    else
                    {
                        index += 1;
                        replaceCacheFile.Add(downFileCachePath);
                        Debug.Log("下载完毕:" + hotFixRuntimeDownConfig.Name);
                        StartCoroutine(HotFixDown(needDownHotFixRuntimeDownConfig, index, action));
                    }
                }
            }
        }
        else
        {
            action.Invoke();
        }
    }*/
}