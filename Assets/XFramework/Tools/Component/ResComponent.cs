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
    /// 资源加载组件
    /// </summary>
    public class ResComponent : ComponentBase
    {
        public static ResComponent Instance;

        [SerializeField] [LabelText("资源池")] private Dictionary<string, Object> objDic;

        public override void FrameInitComponent()
        {
            Instance = GetComponent<ResComponent>();
        }

        public override void SceneInitComponent()
        {
            objDic = new Dictionary<string, Object>();
        }

        public override void EndComponent()
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
       
       
     
    }
}