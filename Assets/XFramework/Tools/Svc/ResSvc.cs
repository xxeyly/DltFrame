using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = System.Object;

namespace XFramework
{
    /// <summary>
    /// 资源加载服务
    /// </summary>
    public class ResSvc : SvcBase
    {
        public static ResSvc Instance;

        [SerializeField] [LabelText("资源池")] private Dictionary<string, Object> objDic;

        public override void StartSvc()
        {
            Instance = GetComponent<ResSvc>();
        }

        public override void InitSvc()
        {
            objDic = new Dictionary<string, Object>();
        }

        public override void EndSvc()
        {
        }

        /// <summary>
        /// 根据传入的物件路径从Resources文件夹中获得指定类型的文件
        /// 如果有缓存,获得缓存的文件,没有就从本地获取,并加入到缓存文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objPath"></param>
        /// <returns></returns>
        public T GetData<T>(string objPath) where T : UnityEngine.Object
        {
            Object obj;
            if (objDic.TryGetValue(objPath, out obj))
            {
                return (T) obj;
            }
            else
            {
                Object newObj = Resources.Load<T>(objPath);
                objDic.Add(objPath, newObj);
                return (T) newObj;
            }
        }

        /// <summary>
        /// 异步从网络上加载
        /// </summary>
        /// <param name="assetBundleNetPath"></param>
        /// <param name="eventType"></param>
        public void AsyncResourcesByNetwork(string assetBundleNetPath, string eventType)
        {
            StartCoroutine(LoadResourcesByNetwork(assetBundleNetPath, eventType));
        }

        /// <summary>
        /// 异步从网络上加载
        /// </summary>
        /// <param name="assetBundleNetPath"></param>
        /// <param name="eventType"></param>
        public void AsyncResourcesByNetwork<T>(string assetBundleNetPath, string eventType, T t)
        {
            StartCoroutine(LoadResourcesByNetwork(assetBundleNetPath, eventType, t));
        }

        IEnumerator LoadResourcesByNetwork(string serverResourcesPath, string eventType)
        {
            //1、使用UnityWebRequest.Get(路径)【服务器 / 本地都可以】 去获取到网页请求
            UnityWebRequest request = UnityWebRequest.Get(serverResourcesPath);
            //2、等待这个请求进行发送完
            yield return request.SendWebRequest();
            ListenerSvc.Instance.ExecuteEvent(eventType, request.downloadHandler.data);
        }

        IEnumerator LoadResourcesByNetwork<T>(string serverResourcesPath, string eventType, T t)
        {
            //1、使用UnityWebRequest.Get(路径)【服务器 / 本地都可以】 去获取到网页请求
            UnityWebRequest request = UnityWebRequest.Get(serverResourcesPath);
            //2、等待这个请求进行发送完
            yield return request.SendWebRequest();
            ListenerSvc.Instance.ExecuteEvent(eventType, request.downloadHandler.data, t);
        }

        /// <summary>
        /// 异步从网络上加载AssetBundle
        /// </summary>
        /// <param name="assetBundleNetPath"></param>
        /// <param name="action"></param>
        public void AsyncAssetBundleByNetwork(string assetBundleNetPath, Action action)
        {
            StartCoroutine(LoadAssetBundleByNetwork(assetBundleNetPath, action));
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="serverAssetBundlePath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IEnumerator LoadAssetBundleByNetwork(string serverAssetBundlePath, Action action)
        {
            //1、使用UnityWebRequest.GetAssetBundle(路径)【服务器 / 本地都可以】 去获取到网页请求
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(serverAssetBundlePath);

            //2、等待这个请求进行发送完
            yield return request.SendWebRequest();
            Debug.Log(request.responseCode);
            //3、发送完请求之后，就要从DownloadHandlerAssetBundle进行获取一个request，得到出来的是一个AssetBundle类对象
            DownloadHandlerAssetBundle.GetContent(request);
            //4、加载完毕后，执行对应的事件
            action.Invoke();
        }
       
        /// <summary>
        /// 下载文件
        /// </summary>
        [Serializable]
        public class DownFile
        {
            /// <summary>
            /// 下载文件列表
            /// </summary>
            [Header("版本信息列表")] public List<FileInfo> fileInfoList;

            /// <summary>
            /// 下载文件信息
            /// </summary>
            [Serializable]
            public struct FileInfo
            {
                [Header("文件名称")] public string fileName;
                [Header("文件原名称")] public string fileOriginalName;

                [Header("文件路径")] public string filePath;
                [Header("文件大小")] public long fileSize;
            }
        }

     
    }
}