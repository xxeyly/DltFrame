using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Editor
{
    [Serializable]
    public class BuildData
    {
        /// <summary>
        /// 平台
        /// </summary>
        public BuildTarget buildTarget;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        public string exportPath;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string exportProjectName;

        /// <summary>
        /// 项目日期
        /// </summary>
        public bool projectNameDate;

        /// <summary>
        /// 离线打包
        /// </summary>
        public bool projectOffline;

        /// <summary>
        /// 项目类型
        /// </summary>
        public bool ProjectEdition;

        /// <summary>
        /// 项目类型
        /// </summary>
        public CustomBuild.EProjectEdition projectEdition;

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        public List<string> copyFolderPaths = new List<string>();

        /// <summary>
        ///粘贴文件夹
        /// </summary>
        public List<string> pasteFolderPaths = new List<string>();

        /// <summary>
        /// 拷贝文件夹数量
        /// </summary>
        public int copyFolderCount;
    }

    public class CustomBuild : EditorWindow
    {
        public enum EProjectEdition
        {
            Study,
            Assessment
        }

        [MenuItem("xxslit/打包工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomBuild>();
            window.minSize = new Vector2(500, 650);
            window.maxSize = new Vector2(500, 650);
            window.titleContent = new GUIContent() {image = null, text = "打包工具"};
            window.Show();
        }

        private void InitData()
        {
            if (File.Exists(Application.dataPath + "/Resources/BuildData.json"))
            {
                BuildData buildData = JsonUtility.FromJson<BuildData>(FileOperation.GetTextToLoad(Application.dataPath + "/Resources", "BuildData.json"));
                buildTarget = buildData.buildTarget;
                exportPath = buildData.exportPath;
                exportProjectName = buildData.exportProjectName;
                ProjectNameDate = buildData.projectNameDate;
                ProjectOffline = buildData.projectOffline;
                ProjectEdition = buildData.ProjectEdition;
                copyFolderPaths = buildData.copyFolderPaths;
                pasteFolderPaths = buildData.pasteFolderPaths;
                copyFolderCount = buildData.copyFolderCount;
                projectEdition = buildData.projectEdition;
            }
        }

        /// <summary>
        /// 平台
        /// </summary>
        private BuildTarget buildTarget = BuildTarget.StandaloneWindows;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        private string exportPath;

        /// <summary>
        /// 项目名称
        /// </summary>
        private string exportProjectName;

        /// <summary>
        /// 项目日期
        /// </summary>
        private bool ProjectNameDate;

        /// <summary>
        /// 离线打包
        /// </summary>
        private bool ProjectOffline;

        /// <summary>
        /// 项目类型
        /// </summary>
        private bool ProjectEdition;

        /// <summary>
        /// 项目类型
        /// </summary>
        public EProjectEdition projectEdition;

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        private List<string> copyFolderPaths = new List<string>();

        /// <summary>
        ///粘贴文件夹
        /// </summary>
        private List<string> pasteFolderPaths = new List<string>();

        /// <summary>
        /// 拷贝文件夹数量
        /// </summary>
        private int copyFolderCount;

        private bool initData;
        Vector2 copyFolderCountScroll = Vector2.zero;

        public void OnGUI()
        {
            if (!initData)
            {
                InitData();
                initData = true;
            }

            #region 打包方式

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("选择当前项目的打包方式:", GUILayout.MaxWidth(130));
            buildTarget = (BuildTarget) EditorGUILayout.EnumPopup(this.buildTarget, GUILayout.MaxWidth(150));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 项目类型

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("选择当前项目类型:", GUILayout.MaxWidth(130));
            projectEdition = (EProjectEdition) EditorGUILayout.EnumPopup(this.projectEdition, GUILayout.MaxWidth(150));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 项目名称

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("项目名称:", GUILayout.MaxWidth(60));
            exportProjectName = EditorGUILayout.TextField(exportProjectName);
            EditorGUILayout.LabelField("日期", GUILayout.MaxWidth(25));
            ProjectNameDate = EditorGUILayout.Toggle("", ProjectNameDate, GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("WebPlayer离线", GUILayout.MaxWidth(90));
            ProjectOffline = EditorGUILayout.Toggle("", ProjectOffline, GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("类型", GUILayout.MaxWidth(25));
            ProjectEdition = EditorGUILayout.Toggle("", ProjectEdition, GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包路径

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择打包路径", GUILayout.MaxWidth(80)))
            {
                this.exportPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
            }

            this.exportPath = EditorGUILayout.TextField(this.exportPath, GUILayout.MaxWidth(420), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 动态增加拷贝文件夹

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("拷贝文件数量", GUILayout.MaxWidth(80));
            copyFolderCount = EditorGUILayout.IntField(copyFolderCount, GUILayout.MaxWidth(50));
            //增加拷贝数量
            if (GUILayout.Button("增加拷贝数量", GUILayout.MaxWidth(80)))
            {
                copyFolderCount += 1;
            } //自动编排打包配置信息

            if (GUILayout.Button("自动编排打包配置信息", GUILayout.MaxWidth(120)))
            {
                //下载文件更新
                DirectoryInfo downFileDir = new DirectoryInfo(Application.dataPath + "/XxSlitFrame/AssetBundles");
                FileInfo[] downFileInfos = downFileDir.GetFiles();
                ResSvc.DownFile downFile = new ResSvc.DownFile {fileInfoList = new List<ResSvc.DownFile.FileInfo>()};
                for (int i = 0; i < downFileInfos.Length; i++)
                {
                    if (!downFileInfos[i].Name.Contains(".meta") && !downFileInfos[i].Name.Contains(".manifest"))
                    {
                        downFile.fileInfoList.Add(new ResSvc.DownFile.FileInfo()
                        {
                            fileName = downFileInfos[i].Name,
                            filePath = "AssetBundles/" + downFileInfos[i].Name
                        });
                    }
                }

                ResSvc.FileOperation.SaveTextToLoad(Application.dataPath + "/XxSlitFrame/Resources/DownFile", "DownFileInfo.json", JsonUtility.ToJson(downFile));
            }

            EditorGUILayout.EndHorizontal();
            if (copyFolderCount > 0)
            {
                if (copyFolderCount > copyFolderPaths.Count)
                {
                    for (int i = 0; i < copyFolderCount - copyFolderPaths.Count; i++)
                    {
                        copyFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < copyFolderPaths.Count - copyFolderCount; i++)
                    {
                        copyFolderPaths.RemoveAt(copyFolderPaths.Count - 1);
                    }
                }

                if (copyFolderCount > pasteFolderPaths.Count)
                {
                    for (int i = 0; i < copyFolderCount - pasteFolderPaths.Count; i++)
                    {
                        pasteFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < pasteFolderPaths.Count - copyFolderCount; i++)
                    {
                        pasteFolderPaths.RemoveAt(pasteFolderPaths.Count - 1);
                    }
                }

                copyFolderCountScroll = EditorGUILayout.BeginScrollView(copyFolderCountScroll);
                for (int i = 0; i < copyFolderPaths.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    //选择复制路径
                    if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                    {
                        copyFolderPaths[i] = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    }

                    this.copyFolderPaths[i] = EditorGUILayout.TextField(this.copyFolderPaths[i], GUILayout.MaxWidth(350), GUILayout.MaxHeight(20));


                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                    this.pasteFolderPaths[i] = EditorGUILayout.TextField(this.pasteFolderPaths[i], GUILayout.MaxWidth(350), GUILayout.MaxHeight(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    //增加拷贝数量
                    if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                    {
                        copyFolderPaths.Remove(copyFolderPaths[i]);
                        pasteFolderPaths.Remove(pasteFolderPaths[i]);
                        copyFolderCount -= 1;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }

            #endregion

            #region 打包

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("保存打包数据", GUILayout.MaxWidth(450), GUILayout.MaxHeight(40)))
            {
                SaveBuildData();
            }

            //选择打包路径
            if (GUILayout.Button("开始打包", GUILayout.MaxWidth(450), GUILayout.MaxHeight(40)))
            {
                for (int i = 0; i < copyFolderCount; i++)
                {
                    if (copyFolderPaths[i] != string.Empty && pasteFolderPaths[i] != string.Empty)
                    {
                        Copy(copyFolderPaths[i], ProjectPath() + "/" + pasteFolderPaths[i]);
                    }
                }

                BuildData buildData = new BuildData
                {
                    buildTarget = buildTarget,
                    exportPath = exportPath,
                    projectEdition = projectEdition,
                    copyFolderCount = copyFolderCount,
                    copyFolderPaths = copyFolderPaths,
                    exportProjectName = exportProjectName,
                    pasteFolderPaths = pasteFolderPaths,
                    projectNameDate = ProjectNameDate,
                    projectOffline = ProjectOffline,
                    ProjectEdition = ProjectEdition,
                };
                SaveBuildData();

#if UNITY_5
            if (buildTarget == BuildTarget.WebPlayer)
            {
                if (ProjectOffline)
                {
                    BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), buildTarget, BuildOptions.WebPlayerOfflineDeployment);
                }
                else
                {
                    BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), buildTarget, BuildOptions.None);
                }
            }
#endif

#if UNITY_2017_1_OR_NEWER
                BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), buildTarget, BuildOptions.CompressWithLz4HC);
#endif
            }


            EditorGUILayout.EndHorizontal();

            #endregion
        }

        private void SaveBuildData()
        {
            BuildData buildData = new BuildData
            {
                buildTarget = buildTarget,
                exportPath = exportPath,
                projectEdition = projectEdition,
                copyFolderCount = copyFolderCount,
                copyFolderPaths = copyFolderPaths,
                exportProjectName = exportProjectName,
                pasteFolderPaths = pasteFolderPaths,
                projectNameDate = ProjectNameDate,
                projectOffline = ProjectOffline,
                ProjectEdition = ProjectEdition,
            };
            FileOperation.SaveTextToLoad(Application.dataPath + "/Resources", "BuildData.json", JsonUtility.ToJson(buildData));
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        private static void Copy(string sourceDirName, string destDirName)
        {
            if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
            {
                sourceDirName = sourceDirName + "\\";
            }

            if (destDirName.Substring(destDirName.Length - 1) != "\\")
            {
                destDirName = destDirName + "\\";
            }

            if (Directory.Exists(sourceDirName))
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                foreach (string item in Directory.GetFiles(sourceDirName))
                {
                    if (item.Contains("meta"))
                    {
                        continue;
                    }

                    File.Copy(item, destDirName + Path.GetFileName(item), true);
                }

                foreach (string item in Directory.GetDirectories(sourceDirName))
                {
                    Copy(item, destDirName + item.Substring(item.LastIndexOf("\\") + 1));
                }
            }
        }

        /// <summary>
        /// 获得打包的场景
        /// </summary>
        /// <returns></returns>
        public string[] FindEnableEditorScenes()
        {
            List<string> editorScenes = new List<string>();
            foreach (EditorBuildSettingsScene editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                if (editorBuildSettingsScene.enabled)
                {
                    editorScenes.Add(editorBuildSettingsScene.path);
                }
            }

            return editorScenes.ToArray();
        }

        /// <summary>
        /// 项目路径
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string ProjectPath()
        {
            string path = exportPath + "/" + exportProjectName;
            if (ProjectEdition)
            {
                switch (projectEdition)
                {
                    case EProjectEdition.Study:
                        path += "学习版";
                        break;
                    case EProjectEdition.Assessment:
                        path += "考核版";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (ProjectNameDate)
            {
                path += "--" + DateTime.Now.ToString("yy.MM.dd");
            }

            return path;
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
//            UnityEngine.Debug.Log(Path + "/" + FileName);

            if (Directory.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(information);
            sw.Close();
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
//            UnityEngine.Debug.Log(Path + "/" + FileName);
            if (Directory.Exists(path))
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
    }
}