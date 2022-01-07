using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework
{
    public class DownSvc : SvcBase
    {
        public static DownSvc Instance;
        [LabelText("开启下载")] public bool isOnDown;
        [LabelText("文件下载完毕")] public bool downOver;
        [LabelText("文件下载中")] public bool downloading;
        [LabelText("当前下载中任务")] private DownData _currentDownDataTask;
        [LabelText("下载文件")] public List<DownData> allDownSvcData;
        [LabelText("下载文件更新信息间隔")] private float _downFileUpdateInterval;
        private ResSvc.DownFile _downSceneFile;
        private UnityWebRequest _request;

        //下载委托
        public delegate void DownTaskDelegate(float progress, bool downOver);

        public DownTaskDelegate downTaskDelegate;

        public override void StartSvc()
        {
            Instance = GetComponent<DownSvc>();
            // StartCoroutine(DownLoadTest());
        }

        /// <summary>
        /// 获得当前下载数据
        /// </summary>
        /// <returns></returns>
        public DownData GetCurrentDownSvcData()
        {
            return _currentDownDataTask;
        }

        /// <summary>
        /// 根据名称获得下载内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DownData GetDownSvcDataByFileName(string fileName)
        {
            foreach (DownData downSvcData in allDownSvcData)
            {
                if (downSvcData.downName == DataSvc.AllCharToLower(fileName))
                {
                    return downSvcData;
                }
            }

            return null;
        }


        public override void InitSvc()
        {
            //获得下载场景配置信息
            _downSceneFile =
                JsonMapper.ToObject<ResSvc.DownFile>(Resources.Load<TextAsset>("DownFile/SceneFileInfo").text);
            //存储下载信息
            foreach (ResSvc.DownFile.FileInfo fileInfo in _downSceneFile.fileInfoList)
            {
                DownData downData = new DownData
                {
                    downName = fileInfo.fileName, downFileOriginalName = fileInfo.fileOriginalName,
                    downPath = fileInfo.filePath, downCurrentSize = 0,
                    downTotalSize = fileInfo.fileSize
                };
                //Android或者PC平台检测下载存档
                if (Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.WindowsEditor)
                {
                    string downPath = General.GetPlatformDownLoadDataPath() + fileInfo.filePath;
                    if (File.Exists(downPath))
                    {
                        int fileSize = File.ReadAllBytes(downPath).Length;
                        string MD5 = FileOperation.GetMD5HashFromFile(downPath);
                        if (fileSize != fileInfo.fileSize || MD5 != fileInfo.fileMd5)
                        {
                            downData.downCurrentSize = fileSize;
                            allDownSvcData.Add(downData);
                        }
                        else
                        {
                            downData.downOver = true;
                            Debug.Log(downData.downName + "已有缓存");
                        }
                    }
                }
                else
                {
                }

                allDownSvcData.Add(downData);
            }

            /*//如果开启下载,直接下载任务
            if (isOnDown)
            {
                StartCoroutine(DownFileData(GetNotDownOverTask()));
            }*/
        }

        public override void EndSvc()
        {
            UpdateDownFile();
        }

        /// <summary>
        /// 获得场景文件缓存
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public bool GetSceneFileCacheState(string sceneName)
        {
            foreach (DownData downData in allDownSvcData)
            {
                if (downData.downFileOriginalName == sceneName)
                {
                    return downData.downOver;
                }
            }

            return false;
        }

        /// <summary>
        /// 获得场景文件的缓存路径
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public string GetGetSceneFileCachePath(string sceneName)
        {
            foreach (DownData downData in allDownSvcData)
            {
                if (downData.downFileOriginalName == sceneName)
                {
                    return downData.downPath;
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// 下载场景任务
        /// </summary>
        /// <param name="sceneName"></param>
        public void DownSceneTask(string sceneName)
        {
            _currentDownDataTask = GetSceneDownTaskDownData(sceneName);
            StartCoroutine(StartDownFile(_currentDownDataTask));
        }

        /// <summary>
        /// 获得下载任务
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private DownData GetSceneDownTaskDownData(string sceneName)
        {
            foreach (DownData downData in allDownSvcData)
            {
                if (downData.downFileOriginalName == sceneName)
                {
                    return downData;
                }
            }

            return null;
        }

        /// <summary>
        /// 插入下载任务，原任务暂停
        /// </summary>
        /// <param name="downName"></param>
        public void InsertDownTask(string downName)
        {
            //正在下载的与切入的是同一个，不需要切换
            if (_currentDownDataTask != null && _currentDownDataTask.downName == DataSvc.AllCharToLower(downName))
            {
                Debug.Log("插入的下载任务与当前下载的一致");
                return;
            }

            //停掉当前下载任务
            StopCoroutine(DownFileData());
            _currentDownDataTask = GetDownOverTaskByFileName(downName);
            StartCoroutine(DownFileData(_currentDownDataTask));
        }

        private IEnumerator StartDownFile(DownData downData)
        {
            _currentDownDataTask = downData;
            //文件地址
            string filePath = General.GetFileDataPath(downData.downPath);
            Debug.Log("当前下载路径" + filePath);
            //开启下载文件
            _request = UnityWebRequest.Get(filePath);
            //定义下载头文件
            _request.SetRequestHeader("Range",
                "bytes=" + _currentDownDataTask.downCurrentSize + "-" + _currentDownDataTask.downTotalSize);
            yield return _request.SendWebRequest();

            if (_request.responseCode == 404)
            {
                Debug.Log("文件下载错误路径不存在:" + General.GetFileDataPath(downData.downPath));
            }
            else
            {
                _currentDownDataTask.downCurrentSize = _currentDownDataTask.downTotalSize;
                Debug.Log("当前文件下载完毕:" + filePath);
                Debug.Log("当前文件下载完毕:" + _currentDownDataTask.downName);
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    string downPath = General.GetPlatformDownLoadDataPath() +
                                      downData.downPath.Replace(downData.downName, "");
                    Debug.Log("文件追加:" + downPath);
                    FileOperation.SaveFileToLocal(downPath, downData.downName, _request.downloadHandler.data,
                        FileMode.Append);
                }
                else
                {
                    _currentDownDataTask.downContent = _request.downloadHandler.data;
                }

                _request = null;
                downTaskDelegate.Invoke(1, true);
                _currentDownDataTask.downOver = true;
            }
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownFileData(DownData downData = null)
        {
            _currentDownDataTask = downData;
            if (_currentDownDataTask != null)
            {
                downloading = true;
                //文件地址
                string filePath = _currentDownDataTask.downPath;
                //开启下载文件
                _request = UnityWebRequest.Get(General.GetFileDataPath(filePath));
                Debug.Log("开始下载数据:" + _currentDownDataTask.downName);
                _request.SetRequestHeader("Range",
                    "bytes=" + _currentDownDataTask.downCurrentSize + "-" + _currentDownDataTask.downTotalSize);
                yield return _request.SendWebRequest();
                if (_request.responseCode == 404)
                {
                    Debug.Log("文件下载错误路径不存在:" + General.GetFileDataPath(filePath));
                }
                else
                {
                    Debug.Log("当前文件下载完毕:" + _currentDownDataTask.downName);
                }

                _currentDownDataTask.downOver = true;
                downloading = false;
                _currentDownDataTask.downCurrentSize = _currentDownDataTask.downTotalSize;
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.Android ||
                    Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    string downPath = General.GetPlatformDownLoadDataPath() +
                                      downData.downPath.Replace(downData.downName, "");
                    FileOperation.SaveFileToLocal(downPath, downData.downName, _request.downloadHandler.data,
                        FileMode.Append);
                }
                else
                {
                    _currentDownDataTask.downContent = _request.downloadHandler.data;
                }

                _request = null;

                StartCoroutine(DownFileData(GetNotDownOverTask()));
            }
        }

        /// <summary>
        /// 获得一个未下载完毕的任务
        /// </summary>
        /// <returns></returns>
        private DownData GetNotDownOverTask()
        {
            foreach (DownData downData in allDownSvcData)
            {
                if (!downData.downOver)
                {
                    return downData;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据名称获得一个下载任务
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DownData GetDownOverTaskByFileName(string fileName)
        {
            foreach (DownData downData in allDownSvcData)
            {
                if (downData.downName == fileName)
                {
                    return downData;
                }
            }

            return null;
        }

        private void Update()
        {
            if (downloading)
            {
                UpdateDownProgress();
            }

            if (_request != null)
            {
                downTaskDelegate.Invoke(_request.downloadProgress, _request.isDone);
            }
        }

        /// <summary>
        /// 更新下载进度
        /// </summary>
        private void UpdateDownProgress()
        {
            if (_request != null && _currentDownDataTask != null)
            {
                UpdateDownProgress(_currentDownDataTask.downName, (long) _request.downloadedBytes);
            }
        }

        /// <summary>
        /// 更新下载文件
        /// </summary>
        private void UpdateDownFile()
        {
            if (_request != null && _currentDownDataTask != null)
            {
                FileOperation.SaveFileToLocal(
                    _currentDownDataTask.downPath.Replace(_currentDownDataTask.downName, ""),
                    _currentDownDataTask.downName, _request.downloadHandler.data, FileMode.Append);
            }
        }

        [Serializable]
        public class DownData
        {
            [LabelText("下载名称")] public string downName;
            [LabelText("文件原名称")] public string downFileOriginalName;
            [LabelText("下载地址")] public string downPath;
            [LabelText("当前下载大小")] public long downCurrentSize;
            [LabelText("总的下载大小")] public long downTotalSize;
            [LabelText("下载完毕")] public bool downOver;
            [LabelText("下载数据")] [HideInInspector] public byte[] downContent;
        }

        /// <summary>
        /// 更新下载进度
        /// </summary>
        /// <param name="downName"></param>
        /// <param name="downCurrentSize"></param>
        private void UpdateDownProgress(string downName, long downCurrentSize)
        {
            foreach (DownData downSvcData in allDownSvcData)
            {
                if (downSvcData.downName == downName)
                {
                    downSvcData.downCurrentSize = downCurrentSize;
                    break;
                }
            }
        }
    }
}