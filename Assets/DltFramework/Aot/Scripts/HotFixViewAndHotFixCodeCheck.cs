using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace Aot
{
    public class HotFixViewAndHotFixCodeCheck : MonoBehaviour
    {
        [LabelText("本地开启更新")] public bool localIsUpdate;
        [LabelText("下载地址")] public string hotFixPath = "http://127.0.0.1/";
        [LabelText("总的下载量数据")] public double totalDownloadValue;

        [LabelText("当前下载量数据")] public double currentDownloadValue;

        [BoxGroup("HotFixView")] [LabelText("本地HotFixView文件数据")]
        public HotFixAssetConfig localHotFixViewHotFixAssetConfig;

        [BoxGroup("HotFixView")] [LabelText("远程HotFixView文件数据")]
        public HotFixAssetConfig remoteHotFixViewHotFixAssetConfig;

        [BoxGroup("HotFixView")] [LabelText("HotFixView是否需要下载")]
        public bool hotFixViewIsNeedDown;

        [BoxGroup("HotFixCode")] [LabelText("本地HotFixCode文件数据")]
        public HotFixAssetConfig localHotFixCodeHotFixAssetConfig;

        [BoxGroup("HotFixCode")] [LabelText("远程HotFixCode文件数据")]
        public HotFixAssetConfig remoteHotFixCodeHotFixAssetConfig;

        [BoxGroup("HotFixCode")] [LabelText("HotFixCode是否需要下载")]
        public bool hotFixCodeIsNeedDown;

        [LabelText("当前检测时间")] [SerializeField] private float currentCheckTime;
        [LabelText("检测时间")] [SerializeField] private float checkTime = 1;
        [LabelText("下载流")] private FileStream _hotFixFileStream;
        [LabelText("下载请求")] private UnityWebRequest _hotFixUnityWebRequest;
        [LabelText("上一次下载字节长度")] public int oldDownByteLength;
        [LabelText("缓存更改路径")] public List<string> replaceCacheFile = new List<string>();
        [LabelText("超时时间")] private const float timeOut = 1;

        //继承该接口的所有类型
        private List<IHotFixViewAndHotFixCode> _hotFixViewAndHotFixCodes = new List<IHotFixViewAndHotFixCode>();
        private List<IAotFilePathError> _aotFilePathErrors = new List<IAotFilePathError>();

        async void Start()
        {
            await UniTask.DelayFrame(5);
            _hotFixViewAndHotFixCodes = AotGlobal.GetAllObjectsInScene<IHotFixViewAndHotFixCode>();
            _aotFilePathErrors = AotGlobal.GetAllObjectsInScene<IAotFilePathError>();
            await LocalIsUpdate();
        }

        async UniTask LocalIsUpdate()
        {
            // AotDebug.Log("检测本地更新是否开始");
            if (!File.Exists(AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath(), "/Config/localIsUpdate.txt")))
            {
                Debug.Log("本地未找到,拷贝文件");
                await CopyStreamingAssetsPathToPersistentDataPath(AotGlobal.StringBuilderString(Application.streamingAssetsPath, "/Config/localIsUpdate.txt"), Application.persistentDataPath + "/HotFix/", "localIsUpdate.txt");
            }

            //本地更新文件读取
            await LocalIsUpdateLoad();
            // AotDebug.Log("检测本地更新是否完毕");
            if (localIsUpdate)
            {
                // AotDebug.Log("开启更新");
                foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                {
                    hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeLocalIsUpdate(true);
                }

                //开始本地文件检测
                await HotFixAssetContrast();
            }
            else
            {
                AotDebug.Log("关闭更新");
                foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                {
                    hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeLocalIsUpdate(false);
                }

                //直接加载
                LoadHotFixCode();
            }
        }

        async UniTask CopyStreamingAssetsPathToPersistentDataPath(string sourcePath, string destinationPath, string fileName)
        {
            _hotFixUnityWebRequest = UnityWebRequest.Get(sourcePath);
            try
            {
                await _hotFixUnityWebRequest.SendWebRequest();
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                AotGlobal.SaveTextToLoad(AotGlobal.StringBuilderString(destinationPath + "/", fileName), _hotFixUnityWebRequest.downloadHandler.text);
            }
            catch (Exception e)
            {
                AotDebug.Log(AotGlobal.StringBuilderString("访问错误:", e.ToString(), _hotFixUnityWebRequest.url, ":" + _hotFixUnityWebRequest.responseCode));
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                await CopyStreamingAssetsPathToPersistentDataPath(sourcePath, destinationPath, fileName);
            }
        }


        async UniTask HotFixAssetContrast()
        {
            //未在本地找到拷贝HotFixDownPath.txt
            if (!File.Exists(AotGlobal.GetDeviceStoragePath() + "/Config/HotFixDownPath.txt"))
            {
                await CopyStreamingAssetsPathToPersistentDataPath(
                    AotGlobal.StringBuilderString(Application.streamingAssetsPath, "/HotFix/HotFixDownPath.txt"),
                    AotGlobal.StringBuilderString(Application.persistentDataPath, "/Config/"), "HotFixDownPath.txt");
            }

            //HotFix路径
            AotDebug.Log("本地HotFixPathDownPath路径读取");
            await LocalHotFixPathDownPathLoad();

            //-------------------------------------------
            //HotFixView本地配置表读取
            LocalHotFixViewConfigLoad();
            //HotFixView远端配置表读取
            await RemoteHotFixViewConfigDownLoad();
            //HotFixView本地对比
            HotFixViewLocalContrast();
            //hotFixView缓存配置表保存
            SaveHotFixViewConfigCacheFile();
            //-------------------------------------------
            //HotFixCode本地配置表读取
            LocalHotFixCodeConfigLoad();
            //HotFixCode远端配置表读取
            await RemoteHotFixCodeConfigDownLoad();
            //HotFixCode本地对比
            HotFixCodeLocalContrast();
            //hotFixView缓存配置表保存
            SaveHotFixCodeConfigCacheFile();
            //-------------------------------------------

            foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
            {
                hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeDownloadValue(currentDownloadValue, totalDownloadValue);
            }

            if (hotFixCodeIsNeedDown || hotFixViewIsNeedDown)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));

                foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                {
                    hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeIsDown(true);
                }

                //HotFixView需要下载
                if (hotFixViewIsNeedDown)
                {
                    // AotDebug.Log("HotFixView需要下载");
                    await HotFixAssetConfigLocalCacheContrast(remoteHotFixViewHotFixAssetConfig);
                }

                if (hotFixCodeIsNeedDown)
                {
                    // AotDebug.Log("HotFixCode需要下载");
                    await HotFixAssetConfigLocalCacheContrast(remoteHotFixCodeHotFixAssetConfig);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
            else
            {
                foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                {
                    hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeIsDown(false);
                }
            }

            //等待1秒后
            ReplaceCacheFile();
            LoadHotFixCode();
        }

        /// <summary>
        /// 本地更新文件读取
        /// </summary>
        /// <returns></returns>
        async UniTask LocalIsUpdateLoad()
        {
            //本地下载路径
            string hotFixDownPath = AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath(true), "/Config/LocalIsUpdate.txt");
            _hotFixUnityWebRequest = UnityWebRequest.Get(hotFixDownPath);

            try
            {
                await _hotFixUnityWebRequest.SendWebRequest();
                if (_hotFixUnityWebRequest.responseCode == 200)
                {
                    localIsUpdate = bool.Parse(_hotFixUnityWebRequest.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                AotDebug.Log(AotGlobal.StringBuilderString("访问错误:", e.ToString(), _hotFixUnityWebRequest.url, ":", _hotFixUnityWebRequest.responseCode.ToString()));
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                localIsUpdate = true;
            }
        }

        /// <summary>
        /// 本地HotFixDownPath读取
        /// </summary>
        async UniTask LocalHotFixPathDownPathLoad()
        {
            //本地下载路径
            string hotFixDownPath = AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath(true), "/Config/HotFixDownPath.txt");
            _hotFixUnityWebRequest = UnityWebRequest.Get(hotFixDownPath);
            try
            {
                await _hotFixUnityWebRequest.SendWebRequest();

                if (_hotFixUnityWebRequest.responseCode == 200)
                {
                    foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                    {
                        aotFilePathError.FilePathCorrect();
                    }

                    hotFixPath = _hotFixUnityWebRequest.downloadHandler.text;
                    //如果结尾不是/,添加/
                    if (hotFixPath[hotFixPath.Length - 1] != '/')
                    {
                        hotFixPath = AotGlobal.StringBuilderString(hotFixPath, "/");
                    }
                }
            }
            catch (Exception e)
            {
                AotDebug.Log(AotGlobal.StringBuilderString("访问错误:", e.ToString(), _hotFixUnityWebRequest.url, ":", _hotFixUnityWebRequest.responseCode.ToString()));

                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathError(_hotFixUnityWebRequest.url);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(timeOut));
                await LocalHotFixPathDownPathLoad();
            }
        }

        async UniTask HotFixPathCheck()
        {
            _hotFixUnityWebRequest = UnityWebRequest.Get(hotFixPath);
            try
            {
                await _hotFixUnityWebRequest.SendWebRequest();
                if (_hotFixUnityWebRequest.responseCode != 200)
                {
                    foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                    {
                        aotFilePathError.FilePathError(hotFixPath);
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(timeOut));
                    await HotFixPathCheck();
                }
                else
                {
                    foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                    {
                        aotFilePathError.FilePathCorrect();
                    }
                }
            }
            catch (Exception)
            {
                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathError(hotFixPath);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(timeOut));
                await HotFixPathCheck();
            }
        }

        #region HotFixView

        /// <summary>
        /// 本地HotFixViewConfig读取
        /// </summary>
        private void LocalHotFixViewConfigLoad()
        {
            string hotfixViewConfigPath = AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath() + "/", "HotFix/HotFixViewConfig/HotFixViewConfig.json");
            localHotFixViewHotFixAssetConfig = new HotFixAssetConfig();
            if (File.Exists(hotfixViewConfigPath))
            {
                localHotFixViewHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(AotGlobal.GetTextToLoad(AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixViewConfig/", "HotFixViewConfig.json"));
            }
        }

        /// <summary>
        /// 远程HotFixViewConfig读取
        /// </summary>
        async UniTask RemoteHotFixViewConfigDownLoad()
        {
            _hotFixUnityWebRequest = UnityWebRequest.Get(AotGlobal.StringBuilderString(hotFixPath, "HotFix/HotFixViewConfig/HotFixViewConfig.json"));

            await _hotFixUnityWebRequest.SendWebRequest();
            if (_hotFixUnityWebRequest.responseCode == 200)
            {
                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathCorrect();
                }

                //读取远程配置表数据
                remoteHotFixViewHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(_hotFixUnityWebRequest.downloadHandler.text);
            }
            else
            {
                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathError(_hotFixUnityWebRequest.url);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(timeOut));
                await RemoteHotFixViewConfigDownLoad();
            }
        }

        /// <summary>
        /// HotFixView本地对比
        /// </summary>
        private void HotFixViewLocalContrast()
        {
            //检查文件
            hotFixViewIsNeedDown = false;

            if (localHotFixViewHotFixAssetConfig.version != remoteHotFixViewHotFixAssetConfig.version)
            {
                //需要下载
                hotFixViewIsNeedDown = true;
            }

            if (hotFixViewIsNeedDown)
            {
                //更新最大下载量
                totalDownloadValue += double.Parse(remoteHotFixViewHotFixAssetConfig.size);
            }
        }


        /// <summary>
        /// 保存Assembly配置表缓存文件
        /// </summary>
        private void SaveHotFixViewConfigCacheFile()
        {
            string saveDirectory = AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixViewConfig/";
            string saveName = "HotFixViewConfig.json" + ".Cache";
            AotGlobal.SaveTextToLoad(saveDirectory, saveName, JsonUtility.ToJson(remoteHotFixViewHotFixAssetConfig));
            //添加到缓存列表中
            replaceCacheFile.Add(saveDirectory + saveName);
        }

        #endregion

        #region HotFixCode

        /// <summary>
        /// 本地HotFixCodeConfig读取
        /// </summary>
        private void LocalHotFixCodeConfigLoad()
        {
            string hotFixCodeConfigPath = AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath() + "/", "HotFix/HotFixCodeConfig/HotFixCodeConfig.json");
            localHotFixCodeHotFixAssetConfig = new HotFixAssetConfig();
            if (File.Exists(hotFixCodeConfigPath))
            {
                localHotFixCodeHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(AotGlobal.GetTextToLoad(AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixCodeConfig", "HotFixCodeConfig.json"));
            }
        }

        /// <summary>
        /// 远程HotFixViewConfig读取
        /// </summary>
        async UniTask RemoteHotFixCodeConfigDownLoad()
        {
            _hotFixUnityWebRequest = UnityWebRequest.Get(AotGlobal.StringBuilderString(hotFixPath, "HotFix/HotFixCodeConfig/HotFixCodeConfig.json"));

            try
            {
                await _hotFixUnityWebRequest.SendWebRequest();
                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathCorrect();
                }

                //读取远程配置表数据
                remoteHotFixCodeHotFixAssetConfig = JsonUtility.FromJson<HotFixAssetConfig>(_hotFixUnityWebRequest.downloadHandler.text);
            }
            catch (Exception e)
            {
                foreach (IAotFilePathError aotFilePathError in _aotFilePathErrors)
                {
                    aotFilePathError.FilePathError(_hotFixUnityWebRequest.url);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(timeOut));
                await RemoteHotFixViewConfigDownLoad();
            }
        }

        /// <summary>
        /// HotFixCode本地对
        /// </summary>
        private void HotFixCodeLocalContrast()
        {
            //检查文件
            hotFixCodeIsNeedDown = false;
            //本地HotFixCode路径
            if (localHotFixCodeHotFixAssetConfig.version != remoteHotFixCodeHotFixAssetConfig.version)
            {
                hotFixCodeIsNeedDown = true;
            }

            if (hotFixCodeIsNeedDown)
            {
                //更新最大下载量
                totalDownloadValue += double.Parse(remoteHotFixCodeHotFixAssetConfig.size);
            }
        }

        /// <summary>
        /// 保存Assembly配置表缓存文件
        /// </summary>
        private void SaveHotFixCodeConfigCacheFile()
        {
            string saveDirectory = AotGlobal.GetDeviceStoragePath() + "/HotFix/HotFixCodeConfig/";
            string saveName = "HotFixCodeConfig.json" + ".Cache";
            AotGlobal.SaveTextToLoad(saveDirectory, saveName, JsonUtility.ToJson(remoteHotFixCodeHotFixAssetConfig));
            //添加到缓存列表中
            replaceCacheFile.Add(saveDirectory + saveName);
        }

        #endregion


        /// <summary>
        /// HotFixAssetConfig缓存对比
        /// </summary>
        /// <param name="hotFixAssetConfig"></param>
        async UniTask HotFixAssetConfigLocalCacheContrast(HotFixAssetConfig hotFixAssetConfig)
        {
            //下载路径
            string downFileUrl = hotFixPath + hotFixAssetConfig.path + hotFixAssetConfig.name;
            //本地路径文件夹
            string localPathDirectory = AotGlobal.StringBuilderString(AotGlobal.GetDeviceStoragePath(), "/" + hotFixAssetConfig.path);
            //文件夹不存在,创建文件夹
            if (!Directory.Exists(localPathDirectory))
            {
                Directory.CreateDirectory(localPathDirectory);
            }

            //下载文件缓存路径
            string downFileCachePath = AotGlobal.StringBuilderString(localPathDirectory, hotFixAssetConfig.name, ".", hotFixAssetConfig.version.ToString(), ".Cache");
            bool isCache = File.Exists(downFileCachePath);
            if (isCache)
            {
                //本地缓存文件的Md5
                string localCacheMd5 = AotGlobal.GetMD5HashFromFile(downFileCachePath);
                AotDebug.LogWarning(AotGlobal.StringBuilderString("存在缓存文件:", downFileCachePath, ":", "本地文件Md5:", localCacheMd5));
                //当前下载量加上已经下载的缓存量
                currentDownloadValue += AotGlobal.GetFileSize(downFileCachePath);
                //缓存文件的Md5和服务器的Md5相同,表示已经下载完毕
                if (localCacheMd5 == hotFixAssetConfig.md5)
                {
                    replaceCacheFile.Add(downFileCachePath);
                }
                else
                {
                    foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                    {
                        hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeDownloadValue(currentDownloadValue, totalDownloadValue);
                    }

                    //下载请求
                    _hotFixUnityWebRequest = UnityWebRequest.Get(downFileUrl);
                    //使用断点续传下载
                    _hotFixUnityWebRequest.SetRequestHeader("Range", "bytes=" + AotGlobal.GetFileSize(downFileCachePath) + "-");
                    await DownHotFixAssetConfig(downFileCachePath, hotFixAssetConfig);
                }
            }
            else
            {
                //下载请求
                _hotFixUnityWebRequest = UnityWebRequest.Get(downFileUrl);
                await DownHotFixAssetConfig(downFileCachePath, hotFixAssetConfig);
            }
        }

        /// <summary>
        /// 下载HotFixAssetConfig
        /// </summary>
        /// <param name="downFileCachePath"></param>
        /// <param name="hotFixAssetConfig"></param>
        async UniTask DownHotFixAssetConfig(string downFileCachePath, HotFixAssetConfig hotFixAssetConfig)
        {
            //文件流
            _hotFixFileStream = new FileStream(downFileCachePath, FileMode.OpenOrCreate, FileAccess.Write);
            //开启下载
            await _hotFixUnityWebRequest.SendWebRequest();
            currentCheckTime = 0;
            //重置检测时间
            //下载流程完毕,直接写入文件
            WriteContent(_hotFixFileStream);
            //重置上一次下载字节长度
            oldDownByteLength = 0;
            //关闭下载流
            _hotFixFileStream.Close();
            _hotFixFileStream.Dispose();
            _hotFixFileStream = null;
            //检测下载完后的文件的Md5
            string localCacheMd5 = AotGlobal.GetMD5HashFromFile(downFileCachePath);

            if (localCacheMd5 != hotFixAssetConfig.md5)
            {
                AotDebug.LogError(AotGlobal.StringBuilderString("Md5不匹配,删除文件重新下载:", _hotFixUnityWebRequest.url));
                AotDebug.LogWarning(AotGlobal.StringBuilderString("本地下载的Md5:", localCacheMd5));
                AotDebug.LogWarning(AotGlobal.StringBuilderString("服务器的Md5:" + hotFixAssetConfig.md5));
                //旧大小清空
                oldDownByteLength = 0;
                //清除已经下载的大小
                currentDownloadValue -= AotGlobal.GetFileSize(downFileCachePath);
                //关闭下载流
                _hotFixUnityWebRequest = null;
                //删除文件
                if (File.Exists(downFileCachePath))
                {
                    File.Delete(downFileCachePath);
                }

                //再次发起下载请求
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                await HotFixAssetConfigLocalCacheContrast(hotFixAssetConfig);
            }
            else
            {
                _hotFixUnityWebRequest = null;
                replaceCacheFile.Add(downFileCachePath);
            }
        }


        /// <summary>
        /// 加载HotFix数据
        /// </summary>
        private void LoadHotFixCode()
        {
            AotNetworking.networkStatusDetection = false;
            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。  
#if !UNITY_EDITOR
        Assembly hotFix = Assembly.Load(File.ReadAllBytes($"{AotGlobal.GetDeviceStoragePath()}/HotFix/HotFixCode/HotFixCode.dll.bytes"));
#else
            // Editor下无需加载，直接查找获得HotFix程序集  
            Assembly hotFix = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotFixCode");
#endif
            Type type = hotFix.GetType("HotFix.HotFixInit");
            type.GetMethod("Init")?.Invoke(null, null);
        }

        private void Update()
        {
            currentCheckTime += Time.deltaTime;
            if (currentCheckTime >= checkTime)
            {
                currentCheckTime = 0;
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
                // AotDebug.Log("本地大小:" + fileStream.Length);
                //下载文件大小
                int downSize = _hotFixUnityWebRequest.downloadHandler.data.Length;
                // AotDebug.Log("当前下载大小:" + downSize);
                //写入文件长度
                int newDownSize = downSize - oldDownByteLength;
                // AotDebug.Log("写入大小:" + newDownSize);
                // AotDebug.Log("有下载更新:" + newDownSize);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);

                if (newDownSize > 0)
                {
                    // AotDebug.Log(oldDownByteLength + ";" + newDownSize);
                    fileStream.Write(_hotFixUnityWebRequest.downloadHandler.data, oldDownByteLength, newDownSize);

                    foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                    {
                        hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeDownSpeed(newDownSize);
                    }

                    currentDownloadValue += newDownSize;
                    foreach (IHotFixViewAndHotFixCode hotFixViewAndHotFixCode in _hotFixViewAndHotFixCodes)
                    {
                        hotFixViewAndHotFixCode.HotFixViewAndHotFixCodeDownloadValue(currentDownloadValue, totalDownloadValue);
                    }

                    oldDownByteLength = downSize;
                    // AotDebug.Log("写入后大小:" + fileStream.Length);
                }
                else
                {
                    // AotDebug.Log("无更新内容");
                }
            }
        }

        //删除缓存文件
        private void ReplaceCacheFile()
        {
            foreach (string cachePath in replaceCacheFile)
            {
                string[] pathSplit = cachePath.Split(".");
                string replacePath = string.Empty;
                if (pathSplit[pathSplit.Length - 2] == "json")
                {
                    replacePath = cachePath.Replace("." + pathSplit[pathSplit.Length - 1], "");
                }
                else
                {
                    replacePath = cachePath.Replace("." + pathSplit[pathSplit.Length - 2] + "." + pathSplit[pathSplit.Length - 1], "");
                }

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
}