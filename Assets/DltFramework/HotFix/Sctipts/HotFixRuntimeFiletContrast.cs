using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace HotFix
{
    public class HotFixRuntimeFiletContrast : MonoBehaviour
    {
        [LabelText("本地开启更新")] public bool localIsUpdate;
        [LabelText("本地开启更新读取")] public bool localIsUpdateLoad;
        [LabelText("仅资源版本对比")] public bool onlyResourcesVersionContrast;
        [LabelText("仅资源版本对比读取")] public bool OnlyResourcesVersionContrastLoad;
        [LabelText("下载地址")] public string hotFixPath = "http://127.0.0.1/";
        [LabelText("远程配置表下载完毕")] public bool downAssetConfigOver;
        [LabelText("下载地址加载完毕")] public bool hotFixPathLocalLoad = false;

        [BoxGroup("下载内容")] [LabelText("再次下载等待时间")]
        public float againDownWaitTime;

        [BoxGroup("资源内容")] [LabelText("当前本地检测更新资源数量")]
        public int currentLocalFileUpdateCheckAssetNumber;

        [BoxGroup("资源内容")] [LabelText("本地检测更新资源数量上限")]
        public int localFileUpdateCheckAssetNumberMax;

        //------------------------------------------------------------
        [BoxGroup("元数据资源内容")] [LabelText("本地元数据配置列表")]
        public List<HotFixRuntimeDownConfig> localMetadataHotFixRuntimeDownConfigTable = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("元数据资源内容")] [LabelText("远程元数据配置列表")]
        public List<HotFixRuntimeDownConfig> remoteMetadataHotFixRuntimeDownConfigTable = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("元数据资源内容")] [LabelText("本地元数据表读取")]
        public bool isLocalMetadataConfigLoad;

        [BoxGroup("元数据资源内容")] [LabelText("远程元数据表下载")]
        public bool isRemoteMetadataConfigDownLoad = false;

        [BoxGroup("元数据资源内容")] [LabelText("元数据对比")]
        public bool isMetadataContrast = false;

        //------------------------------------------------------------
        [BoxGroup("Assembly资源内容")] [LabelText("本地Assembly数据")]
        public HotFixRuntimeDownConfig localAssemblyHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

        [BoxGroup("Assembly资源内容")] [LabelText("远程Assembly数据")]
        public HotFixRuntimeDownConfig remoteAssemblyHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

        [BoxGroup("Assembly资源内容")] [LabelText("本地Assembly数据表读取")]
        public bool isLocalAssemblyConfigLoad;

        [BoxGroup("Assembly资源内容")] [LabelText("远程Assembly数据表下载")]
        public bool isRemoteAssemblyConfigDownLoad;

        [BoxGroup("Assembly资源内容")] [LabelText("Assembly对比")]
        public bool isAssemblyContrast = false;


        //------------------------------------------------------------
        [BoxGroup("OtherAssembly资源内容")] [LabelText("本地OtherAssembly数据")]
        public List<HotFixRuntimeDownConfig> localOtherAssemblyHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("OtherAssembly资源内容")] [LabelText("远程OtherAssembly数据")]
        public List<HotFixRuntimeDownConfig> remoteOtherAssemblyHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("OtherAssembly资源内容")] [LabelText("本地OtherAssembly数据表读取")]
        public bool isLocalOtherAssemblyConfigLoad;

        [BoxGroup("OtherAssembly资源内容")] [LabelText("远程OtherAssembly数据表下载")]
        public bool isRemoteOtherAssemblyConfigDownLoad;

        [BoxGroup("Assembly资源内容")] [LabelText("Assembly对比")]
        public bool isOtherAssemblyContrast = false;

        //------------------------------------------------------------
        [BoxGroup("GameRootStart资源内容")] [LabelText("本地GameRootStart数据")]
        public HotFixRuntimeDownConfig localGameRootStartHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

        [BoxGroup("GameRootStart资源内容")] [LabelText("远程GameRootStart数据")]
        public HotFixRuntimeDownConfig remoteGameRootStartHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();

        [BoxGroup("GameRootStart资源内容")] [LabelText("本地GameRootStart数据表读取")]
        public bool isLocalGameRootStartConfigLoad;

        [BoxGroup("GameRootStart资源内容")] [LabelText("远程GameRootStart数据表下载")]
        public bool isRemoteGameRootStartDownLoad;

        [BoxGroup("GameRootStart资源内容")] [LabelText("GameRootStart对比")]
        public bool isGameRootStartContrast = false;

        //------------------------------------------------------------

        [BoxGroup("场景资源内容")] [LabelText("场景内所有AssetBundle资源信息表")]
        public List<string> hotFixRuntimeAssetBundleSceneConfigTable;

        [BoxGroup("场景资源内容")] [LabelText("远程资源下载完毕")]
        public bool isRemoteSceneConfigDownLoad;

        [BoxGroup("场景资源内容")] [LabelText("本地场景数据表")]
        public List<HotFixRuntimeDownConfig> localSceneHotFixRuntimeDownConfig = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("场景资源内容")] [LabelText("远程场景数据表")]
        public List<HotFixRuntimeSceneAssetBundleConfig> remoteSceneHotFixRuntimeDownConfig = new List<HotFixRuntimeSceneAssetBundleConfig>();

        [BoxGroup("场景资源内容")] [LabelText("本地场景数据表读取")]
        public bool islocalSceneHotFixRuntimeDownConfigLoad;

        [BoxGroup("场景资源内容")] [LabelText("远程场景数据表下载")]
        public bool isRemoteSceneHotFixRuntimeDownConfigDownLoad;

        [BoxGroup("本地检测")] [LabelText("SceneAssetBundle对比")]
        public bool isSceneAssetBundleContrast;

        [BoxGroup("本地检测")] [LabelText("需要下载的文件")]
        public List<HotFixRuntimeDownConfig> needDownHotFixRuntimeDownConfig = new List<HotFixRuntimeDownConfig>();

        [BoxGroup("本地检测")] [LabelText("HotFixRuntimeFileDown本地检测")]
        public HotFixRuntimeFileDown hotFixRuntimeFileDown;

        //对比进度接口
        private List<IHotFixRuntimeFileContrast> _hotFixRuntimeFileContrasts = new List<IHotFixRuntimeFileContrast>();

        //文件错误接口
        List<IHotFixFileError> _hotFixFileErrorList = new List<IHotFixFileError>();

        IEnumerator Start()
        {
            _hotFixRuntimeFileContrasts = HotFixGlobal.GetAllObjectsInScene<IHotFixRuntimeFileContrast>();
            _hotFixFileErrorList = HotFixGlobal.GetAllObjectsInScene<IHotFixFileError>();
            StartCoroutine(LocalIsUpdateLoad());
            yield return new WaitUntil(() => localIsUpdateLoad);
            Debug.Log("本地更新:" + localIsUpdate);
            StartCoroutine(OnlyResourcesVersionContrast());
            yield return new WaitUntil(() => OnlyResourcesVersionContrastLoad);
            Debug.Log("仅资源版本对比:" + onlyResourcesVersionContrast);
            StartCoroutine(HotFixPathLocalLoad());
            yield return new WaitUntil(() => hotFixPathLocalLoad);
            if (localIsUpdate)
            {
                //开始下载远程配置表
                StartCoroutine(StartDownAssetConfig());
                yield return new WaitUntil(() => downAssetConfigOver);
                //开始本地资源对比
                StartCoroutine(StartLocalAssetContrast());
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
            string hotFixDownPath = HotFixGlobal.GetDeviceStoragePath(true) + "/Config/" + "LocalIsUpdate.txt";
            UnityWebRequest hotFixPathLoadLocalFile = UnityWebRequest.Get(hotFixDownPath);
            yield return hotFixPathLoadLocalFile.SendWebRequest();
            if (hotFixPathLoadLocalFile.responseCode == 200)
            {
                localIsUpdate = bool.Parse(hotFixPathLoadLocalFile.downloadHandler.text);
            }
            else
            {
                HotFixDebug.Log("本地下载路径不存在:" + hotFixDownPath);
                //如果本地未读取到这个配置表,直接设置为本地更新
                localIsUpdate = true;
            }

            localIsUpdateLoad = true;
        }

        /// <summary>
        /// 仅资源版本对比
        /// </summary>
        /// <returns></returns>
        IEnumerator OnlyResourcesVersionContrast()
        {
            //本地下载路径
            string allResourcesContrastPath = HotFixGlobal.GetDeviceStoragePath(true) + "/Config/" + "OnlyResourcesVersionContrast.txt";
            UnityWebRequest allResourcesContrastFile = UnityWebRequest.Get(allResourcesContrastPath);
            yield return allResourcesContrastFile.SendWebRequest();
            if (allResourcesContrastFile.responseCode == 200)
            {
                onlyResourcesVersionContrast = bool.Parse(allResourcesContrastFile.downloadHandler.text);
            }
            else
            {
                HotFixDebug.Log("本地下载路径不存在:" + allResourcesContrastPath);
                //如果仅资源对比不存在,设置为True
                onlyResourcesVersionContrast = true;
                HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/Config", "OnlyResourcesVersionContrast.txt", "True");
            }

            OnlyResourcesVersionContrastLoad = true;
        }

        /// <summary>
        /// HotFixPath读取
        /// </summary>
        /// <returns></returns>
        IEnumerator HotFixPathLocalLoad()
        {
            //本地下载路径
            string hotFixDownPath = HotFixGlobal.GetDeviceStoragePath(true) + "/Config/" + "HotFixDownPath.txt";
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
                foreach (IHotFixFileError fixFileError in _hotFixFileErrorList)
                {
                    fixFileError.HotFixFileError("本地下载路径不存在:" + hotFixDownPath);
                }
            }


            hotFixPathLocalLoad = true;
        }

        //开始下载配置表
        IEnumerator StartDownAssetConfig()
        {
            HotFixDebug.Log("配置表开始下载----------");
            foreach (IHotFixRuntimeFileContrast hotFixRuntimeFileCheck in _hotFixRuntimeFileContrasts)
            {
                hotFixRuntimeFileCheck.HotFixRuntimeTableDownStart();
            }

            HotFixDebug.Log("元数据表开始下载");
            StartCoroutine(DownMetadataConfig());
            yield return new WaitUntil(() => isRemoteMetadataConfigDownLoad);
            HotFixDebug.Log("元数据表下载完毕");

            HotFixDebug.Log("Assembly表开始下载");
            StartCoroutine(DownAssemblyConfig());
            yield return new WaitUntil(() => isRemoteAssemblyConfigDownLoad);
            HotFixDebug.Log("Assembly表下载完毕");

            HotFixDebug.Log("OtherAssembly表开始下载");
            StartCoroutine(DownOtherAssemblyConfig());
            yield return new WaitUntil(() => isRemoteOtherAssemblyConfigDownLoad);
            HotFixDebug.Log("OtherAssembly表下载完毕");

            HotFixDebug.Log("GameRoot表开始下载");
            StartCoroutine(DownGameRootStartConfig());
            yield return new WaitUntil(() => isRemoteGameRootStartDownLoad);
            HotFixDebug.Log("GameRoot表下载完毕");
            StartCoroutine(DownSceneCountConfig());
            yield return new WaitUntil(() => isRemoteSceneConfigDownLoad);
            HotFixDebug.Log("SceneCount下载完毕");

            StartCoroutine(StartDownSceneAssetBundleConfig());
            yield return new WaitUntil(() => isRemoteSceneHotFixRuntimeDownConfigDownLoad);
            HotFixDebug.Log("配置表下载完毕----------");
            // yield return new WaitForSeconds(0.02f);
            foreach (IHotFixRuntimeFileContrast hotFixRuntimeFileCheck in _hotFixRuntimeFileContrasts)
            {
                hotFixRuntimeFileCheck.HotFixRuntimeTableDownOver();
            }

            downAssetConfigOver = true;
        }

        #region 下载远程数据配置表

        //下载远程元数据配置表
        IEnumerator DownMetadataConfig()
        {
            UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/MetadataConfig/" + "MetadataConfig.json");
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
                remoteMetadataHotFixRuntimeDownConfigTable = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(request.downloadHandler.text);

                //元数据下载完毕
                isRemoteMetadataConfigDownLoad = true;
                //更新检测数量
                localFileUpdateCheckAssetNumberMax += remoteMetadataHotFixRuntimeDownConfigTable.Count;
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
                remoteAssemblyHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(request.downloadHandler.text);
                //Assembly下载完毕
                isRemoteAssemblyConfigDownLoad = true;
                //更新检测数量
                localFileUpdateCheckAssetNumberMax += 1;
            }
        }

        //下载远程Assembly配置表
        IEnumerator DownOtherAssemblyConfig()
        {
            UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/OtherAssemblyConfig/" + "OtherAssemblyConfig.json");
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                //请求错误,等待一定时间后再次请求
                yield return new WaitForSeconds(againDownWaitTime);
                StartCoroutine(DownOtherAssemblyConfig());
            }
            else
            {
                //获得OtherAssembly配置表
                remoteOtherAssemblyHotFixRuntimeDownConfigs = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(request.downloadHandler.text);
                //Assembly下载完毕
                isRemoteOtherAssemblyConfigDownLoad = true;
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
                remoteGameRootStartHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(request.downloadHandler.text);
                //GameRootStart下载完毕
                isRemoteGameRootStartDownLoad = true;
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
                hotFixRuntimeAssetBundleSceneConfigTable = JsonUtil.FromJson<List<string>>(request.downloadHandler.text);
                //运行场景数量下载完毕
                isRemoteSceneConfigDownLoad = true;
            }
        }


        IEnumerator StartDownSceneAssetBundleConfig()
        {
            for (int i = 0; i < hotFixRuntimeAssetBundleSceneConfigTable.Count; i++)
            {
                yield return StartCoroutine(DownSceneAssetBundleConfig(hotFixRuntimeAssetBundleSceneConfigTable[i]));
            }

            isRemoteSceneHotFixRuntimeDownConfigDownLoad = true;
        }

        //下载场景AssetBundle配置表
        IEnumerator DownSceneAssetBundleConfig(string sceneName)
        {
            UnityWebRequest request = UnityWebRequest.Get(hotFixPath + "HotFixRuntime/HotFixAssetBundleConfig/" + sceneName + ".json");
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                Debug.Log(request.responseCode + ":" + request.url);
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
                //场景内资源
                localFileUpdateCheckAssetNumberMax += hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixRuntimeDownConfigs.Count;
                //添加到场景AssetBundle配置表
                remoteSceneHotFixRuntimeDownConfig.Add(hotFixRuntimeSceneAssetBundleConfig);
                // isRemoteSceneHotFixRuntimeDownConfigDownLoad = true;
                Debug.Log("添加场景:" + hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig.name);
            }
        }

        #endregion

        //检测本地资源

        IEnumerator StartLocalAssetContrast()
        {
            //元数据配置表本地读取
            yield return StartCoroutine(MetadataConfigLocalLoad());
            //元数据对比
            yield return StartCoroutine(MetadataLocalContrast());
            //保存配置表缓存文件
            SaveMetadataConfigCacheFile();

            //Assembly配置表本地读取
            yield return StartCoroutine(AssemblyConfigLocalLoad());
            //Assembly对比
            yield return StartCoroutine(AssemblyLocalContrast());
            //保存配置表缓存文件
            SaveAssemblyConfigCacheFile();

            //OtherAssembly配置表本地读取
            yield return StartCoroutine(OtherAssemblyConfigLocalLoad());
            //Assembly对比
            yield return StartCoroutine(OtherAssemblyLocalContrast());
            //保存配置表缓存文件
            SaveOtherAssemblyConfigCacheFile();

            //GameRootStart本地读取
            yield return StartCoroutine(GameRootStartConfigLocalLoad());
            //GameRootStart对比
            yield return StartCoroutine(GameRootStartLocalContrast());
            //保存配置表缓存文件
            SaveGameRootStartConfigCacheFile();

            //HotFixAssetAssetBundle本地读取
            yield return StartCoroutine(HotFixAssetAssetBundleSceneConfigLocalLoad());
            //HotFixAssetAssetBundle远程读取
            List<HotFixRuntimeDownConfig> hotFixAssetAssetBundleAssetConfigs = HotFixAssetAssetBundleSceneConfigGroup();

            //HotFixAssetAssetBundle对比
            yield return StartCoroutine(AssetBundleLocalContrast(hotFixAssetAssetBundleAssetConfigs));
            //保存配置表缓存文件
            SaveSceneHotFixRuntimeAssetBundleConfigCacheFile();
            Debug.Log("本地检测完毕");
            yield return new WaitForSeconds(0.02f);
            foreach (IHotFixRuntimeFileContrast hotFixRuntimeFileCheck in _hotFixRuntimeFileContrasts)
            {
                hotFixRuntimeFileCheck.HotFixRuntimeLocalFileContrastOver();
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
                foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in needDownHotFixRuntimeDownConfig)
                {
                    Debug.Log("需要下载的资源:"+hotFixRuntimeDownConfig.name);
                }
                hotFixRuntimeFileDown.DownHotFixRuntimeDownConfig(needDownHotFixRuntimeDownConfig, hotFixPath);
            }
        }


        #region 本地数据检测

        /// <summary>
        /// 根据名字获取HotFixRuntimeDownConfig数据
        /// </summary>
        /// <param name="HotFixRuntimeDownConfigName"></param>
        /// <param name="hotFixRuntimeDownConfigs"></param>
        /// <returns></returns>
        private HotFixRuntimeDownConfig GetHotFixRuntimeDownConfigByName(string HotFixRuntimeDownConfigName, List<HotFixRuntimeDownConfig> hotFixRuntimeDownConfigs)
        {
            for (int i = 0; i < hotFixRuntimeDownConfigs.Count; i++)
            {
                if (hotFixRuntimeDownConfigs[i].name == HotFixRuntimeDownConfigName)
                {
                    return hotFixRuntimeDownConfigs[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 运行时资源数据对比
        /// </summary>
        /// <param name="remoteHotFixRuntimeDownConfig"></param>
        /// <param name="localHotFixRuntimeDownConfig"></param>
        /// <returns></returns>
        IEnumerator HotFixRuntimeFileContrast(HotFixRuntimeDownConfig remoteHotFixRuntimeDownConfig, HotFixRuntimeDownConfig localHotFixRuntimeDownConfig = null)
        {
            //开启仅资源对比
            if (onlyResourcesVersionContrast)
            {
                if (localHotFixRuntimeDownConfig == null)
                {
                    needDownHotFixRuntimeDownConfig.Add(remoteHotFixRuntimeDownConfig);
                    Debug.Log("本地文件不存在:" + remoteHotFixRuntimeDownConfig.name + "\n" + "远程版本号:" + remoteHotFixRuntimeDownConfig.version);
                }
                else
                {
                    if (localHotFixRuntimeDownConfig.version != remoteHotFixRuntimeDownConfig.version)
                    {
                        Debug.Log("文件不一致:" + remoteHotFixRuntimeDownConfig.name + "\n" + "远程版本号:" + remoteHotFixRuntimeDownConfig.version + "\n" + "本地版本号:" + localHotFixRuntimeDownConfig.version +
                                  "\n" + "远程md5:" + remoteHotFixRuntimeDownConfig.md5 + "\n" + "本地md5:" + localHotFixRuntimeDownConfig.md5);
                        needDownHotFixRuntimeDownConfig.Add(remoteHotFixRuntimeDownConfig);
                    }
                    else
                    {
                        /*Debug.Log("文件一致:" + remoteHotFixRuntimeDownConfig.name + "\n" + "远程版本号:" + remoteHotFixRuntimeDownConfig.version + "\n" + "本地版本号:" + localHotFixRuntimeDownConfig.version +
                                  "\n" + "远程md5:" + remoteHotFixRuntimeDownConfig.md5 + "\n" + "本地md5:" + localHotFixRuntimeDownConfig.md5);*/
                    }
                }
            }
            else
            {
                string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + remoteHotFixRuntimeDownConfig.path + remoteHotFixRuntimeDownConfig.name;
                UnityWebRequest request = UnityWebRequest.Get(localFilePath);
                yield return request.SendWebRequest();
                if (request.responseCode != 200)
                {
                    //本地文件不存在,添加到下载列表
                    Debug.Log("本地文件不存在:" + localFilePath);
                    needDownHotFixRuntimeDownConfig.Add(remoteHotFixRuntimeDownConfig);
                }
                else
                {
                    //Md5对比
                    if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != remoteHotFixRuntimeDownConfig.md5)
                    {
                        //本地文件不一致,添加到下载列表
                        Debug.Log("远端md5:" + remoteHotFixRuntimeDownConfig.md5);
                        Debug.Log("本地md5:" + HotFixGlobal.GetMD5HashByte(request.downloadHandler.data));
                        needDownHotFixRuntimeDownConfig.Add(remoteHotFixRuntimeDownConfig);
                    }
                    else
                    {
                        // Debug.Log("文件一致:" + remoteHotFixRuntimeDownConfig.name + "md5:" + remoteHotFixRuntimeDownConfig.md5);
                    }
                }
            }

            isMetadataContrast = true;
            //更新检测数量
            currentLocalFileUpdateCheckAssetNumber += 1;
            foreach (IHotFixRuntimeFileContrast hotFixRuntimeFileCheck in _hotFixRuntimeFileContrasts)
            {
                hotFixRuntimeFileCheck.HotFixRuntimeLocalFileContrast(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
            }
        }

        #region 元数据

        /// <summary>
        /// 元数据配置本地加载
        /// </summary>
        /// <returns></returns>
        IEnumerator MetadataConfigLocalLoad()
        {
            string metadataConfigLocalPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFixRuntime/MetadataConfig/" + "MetadataConfig.json";
            UnityWebRequest metadataConfigLocalFile = UnityWebRequest.Get(metadataConfigLocalPath);
            yield return metadataConfigLocalFile.SendWebRequest();
            if (metadataConfigLocalFile.responseCode == 200)
            {
                localMetadataHotFixRuntimeDownConfigTable = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(metadataConfigLocalFile.downloadHandler.text);
            }
            else
            {
                localMetadataHotFixRuntimeDownConfigTable = new List<HotFixRuntimeDownConfig>();
            }

            isLocalMetadataConfigLoad = true;
        }

        /// <summary>
        /// 元数据资源对比
        /// </summary>
        /// <returns></returns>
        IEnumerator MetadataLocalContrast()
        {
            for (int i = 0; i < remoteMetadataHotFixRuntimeDownConfigTable.Count; i++)
            {
                yield return StartCoroutine(
                    HotFixRuntimeFileContrast(remoteMetadataHotFixRuntimeDownConfigTable[i],
                        GetHotFixRuntimeDownConfigByName(remoteMetadataHotFixRuntimeDownConfigTable[i].name, localMetadataHotFixRuntimeDownConfigTable)));
            }

            isMetadataContrast = true;
        }

        /// <summary>
        /// 保存元数据配置表缓存文件
        /// </summary>
        private void SaveMetadataConfigCacheFile()
        {
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/MetadataConfig", "MetadataConfig.json" + ".Cache",
                JsonUtil.ToJson(remoteMetadataHotFixRuntimeDownConfigTable));

            //添加到缓存列表中
            hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/MetadataConfig/" + "MetadataConfig.json" + ".Cache");
        }

        #endregion

        #region Assembly

        /// <summary>
        /// Assembly配置本地加载
        /// </summary>
        /// <returns></returns>
        IEnumerator AssemblyConfigLocalLoad()
        {
            string assemblyConfigLocalPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFixRuntime/AssemblyConfig/" + "AssemblyConfig.json";
            UnityWebRequest assemblyConfigLocalFile = UnityWebRequest.Get(assemblyConfigLocalPath);
            yield return assemblyConfigLocalFile.SendWebRequest();
            if (assemblyConfigLocalFile.responseCode == 200)
            {
                localAssemblyHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(assemblyConfigLocalFile.downloadHandler.text);
            }
            else
            {
                localAssemblyHotFixRuntimeDownConfig = new HotFixRuntimeDownConfig();
            }

            isLocalAssemblyConfigLoad = true;
        }

        /// <summary>
        /// Assembly资源对比
        /// </summary>
        /// <returns></returns>
        IEnumerator AssemblyLocalContrast()
        {
            yield return StartCoroutine(HotFixRuntimeFileContrast(remoteAssemblyHotFixRuntimeDownConfig, localAssemblyHotFixRuntimeDownConfig));
            isAssemblyContrast = true;
        }

        /// <summary>
        /// 保存Assembly配置表缓存文件
        /// </summary>
        private void SaveAssemblyConfigCacheFile()
        {
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/AssemblyConfig", "AssemblyConfig.json" + ".Cache",
                JsonUtil.ToJson(remoteAssemblyHotFixRuntimeDownConfig));
            //添加到缓存列表中
            hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/AssemblyConfig/" + "AssemblyConfig.json" + ".Cache");
        }

        #endregion

        #region OtherAssembly

        /// <summary>
        /// OtherAssembly配置本地加载
        /// </summary>
        /// <returns></returns>
        IEnumerator OtherAssemblyConfigLocalLoad()
        {
            string otherAssemblyConfigLocalPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFixRuntime/OtherAssemblyConfig/" + "OtherAssemblyConfig.json";
            UnityWebRequest assemblyConfigLocalFile = UnityWebRequest.Get(otherAssemblyConfigLocalPath);
            yield return assemblyConfigLocalFile.SendWebRequest();
            if (assemblyConfigLocalFile.responseCode == 200)
            {
                localOtherAssemblyHotFixRuntimeDownConfigs = JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(assemblyConfigLocalFile.downloadHandler.text);
            }
            else
            {
                localOtherAssemblyHotFixRuntimeDownConfigs = new List<HotFixRuntimeDownConfig>();
            }

            isLocalOtherAssemblyConfigLoad = true;
        }

        /// <summary>
        /// OtherAssembly资源对比
        /// </summary>
        /// <returns></returns>
        IEnumerator OtherAssemblyLocalContrast()
        {
            for (int i = 0; i < remoteOtherAssemblyHotFixRuntimeDownConfigs.Count; i++)
            {
                HotFixRuntimeDownConfig localOtherAssemblyHotFixRuntimeDownConfig = null;
                for (int j = 0; j < localOtherAssemblyHotFixRuntimeDownConfigs.Count; j++)
                {
                    if (localOtherAssemblyHotFixRuntimeDownConfigs[j].name == remoteOtherAssemblyHotFixRuntimeDownConfigs[i].name)
                    {
                        localOtherAssemblyHotFixRuntimeDownConfig = localOtherAssemblyHotFixRuntimeDownConfigs[j];
                    }
                }

                yield return StartCoroutine(HotFixRuntimeFileContrast(remoteOtherAssemblyHotFixRuntimeDownConfigs[i], localOtherAssemblyHotFixRuntimeDownConfig));
            }

            isOtherAssemblyContrast = true;
        }

        /// <summary>
        /// 保存OtherAssembly配置表缓存文件
        /// </summary>
        private void SaveOtherAssemblyConfigCacheFile()
        {
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/OtherAssemblyConfig", "OtherAssemblyConfig.json" + ".Cache",
                JsonUtil.ToJson(remoteOtherAssemblyHotFixRuntimeDownConfigs));
            //添加到缓存列表中
            hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/OtherAssemblyConfig/" + "OtherAssemblyConfig.json" + ".Cache");
        }

        #endregion

        #region GameRootStart

        /// <summary>
        /// GameRootStart配置本地加载
        /// </summary>
        /// <returns></returns>
        IEnumerator GameRootStartConfigLocalLoad()
        {
            string gameRootStartConfigLocalPath = HotFixGlobal.GetDeviceStoragePath(true) + "/HotFixRuntime/GameRootStartAssetBundleConfig/" + "GameRootStartConfig.json";
            UnityWebRequest gameRootStartConfigLocalFile = UnityWebRequest.Get(gameRootStartConfigLocalPath);
            yield return gameRootStartConfigLocalFile.SendWebRequest();
            if (gameRootStartConfigLocalFile.responseCode == 200)
            {
                localGameRootStartHotFixRuntimeDownConfig = JsonUtil.FromJson<HotFixRuntimeDownConfig>(gameRootStartConfigLocalFile.downloadHandler.text);
            }
            else
            {
                localGameRootStartHotFixRuntimeDownConfig = null;
            }

            isLocalGameRootStartConfigLoad = true;
        }

        /// <summary>
        /// GameRootStart资源对比
        /// </summary>
        /// <returns></returns>
        IEnumerator GameRootStartLocalContrast()
        {
            string localFilePath = HotFixGlobal.GetDeviceStoragePath(true) + "/" + remoteGameRootStartHotFixRuntimeDownConfig.path + remoteGameRootStartHotFixRuntimeDownConfig.name;
            UnityWebRequest request = UnityWebRequest.Get(localFilePath);
            yield return request.SendWebRequest();
            if (request.responseCode != 200)
            {
                //本地文件不存在,添加到下载列表
                needDownHotFixRuntimeDownConfig.Add(remoteGameRootStartHotFixRuntimeDownConfig);
            }
            else
            {
                //本地Md5校验不通过,添加到下载列表
                if (HotFixGlobal.GetMD5HashByte(request.downloadHandler.data) != remoteGameRootStartHotFixRuntimeDownConfig.md5)
                {
                    needDownHotFixRuntimeDownConfig.Add(remoteGameRootStartHotFixRuntimeDownConfig);
                }
            }

            //更新检测数量
            currentLocalFileUpdateCheckAssetNumber += 1;
            foreach (IHotFixRuntimeFileContrast hotFixRuntimeFileCheck in _hotFixRuntimeFileContrasts)
            {
                hotFixRuntimeFileCheck.HotFixRuntimeLocalFileContrast(currentLocalFileUpdateCheckAssetNumber, localFileUpdateCheckAssetNumberMax);
            }

            isGameRootStartContrast = true;
        }

        /// <summary>
        /// 保存GameRootStart配置表缓存文件
        /// </summary>
        private void SaveGameRootStartConfigCacheFile()
        {
            HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/GameRootStartAssetBundleConfig", "GameRootStartConfig.json" + ".Cache",
                JsonUtil.ToJson(remoteGameRootStartHotFixRuntimeDownConfig));
            //添加到缓存列表中
            hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/GameRootStartAssetBundleConfig/" + "GameRootStartConfig.json" + ".Cache");
        }

        #endregion

        #region HotFixAssetAssetBundle

        /// <summary>
        /// HotFixAssetAssetBundle配置本地加载
        /// </summary>
        /// <returns></returns>
        IEnumerator HotFixAssetAssetBundleSceneConfigLocalLoad()
        {
            for (int i = 0; i < hotFixRuntimeAssetBundleSceneConfigTable.Count; i++)
            {
                UnityWebRequest request = UnityWebRequest.Get(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + hotFixRuntimeAssetBundleSceneConfigTable[i] + ".json");
                yield return request.SendWebRequest();
                if (request.responseCode == 200)
                {
                    //场景AssetBundle配置表
                    HotFixRuntimeSceneAssetBundleConfig hotFixRuntimeSceneAssetBundleConfig = JsonUtil.FromJson<HotFixRuntimeSceneAssetBundleConfig>(request.downloadHandler.text);
                    localSceneHotFixRuntimeDownConfig.Add(hotFixRuntimeSceneAssetBundleConfig.sceneHotFixRuntimeDownConfig);
                    foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in hotFixRuntimeSceneAssetBundleConfig.repeatSceneHotFixRuntimeDownConfigs)
                    {
                        localSceneHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                    }

                    foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in hotFixRuntimeSceneAssetBundleConfig.assetBundleHotFixRuntimeDownConfigs)
                    {
                        localSceneHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                    }

                    foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in hotFixRuntimeSceneAssetBundleConfig.sceneExceptConfigsHotFixRuntimeDownConfigs)
                    {
                        localSceneHotFixRuntimeDownConfig.Add(hotFixRuntimeDownConfig);
                    }
                }
            }

            islocalSceneHotFixRuntimeDownConfigLoad = true;
        }

        /// <summary>
        /// HotFixAssetAssetBundle转换为HotFixRuntimeDownConfig表
        /// </summary>
        /// <returns></returns>
        private List<HotFixRuntimeDownConfig> HotFixAssetAssetBundleSceneConfigGroup()
        {
            List<HotFixRuntimeDownConfig> hotFixRuntimeAssetConfigs = new List<HotFixRuntimeDownConfig>();
            //遍历所有场景配置表
            foreach (HotFixRuntimeSceneAssetBundleConfig hotFixAssetAssetBundleSceneConfig in remoteSceneHotFixRuntimeDownConfig)
            {
                //添加场景
                hotFixRuntimeAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneHotFixRuntimeDownConfig);
                //添加重复资源
                foreach (HotFixRuntimeDownConfig hotFixRuntimeAssetBundleConfig in hotFixAssetAssetBundleSceneConfig.repeatSceneHotFixRuntimeDownConfigs)
                {
                    hotFixRuntimeAssetConfigs.Add(hotFixRuntimeAssetBundleConfig);
                }

                //添加其他
                for (int i = 0; i < hotFixAssetAssetBundleSceneConfig.assetBundleHotFixRuntimeDownConfigs.Count; i++)
                {
                    hotFixRuntimeAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.assetBundleHotFixRuntimeDownConfigs[i]);
                }

                //额外数据
                for (int i = 0; i < hotFixAssetAssetBundleSceneConfig.sceneExceptConfigsHotFixRuntimeDownConfigs.Count; i++)
                {
                    hotFixRuntimeAssetConfigs.Add(hotFixAssetAssetBundleSceneConfig.sceneExceptConfigsHotFixRuntimeDownConfigs[i]);
                }
            }

            return hotFixRuntimeAssetConfigs;
        }

        /// <summary>
        /// HotFixAssetBundle对比
        /// </summary>
        /// <param name="hotFixAssetAssetBundleAssetConfigs"></param>
        /// <returns></returns>
        IEnumerator AssetBundleLocalContrast(List<HotFixRuntimeDownConfig> hotFixAssetAssetBundleAssetConfigs)
        {
            for (int i = 0; i < hotFixAssetAssetBundleAssetConfigs.Count; i++)
            {
                yield return StartCoroutine(HotFixRuntimeFileContrast(hotFixAssetAssetBundleAssetConfigs[i],
                    GetHotFixRuntimeDownConfigByName(hotFixAssetAssetBundleAssetConfigs[i].name, localSceneHotFixRuntimeDownConfig)));
            }

            isSceneAssetBundleContrast = true;
        }

        /// <summary>
        /// 保存AssetBundle配置表缓存文件
        /// </summary>
        private void SaveSceneHotFixRuntimeAssetBundleConfigCacheFile()
        {
            //遍历所有场景配置表
            foreach (HotFixRuntimeSceneAssetBundleConfig hotFixAssetAssetBundleSceneConfig in remoteSceneHotFixRuntimeDownConfig)
            {
                string sceneName = string.Empty;
                for (int i = 0; i < hotFixRuntimeAssetBundleSceneConfigTable.Count; i++)
                {
                    if (HotFixGlobal.String_AllCharToLower(hotFixRuntimeAssetBundleSceneConfigTable[i]) == hotFixAssetAssetBundleSceneConfig.sceneHotFixRuntimeDownConfig.name)
                    {
                        sceneName = hotFixRuntimeAssetBundleSceneConfigTable[i];
                    }
                }

                string localPathCacheName = sceneName + ".json.Cache";
                HotFixGlobal.SaveTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig", localPathCacheName, JsonUtil.ToJson(hotFixAssetAssetBundleSceneConfig));
                //添加到缓存列表中
                hotFixRuntimeFileDown.replaceCacheFile.Add(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/HotFixAssetBundleConfig/" + localPathCacheName);
            }
        }

        #endregion

        #endregion
    }
}