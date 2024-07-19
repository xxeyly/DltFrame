using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace DltFramework
{
    public static class DataFrameComponent
    {
        [LabelText("字符长度")] public static Dictionary<string, int> CharacterLengthDic = new Dictionary<string, int>()
        {
            { "A", 8 },
            { "B", 8 },
            { "C", 9 },
            { "D", 9 },
            { "E", 7 },
            { "F", 7 },
            { "G", 9 },
            { "H", 9 },
            { "I", 3 },
            { "J", 7 },
            { "K", 8 },
            { "L", 7 },
            { "M", 11 },
            { "N", 9 },
            { "O", 9 },
            { "P", 8 },
            { "Q", 9 },
            { "R", 8 },
            { "S", 8 },
            { "T", 8 },
            { "U", 9 },
            { "V", 8 },
            { "W", 11 },
            { "X", 8 },
            { "Y", 8 },
            { "Z", 8 },
            { "a", 7 },
            { "b", 7 },
            { "c", 7 },
            { "d", 7 },
            { "e", 7 },
            { "f", 4 },
            { "g", 7 },
            { "h", 7 },
            { "i", 3 },
            { "j", 3 },
            { "k", 7 },
            { "l", 3 },
            { "m", 10 },
            { "n", 7 },
            { "o", 7 },
            { "p", 7 },
            { "q", 7 },
            { "r", 4 },
            { "s", 6 },
            { "t", 4 },
            { "u", 7 },
            { "v", 7 },
            { "w", 10 },
            { "x", 6 },
            { "y", 7 },
            { "z", 7 },
            { "!", 3 },
            { "@", 11 },
            { "#", 8 },
            { "$", 8 },
            { "%", 10 },
            { "^", 6 },
            { "&", 8 },
            { "*", 6 },
            { "(", 4 },
            { ")", 4 },
            { "-", 6 },
            { "_", 5 },
            { "=", 8 },
            { "+", 8 },
            { "[", 4 },
            { "]", 4 },
            { "{", 4 },
            { "}", 4 },
            { @"\", 4 },
            { "|", 4 },
            { ";", 3 },
            { ":", 3 },
            { "'", 3 },
            { "\"", 5 },
            { ",", 3 },
            { "<", 8 },
            { ".", 3 },
            { ">", 8 },
            { "/", 4 },
            { "?", 6 },
            { " ", 3 },
            { "汉", 12 },
            { "0", 8 },
            { "1", 6 },
            { "2", 7 },
            { "3", 8 },
            { "4", 8 },
            { "5", 7 },
            { "6", 7 },
            { "7", 7 },
            { "8", 7 },
            { "9", 7 },
        };

        /// <summary>
        /// 移除所有AssetBundle名称
        /// </summary>
        public static void RemoveAllAssetBundleName()
        {
#if UNITY_EDITOR

            List<string> allAsstBundleName = new List<string>(AssetDatabase.GetAllAssetBundleNames());

            foreach (string assetName in allAsstBundleName)
            {
                AssetDatabase.RemoveAssetBundleName(assetName, true);
            }

            AssetDatabase.Refresh();
#endif
        }

        [LabelText("获得两点之间的角度360")]
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

        #region Hierarchy

        [LabelText("查找场景中所有类型")]
        public static List<T> Hierarchy_GetAllObjectsInScene<T>()
        {
            List<GameObject> objectsInScene = Hierarchy_GetAllObjectsOnlyInScene();
            List<T> specifiedType = new List<T>();
            foreach (GameObject go in objectsInScene)
            {
                List<T> ts = new List<T>(go.GetComponents<T>());
                for (int i = 0; i < ts.Count; i++)
                {
                    if (ts[i] != null)
                    {
                        specifiedType.Add(ts[i]);
                    }
                }
            }

            return specifiedType;
        }

        [LabelText("查找场景中所有类型")]
        public static List<T> Hierarchy_GetAllObjectsInScene<T>(string sceneName)
        {
            List<GameObject> objectsInScene = Hierarchy_GetAllObjectsOnlyInScene(sceneName);
            List<T> specifiedType = new List<T>();
            foreach (GameObject go in objectsInScene)
            {
                List<T> ts = new List<T>(go.GetComponents<T>());
                for (int i = 0; i < ts.Count; i++)
                {
                    if (ts[i] != null)
                    {
                        specifiedType.Add(ts[i]);
                    }
                }
            }

            return specifiedType;
        }


        [LabelText("查找场景中第一个类型")]
        public static T Hierarchy_GetObjectsInScene<T>()
        {
            GameObject objectsInScene = Hierarchy_GetObjectsOnlyInScene<T>();
            if (objectsInScene != null)
            {
                return objectsInScene.GetComponent<T>();
            }

            return default(T);
        }

        [LabelText("获得场景中所有物体")]
        public static List<GameObject> Hierarchy_GetAllObjectsOnlyInScene()
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            foreach (GameObject go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                if (go.scene.name == null)
                {
                    continue;
                }
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                    !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                {
                    objectsInScene.Add(go);
                }
#else
                objectsInScene.Add(go);
#endif
            }

            return objectsInScene;
        }

        [LabelText("获得场景中所有物体")]
        public static List<GameObject> Hierarchy_GetAllObjectsOnlyInScene(string sceneName)
        {
            List<GameObject> objectsInScene = new List<GameObject>();
            foreach (GameObject go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                    !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                {
                    if (go.scene.name == sceneName)
                    {
                        objectsInScene.Add(go);
                    }
                }
#else
                if (go.scene.name == sceneName)
                {
                    objectsInScene.Add(go);
                }
#endif
            }

            return objectsInScene;
        }

        [LabelText("获得场景中所有第一个物体")]
        private static GameObject Hierarchy_GetObjectsOnlyInScene<T>()
        {
            foreach (GameObject go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
#if UNITY_EDITOR
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                    !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
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

        [LabelText("获得物体所在跟目录层级")]
        public static int Hierarchy_GetObjWhereRootLevel(Transform target)
        {
            int level = 0;
            while (target.parent != null)
            {
                target = target.parent;
                level += 1;
            }

            return level;
        }

        [LabelText("计算Hierarchy内容长度")]
        public static float Hierarchy_CalculationHierarchyContentLength(string hierarchyContent)
        {
            int length = 0;

            for (int i = 0; i < hierarchyContent.Length; i++)
            {
                if (CharacterLengthDic.ContainsKey(hierarchyContent[i].ToString()))
                {
                    length += CharacterLengthDic[hierarchyContent[i].ToString()];
                }
                else
                {
                    if (String_CheckStringIsChinese(hierarchyContent[i].ToString()))
                    {
                        length += CharacterLengthDic["汉"];
                    }
                }
            }

            return length;
        }

        [LabelText("获取面板上的值")]
        public static Vector3 Hierarchy_GetInspectorEuler(Transform mTransform, bool world = true)
        {
            Vector3 angle;
            if (world)
            {
                angle = mTransform.eulerAngles;
            }
            else
            {
                angle = mTransform.localEulerAngles;
            }

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


        [LabelText("获得路径")]
        public static string Hierarchy_GetTransformHierarchy(Transform target, bool containThis = true)
        {
            string path = String.Empty;
            Transform defaultUiTr = target;
            int hierarchy = 0;
            while (target.parent != null)
            {
                hierarchy++;
                target = target.parent;
            }


            for (int i = 1; i <= hierarchy; i++)
            {
                target = Hierarchy_GetParentHierarchy(defaultUiTr, i);
                if (containThis)
                {
                    path = String_BuilderString(target.name, "/", path);
                }
                else
                {
                    if (i == 1)
                    {
                        path = target.name;
                    }
                    else
                    {
                        path = String_BuilderString(target.name, "/", path);
                    }
                }
            }

            if (containThis)
            {
                return String_BuilderString(path, defaultUiTr.name);
            }
            else
            {
                return path;
            }
        }

        [LabelText("根据UI层级获得父物体")]
        private static Transform Hierarchy_GetParentHierarchy(Transform uiTr, int hierarchy)
        {
            for (int i = 0; i < hierarchy; i++)
            {
                uiTr = uiTr.parent;
            }

            return uiTr;
        }

        #endregion

        #region 字符串

        [LabelText("首字母大写")]
        public static string String_FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            return String_BuilderString(input.First().ToString().ToUpper(), input.Substring(1));
        }

        [LabelText("首字母小写")]
        public static string String_FirstCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            return String_BuilderString(input.First().ToString().ToLower() + input.Substring(1));
        }

        [LabelText("所有转换为小写")]
        public static string String_AllCharToLower(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = "";
            foreach (char c in input)
            {
                str = String_BuilderString(str, c.ToString().ToLower());
            }

            return str;
        }

        [LabelText("StringBuilder字符串拼接")]
        public static string String_BuilderString(params string[] strList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strList)
            {
                sb.Append(str);
            }

            return sb.ToString();
        }

        [LabelText("左斜杠转右斜杠")]
        public static string String_LeftSlashChangeRightSlash(string path)
        {
            return path.Replace("/", "\\");
        }

        [LabelText("右斜杠转左斜杠")]
        public static string String_RightSlashChangeLeftSlash(string path)
        {
            return path.Replace("/", "\\");
        }

        [LabelText("检查String是否是汉字")]
        public static bool String_CheckStringIsChinese(string str)
        {
            char[] ch = str.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (String_CharisChinese(ch[i]))
                {
                    return true;
                }
            }

            return false;
        }

        [LabelText("检查Char是否是汉字")]
        public static bool String_CharisChinese(char c)
        {
            return c >= 0x4E00 && c <= 0x9FA5;
        }

        [LabelText("文字换行")]
        public static string String_ContentAddLine(string content, int lineFeedCount)
        {
            int lineCount = 0;
            string newContent = string.Empty;
            for (int i = 0; i < content.Length; i++)
            {
                newContent += content[i];
                int temp = (lineCount + 1) * lineFeedCount;
                if (i >= lineFeedCount - 1 && (i + 1 - temp) == 0)
                {
                    lineCount += 1;
                    newContent += "\n";
                }
            }

            return newContent;
        }

        #endregion

        #region 集合

        [LabelText("随机排序")]
        public static List<T> List_RandomSort<T>(List<T> list)
        {
            var random = new Random();
            T time;
            int index;
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

        [LabelText("集合合并")]
        public static List<T> List_Merge<T>(params List<T>[] needMergeList)
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

        [LabelText("获得继承类的所有子类")]
        public static List<T> List_GetInheritAllSubclass<T>() where T : class
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
                        if (type.FullName != null)
                        {
                            Type objType = Type.GetType(type.FullName, true);
                            object obj = Activator.CreateInstance(objType);
                            if (obj != null)
                            {
                                T info = obj as T;
                                cList.Add(info);
                            }
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

        [LabelText("集合删除重复项")]
        public static List<T> List_RemoveRepeat<T>(List<T> currentList, List<T> targetList)
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

        [LabelText("值克隆")]
        public static List<T> List_DataValueClone<T>(List<T> targetValue)
        {
            List<T> temp = new List<T>();
            foreach (T t in targetValue)
            {
                temp.Add(t);
            }

            return temp;
        }

        #endregion

        #region 图片

        [LabelText("图片转byte数组")]
        public static byte[] ImageToByte(Image img)
        {
            return img.sprite.texture.EncodeToPNG();
        }

        [LabelText("数据转精灵")]
        public static Sprite ByteToSprite(byte[] imgByte, int spriteWidth, int spriteHeight)
        {
            Texture2D texture2D = new Texture2D(spriteWidth, spriteHeight);
            texture2D.LoadImage(imgByte);
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f));
        }

        #endregion

        #region 路径

        [LabelText("获得所有文件的路径(.meta文件除外)")]
        public static Dictionary<string, List<string>> Path_GetAllObjectsOnlyInAssets()
        {
            Dictionary<string, List<string>> assetsTypePathDic = new Dictionary<string, List<string>>();
            DirectoryInfo direction = new DirectoryInfo("Assets");
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                //后缀名
                string extension = files[i].Extension.Replace(".", "");
                //绝对路径
                string absolutePath = files[i].FullName;
                //相对路径
                string relativePath = absolutePath.Substring(absolutePath.IndexOf("Assets", StringComparison.Ordinal));
                if (!assetsTypePathDic.ContainsKey(extension))
                {
                    assetsTypePathDic.Add(extension, new List<string>() { relativePath });
                }
                else
                {
                    assetsTypePathDic[extension].Add(relativePath);
                }
            }

            return assetsTypePathDic;
        }

        [LabelText("返回所有类路径")]
        public static List<string> Path_GetAllScriptsPathOnlyInAssets()
        {
            Dictionary<string, List<string>> allObject = Path_GetAllObjectsOnlyInAssets();
            if (allObject.ContainsKey("cs"))
            {
                return allObject["cs"];
            }

            return null;
        }

        [LabelText("返回所有类名称")]
        public static List<string> Path_GetAllScriptsNameOnlyInAssets()
        {
            List<string> scriptsPath = Path_GetAllScriptsPathOnlyInAssets();
            List<string> scriptName = new List<string>();
            if (scriptsPath != null)
            {
                foreach (string scriptPath in scriptsPath)
                {
                    List<string> scriptPathSpice = new List<string>(scriptPath.Split('\\'));
                    scriptName.Add(scriptPathSpice[scriptPathSpice.Count - 1]);
                }
            }

            return scriptName;
        }

        [LabelText("获得指定类型文件路径")]
        public static List<string> Path_GetSpecifyTypeOnlyInAssets(string fileExtension)
        {
            if (Path_GetAllObjectsOnlyInAssets().ContainsKey(fileExtension))
            {
                return Path_GetAllObjectsOnlyInAssets()[fileExtension];
            }

            return new List<string>();
        }

        [LabelText("获得指定路径下指定类型的所有物体路径")]
        public static List<string> Path_GetGetSpecifyPathInAllType(string path, string type)
        {
            List<string> allPath = new List<string>();
            foreach (KeyValuePair<string, List<string>> pair in Path_GetAllObjectsOnlyInAssets())
            {
                if (pair.Key == type)
                {
                    foreach (string filePath in pair.Value)
                    {
                        if (String_RightSlashChangeLeftSlash(path).Contains(Path_GetPathDontContainFileName(filePath)))
                        {
                            allPath.Add(filePath);
                        }
                    }
                }
            }

            return allPath;
        }

        [LabelText("获得指定类型文件")]
        public static List<T> Path_GetSpecifyTypeOnlyInAssets<T>(List<string> filePath) where T : Object
        {
            List<T> specifyType = new List<T>();
#if UNITY_EDITOR

            foreach (string path in filePath)
            {
                specifyType.Add(AssetDatabase.LoadAssetAtPath<T>(path));
            }
#endif

            return specifyType;
        }

        [LabelText("获得文件类型")]
        public static string Path_GetPathFileType(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Extension.Replace(".", "");
        }

        [LabelText("获得文件名称")]
        public static string Path_GetPathFileName(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.Name;
        }

        [LabelText("回文件名称,不包含类型")]
        public static string Path_GetPathFileNameDontContainFileType(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        [LabelText("获得文件路径,不包包含文件名称")]
        public static string Path_GetPathDontContainFileName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        [LabelText("获得上级目录")]
        public static string Path_GetParentDirectory(string path, int hierarchy)
        {
            string newPath = path;
            for (int i = 0; i < hierarchy; i++)
            {
                newPath = Path.GetDirectoryName(newPath);
            }

            return newPath;
        }

        [LabelText("获得设备存储路径")]
        public static string Path_DeviceStorage(bool unityWebRequestPath = false)
        {
            string path = String.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    path = Application.dataPath + "/UnStreamingAssets";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    path = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerARM:
                    path = Application.persistentDataPath;
                    break;
                case RuntimePlatform.Android:
                    if (unityWebRequestPath)
                    {
                        path = "file://" + Application.persistentDataPath;
                    }
                    else
                    {
                        path = Application.persistentDataPath;
                    }

                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.persistentDataPath;
                    break;
            }

            return path;
        }

        #endregion


#if UNITY_EDITOR

#endif
    }
}