using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Aot : MonoBehaviour
{
    [LabelText("下载UI进度条")] public Slider progress;
    [LabelText("下载进度文本")] public Text progressPercentage;
    [LabelText("下载地址")] public string downPath = "http://127.0.0.1/";
    [LabelText("当前进度")] public float currentProgress;
    [LabelText("进度上限")] public float progressMax = 6;
    [LabelText("总的下载量")] public Text totalDownload;
    [LabelText("当前下载速度")] public Text currentDownSpeed;
    [LabelText("总的下载量数据")] public double totalDownloadValue;
    [LabelText("当前下载量数据")] public double currentDownloadValue;
    [BoxGroup("HotFixView")] public HotFixAssetConfig hotFixViewHotFixAssetConfig;

    [LabelText("HotFixViewAssetConfig下载完毕")] [BoxGroup("HotFixView")]
    public bool hotFixViewConfigDownOver;

    [BoxGroup("HotFixView")] [LabelText("HotFixView本地检测")]
    public bool hotFixViewLocalCheck;

    [BoxGroup("HotFixView")] [LabelText("HotFixView是否需要下载")]
    public bool hotFixViewIsNeedDown;

    [BoxGroup("HotFixView")] [LabelText("HotFixView下载完毕")]
    public bool hotFixViewDownOver;

    [BoxGroup("HotFixCode")] public HotFixAssetConfig hotFixCodeHotFixAssetConfig;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCodeAssetConfig下载完毕")]
    public bool hotFixCodeConfigDownOver;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode本地检测")]
    public bool hotFixCodeLocalCheck;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode是否需要下载")]
    public bool hotFixCodeIsNeedDown;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode下载完毕")]
    public bool hotFixCodeDownOver;

    [LabelText("下载大小")] private Dictionary<string, int> _hotFixAssetConfigDownSize = new Dictionary<string, int>();
    [LabelText("下载流")] private FileStream _hotFixFileStream;
    [LabelText("下载请求")] private UnityWebRequest _hotFixUnityWebRequest;
    [LabelText("当前下载HotFixAssetConfig")] public HotFixAssetConfig currentOperationHotFixAssetConfig;
    [LabelText("缓存更改路径")] public List<string> replaceCacheFile = new List<string>();
    private float time;
    private float timer = 1;

    void Start()
    {
        StartCoroutine(StartCheckAssetBundleUpdate());
    }

    IEnumerator StartCheckAssetBundleUpdate()
    {
        //HotFixView服务器配置表检测
        StartCoroutine(HotFixViewConfigCheck());
        yield return new WaitUntil(() => hotFixViewConfigDownOver);
        //HotFixView本地检查
        StartCoroutine(HotFixViewLocalCheck());
        yield return new WaitUntil(() => hotFixViewLocalCheck);
        //HotFixCode服务器配置表检测
        StartCoroutine(HotFixCodeConfigCheck());
        yield return new WaitUntil(() => hotFixCodeConfigDownOver);
        //HotFixCode本地检查
        StartCoroutine(HotFixCodeLocalCheck());
        yield return new WaitUntil(() => hotFixCodeLocalCheck);
        if (hotFixViewIsNeedDown)
        {
            //HotFixView下载
            if (!_hotFixAssetConfigDownSize.ContainsKey(hotFixViewHotFixAssetConfig.name))
            {
                _hotFixAssetConfigDownSize.Add(hotFixViewHotFixAssetConfig.name, 0);
            }

            currentOperationHotFixAssetConfig = hotFixViewHotFixAssetConfig;
            StartCoroutine(HotFixDown(hotFixViewHotFixAssetConfig, () => { hotFixViewDownOver = true; }));
        }
        else
        {
            hotFixViewDownOver = true;
            UpdateView();
        }

        yield return new WaitUntil(() => hotFixViewDownOver);
        if (hotFixCodeIsNeedDown)
        {
            //HotFixCode下载
            if (!_hotFixAssetConfigDownSize.ContainsKey(hotFixCodeHotFixAssetConfig.name))
            {
                _hotFixAssetConfigDownSize.Add(hotFixCodeHotFixAssetConfig.name, 0);
            }

            currentOperationHotFixAssetConfig = hotFixCodeHotFixAssetConfig;
            StartCoroutine(HotFixDown(hotFixCodeHotFixAssetConfig, () => { hotFixCodeDownOver = true; }));
        }
        else
        {
            hotFixCodeDownOver = true;
            UpdateView();
        }

        yield return new WaitUntil(() => hotFixCodeDownOver);
        ReplaceCacheFile();
        LoadHotFixCode();
    }

    //HotFixView配置
    IEnumerator HotFixViewConfigCheck()
    {
        UnityWebRequest hotFixViewConfigWebRequest = UnityWebRequest.Get(downPath + "HotFix/HotFixConfig/HotFixViewConfig.json");
        yield return hotFixViewConfigWebRequest.SendWebRequest();
        if (hotFixViewConfigWebRequest.responseCode != 200)
        {
            StartCoroutine(HotFixViewConfigCheck());
        }
        else
        {
            hotFixViewHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixViewConfigWebRequest.downloadHandler.text);
            UpdateView();
            hotFixViewConfigDownOver = true;
        }
    }

    //HotFixView本地检测
    IEnumerator HotFixViewLocalCheck()
    {
        hotFixViewLocalCheck = false;
        //检查文件
        hotFixViewIsNeedDown = false;
        if (File.Exists(Application.streamingAssetsPath + "/HotFix/HotFixAsset/" + hotFixViewHotFixAssetConfig.name))
        {
            UnityWebRequest hotFixViewLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix/HotFixAsset/" + hotFixViewHotFixAssetConfig.name);
            yield return hotFixViewLoadLocalFile.SendWebRequest();

            if (hotFixViewLoadLocalFile.downloadHandler.data.Length > 0)
            {
                string localFileMD5 = GetMD5HashByte(hotFixViewLoadLocalFile.downloadHandler.data);
                if (hotFixViewHotFixAssetConfig.md5 != localFileMD5)
                {
                    hotFixViewIsNeedDown = true;
                }
            }
            else
            {
                hotFixViewIsNeedDown = true;
            }
        }
        else
        {
            hotFixViewIsNeedDown = true;
        }

        if (hotFixViewIsNeedDown)
        {
            totalDownloadValue += double.Parse(hotFixViewHotFixAssetConfig.size);
            totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
        }

        hotFixViewLocalCheck = true;
    }

    //HotFixCode配置
    IEnumerator HotFixCodeConfigCheck()
    {
        UnityWebRequest hotFixCodeConfigWebRequest = UnityWebRequest.Get(downPath + "HotFix/HotFixConfig/HotFixCodeConfig.json");
        yield return hotFixCodeConfigWebRequest.SendWebRequest();
        if (hotFixCodeConfigWebRequest.responseCode != 200)
        {
            StartCoroutine(HotFixCodeConfigCheck());
        }
        else
        {
            hotFixCodeHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixCodeConfigWebRequest.downloadHandler.text);
            UpdateView();

            hotFixCodeConfigDownOver = true;
        }
    }

    //HotFixCode本地检测
    IEnumerator HotFixCodeLocalCheck()
    {
        hotFixCodeLocalCheck = false;
        //检查文件
        hotFixCodeIsNeedDown = false;
        if (File.Exists(Application.streamingAssetsPath + "/HotFix/HotFixAsset/" + hotFixCodeHotFixAssetConfig.name))
        {
            UnityWebRequest hotFixCodeLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix/HotFixAsset/" + hotFixCodeHotFixAssetConfig.name);
            yield return hotFixCodeLoadLocalFile.SendWebRequest();

            if (hotFixCodeLoadLocalFile.downloadHandler.data.Length > 0)
            {
                if (hotFixCodeHotFixAssetConfig.md5 != GetMD5HashByte(hotFixCodeLoadLocalFile.downloadHandler.data))
                {
                    hotFixCodeIsNeedDown = true;
                }
            }
            else
            {
                hotFixCodeIsNeedDown = true;
            }
        }
        else
        {
            hotFixCodeIsNeedDown = true;
        }

        if (hotFixCodeIsNeedDown)
        {
            totalDownloadValue += double.Parse(hotFixCodeHotFixAssetConfig.size);
            totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
        }

        hotFixCodeLocalCheck = true;
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

    //获得文件大小
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

    IEnumerator HotFixDown(HotFixAssetConfig hotFixAssetConfig, Action action)
    {
        string url = downPath + "HotFix/HotFixAsset/" + hotFixAssetConfig.name;
        //文件夹不存在要创建
        if (!Directory.Exists(Application.streamingAssetsPath + "/" + "HotFix/HotFixAsset"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/" + "HotFix/HotFixAsset");
        }

        //本地路径
        string downFilePath = Application.streamingAssetsPath + "/" + "HotFix/HotFixAsset" + "/" + hotFixAssetConfig.name;

        //缓存路径
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
        _hotFixAssetConfigDownSize[hotFixAssetConfig.name] = 0;

        if (localMd5 == hotFixAssetConfig.md5)
        {
            _hotFixFileStream.Close();
            _hotFixFileStream.Dispose();
            _hotFixFileStream = null;
            _hotFixUnityWebRequest = null;
            UpdateView();
            action.Invoke();
        }
        else
        {
            //请求地址
            //有缓存文件
            if (_hotFixFileStream.Length > 0)
            {
                Debug.Log("已有缓存文件,继续下载:" + hotFixAssetConfig.name);
                currentDownloadValue += _hotFixFileStream.Length;
                totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
                UpdateView((float)(_hotFixFileStream.Length / double.Parse(currentOperationHotFixAssetConfig.size)));
                _hotFixUnityWebRequest.SetRequestHeader("Range", "bytes=" + _hotFixFileStream.Length + "-");
            }
            else
            {
                Debug.Log("新文件,重新下载:" + hotFixAssetConfig.name);
            }

            yield return _hotFixUnityWebRequest.SendWebRequest();
            WriteContent(_hotFixFileStream, hotFixAssetConfig.name);
            _hotFixFileStream.Close();
            _hotFixFileStream.Dispose();
            _hotFixFileStream = null;
            _hotFixAssetConfigDownSize[hotFixAssetConfig.name] = (int)GetFileSize(downFileCachePath);
            string localFileMd5 = GetMD5HashFromFile(downFileCachePath);
            // Debug.Log(_hotFixUnityWebRequest.responseCode);
            if (_hotFixUnityWebRequest.responseCode != 200 && _hotFixUnityWebRequest.responseCode != 206)
            {
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(HotFixDown(hotFixAssetConfig, action));
            }
            else
            {
                _hotFixAssetConfigDownSize[hotFixAssetConfig.name] = 0;
                if (localFileMd5 != hotFixAssetConfig.md5)
                {
                    Debug.LogError("Md5不匹配,删除文件重新下载:" + _hotFixUnityWebRequest.url);
                    Debug.Log("本地下载的Md5:" + localFileMd5);
                    Debug.Log("服务器的Md5:" + hotFixAssetConfig.md5);
                    _hotFixUnityWebRequest = null;
                    if (File.Exists(downFileCachePath))
                    {
                        File.Delete(downFileCachePath);
                    }

                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(HotFixDown(hotFixAssetConfig, action));
                }
                else
                {
                    Debug.Log("下载完毕:" + hotFixAssetConfig.name);
                    _hotFixUnityWebRequest = null;
                    replaceCacheFile.Add(downFileCachePath);
                    action.Invoke();
                }
            }
        }
    }


    //加载XFrameworkHotFix数据
    private void LoadHotFixCode()
    {
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。  
#if !UNITY_EDITOR
        Assembly hotFix = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotFix/HotFixAsset/XFrameworkHotFix.dll.bytes"));
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集  
        Assembly hotFix = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "XFrameworkHotFix");
#endif
        Type type = hotFix.GetType("HotFixInit");
        type.GetMethod("Init").Invoke(null, null);
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
                WriteContent(_hotFixFileStream, currentOperationHotFixAssetConfig.name);
            }
        }
    }

    [LabelText("写入内容")]
    private void WriteContent(FileStream fileStream, string fileName)
    {
        if (fileStream != null && _hotFixUnityWebRequest != null && _hotFixUnityWebRequest.downloadHandler != null && _hotFixUnityWebRequest.downloadHandler.data != null)
        {
            int index = (int)fileStream.Length;
            // Debug.Log("本地文件大小:" + index);
            int downSize = _hotFixUnityWebRequest.downloadHandler.data.Length;
            // Debug.Log("下载大小:" + downSize);
            int newDownSize = (downSize - _hotFixAssetConfigDownSize[fileName]);
            // Debug.Log("新内容大小:" + newDownSize);
            fileStream.Seek(index, SeekOrigin.Begin);
            // Debug.Log("有下载更新:" + newDownSize);
            if (newDownSize > 0)
            {
                fileStream.Write(_hotFixUnityWebRequest.downloadHandler.data, _hotFixAssetConfigDownSize[fileName], newDownSize);
                _hotFixAssetConfigDownSize[fileName] = downSize;
                currentDownSpeed.text = FileSizeString(newDownSize);
                currentDownloadValue += newDownSize;
                totalDownload.text = FileSizeString(currentDownloadValue) + "/" + FileSizeString(totalDownloadValue);
                UpdateView((float)(newDownSize / double.Parse(currentOperationHotFixAssetConfig.size)));
            }
            else
            {
                // Debug.Log("无更新内容");
            }
        }
        else
        {
            _hotFixAssetConfigDownSize[fileName] = 0;
        }
    }


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

    private string GetMD5HashByte(byte[] fileByte)
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

    //更新UI
    private void UpdateView(float addProgress = 1)
    {
        currentProgress += addProgress;
        progress.value = currentProgress / progressMax;
        progressPercentage.text = (int)(currentProgress / progressMax * 100) + "/" + 100;
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
    }

    private void OnApplicationQuit()
    {
        if (_hotFixFileStream != null)
        {
            _hotFixFileStream.Dispose();
            _hotFixFileStream.Close();
        }
    }
}