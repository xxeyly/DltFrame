using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
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
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Insert(random.Next(newList.Count), item);
            }

            return newList;
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
        /// 获得场景中所有物体
        /// </summary>
        /// <returns></returns>
        private static List<GameObject> GetAllObjectsOnlyInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
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
#if UNITY_EDITOR

        /// <summary>
        /// 获得继承类的所有子类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetInheritAllSubclass<T>() where T : class
        {
            var types = Assembly.GetCallingAssembly().GetTypes();
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
#endif
    }
}