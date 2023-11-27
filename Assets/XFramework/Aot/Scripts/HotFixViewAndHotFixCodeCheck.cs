using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;


//下载速度
public delegate void HotFixViewAndHotFixCodeDownSpeed(float downSpeed);

//当前下载量
public delegate void HotFixViewAndHotFixCodeCurrentDownValue(double downValue);

//总下载量
public delegate void HotFixViewAndHotFixCodeTotalDownValue(double downValue);

public class HotFixViewAndHotFixCodeCheck : MonoBehaviour
{
    [LabelText("下载地址")] public string hotFixPath = "http://127.0.0.1/";
    [LabelText("下载地址本地读取")] public bool hotFixPathLocalLoad;
    [LabelText("总的下载量数据")] public double totalDownloadValue;
    [LabelText("当前下载量数据")] public double currentDownloadValue;


    //下载速度
    public static HotFixViewAndHotFixCodeDownSpeed HotFixViewAndHotFixCodeDownSpeed;

    //当前下载量
    public static HotFixViewAndHotFixCodeCurrentDownValue HotFixViewAndHotFixCodeCurrentDownValue;

    //总的下载量
    public static HotFixViewAndHotFixCodeTotalDownValue HotFixViewAndHotFixCodeTotalDownValue;


    [BoxGroup("HotFixView")] [LabelText("HotFixView文件数据")]
    public HotFixAssetConfig hotFixViewHotFixAssetConfig;

    [LabelText("HotFixViewAssetConfig下载完毕")] [BoxGroup("HotFixView")]
    public bool hotFixViewConfigDownOver;

    [BoxGroup("HotFixView")] [LabelText("HotFixView本地检测")]
    public bool hotFixViewLocalCheck;

    [BoxGroup("HotFixView")] [LabelText("HotFixView是否需要下载")]
    public bool hotFixViewIsNeedDown;

    [BoxGroup("HotFixView")] [LabelText("HotFixView下载完毕")]
    public bool hotFixViewDownOver;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode文件数据")]
    public HotFixAssetConfig hotFixCodeHotFixAssetConfig;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCodeAssetConfig下载完毕")]
    public bool hotFixCodeConfigDownOver;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode本地检测")]
    public bool hotFixCodeLocalCheck;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode是否需要下载")]
    public bool hotFixCodeIsNeedDown;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode下载完毕")]
    public bool hotFixCodeDownOver;

    [LabelText("当前检测时间")] [SerializeField] private float currentCheckTime;

    [LabelText("检测时间")] [SerializeField] private float checkTime = 1;

    // [LabelText("下载大小")] [SerializeField] private Dictionary<string, int> _hotFixAssetConfigDownSize = new Dictionary<string, int>();
    [LabelText("下载流")] private FileStream _hotFixFileStream;
    [LabelText("下载请求")] private UnityWebRequest _hotFixUnityWebRequest;
    [LabelText("当前下载HotFixAssetConfig")] public HotFixAssetConfig currentOperationHotFixAssetConfig;
    [LabelText("缓存更改路径")] public List<string> replaceCacheFile = new List<string>();


    void Start()
    {
        //如果不是PC平台,会把StreamingAssets中的数据拷贝到设备存储路径中
        string path = AotGlobal.GetDeviceStoragePath();
        if (path != Application.streamingAssetsPath)
        {
            AotGlobal.Copy(Application.streamingAssetsPath, path);
        }

        //开始本地文件检测
        StartCoroutine(StartCheckAssetBundleUpdate());
    }

    IEnumerator StartCheckAssetBundleUpdate()
    {
        //HotFix路径
        StartCoroutine(HotFixPathLocalLoad());
        yield return new WaitUntil(() => hotFixPathLocalLoad);
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
        //HotFixView需要下载
        if (hotFixViewIsNeedDown)
        {
            currentOperationHotFixAssetConfig = hotFixViewHotFixAssetConfig;
            StartCoroutine(HotFixDown(hotFixViewHotFixAssetConfig, () => { hotFixViewDownOver = true; }));
        }
        else
        {
            hotFixViewDownOver = true;
        }

        yield return new WaitUntil(() => hotFixViewDownOver);
        if (hotFixCodeIsNeedDown)
        {
            currentOperationHotFixAssetConfig = hotFixCodeHotFixAssetConfig;
            StartCoroutine(HotFixDown(hotFixCodeHotFixAssetConfig, () => { hotFixCodeDownOver = true; }));
        }
        else
        {
            hotFixCodeDownOver = true;
        }

        yield return new WaitUntil(() => hotFixCodeDownOver);
        ReplaceCacheFile();
        LoadHotFixCode();
    }

    //HotFixPath本地检测
    IEnumerator HotFixPathLocalLoad()
    {
        //本地下载路径
        string hotFixDownPath = AotGlobal.GetDeviceStoragePath() + "/HotFix/" + "HotFixDownPath.txt";
        if (File.Exists(hotFixDownPath))
        {
            UnityWebRequest hotFixPathLoadLocalFile = UnityWebRequest.Get(hotFixDownPath);
            yield return hotFixPathLoadLocalFile.SendWebRequest();
            hotFixPath = hotFixPathLoadLocalFile.downloadHandler.text;
            //如果结尾不是/,添加/
            if (hotFixPath[hotFixPath.Length - 1] != '/')
            {
                hotFixPath += "/";
            }
        }
        else
        {
            Debug.LogWarning(hotFixDownPath + "不存在,使用默认下载地址");
        }

        hotFixPathLocalLoad = true;
    }

    //HotFixView配置
    IEnumerator HotFixViewConfigCheck()
    {
        UnityWebRequest hotFixViewConfigWebRequest = UnityWebRequest.Get(hotFixPath + "HotFix/HotFixViewConfig/HotFixViewConfig.json");
        yield return hotFixViewConfigWebRequest.SendWebRequest();
        if (hotFixViewConfigWebRequest.responseCode != 200)
        {
            Debug.Log("访问错误:" + hotFixViewConfigWebRequest.url + hotFixViewConfigWebRequest.responseCode);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(HotFixViewConfigCheck());
        }
        else
        {
            //读取远程配置表数据
            hotFixViewHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixViewConfigWebRequest.downloadHandler.text);
            //配置表下载完毕
            hotFixViewConfigDownOver = true;
        }
    }

    //HotFixView本地检测
    IEnumerator HotFixViewLocalCheck()
    {
        //本地hotFixView路径检测
        hotFixViewLocalCheck = false;
        //检查文件
        hotFixViewIsNeedDown = false;
        //本地HotFixView路径
        string localHotFixViewPath = AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixView/" + hotFixViewHotFixAssetConfig.name;
        if (File.Exists(localHotFixViewPath))
        {
            UnityWebRequest hotFixViewLoadLocalFile = UnityWebRequest.Get(localHotFixViewPath);
            yield return hotFixViewLoadLocalFile.SendWebRequest();
            //本地文件数据大于0
            if (hotFixViewLoadLocalFile.downloadHandler.data.Length > 0)
            {
                //获得当前文件的Md5
                string localFileMD5 = AotGlobal.GetMD5HashByte(hotFixViewLoadLocalFile.downloadHandler.data);
                //Md5值不同,表示服务器端有更新
                if (hotFixViewHotFixAssetConfig.md5 != localFileMD5)
                {
                    //需要下载
                    hotFixViewIsNeedDown = true;
                }
            }
            else
            {
                //需要下载
                hotFixViewIsNeedDown = true;
            }
        }
        else
        {
            //需要下载
            hotFixViewIsNeedDown = true;
        }

        if (hotFixViewIsNeedDown)
        {
            //更新最大下载量
            totalDownloadValue += double.Parse(hotFixViewHotFixAssetConfig.size);
            HotFixViewAndHotFixCodeTotalDownValue?.Invoke(totalDownloadValue);
        }

        hotFixViewLocalCheck = true;
    }

    //HotFixCode配置
    IEnumerator HotFixCodeConfigCheck()
    {
        UnityWebRequest hotFixCodeConfigWebRequest = UnityWebRequest.Get(hotFixPath + "HotFix/HotFixCodeConfig/HotFixCodeConfig.json");
        yield return hotFixCodeConfigWebRequest.SendWebRequest();
        if (hotFixCodeConfigWebRequest.responseCode != 200)
        {
            Debug.Log("访问错误:" + hotFixCodeConfigWebRequest.url + hotFixCodeConfigWebRequest.responseCode);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(HotFixCodeConfigCheck());
        }
        else
        {
            //读取配置表
            hotFixCodeHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixCodeConfigWebRequest.downloadHandler.text);
            hotFixCodeConfigDownOver = true;
        }
    }

    //HotFixCode本地检测
    IEnumerator HotFixCodeLocalCheck()
    {
        //本地hotFixCode路径检测
        hotFixCodeLocalCheck = false;
        //检查文件
        hotFixCodeIsNeedDown = false;
        //本地HotFixCode路径
        string localHotFixCodePath = AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixCode/" + hotFixCodeHotFixAssetConfig.name;
        if (File.Exists(localHotFixCodePath))
        {
            UnityWebRequest hotFixCodeLoadLocalFile = UnityWebRequest.Get(localHotFixCodePath);
            yield return hotFixCodeLoadLocalFile.SendWebRequest();
            //本地文件数据大于0
            if (hotFixCodeLoadLocalFile.downloadHandler.data.Length > 0)
            {
                //获得当前文件的Md5
                string localFileMD5 = AotGlobal.GetMD5HashByte(hotFixCodeLoadLocalFile.downloadHandler.data);
                //Md5值不同,表示服务器端有更新
                if (hotFixCodeHotFixAssetConfig.md5 != localFileMD5)
                {
                    //需要下载
                    hotFixCodeIsNeedDown = true;
                }
            }
            else
            {
                //需要下载
                hotFixCodeIsNeedDown = true;
            }
        }
        else
        {
            //需要下载
            hotFixCodeIsNeedDown = true;
        }

        if (hotFixCodeIsNeedDown)
        {
            //更新最大下载量
            totalDownloadValue += double.Parse(hotFixCodeHotFixAssetConfig.size);
            HotFixViewAndHotFixCodeTotalDownValue?.Invoke(totalDownloadValue);
        }

        hotFixCodeLocalCheck = true;
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
        //下载路径
        string downFileUrl = hotFixPath + hotFixAssetConfig.path + "/" + hotFixAssetConfig.name;
        //本地路径文件夹
        string localPathDirectory = AotGlobal.GetDeviceStoragePath() + "/" + hotFixAssetConfig.path + "/";
        //文件夹不存在,创建文件夹
        if (!Directory.Exists(localPathDirectory))
        {
            Directory.CreateDirectory(localPathDirectory);
        }

        //下载文件缓存路径
        string downFileCachePath = localPathDirectory + hotFixAssetConfig.name + ".Cache";

        Debug.Log(downFileCachePath);
        //本地缓存文件的Md5
        string localCacheMd5 = String.Empty;
        if (File.Exists(downFileCachePath))
        {
            byte[] localCache = File.ReadAllBytes(downFileCachePath);
            localCacheMd5 = AotGlobal.GetMD5HashByte(localCache);
        }
        else
        {
            Debug.Log(hotFixAssetConfig.name + ":未查找到");
        }

        //文件流
        _hotFixFileStream = new FileStream(downFileCachePath, FileMode.OpenOrCreate, FileAccess.Write);
        //下载请求
        _hotFixUnityWebRequest = UnityWebRequest.Get(downFileUrl);
        //缓存文件的Md5和服务器的Md5相同,表示已经下载完毕
        if (localCacheMd5 == hotFixAssetConfig.md5)
        {
            _hotFixFileStream.Close();
            _hotFixFileStream.Dispose();
            _hotFixFileStream = null;
            _hotFixUnityWebRequest = null;
            action.Invoke();
        }
        else
        {
            //已有有缓存文件,继续下载
            if (_hotFixFileStream.Length > 0)
            {
                //当前下载量加上已经下载的缓存量
                currentDownloadValue += _hotFixFileStream.Length;
                //更新下载数据
                HotFixViewAndHotFixCodeCurrentDownValue?.Invoke(currentDownloadValue);
                //读取缓存文件
                _hotFixUnityWebRequest.downloadHandler = new DownloadHandlerFile(downFileCachePath);
                //使用断点续传下载
                _hotFixUnityWebRequest.SetRequestHeader("Range", "bytes=" + _hotFixFileStream.Length + "-");
            }

            //开启下载
            yield return _hotFixUnityWebRequest.SendWebRequest();
            //重置检测时间
            currentCheckTime = 0;
            //下载流程完毕,直接写入文件
            WriteContent(_hotFixFileStream);
            //关闭下载流
            _hotFixFileStream.Close();
            _hotFixFileStream.Dispose();
            _hotFixFileStream = null;
            //检测下载完后的文件的Md5
            localCacheMd5 = AotGlobal.GetMD5HashFromFile(downFileCachePath);
            if (_hotFixUnityWebRequest.responseCode != 200 && _hotFixUnityWebRequest.responseCode != 206)
            {
                //下载出错,发起下次下载请求
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(HotFixDown(hotFixAssetConfig, action));
            }
            else
            {
                if (localCacheMd5 != hotFixAssetConfig.md5)
                {
                    Debug.LogError("Md5不匹配,删除文件重新下载:" + _hotFixUnityWebRequest.url);
                    Debug.Log("本地下载的Md5:" + localCacheMd5);
                    Debug.Log("服务器的Md5:" + hotFixAssetConfig.md5);
                    //清除已经下载的大小
                    currentDownloadValue -= GetFileSize(downFileCachePath);
                    //关闭下载流
                    _hotFixUnityWebRequest = null;
                    //删除文件
                    if (File.Exists(downFileCachePath))
                    {
                        File.Delete(downFileCachePath);
                    }

                    //再次发起下载请求
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
        Assembly hotFix = Assembly.Load(File.ReadAllBytes($"{AotGlobal.GetDeviceStoragePath()}/HotFix/HotFixCode/HotFixCode.dll.bytes"));
#else
        // Editor下无需加载，直接查找获得HotFix程序集  
        Assembly hotFix = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotFixCode");
#endif
        Type type = hotFix.GetType("HotFixInit");
        type.GetMethod("Init")?.Invoke(null, null);
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
            //写入前字节长度
            int offset = (int)fileStream.Length;
            //下载文件大小
            int downSize = _hotFixUnityWebRequest.downloadHandler.data.Length;
            //写入文件长度
            int newDownSize = downSize - offset;
            fileStream.Seek(offset, SeekOrigin.Begin);
            // Debug.Log("有下载更新:" + newDownSize);
            if (newDownSize > 0)
            {
                fileStream.Write(_hotFixUnityWebRequest.downloadHandler.data, offset, newDownSize);
                HotFixViewAndHotFixCodeDownSpeed?.Invoke(newDownSize);
                currentDownloadValue += newDownSize;
                HotFixViewAndHotFixCodeCurrentDownValue?.Invoke(currentDownloadValue);
            }
            else
            {
                // Debug.Log("无更新内容");
            }
        }
    }

    //删除缓存文件
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