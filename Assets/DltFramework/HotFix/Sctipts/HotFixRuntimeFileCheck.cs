using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class HotFixRuntimeFileCheck : MonoBehaviour
{
    [LabelText("本地开启更新")] public bool localIsUpdate;
    [LabelText("本地开启更新读取")] public bool localIsUpdateLoad;
    [LabelText("下载地址")] public string hotFixPath = "http://127.0.0.1/";
    [LabelText("下载地址加载完毕")] public bool hotFixPathLocalLoad = false;

    [BoxGroup("下载内容")] [LabelText("再次下载等待时间")]
    public float againDownWaitTime;

    [BoxGroup("资源内容")] [LabelText("当前本地检测更新资源数量")]
    public int currentLocalFileUpdateCheckAssetNumber;

    [BoxGroup("资源内容")] [LabelText("本地检测更新资源数量上限")]
    public int localFileUpdateCheckAssetNumberMax;


    [BoxGroup("元数据资源内容")] [LabelText("元数据配置列表")]
    public List<HotFixRuntimeDownConfig> metadataHotFixRuntimeDownConfigTable = new List<HotFixRuntimeDownConfig>();

    [BoxGroup("元数据资源内容")] [LabelText("元数据检测")]
    public bool hotFixRuntimeDownConfigLocalCheckOver;

    [BoxGroup("元数据资源内容")] [LabelText("元数据下载完毕")]
    public bool isMetadataDownOver = false;

    [BoxGroup("Assembly资源内容")] [LabelText("Assembly数据")]
    public HotFixRuntimeDownConfig assemblyHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

    [BoxGroup("Assembly资源内容")] [LabelText("Assembly数据")]
    public bool isAssemblyDownOver;

    [BoxGroup("GameRootStart资源内容")] [LabelText("GameRootStart数据")]
    public HotFixRuntimeDownConfig gameRootStartHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

    [BoxGroup("GameRootStart资源内容")] [LabelText("GameRootStart下载完毕")]
    public bool isGameRootStartDownOver;

    [BoxGroup("场景资源内容")] [LabelText("场景内所有AssetBundle资源信息表")]
    public List<string> hotFixRuntimeAssetBundleSceneConfigTable;

    [BoxGroup("场景资源内容")] [LabelText("服务器资源下载完毕")]
    public bool isSceneConfigOver;

    [BoxGroup("场景资源内容")] [LabelText("场景内所有AssetBundle资源信息")]
    public List<HotFixRuntimeSceneAssetBundleConfig> hotFixRuntimeSceneAssetBundleConfigs = new List<HotFixRuntimeSceneAssetBundleConfig>();

    [BoxGroup("场景资源内容")] [LabelText("场景资源下载完毕")]
    public bool isHotFixAssetBundleConfigOver;

    [BoxGroup("场景资源内容")] [LabelText("服务器场景资源下载完毕")]
    public bool isSceneAssetBundleOver;

    [BoxGroup("本地检测")] [LabelText("需要下载的文件")]
    public List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig = new List<HotFixRuntimeDownConfig>();

    [BoxGroup("本地检测")] [LabelText("元数据本地检测")]
    public bool isMetadataLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("Assembly本地检测")]
    public bool isAssemblyLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("GameRootStart本地检测")]
    public bool isGameRootStartLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("HotFixAssetBundle本地检测")]
    public bool isAssetBundleLocalCheck;

    [BoxGroup("本地检测")] [LabelText("HotFixRuntimeAssetBundleConfig本地检测")]
    public bool HotFixRuntimeAssetBundleConfigLocalCheckOver;

    [BoxGroup("本地检测")] [LabelText("HotFixRuntimeFileDown本地检测")]
    public HotFixRuntimeFileDown hotFixRuntimeFileDown;

    private List<IHotFixRuntimeFileCheck> _hotFixRuntimeFileCheckList = new List<IHotFixRuntimeFileCheck>();

    void Start()
    {
        _hotFixRuntimeFileCheckList = HotFixGlobal.GetAllObjectsInScene<IHotFixRuntimeFileCheck>();
        StartCoroutine(LocalIsUpdate());
    }

    IEnumerator LocalIsUpdate()
    {
        StartCoroutine(LocalIsUpdateLoad());
        yield return new WaitUntil(() => localIsUpdateLoad);
        if (localIsUpdate)
        {
            //开始本地文件检测
            StartCoroutine(StartDownAssetConfig());
        }
        else
        {
            //直接加载
            HotFixOver.Over();
        }
    }

    /// <summary>
    /// 本地更新文件读取
    /// </summary>
    /// <returns></returns>
    IEnumerator LocalIsUpdateLoad()
    {
        //本地下载路径
        string hotFixDownPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "LocalIsUpdate.txt";
        UnityWebRequest hotFixPathLoadLocalFile = UnityWebRequest.Get(hotFixDownPath);
        yield return hotFixPathLoadLocalFile.SendWebRequest();
        if (hotFixPathLoadLocalFile.responseCode == 200)
        {
            localIsUpdate = bool.Parse(hotFixPathLoadLocalFile.downloadHandler.text);
        }
        else
        {
            Debug.Log("本地下载路径不存在:" + hotFixDownPath);
            localIsUpdate = true;
        }

        localIsUpdateLoad = true;
    }

    //HotFixPath本地检测
    IEnumerator HotFixPathLocalLoad()
    {
        //本地下载路径
        string hotFixDownPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "HotFixDownPath.txt";
        /*
        Debug.Log("File:" + File.Exists(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/" + "HotFixDownPath.txt"));
        //获取不到
        //Debug.Log("File Jar:" + File.Exists(HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "HotFixDownPath.txt"));
        Debug.Log("HotFixDownPath内容:" + HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/", "HotFixDownPath.txt"));
        //获取不到
        // Debug.Log("HotFixDownPath内容 Jar:" + HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/", "HotFixDownPath.txt"));
        Debug.Log("HotFixDownPathMd5:" + HotFixGlobal.GetMD5HashFromFile(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/" + "HotFixDownPath.txt"));
        //获取不到
        // Debug.Log("HotFixDownPathMd5 Jar:" + HotFixGlobal.GetMD5HashFromFile(HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "HotFixDownPath.txt"));
        //获取不到
        /*UnityWebRequest hotFixPathLoadLocalFile1 = UnityWebRequest.Get(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/" + "HotFixDownPath.txt");
        yield return hotFixPathLoadLocalFile1.SendWebRequest();
        Debug.Log("UnityWebRequest:" + hotFixPathLoadLocalFile1.responseCode);#1#

        UnityWebRequest hotFixPathLoadLocalFile2 = UnityWebRequest.Get(HotFixGlobal.GetDeviceStoragePath(true) + "/HotFix/" + "HotFixDownPath.txt");
        yield return hotFixPathLoadLocalFile2.SendWebRequest();
        Debug.Log("UnityWebRequest Jar:" + hotFixPathLoadLocalFile2.responseCode);


        UnityWebRequest hotFixPathLoadLocalFile3 = UnityWebRequest.Get(Application.streamingAssetsPath + "/HotFix/" + "HotFixDownPath.txt");
        yield return hotFixPathLoadLocalFile3.SendWebRequest();
        Debug.Log("UnityWebRequest streamingAssetsPath:" + hotFixPathLoadLocalFile3.responseCode);
        //获取不到
        // Debug.Log("File streamingAssetsPath:" + File.Exists(Application.streamingAssetsPath + "/HotFix/" + "HotFixDownPath.txt"));
        //获取不到
        // Debug.Log("File Md5 streamingAssetsPath:" + HotFixGlobal.GetMD5HashFromFile(Application.streamingAssetsPath + "/HotFix/" + "HotFixDownPath.txt"));
        */


        UnityWebRequest hotFixPathLoadLocalFile = UnityWebRequest.Get(hotFixDownPath);
        yield return hotFixPathLoadLocalFile.SendWebRequest();
        if (hotFixPathLoadLocalFile.responseCode == 200)
        {
            hotFixPath = hotFixPathLoadLocalFile.downloadHandler.text;
            //如果结尾不是/,添加/
            if (hotFixPath[hotFixPath.Length - 1] != '/')
            {
                hotFixPath += "/";
            }
        }
        else
        {
            Debug.Log("本地下载路径不存在:" + hotFixDownPath);
        }


        hotFixPathLocalLoad = true;
    }

    //开始下载配置表
    IEnumerator StartDownAssetConfig()
    {
        StartCoroutine(HotFixPathLocalLoad());
        yield return new WaitUntil(() => hotFixPathLocalLoad);
        Debug.Log("配置表开始下载----------");
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeTableDownStart();
        }

        Debug.Log("元数据表开始下载");
        StartCoroutine(DownMetadataConfig());
        yield return new WaitUntil(() => isMetadataDownOver);
        Debug.Log("元数据表下载完毕");
        Debug.Log("Assembly表开始下载");
        StartCoroutine(DownAssemblyConfig());
        yield return new WaitUntil(() => isAssemblyDownOver);
        Debug.Log("Assembly表下载完毕");
        Debug.Log("GameRoot表开始下载");
        StartCoroutine(DownGameRootStartConfig());
        yield return new WaitUntil(() => isGameRootStartDownOver);
        Debug.Log("GameRoot表下载完毕");
        StartCoroutine(DownSceneCountConfig());
        yield return new WaitUntil(() => isSceneConfigOver);
        Debug.Log("SceneCount下载完毕");

        StartCoroutine(StartDownSceneAssetBundleConfig());
        yield return new WaitUntil(() => isSceneAssetBundleOver);
        Debug.Log("配置表下载完毕----------");
        yield return new WaitForSeconds(0.5f);
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeTableDownOver();
        }

        StartCoroutine(StartLocalAssetCheck());
    }

    #region 下载远程数据配置表

    //下载远程元数据配置表
    IEnumerator DownMetadataConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFix/MetadataConfig/" + "MetadataConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //请求错误,等待一定时间后再次请求
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(DownMetadataConfig());
        }
        else
        {
            //获得元数据配置表
            metadataHotFixRuntimeDownConfigTable = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(request.downloadHandler.text);

            //元数据下载完毕
            isMetadataDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += metadataHotFixRuntimeDownConfigTable.Count;
        }
    }

    //下载远程Assembly配置表
    IEnumerator DownAssemblyConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/AssemblyConfig/" + "AssemblyConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //请求错误,等待一定时间后再次请求
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(DownAssemblyConfig());
        }
        else
        {
            //获得Assembly配置表
            assemblyHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            //Assembly下载完毕
            isAssemblyDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    //下载远程GameRootStart配置表
    IEnumerator DownGameRootStartConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/GameRootStartAssetBundleConfig/" + "GameRootStartConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //请求错误,等待一定时间后再次请求
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(DownGameRootStartConfig());
        }
        else
        {
            //获得GameRootStart配置表
            gameRootStartHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            //GameRootStart下载完毕
            isGameRootStartDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    //下载远程运行场景数量
    IEnumerator DownSceneCountConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/" + "HotFixServerResourcesCount.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //请求错误,等待一定时间后再次请求
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(DownSceneCountConfig());
        }
        else
        {
            //获得运行场景数量
            hotFixRuntimeAssetBundleSceneConfigTable = JsonUtil.FromJson<System.Collections.Generic.List<string>>(request.downloadHandler.text);
            //运行场景数量下载完毕
            isSceneConfigOver = true;
        }
    }


    IEnumerator StartDownSceneAssetBundleConfig()
    {
        for (int i = 0; i < hotFixRuntimeAssetBundleSceneConfigTable.Count; i++)
        {
            isHotFixAssetBundleConfigOver = false;
            yield return StartCoroutine(DownSceneAssetBundleConfig(hotFixRuntimeAssetBundleSceneConfigTable[i]));
            yield return new WaitUntil(() => isHotFixAssetBundleConfigOver);
        }

        isSceneAssetBundleOver = true;
    }

    //下载场景AssetBundle配置表
    IEnumerator DownSceneAssetBundleConfig(string sceneName)
    {
        UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/HotFixAssetBundleConfig/" + sceneName + ".json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //请求错误,等待一定时间后再次请求
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(DownSceneAssetBundleConfig(sceneName));
        }
        else
        {
            //场景AssetBundle配置表
            HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = JsonUtil.FromJson<HotFixRuntimeSceneAssetBundleConfig>(request.downloadHandler.text);
            //更新检测数量
            //场景
            localFileUpdateCheckAssetNumberMax += 1;
            //字体
            localFileUpdateCheckAssetNumberMax += 1;
            //场景内资源
            localFileUpdateCheckAssetNumberMax += hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count;
            //添加到场景AssetBundle配置表
            hotFixRuntimeSceneAssetBundleConfigs.Add(hotFixRuntimeSceneAssetBundleConfig);
            isHotFixAssetBundleConfigOver = true;
        }
    }

    #endregion

    //检测本地资源

    IEnumerator StartLocalAssetCheck()
    {
        StartCoroutine(MetadataLocalCheck());
        yield return new WaitUntil(() => isMetadataLocalCheck);
        //保存元数据配置表缓存文件
        SaveMetadataConfigCacheFile();
        //Assembly本地检测
        StartCoroutine(AssemblyLocalCheck());
        yield return new WaitUntil(() => isAssemblyLocalCheck);
        //GameRootStart本地检测
        StartCoroutine(GameRootStartLocalCheck());
        yield return new WaitUntil(() => isGameRootStartLocalCheck);
        List<HotFixRuntimeAssetBundleConfig> hotFixAssetAssetBundleAssetConfigs = HotFixAssetAssetBundleSceneConfigGroup();
        //保存场景AssetBundle配置表缓存文件
        SaveSceneHotFixRuntimeAssetBundleConfigCacheFile();
        StartCoroutine(AssetBundleLocalCheck(hotFixAssetAssetBundleAssetConfigs));
        yield return new WaitUntil(() => isAssetBundleLocalCheck);
        Debug.Log("本地检测完毕");
        yield return new WaitForSeconds(1f);
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheckOver();
        }

        //没有要更新的文件,直接进入游戏
        if (needDownHotFixRuntimeDownConfig.Count == 0)
        {
            Debug.Log("无新配置,直接进入游戏");
            hotFixRuntimeFileDown.ReplaceCacheFile();
            HotFixOver.Over();
        }
        else
        {
            hotFixRuntimeFileDown.DownHotFixRuntimeDownConfig(needDownHotFixRuntimeDownConfig, hotFixPath);
        }
    }


    #region 本地数据检测

    //元数据本地检测
    IEnumerator MetadataLocalCheck()
    {
        for (int i = 0; i < metadataHotFixRuntimeDownConfigTable.Count; i++)
        {
            hotFixRuntimeDownConfigLocalCheckOver = false;
            StartCoroutine(HotFixRuntimeDownConfigLocalCheck(metadataHotFixRuntimeDownConfigTable[i]));
            yield return new WaitUntil(() => hotFixRuntimeDownConfigLocalCheckOver);
        }


        isMetadataLocalCheck = true;
        yield return null;
    }

    //保存元数据配置表缓存文件
    private void SaveMetadataConfigCacheFile()
    {
        HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/MetadataConfig", "MetadataConfig.json" + ".Cache", JsonUtil.ToJson(metadataHotFixRuntimeDownConfigTable));

        //添加到缓存列表中
        hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/MetadataConfig/" + "MetadataConfig.json" + ".Cache");
    }

    IEnumerator HotFixRuntimeDownConfigLocalCheck(HotFixRuntimeDownConfig hotFixRuntimeDownConfig)
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + hotFixRuntimeDownConfig.path + hotFixRuntimeDownConfig.name;
        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            Debug.Log("元数据:" + hotFixRuntimeDownConfig.name + "不存在");
            //本地文件不存在,添加到下载列表
            needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
        }
        else
        {
            //本地Md5校验不通过,添加到下载列表
            if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != hotFixRuntimeDownConfig.md5)
            {
                Debug.Log("元数据:" + hotFixRuntimeDownConfig.name + "Md5不匹配");
                Debug.Log("元数据:" + hotFixRuntimeDownConfig.name + "本地Md5" + HotFixGlobal.GetMD5HashByte(request.downloadHandler.data));
                Debug.Log("元数据:" + hotFixRuntimeDownConfig.name + "服务器Md5" + hotFixRuntimeDownConfig.md5);
                needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
            }
        }

        hotFixRuntimeDownConfigLocalCheckOver = true;
        //更新检测数量
        currentLocalFileUpdateCheckAssetNumber += 1;
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
        }
    }


    //Assembly本地检测
    IEnumerator AssemblyLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + assemblyHotFixRuntimeDownConfig.path + assemblyHotFixRuntimeDownConfig.name;
        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //本地文件不存在,添加到下载列表
            needDownHotFixRuntimeDownConfig.Add(assemblyHotFixRuntimeDownConfig);
        }
        else
        {
            //本地Md5校验不通过,添加到下载列表
            if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != assemblyHotFixRuntimeDownConfig.md5)
            {
                needDownHotFixRuntimeDownConfig.Add(assemblyHotFixRuntimeDownConfig);
            }
        }

        //更新检测数量
        currentLocalFileUpdateCheckAssetNumber += 1;
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
        }

        isAssemblyLocalCheck = true;
    }

    //GameRootStart本地检测
    IEnumerator GameRootStartLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + gameRootStartHotFixRuntimeDownConfig.path + gameRootStartHotFixRuntimeDownConfig.name;
        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            //本地文件不存在,添加到下载列表
            needDownHotFixRuntimeDownConfig.Add(gameRootStartHotFixRuntimeDownConfig);
        }
        else
        {
            //本地Md5校验不通过,添加到下载列表
            if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != gameRootStartHotFixRuntimeDownConfig.md5)
            {
                needDownHotFixRuntimeDownConfig.Add(gameRootStartHotFixRuntimeDownConfig);
            }
        }

        //更新检测数量
        currentLocalFileUpdateCheckAssetNumber += 1;
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
        }

        isGameRootStartLocalCheck = true;
    }

    //场景AssetBundle配置表
    private List<HotFixRuntimeAssetBundleConfig> HotFixAssetAssetBundleSceneConfigGroup()
    {
        List<HotFixRuntimeAssetBundleConfig> hotFixAssetAssetBundleAssetConfigs = new List<HotFixRuntimeAssetBundleConfig>();
        //遍历所有场景配置表
        foreach (HotFixRuntimeSceneAssetBundleConfig hotFixAssetAssetBundleSceneConfig in hotFixRuntimeSceneAssetBundleConfigs)
        {
            //添加场景
            hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneHotFixRuntimeAssetBundleConfig);
            //添加字体
            hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneFontFixRuntimeAssetConfig);
            //添加其他
            for (int i = 0; i < hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i]);
            }
        }

        return hotFixAssetAssetBundleAssetConfigs;
    }

    //保存场景AssetBundle配置表缓存文件
    private void SaveSceneHotFixRuntimeAssetBundleConfigCacheFile()
    {
        //遍历所有场景配置表
        foreach (HotFixRuntimeSceneAssetBundleConfig hotFixAssetAssetBundleSceneConfig in hotFixRuntimeSceneAssetBundleConfigs)
        {
            string localPathCacheName = hotFixAssetAssetBundleSceneConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleName + ".json.Cache";
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig", localPathCacheName, JsonUtil.ToJson(hotFixAssetAssetBundleSceneConfig));
            //添加到缓存列表中
            hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + localPathCacheName);
        }
    }

    //HotFixAssetBundle本地检测
    IEnumerator AssetBundleLocalCheck(List<HotFixRuntimeAssetBundleConfig> hotFixAssetAssetBundleAssetConfigs)
    {
        for (int i = 0; i < hotFixAssetAssetBundleAssetConfigs.Count; i++)
        {
            HotFixRuntimeAssetBundleConfigLocalCheckOver = false;
            StartCoroutine(HotFixRuntimeAssetBundleConfigLocalCheck(hotFixAssetAssetBundleAssetConfigs[i]));
            yield return new WaitUntil(() => HotFixRuntimeAssetBundleConfigLocalCheckOver);
        }

        isAssetBundleLocalCheck = true;
    }

    //HotFixRuntimeAssetBundleConfig 本地检测
    IEnumerator HotFixRuntimeAssetBundleConfigLocalCheck(HotFixRuntimeAssetBundleConfig hotFixRuntimeAssetBundleConfig)
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + hotFixRuntimeAssetBundleConfig.assetBundlePath + hotFixRuntimeAssetBundleConfig.assetBundleName;
        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();

        //空文件,跳过,目前只有场景中的Font文件是空文件
        if (hotFixRuntimeAssetBundleConfig.assetBundleName == "" && hotFixRuntimeAssetBundleConfig.assetBundlePath == "" && hotFixRuntimeAssetBundleConfig.md5 == "" && hotFixRuntimeAssetBundleConfig.assetBundleSize == "")
        {
        }
        else
        {
            HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig()
            {
                name = hotFixRuntimeAssetBundleConfig.assetBundleName, path = hotFixRuntimeAssetBundleConfig.assetBundlePath, md5 = hotFixRuntimeAssetBundleConfig.md5, size = hotFixRuntimeAssetBundleConfig.assetBundleSize,
            };
            if (request.responseCode != 200)
            {
                needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
            }
            else
            {
                //本地Md5校验
                if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != hotFixRuntimeAssetBundleConfig.md5)
                {
                    needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                }
            }
        }

        currentLocalFileUpdateCheckAssetNumber += 1;
        foreach (IHotFixRuntimeFileCheck hotFixRuntimeFileCheck in _hotFixRuntimeFileCheckList)
        {
            hotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
        }

        HotFixRuntimeAssetBundleConfigLocalCheckOver = true;
    }

    #endregion
}