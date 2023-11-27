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


public class HotFixRuntimeServerFileCheck : MonoBehaviour
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
    public bool isMetadataHotFixRuntimeDownConfigTableDownOver = false;

    [BoxGroup("Assembly资源内容")] [LabelText("Assembly数据")]
    public HotFixRuntimeDownConfig assemblyHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

    [BoxGroup("Assembly资源内容")] [LabelText("Assembly数据")]
    public bool isAssemblyHotFixRuntimeDownConfigDownOver;

    [BoxGroup("GameRootStart资源内容")] [LabelText("GameRootStart数据")]
    public HotFixRuntimeDownConfig gameRootStartHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

    [BoxGroup("GameRootStart资源内容")] [LabelText("GameRootStart下载完毕")]
    public bool isGameRootStartDownOver;

    [BoxGroup("场景资源内容")] [LabelText("场景内所有AssetBundle资源信息表")]
    public List<string> hotFixAssetAssetBundleSceneConfigTable;

    [BoxGroup("场景资源内容")] [LabelText("服务器资源下载完毕")]
    public bool isHotFixAssetAssetBundleSceneConfigTableDownOver;

    [BoxGroup("场景资源内容")] [LabelText("场景内所有AssetBundle资源信息")]
    public List<HotFixAssetAssetBundleSceneConfig> hotFixAssetAssetBundleSceneConfigs = new List<HotFixAssetAssetBundleSceneConfig>();

    [BoxGroup("场景资源内容")] [LabelText("服务器场景资源下载完毕")]
    public bool isHotFixAssetAssetBundleSceneConfigDownOver;

    [BoxGroup("本地检测")] [LabelText("需要下载的文件")]
    public List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig = new List<HotFixRuntimeDownConfig>();

    [BoxGroup("本地检测")] [LabelText("元数据本地检测")]
    public bool metadataHotFixRuntimeDownConfigLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("Assembly本地检测")]
    public bool assemblyHotFixRuntimeDownConfigLocalCheckLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("GameRootStart本地检测")]
    public bool gameRootStartHotFixRuntimeDownConfigLocalCheck = false;

    [BoxGroup("本地检测")] [LabelText("HotFixAssetBundle本地检测")]
    public bool hotFixAssetBundleLocalCheck;

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
        StartCoroutine(MetadataHotFixRuntimeDownConfig());
        yield return new WaitUntil(() => isMetadataHotFixRuntimeDownConfigTableDownOver);
        Debug.Log("元数据表下载完毕");
        Debug.Log("Assembly表开始下载");
        StartCoroutine(AssemblyHotFixRuntimeDownConfig());
        yield return new WaitUntil(() => isAssemblyHotFixRuntimeDownConfigDownOver);
        Debug.Log("Assembly表下载");

        StartCoroutine(GameRootStartHotFixRuntimeDownConfig());
        yield return new WaitUntil(() => isGameRootStartDownOver);
        Debug.Log("GameRoot表下载");
        StartCoroutine(ServerResourcesCount());
        yield return new WaitUntil(() => isHotFixAssetAssetBundleSceneConfigTableDownOver);
        Debug.Log("ServerResources表下载");

        StartCoroutine(HotFixAssetAssetBundleScene(0));
        yield return new WaitUntil(() => isHotFixAssetAssetBundleSceneConfigDownOver);
        Debug.Log("配置表下载完毕");
        HotFixRuntimeUpdateTableEnd?.Invoke();
        StartCoroutine(StartAssetLocalCheck());
    }
    //检测本地资源
    IEnumerator StartAssetLocalCheck()
    {
        StartCoroutine(MetadataHotFixRuntimeDownConfigLocalCheck(0));
        yield return new WaitUntil(() => metadataHotFixRuntimeDownConfigLocalCheck);
        StartCoroutine(AssemblyHotFixRuntimeDownConfigLocalCheck());
        yield return new WaitUntil(() => assemblyHotFixRuntimeDownConfigLocalCheckLocalCheck);
        StartCoroutine(GameRootStartHotFixRuntimeDownConfigLocalCheck());
        yield return new WaitUntil(() => gameRootStartHotFixRuntimeDownConfigLocalCheck);
        List<HotFixAssetAssetBundleAssetConfig> hotFixAssetAssetBundleAssetConfigs = HotFixAssetAssetBundleSceneConfigGroup();
        StartCoroutine(HotFixAssetAssetBundleSceneLocalCheck(hotFixAssetAssetBundleAssetConfigs, 0));
        yield return new WaitUntil(() => hotFixAssetBundleLocalCheck);
        Debug.Log("本地检测完毕");
        HotFixConfigDown.Instance.DownHotFixRuntimeDownConfig(needDownHotFixRuntimeDownConfig);
    }

    //下载远程元数据配置表
    IEnumerator MetadataHotFixRuntimeDownConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFix/MetadataConfig/" + "MetadataConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(MetadataHotFixRuntimeDownConfig());
        }
        else
        {
            metadataHotFixRuntimeDownConfigTable = JsonMapper.ToObject<List<HotFixRuntimeDownConfig>>(request.downloadHandler.text);

            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in metadataHotFixRuntimeDownConfigTable)
            {
                metadataHotFixRuntimeDownConfigTableList.Add(hotFixRuntimeDownConfig.Name);
            }

            isMetadataHotFixRuntimeDownConfigTableDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += metadataHotFixRuntimeDownConfigTable.Count;
        }
    }

    IEnumerator MetadataHotFixRuntimeDownConfigLocalCheck(int index)
    {
        if (index <= metadataHotFixRuntimeDownConfigTable.Count - 1)
        {
            string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + metadataHotFixRuntimeDownConfigTable[index].Path + metadataHotFixRuntimeDownConfigTable[index].Name;
            Debug.Log(localFilePath);
            UnityWebRequest request = UnityWebRequest.Get(localFilePath);
            Debug.Log(request.url);
            Debug.Log(File.Exists(localFilePath));
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                needDownHotFixRuntimeDownConfig.Add(metadataHotFixRuntimeDownConfigTable[index]);
            }
            else
            {
                //本地Md5校验
                if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != metadataHotFixRuntimeDownConfigTable[index].Md5)
                {
                    needDownHotFixRuntimeDownConfig.Add(metadataHotFixRuntimeDownConfigTable[index]);
                }
            }

            index += 1;
            StartCoroutine(MetadataHotFixRuntimeDownConfigLocalCheck(index));
        }
        else
        {
            metadataHotFixRuntimeDownConfigLocalCheck = true;
        }
    }

    IEnumerator AssemblyHotFixRuntimeDownConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/AssemblyConfig/" + "AssemblyConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(AssemblyHotFixRuntimeDownConfig());
        }
        else
        {
            assemblyHotFixRuntimeDownConfig = JsonMapper.ToObject<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            isAssemblyHotFixRuntimeDownConfigDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    IEnumerator AssemblyHotFixRuntimeDownConfigLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + assemblyHotFixRuntimeDownConfig.Path + assemblyHotFixRuntimeDownConfig.Name;
        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            needDownHotFixRuntimeDownConfig.Add(assemblyHotFixRuntimeDownConfig);
        }
        else
        {
            //本地Md5校验
            if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != assemblyHotFixRuntimeDownConfig.Md5)
            {
                needDownHotFixRuntimeDownConfig.Add(assemblyHotFixRuntimeDownConfig);
            }
        }

        assemblyHotFixRuntimeDownConfigLocalCheckLocalCheck = true;
    }

    IEnumerator GameRootStartHotFixRuntimeDownConfig()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/GameRootStartAssetBundleConfig/" + "GameRootStartConfig.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(GameRootStartHotFixRuntimeDownConfig());
        }
        else
        {
            gameRootStartHotFixRuntimeDownConfig = JsonMapper.ToObject<HotFixRuntimeDownConfig>(request.downloadHandler.text);
            isGameRootStartDownOver = true;
            //更新检测数量
            localFileUpdateCheckAssetNumberMax += 1;
        }
    }

    IEnumerator GameRootStartHotFixRuntimeDownConfigLocalCheck()
    {
        string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + gameRootStartHotFixRuntimeDownConfig.Path + gameRootStartHotFixRuntimeDownConfig.Name;

        UnityWebRequest request = UnityWebRequest.Get(localFilePath);
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            needDownHotFixRuntimeDownConfig.Add(gameRootStartHotFixRuntimeDownConfig);
        }
        else
        {
            //本地Md5校验
            if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != gameRootStartHotFixRuntimeDownConfig.Md5)
            {
                needDownHotFixRuntimeDownConfig.Add(gameRootStartHotFixRuntimeDownConfig);
            }
        }

        gameRootStartHotFixRuntimeDownConfigLocalCheck = true;
    }

    IEnumerator ServerResourcesCount()
    {
        UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/" + "HotFixServerResourcesCount.json");
        yield return request.SendWebRequest();
        if (request.responseCode != 200)
        {
            Debug.Log(request.url);
            yield return new WaitForSeconds(againDownWaitTime);
            StartCoroutine(ServerResourcesCount());
        }
        else
        {
            currentLocalFileUpdateCheckAssetNumber = 0;
            hotFixAssetAssetBundleSceneConfigTable = JsonMapper.ToObject<List<string>>(request.downloadHandler.text);
            currentLocalFileUpdateCheckAssetNumber = 0;
            localFileUpdateCheckAssetNumberMax = hotFixAssetAssetBundleSceneConfigTable.Count;
            isHotFixAssetAssetBundleSceneConfigTableDownOver = true;
        }
    }

    IEnumerator HotFixAssetAssetBundleScene(int index)
    {
        if (index <= hotFixAssetAssetBundleSceneConfigTable.Count - 1)
        {
            UnityWebRequest request = UnityWebRequest.Get(downPath + "HotFixRuntime/HotFixAssetBundleConfig/" + hotFixAssetAssetBundleSceneConfigTable[index] + ".json");
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                yield return new WaitForSeconds(againDownWaitTime);
                StartCoroutine(HotFixAssetAssetBundleScene(index));
            }
            else
            {
                HotFixAssetAssetBundleSceneConfig hotFixAssetAssetBundleSceneConfig = JsonMapper.ToObject<HotFixAssetAssetBundleSceneConfig>(request.downloadHandler.text);

                //更新检测数量
                //场景
                localFileUpdateCheckAssetNumberMax += 1;
                //字体
                localFileUpdateCheckAssetNumberMax += 1;
                //场景内资源
                localFileUpdateCheckAssetNumberMax += hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count;
                hotFixAssetAssetBundleSceneConfigs.Add(hotFixAssetAssetBundleSceneConfig);
                if (hotFixAssetAssetBundleSceneConfigs.Count < hotFixAssetAssetBundleSceneConfigTable.Count)
                {
                    index += 1;
                    StartCoroutine(HotFixAssetAssetBundleScene(index));
                }
                else
                {
                    isHotFixAssetAssetBundleSceneConfigDownOver = true;
                }
            }
        }
    }

    private List<HotFixAssetAssetBundleAssetConfig> HotFixAssetAssetBundleSceneConfigGroup()
    {
        List<HotFixAssetAssetBundleAssetConfig> hotFixAssetAssetBundleAssetConfigs = new List<HotFixAssetAssetBundleAssetConfig>();
        foreach (HotFixAssetAssetBundleSceneConfig hotFixAssetAssetBundleSceneConfig in hotFixAssetAssetBundleSceneConfigs)
        {
            hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig);
            hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneFontFixAssetConfig);
            for (int i = 0; i < hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                hotFixAssetAssetBundleAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.assetBundleHotFixAssetAssetBundleAssetConfigs[i]);
            }

            string localPathCacheName = hotFixAssetAssetBundleSceneConfig.sceneHotFixAssetAssetBundleAssetConfig.assetBundleName + ".json.Cache";
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig", localPathCacheName, JsonMapper.ToJson(hotFixAssetAssetBundleSceneConfig));

            HotFixConfigDown.Instance.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + localPathCacheName);
        }

        return hotFixAssetAssetBundleAssetConfigs;
    }

    IEnumerator HotFixAssetAssetBundleSceneLocalCheck(List<HotFixAssetAssetBundleAssetConfig> hotFixAssetAssetBundleAssetConfigs, int index)
    {
        if (index <= hotFixAssetAssetBundleAssetConfigs.Count - 1)
        {
            string localFilePath = HotFixGlobal.GetDeviceStoragePath() + "/" + hotFixAssetAssetBundleAssetConfigs[index].assetBundlePath + hotFixAssetAssetBundleAssetConfigs[index].assetBundleName;

            UnityWebRequest request = UnityWebRequest.Get(localFilePath);
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig()
                {
                    Name = hotFixAssetAssetBundleAssetConfigs[index].assetBundleName,
                    Path = hotFixAssetAssetBundleAssetConfigs[index].assetBundlePath,
                    Md5 = hotFixAssetAssetBundleAssetConfigs[index].md5,
                    Size = hotFixAssetAssetBundleAssetConfigs[index].assetBundleSize,
                };
                needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
            }
            else
            {
                //本地Md5校验
                if (HotFixGlobal.GetMD5HashFromFile(localFilePath) != hotFixAssetAssetBundleAssetConfigs[index].md5)
                {
                    HotFixRuntimeDownConfig hotFixRuntimeDownConfig = new HotFixRuntimeDownConfig()
                    {
                        Name = hotFixAssetAssetBundleAssetConfigs[index].assetBundleName,
                        Path = hotFixAssetAssetBundleAssetConfigs[index].assetBundlePath,
                        Md5 = hotFixAssetAssetBundleAssetConfigs[index].md5,
                        Size = hotFixAssetAssetBundleAssetConfigs[index].assetBundleSize,
                    };
                    needDownHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                }
            }

            index += 1;
            StartCoroutine(HotFixAssetAssetBundleSceneLocalCheck(hotFixAssetAssetBundleAssetConfigs, index));
        }
        else
        {
            hotFixAssetBundleLocalCheck = true;
        }
    }
}