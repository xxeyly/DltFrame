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
            //        UnityWebRequest
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
            //        UnityWebRequest
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
        /// 图片转byte数组
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public byte[] ImageToByte(Image img)
        {
            return img.sprite.texture.EncodeToPNG();
        }

        /// <summary>
        /// 数据转精灵
        /// </summary>
        /// <param name="imgByte"></param>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <returns></returns>
        public Sprite ByteToSprite(byte[] imgByte, int spriteWidth, int spriteHeight)
        {
            Texture2D texture2D = new Texture2D(spriteWidth, spriteHeight);
            texture2D.LoadImage(imgByte);
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f));
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

        /// <summary>
        /// 本地文件操作
        /// </summary>
        public class FileOperation
        {
            /// <summary>
            /// 保存文本信息到本地
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <param name="fileName">文件名称</param>
            /// <param name="information">保存信息</param>
            public static void SaveTextToLoad(string path, string fileName, string information)
            {
                if (Directory.Exists(path))
                {
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(aFile, Encoding.UTF8);
                sw.WriteLine(information);
                sw.Close();
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            public static void SaveTextToLoad(string path, string information)
            {
                if (File.Exists(path))
                {
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                FileStream aFile = new FileStream(path, FileMode.Create);
                //得到字符串的UTF8 数据流
                byte[] bts = System.Text.Encoding.UTF8.GetBytes(information);
                // StreamWriter sw = new StreamWriter(aFile, Encoding.UTF8);
                // sw.WriteLine(information);
                // sw.Close();
                aFile.Write(bts, 0, bts.Length);
                if (aFile != null)
                {
                    //清空缓存
                    aFile.Flush();
                    // 关闭流
                    aFile.Close();
                    //销毁资源
                    aFile.Dispose();
                }

#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }

            /// <summary>
            /// 读取本地文件信息
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileName">文件名</param>
            /// <returns></returns>
            public static string GetTextToLoad(string path, string fileName)
            {
//            UnityEngine.Debug.Log(Path + "/" + FileName);
                if (Directory.Exists(path))
                {
                }
                else
                {
                    Debug.LogError("文件不存在:" + path + "/" + fileName);
                }

                FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                var textData = sr.ReadToEnd();
                sr.Close();
                return textData;
            }

            /// <summary>
            /// 读取本地文件信息
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="fileName">文件名</param>
            /// <returns></returns>
            public static string GetTextToLoad(string path)
            {
                if (File.Exists(path))
                {
                }
                else
                {
                    Debug.LogError("文件不存在:" + path);
                }

                FileStream aFile = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                var textData = sr.ReadToEnd();
                sr.Close();
                return textData;
            }

            /// <summary>
            /// 转换为本地路径
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static string ConvertToLocalPath(string path)
            {
                return path.Remove(0, path.IndexOf("Assets", StringComparison.Ordinal));
            }

            /// <summary>获取文件的md5校验码</summary>
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
    }
}