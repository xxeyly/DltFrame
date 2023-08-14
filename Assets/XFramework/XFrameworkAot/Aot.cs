using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Aot : MonoBehaviour
{
    public Slider progress;
    public Text progressPercentage;
    public static string downPath = "http://127.0.0.1/HotFixDemo/StreamingAssets/";

    public float currentProgress;
    public float progressMax = 6;

    [BoxGroup("HotFixView")] public HotFixAssetConfig hotFixViewHotFixAssetConfig;

    [LabelText("HotFixViewAssetConfig下载完毕")] [BoxGroup("HotFixView")]
    public bool hotFixViewAssetConfigDownOver;

    [BoxGroup("HotFixView")] [LabelText("HotFixView本地检测")]
    public bool hotFixViewLocalCheck;

    [BoxGroup("HotFixView")] [LabelText("HotFixView是否需要下载")]
    public bool hotFixViewIsNeedDown;

    private UnityWebRequest _hotFixViewDownUnityWebRequest;

    [BoxGroup("HotFixView")] [LabelText("HotFixView下载速度")]
    private long _hotFixViewSpeed;

    [BoxGroup("HotFixView")] [LabelText("HotFixView上一次下载大小")]
    private long _hotFixViewLastDownSize;

    [BoxGroup("HotFixView")] [LabelText("HotFixView上一次下载进度")]
    private long _hotFixViewLastDownProgress;

    [BoxGroup("HotFixView")] [LabelText("HotFixView下载完毕")]
    public bool hotFixViewDownOver;

    [BoxGroup("HotFixCode")] public HotFixAssetConfig hotFixCodeHotFixAssetConfig;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCodeAssetConfig下载完毕")]
    public bool hotFixCodeAssetConfigDownOver;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode本地检测")]
    public bool hotFixCodeLocalCheck;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode是否需要下载")]
    public bool hotFixCodeIsNeedDown;

    private UnityWebRequest _hotFixCodeDownUnityWebRequest;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode下载速度")]
    private long _hotFixCodeSpeed;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode上一次下载大小")]
    private long _hotFixCodeLastDownSize;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode上一次下载进度")]
    private long _hotFixCodeLastDownProgress;

    [BoxGroup("HotFixCode")] [LabelText("HotFixCode下载完毕")]
    public bool hotFixCodeDownOver;


    private float time;
    private float timer = 1;

    void Start()
    {
        StartCoroutine(StartCheckAssetBundleUpdate());
    }

    //HotFixView配置
    IEnumerator HotFixViewAssetConfigCheck()
    {
        UnityWebRequest hotFixViewConfigWebRequest = UnityWebRequest.Get(downPath + "HotFix/UpdateHotFix/HotFixAssetConfig/HotFixView.json");
        yield return hotFixViewConfigWebRequest.SendWebRequest();
        if (hotFixViewConfigWebRequest.responseCode != 200)
        {
            StartCoroutine(HotFixViewAssetConfigCheck());
        }
        else
        {
            hotFixViewHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixViewConfigWebRequest.downloadHandler.text);
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
            hotFixViewAssetConfigDownOver = true;
        }
    }

    //HotFixView本地检测
    IEnumerator HotFixViewLocalCheck()
    {
        hotFixViewLocalCheck = false;
        //检查文件
        hotFixViewIsNeedDown = false;
        if (File.Exists(Application.streamingAssetsPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixViewHotFixAssetConfig.name))
        {
            UnityWebRequest hotFixViewLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixViewHotFixAssetConfig.name);
            yield return hotFixViewLoadLocalFile.SendWebRequest();
            currentProgress += 1;
            progress.value = currentProgress / progressMax;

            if (hotFixViewLoadLocalFile.downloadHandler.data.Length > 0)
            {
                if (hotFixViewHotFixAssetConfig.md5 != GetMD5HashByte(hotFixViewLoadLocalFile.downloadHandler.data))
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

        hotFixViewLocalCheck = true;
    }

    //HotFixCode配置
    IEnumerator HotFixCodeAssetConfigCheck()
    {
        UnityWebRequest hotFixCodeConfigWebRequest = UnityWebRequest.Get(downPath + "HotFix/UpdateHotFix/HotFixAssetConfig/HotFixCode.json");
        yield return hotFixCodeConfigWebRequest.SendWebRequest();
        if (hotFixCodeConfigWebRequest.responseCode != 200)
        {
            StartCoroutine(HotFixCodeAssetConfigCheck());
        }
        else
        {
            hotFixCodeHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(hotFixCodeConfigWebRequest.downloadHandler.text);
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
            hotFixCodeAssetConfigDownOver = true;
        }
    }

    //HotFixCode本地检测
    IEnumerator HotFixCodeLocalCheck()
    {
        hotFixCodeLocalCheck = false;
        //检查文件
        hotFixCodeIsNeedDown = false;
        if (File.Exists(Application.streamingAssetsPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixCodeHotFixAssetConfig.name))
        {
            UnityWebRequest hotFixViewLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixCodeHotFixAssetConfig.name);
            yield return hotFixViewLoadLocalFile.SendWebRequest();
            currentProgress += 1;
            progress.value = currentProgress / progressMax;

            if (hotFixViewLoadLocalFile.downloadHandler.data.Length > 0)
            {
                if (hotFixCodeHotFixAssetConfig.md5 != GetMD5HashByte(hotFixViewLoadLocalFile.downloadHandler.data))
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

        hotFixCodeLocalCheck = true;
    }

    //HotFixView下载
    IEnumerator HotFixViewDown()
    {
        //检查文件
        if (hotFixViewIsNeedDown)
        {
            _hotFixViewDownUnityWebRequest = UnityWebRequest.Get(downPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixViewHotFixAssetConfig.name);
            yield return _hotFixViewDownUnityWebRequest.SendWebRequest();
            if (_hotFixViewDownUnityWebRequest.responseCode != 200)
            {
                StartCoroutine(HotFixViewDown());
            }
            else
            {
                UpdateHotFixViewDownProgress();
                hotFixViewDownOver = true;
                time = 0;
                _hotFixViewDownUnityWebRequest = null;
                currentProgress += 1;
                progress.value = currentProgress / progressMax;
            }
        }
        else
        {
            hotFixViewDownOver = true;
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
        }
    }

    //HotFixCode下载
    IEnumerator HotFixCodeDown()
    {
        //检查文件
        if (hotFixCodeIsNeedDown)
        {
            _hotFixCodeDownUnityWebRequest = UnityWebRequest.Get(downPath + "HotFix/UpdateHotFix/HotFixAsset/" + hotFixCodeHotFixAssetConfig.name);
            yield return _hotFixCodeDownUnityWebRequest.SendWebRequest();
            if (_hotFixCodeDownUnityWebRequest.responseCode != 200)
            {
                StartCoroutine(HotFixViewDown());
            }
            else
            {
                UpdateHotFixCodeDownProgress();
                hotFixCodeDownOver = true;
                time = 0;
                _hotFixCodeDownUnityWebRequest = null;
                currentProgress += 1;
                progress.value = currentProgress / progressMax;
            }
        }
        else
        {
            hotFixCodeDownOver = true;
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
        }
    }

    IEnumerator StartCheckAssetBundleUpdate()
    {
        StartCoroutine(HotFixViewAssetConfigCheck());
        yield return new WaitUntil(() => hotFixViewAssetConfigDownOver);
        StartCoroutine(HotFixViewLocalCheck());
        yield return new WaitUntil(() => hotFixViewLocalCheck);

        StartCoroutine(HotFixCodeAssetConfigCheck());
        yield return new WaitUntil(() => hotFixCodeAssetConfigDownOver);
        StartCoroutine(HotFixCodeLocalCheck());
        yield return new WaitUntil(() => hotFixCodeLocalCheck);

        StartCoroutine(HotFixViewDown());
        yield return new WaitUntil(() => hotFixViewDownOver);
        StartCoroutine(HotFixCodeDown());
        yield return new WaitUntil(() => hotFixCodeDownOver);


        /*//检查文件
        bool hotFixViewDown = false;
        if (File.Exists(Application.streamingAssetsPath + "HotFix/UpdateHotFix/hotfixcanvas"))
        {
            UnityWebRequest hotFixViewLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "HotFix/UpdateHotFix/hotfixcanvas");
            yield return hotFixViewLoadLocalFile.SendWebRequest();
            currentProgress += 1;
            progress.value = currentProgress / progressMax;

            if (hotFixViewLoadLocalFile.downloadHandler.data.Length > 0)
            {
                if (hotFixViewAotAssetConfig.md5 != GetMD5HashByte(hotFixViewLoadLocalFile.downloadHandler.data))
                {
                    hotFixViewDown = true;
                }
            }
            else
            {
                hotFixViewDown = true;
            }
        }
        else
        {
            hotFixViewDown = true;
        }

        //检查文件
        if (hotFixViewDown)
        {
            _hotFixViewDownUnityWebRequest = UnityWebRequest.Get(downPath + "HotFixView");
            yield return _hotFixViewDownUnityWebRequest.SendWebRequest();
            UpdateHotFixViewDownProgress();
            time = 0;
            _hotFixViewDownUnityWebRequest = null;
        }
        else
        {
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
        }


        //检查文件
        bool hotFixCodeDown = false;
        if (File.Exists(Application.streamingAssetsPath + "HotFix/UpdateHotFix/XFrameworkHotFix.dll.bytes"))
        {
            UnityWebRequest hotFixCodeLoadLocalFile = UnityWebRequest.Get(Application.streamingAssetsPath + "HotFix/UpdateHotFix/XFrameworkHotFix.dll.bytes");
            yield return hotFixCodeLoadLocalFile.SendWebRequest();
            currentProgress += 1;
            progress.value = currentProgress / progressMax;


            if (hotFixCodeLoadLocalFile.downloadHandler.data.Length > 0)
            {
                if (hotFixCodeAotAssetConfig.md5 != GetMD5HashByte(hotFixCodeLoadLocalFile.downloadHandler.data))
                {
                    hotFixCodeDown = true;
                }
            }
            else
            {
                hotFixCodeDown = true;
            }
        }
        else
        {
            hotFixCodeDown = true;
        }

        //检查文件
        if (hotFixCodeDown)
        {
            _hotFixCodeDownUnityWebRequest = UnityWebRequest.Get(downPath + "HotFixCode");
            yield return _hotFixCodeDownUnityWebRequest.SendWebRequest();
            UpdateHotFixCodeDownProgress();
            time = 0;
            _hotFixViewDownUnityWebRequest = null;
        }
        else
        {
            currentProgress += 1;
            progress.value = currentProgress / progressMax;
        }*/
    }

    private void Update()
    {
        if (_hotFixViewDownUnityWebRequest != null)
        {
            time += Time.deltaTime;
            if (time >= timer)
            {
                // UpdateHotFixViewDownProgress();
            }
        }

        if (_hotFixCodeDownUnityWebRequest != null)
        {
            time += Time.deltaTime;
            if (time >= timer)
            {
                // UpdateHotFixCodeDownProgress();
            }
        }
    }

    private void UpdateHotFixViewDownProgress()
    {
        currentProgress += (_hotFixViewDownUnityWebRequest.downloadProgress - _hotFixViewLastDownProgress);
        _hotFixViewSpeed = _hotFixViewDownUnityWebRequest.downloadHandler.data.Length - _hotFixViewLastDownSize;
        _hotFixViewLastDownSize = _hotFixViewDownUnityWebRequest.downloadHandler.data.Length;
    }

    private void UpdateHotFixCodeDownProgress()
    {
        currentProgress += (_hotFixCodeDownUnityWebRequest.downloadProgress - _hotFixCodeLastDownProgress);
        _hotFixViewSpeed = _hotFixCodeDownUnityWebRequest.downloadHandler.data.Length - _hotFixCodeLastDownSize;
        _hotFixCodeLastDownSize = _hotFixCodeDownUnityWebRequest.downloadHandler.data.Length;
    }


    [Button]
    public void Tets()
    {
        Debug.Log(JsonUtility.ToJson(new HotFixAssetConfig()
        {
            name = "hotfixview", md5 = GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFix/UpdateHotFix/hotfixview"),
            size = GetFileSize(Application.streamingAssetsPath + "/HotFix/UpdateHotFix/hotfixview").ToString()
        }));

        Debug.Log(JsonUtility.ToJson(new HotFixAssetConfig()
        {
            name = "XFrameworkHotFix.dll.bytes", md5 = GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFix/UpdateHotFix/XFrameworkHotFix.dll.bytes"),
            size = GetFileSize(Application.streamingAssetsPath + "/HotFix/UpdateHotFix/XFrameworkHotFix.dll.bytes").ToString()
        }));
    }

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
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }

        return null;
    }
}