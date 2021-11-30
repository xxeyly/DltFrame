using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace XFramework
{
    public static class DataSvc
    {
        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RandomSort<T>(List<T> list)
        {
            var random = new Random();
            T time;
            int index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                index = random.Next(0, list.Count - 1);
                if (index != i)
                {
                    time = list[i];
                    list[i] = list[index];
                    list[index] = time;
                }
            }

            return list;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 查找场景中所有类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllObjectsInScene<T>()
        {
            // List<GameObject> objectsInScene = GetAllSceneObjectsWithInactive();
            List<GameObject> objectsInScene = GetAllObjectsOnlyInScene();
            List<T> specifiedType = new List<T>();
            foreach (GameObject go in objectsInScene)
            {
                List<T> ts = new List<T>(go.GetComponents<T>());
                for (int i = 0; i < ts.Count; i++)
                {
                    specifiedType.Add(ts[i]);
                }
            }

            return specifiedType;
        }

        /// <summary>
        /// 查找场景中第一个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObjectsInScene<T>()
        {
            // List<GameObject> objectsInScene = GetAllSceneObjectsWithInactive();
            GameObject objectsInScene = GetObjectsOnlyInScene<T>();
            return objectsInScene.GetComponent<T>();
        }

        /// <summary>
        /// 获得场景中所有物体
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetAllObjectsOnlyInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            foreach (GameObject go in (GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                {
                    objectsInScene.Add(go);
                }
#else
                objectsInScene.Add(go);
#endif
            }

            return objectsInScene;
        }

        /// <summary>
        /// 获得场景中所有第一个物体
        /// </summary>
        /// <returns></returns>
        private static GameObject GetObjectsOnlyInScene<T>()
        {
            foreach (GameObject go in (GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                {
                    if (go.GetComponent<T>() != null)
                    {
                        return go;
                    }
                }
#else
                if (go.GetComponent<T>() != null)
                {
                    return go;
                }
#endif
            }

            return null;
        }

        /// <summary>
        /// 所有转换为小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string AllCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = "";
            foreach (char c in input)
            {
                str += c.ToString().ToLower();
            }

            return str;
        }

        /// <summary>
        /// 获得两点之间的角度360
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Angle(Vector3 origin, Vector3 target)
        {
            //两点的x、y值
            float x = origin.x - target.x;
            float y = origin.y - target.y;

            //斜边长度
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

            //求出弧度
            float cos = x / hypotenuse;
            float radian = Mathf.Acos(cos);

            //用弧度算出角度    
            float angle = 180 / (Mathf.PI / radian);
            if (y > 0)
            {
                angle = 180 + (180 - angle);
            }

            return angle;
        }

        /// <summary>
        /// 集合合并
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> MergeList<T>(params List<T>[] needMergeList)
        {
            List<T> mergeList = new List<T>();

            foreach (List<T> list in needMergeList)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    mergeList.Add(list[i]);
                }
            }

            return mergeList;
        }

        /// <summary>
        /// 集合删除重复项
        /// </summary>
        /// <param name="currentList"></param>
        /// <param name="targetList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RemoveRepeat<T>(List<T> currentList, List<T> targetList)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                if (currentList.Contains(targetList[i]))
                {
                    currentList.Remove(targetList[i]);
                }
            }

            return currentList;
        }

        /// <summary>
        /// 图片转byte数组
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image img)
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
        public static Sprite ByteToSprite(byte[] imgByte, int spriteWidth, int spriteHeight)
        {
            Texture2D texture2D = new Texture2D(spriteWidth, spriteHeight);
            texture2D.LoadImage(imgByte);
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// 值克隆
        /// </summary>
        /// <param name="targetValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> DataValueClone<T>(List<T> targetValue)
        {
            List<T> temp = new List<T>();
            foreach (T t in targetValue)
            {
                temp.Add(t);
            }

            return temp;
        }

        /// <summary>
        /// 获取面板上的值
        /// </summary>
        /// <param name="mTransform"></param>
        /// <returns></returns>
        public static Vector3 GetInspectorEuler(Transform mTransform)
        {
            Vector3 angle = mTransform.eulerAngles;
            float x = angle.x;
            float y = angle.y;
            float z = angle.z;

            if (Vector3.Dot(mTransform.up, Vector3.up) >= 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = angle.x;
                }

                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = angle.x - 360f;
                }
            }

            if (Vector3.Dot(mTransform.up, Vector3.up) < 0f)
            {
                if (angle.x >= 0f && angle.x <= 90f)
                {
                    x = 180 - angle.x;
                }

                if (angle.x >= 270f && angle.x <= 360f)
                {
                    x = 180 - angle.x;
                }
            }

            if (angle.y > 180)
            {
                y = angle.y - 360f;
            }

            if (angle.z > 180)
            {
                z = angle.z - 360f;
            }

            Vector3 vector3 = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
            return vector3;
        }

        /// <summary>
        /// 获得继承类的所有子类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetInheritAllSubclass<T>() where T : class
        {
            // var types = Assembly.GetCallingAssembly().GetTypes();
            var types = typeof(T).Assembly.GetTypes();
            var cType = typeof(T);
            List<T> cList = new List<T>();

            foreach (var type in types)
            {
                var baseType = type.BaseType; //获取基类
                while (baseType != null) //获取所有基类
                {
                    if (baseType.Name == cType.Name)
                    {
                        Type objtype = Type.GetType(type.FullName, true);
                        object obj = Activator.CreateInstance(objtype);
                        if (obj != null)
                        {
                            T info = obj as T;
                            cList.Add(info);
                        }

                        break;
                    }
                    else
                    {
                        baseType = baseType.BaseType;
                    }
                }
            }

            return cList;
        }
#if UNITY_EDITOR

#endif
    }

    /// <summary>
    /// 本地文件操作
    /// </summary>
    public static class FileOperation
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