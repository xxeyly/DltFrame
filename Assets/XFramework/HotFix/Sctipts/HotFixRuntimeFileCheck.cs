using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

//下载速度
public delegate void HotFixRuntimeUpdateTableStart();

public delegate void HotFixRuntimeUpdateTableEnd();

public delegate void HotFixRuntimeLocalFileUpdateCheck(int currentCount, int maxCount);


public class HotFixRuntimeFileCheck : MonoBehaviour
{
    [LabelText("下载地址")] public string downPath = "http://127.0.0.1/";

    [BoxGroup("下载内容")] [LabelText("再次下载等待时间")]
    public float againDownWaitTime;

    [BoxGroup("资源内容")] [LabelText("当前本地检测更新资源数量")]
    public int currentLocalFileUpdateCheckAssetNumber;

    [BoxGroup("资源内容")] [LabelText("本地检测更新资源数量上限")]
    public int localFileUpdateCheckAssetNumberMax;

    //开始检测配置表
    public static HotFixRuntimeUpdateTableStart HotFixRuntimeUpdateTableStart;

    //结束检测配置表
    public static HotFixRuntimeUpdateTableEnd HotFixRuntimeUpdateTableEnd;

    //本地文件更新检测
    public static HotFixRuntimeLocalFileUpdateCheck HotFixRuntimeLocalFileUpdateCheck;

    [BoxGroup("元数据资源内容")] [LabelText("元数据配置列表")]
    public List<HotFixRuntimeDownConfig> metadataHotFixRuntimeDownConfigTable = new List<HotFixRuntimeDownConfig>();

    [BoxGroup("元数据资源内容")] [LabelText("元数据列表")]
    public List<string> metadataHotFixRuntimeDownConfigTableList = new List<string>();

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

    void Start()
    {
        StartCoroutine(StartDownAssetConfig());
    }

    //开始下载配置表
    IEnumerator StartDownAssetConfig()
    {
        Debug.Log("配置表开始下载");
        HotFixRuntimeUpdateTableStart?.Invoke();
        Debug.Log("元数据表开始下载");
        StartCoroutine(DownMetadataConfig());
        yield return new WaitUntil(() => isMetadataDownOver);
        Debug.Log("元数据表下载完毕");
        Debug.Log("Assembly表开始下载");
        StartCoroutine(DownAssemblyConfig());
        yield return new WaitUntil(() => isAssemblyDownOver);
        Debug.Log("Assembly表下载");
        StartCoroutine(DownGameRootStartConfig());
        yield return new WaitUntil(() => isGameRootStartDownOver);
        Debug.Log("GameRoot表下载");
        StartCoroutine(DownSceneCountConfig());
        yield return new WaitUntil(() => isSceneConfigOver);
        Debug.Log("ServerResources表下载");

        StartCoroutine(DownSceneAssetBundleConfig(0));
        yield return new WaitUntil(() => isSceneAssetBundleOver);
        Debug.Log("配置表下载完毕");
        HotFixRuntimeUpdateTableEnd?.Invoke();
        StartCoroutine(StartLocalAssetCheck());
    }

    #region 下载远程数据配置表

    //下载远程元数据配置表
    IEnumerator DownMetadataConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFix/MetadataConfig/" + "MetadataConfig.json");
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
            metadataHotFixRuntimeDownConfigTable = JsonMapper.ToObject<List<HotFixRuntimeDownConfig>>(request.downloadHandler.text);
            //统计元数据列表
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in metadataHotFixRuntimeDownConfigTable)
            {
                metadataHotFixRuntimeDownConfigTableList.Add(hotFixRuntimeDownConfig.Name);
            }

            //元数据下载完毕
            isMetadataDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += metadataHotFixRuntimeDownConfigTable.Count;
        }
    }

    //下载远程Assembly配置表
    IEnumerator DownAssemblyConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/AssemblyConfig/" + "AssemblyConfig.json");
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
            assemblyHotFixRuntimeDownConfig = JsonMapper.ToObject<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            //Assembly下载完毕
            isAssemblyDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    //下载远程GameRootStart配置表
    IEnumerator DownGameRootStartConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/GameRootStartAssetBundleConfig/" + "GameRootStartConfig.json");
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
            gameRootStartHotFixRuntimeDownConfig = JsonMapper.ToObject<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            //GameRootStart下载完毕
            isGameRootStartDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    //下载远程运行场景数量
    IEnumerator DownSceneCountConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/" + "HotFixServerResourcesCount.json");
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
            hotFixRuntimeAssetBundleSceneConfigTable = JsonMapper.ToObject<List<string>>(request.downloadHandler.text);
            //运行场景数量下载完毕
            isSceneConfigOver = true;
        }
    }

    //下载场景AssetBundle配置表
    IEnumerator DownSceneAssetBundleConfig(int index)
    {
        if (index <= hotFixRuntimeAssetBundleSceneConfigTable.Count - 1)
        {
            UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/HotFixAssetBundleConfig/" + hotFixRuntimeAssetBundleSceneConfigTable[index] + ".json");
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                //请求错误,等待一定时间后再次请求
                yield return new WaitForSeconds(againDownWaitTime);
                StartCoroutine(DownSceneAssetBundleConfig(index));
            }
            else
            {
                //场景AssetBundle配置表
                HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = JsonMapper.ToObject<HotFixRuntimeSceneAssetBundleConfig>(request.downloadHandler.text);
                //更新检测数量
                //场景
                localFileUpdateCheckAssetNumberMax += 1;
                //字体
                localFileUpdateCheckAssetNumberMax += 1;
                //场景内资源
                localFileUpdateCheckAssetNumberMax += hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count;
                //添加到场景AssetBundle配置表
                hotFixRuntimeSceneAssetBundleConfigs.Add(hotFixRuntimeSceneAssetBundleConfig);
                if (hotFixRuntimeSceneAssetBundleConfigs.Count < hotFixRuntimeAssetBundleSceneConfigTable.Count)
                {
                    index += 1;
                    StartCoroutine(DownSceneAssetBundleConfig(index));
                }
                else
                {
                    isSceneAssetBundleOver = true;
                }
            }
        }
    }

    #endregion

    //检测本地资源

    IEnumerator StartLocalAssetCheck()
    {
        StartCoroutine(MetadataLocalCheck(0));
        yield return new WaitUntil(() => isMetadataLocalCheck);
        StartCoroutine(AssemblyLocalCheck());
        yield return new WaitUntil(() => isAssemblyLocalCheck);
        StartCoroutine(GameRootStartLocalCheck());
        yield return new WaitUntil(() => isGameRootStartLocalCheck);
        List<HotFixRuntimeAssetBundleConfig> hotFixAssetAssetBundleAssetConfigs = HotFixAssetAssetBundleSceneConfigGroup();
        StartCoroutine(AssetBundleLocalCheck(hotFixAssetAssetBundleAssetConfigs, 0));
        yield return new WaitUntil(() => isAssetBundleLocalCheck);
        Debug.Log("本地检测完毕");
        HotFixRuntimeFileDown.DownHotFixRuntimeDownConfig(needDownHotFixRuntimeDownConfig);
    }

    #region 本地数据检测

    //元数据本地检测
    IEnumerator MetadataLocalCheck(int index)
    {
        if (index <= metadataHotFixRuntimeDownConfigTable.Count - 1)
        {
            string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + metadataHotFixRuntimeDownConfigTable[index].Path + metadataHotFixRuntimeDownConfigTable[index].Name;
            UnityWebRequest request = UnityWebRequest.Get(localFilePath);
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                //本地文件不存在,添加到下载列表
                needDownHotFixRuntimeDownConfig.Add(metadataHotFixRuntimeDownConfigTable[index]);
            }
            else
            {
                //本地Md5校验不通过,添加到下载列表
                if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != metadataHotFixRuntimeDownConfigTable[index].Md5)
                {
                    needDownHotFixRuntimeDownConfig.Add(metadataHotFixRuntimeDownConfigTable[index]);
                }
            }

            //更新检测数量
            currentLocalFileUpdateCheckAssetNumber += 1;
            index += 1;
            StartCoroutine(MetadataLocalCheck(index));
        }
        else
        {
            isMetadataLocalCheck = true;
        }
    }

    //Assembly本地检测
    IEnumerator AssemblyLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + assemblyHotFixRuntimeDownConfig.Path + assemblyHotFixRuntimeDownConfig.Name;
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
            if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != assemblyHotFixRuntimeDownConfig.Md5)
            {
                needDownHotFixRuntimeDownConfig.Add(assemblyHotFixRuntimeDownConfig);
            }
        }

        //更新检测数量
        currentLocalFileUpdateCheckAssetNumber += 1;
        isAssemblyLocalCheck = true;
    }

    //GameRootStart本地检测
    IEnumerator GameRootStartLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + gameRootStartHotFixRuntimeDownConfig.Path + gameRootStartHotFixRuntimeDownConfig.Name;
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
            if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != gameRootStartHotFixRuntimeDownConfig.Md5)
            {
                needDownHotFixRuntimeDownConfig.Add(gameRootStartHotFixRuntimeDownConfig);
            }
        }

        //更新检测数量
        currentLocalFileUpdateCheckAssetNumber += 1;
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

            /*string localPathCacheName = hotFixAssetAssetBundleSceneConfig.sceneHotFixRuntimeAssetBundleConfig.assetBundleName + ".json.Cache";
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig", localPathCacheName, JsonMapper.ToJson(hotFixAssetAssetBundleSceneConfig));

            HotFixConfigDown.Instance.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + localPathCacheName);*/
        }

        return hotFixAssetAssetBundleAssetConfigs;
    }

    //HotFixAssetBundle本地检测
    IEnumerator AssetBundleLocalCheck(List<HotFixRuntimeAssetBundleConfig> hotFixAssetAssetBundleAssetConfigs, int index)
    {
        if (index <= hotFixAssetAssetBundleAssetConfigs.Count - 1)
        {
            string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixAssetAssetBundleAssetConfigs[index].assetBundlePath + hotFixAssetAssetBundleAssetConfigs[index].assetBundleName;
            UnityWebRequest request = UnityWebRequest.Get(localFilePath);
            yield return request.SendWebRequest();
            
            HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig()
            {
                Name = hotFixAssetAssetBundleAssetConfigs[index].assetBundleName,
                Path = hotFixAssetAssetBundleAssetConfigs[index].assetBundlePath,
                Md5 = hotFixAssetAssetBundleAssetConfigs[index].md5,
                Size = hotFixAssetAssetBundleAssetConfigs[index].assetBundleSize,
            };
            if (request.responseCode != 200)
            {
                needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
            }
            else
            {
                //本地Md5校验
                if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != hotFixAssetAssetBundleAssetConfigs[index].md5)
                {
                    needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                }
            }

            index += 1;
            StartCoroutine(AssetBundleLocalCheck(hotFixAssetAssetBundleAssetConfigs, index));
        }
        else
        {
            isAssetBundleLocalCheck = true;
        }
    }

    #endregion
}