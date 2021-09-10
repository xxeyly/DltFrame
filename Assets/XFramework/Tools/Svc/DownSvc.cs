using System;
using System.Collections;
using System.Collections.Generic;
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

        public override void StartSvc()
        {
            Instance = GetComponent<DownSvc>();
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
                    downName = fileInfo.fileName, downPath = fileInfo.filePath, downCurrentSize = 0,
                    downTotalSize = fileInfo.fileSize
                };
                allDownSvcData.Add(downData);
            }

            //如果开启下载,直接下载任务
            if (isOnDown)
            {
                StartCoroutine(DownFileData(GetNotDownOverTask()));
            }
        }

        public override void EndSvc()
        {
            Instance = null;
        }

        /// <summary>
        /// 插入下载任务，原任务暂停
        /// </summary>
        /// <param name="downName"></param>
        public void InsertDownTask(string downName)
        {
            //正在下载的与切入的是同一个，不需要切换
            Debug.Log(downName);
            Debug.Log(_currentDownDataTask.downName);

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
                _currentDownDataTask.downCurrentSize = _currentDownDataTask.downTotalSize;
                _currentDownDataTask.downContent = _request.downloadHandler.data;
                downloading = false;
                _request = null;
                StartCoroutine(DownFileData(GetNotDownOverTask()));
            }

            /*//未下载完毕
            if (_downIndex < _downSceneFile.fileInfoList.Count)
            {
                //文件地址
                string filePath = _downSceneFile.fileInfoList[_downIndex].filePath;
                //下载文件
                _request = UnityWebRequest.Get(General.GetFileDataPath(filePath));
                Debug.Log("开始下载数据:" + General.GetFileDataPath(filePath));
                yield return _request.SendWebRequest();
                UpdateDownProgress();
                if (_request.responseCode != 200)
                {
                    Debug.Log("文件下载错误路径不存在:" + General.GetFileDataPath(filePath));
                }
                else
                {
                    Debug.Log("当前文件下载完毕:" + General.GetFileDataPath(filePath));
                }

                allDownSvcData[_downIndex].downOver = true;
                allDownSvcData[_downIndex].downContent = _request.downloadHandler.data;
                _downIndex += 1;
                StartCoroutine(DownFileData());
            }
            else
            {
                downOver = true;
                Debug.Log("所有文件下载完毕");
            }*/
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

        [Serializable]
        public class DownData
        {
            [LabelText("下载名称")] public string downName;
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