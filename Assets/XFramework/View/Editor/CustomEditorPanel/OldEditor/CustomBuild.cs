using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace XFramework
{
    public class CustomBuild : EditorWindow
    {
        private static CustomBuildEditorData _buildEditorData;

        // [MenuItem("XFrame/打包工具", false, 0)]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomBuild>();
            window.minSize = window.minSize;
            window.maxSize = new Vector2(600, 650);
            window.titleContent = new GUIContent() {image = null, text = "打包工具"};
            window.Show();
        }

        Vector2 _copyFolderCountScroll = Vector2.zero;

        private static FtpOperation _ftpOperation;

        private void OnEnable()
        {
            if (_buildEditorData == null)
            {
                _buildEditorData =
                    (CustomBuildEditorData) AssetDatabase.LoadAssetAtPath("Assets/XFramework/Config/CustomBuildData.asset",
                        typeof(CustomBuildEditorData));
            }
        }

        private void OnDestroy()
        {
            SaveData();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            EditorUtility.SetDirty(_buildEditorData);
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 打包方式

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("选择当前项目的打包方式:", GUILayout.MaxWidth(130));
            _buildEditorData.buildTarget = (BuildTarget) EditorGUILayout.EnumPopup(_buildEditorData.buildTarget);

            #endregion

            #region 数据存储

            if (GUILayout.Button("保存数据", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)))
            {
                //标记脏区
                EditorUtility.SetDirty(_buildEditorData);
                // 保存所有修改
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包路径

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择打包路径", GUILayout.MaxWidth(80)))
            {
                _buildEditorData.exportPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
            }

            _buildEditorData.exportPath =
                EditorGUILayout.TextField(_buildEditorData.exportPath, GUILayout.MaxWidth(520), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 项目名称

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("项目中文名称:", GUILayout.MaxWidth(70));
            _buildEditorData.exportCnProjectName = EditorGUILayout.TextField(_buildEditorData.exportCnProjectName);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("项目英文名称:", GUILayout.MaxWidth(70));
            _buildEditorData.exportEnProjectName = EditorGUILayout.TextField(_buildEditorData.exportEnProjectName);


            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            _buildEditorData.chineseShell = GUILayout.Toggle(_buildEditorData.chineseShell, "中文输出外壳", GUILayout.MaxWidth(90));

            _buildEditorData.learningModel = GUILayout.Toggle(_buildEditorData.learningModel, "学习模式", GUILayout.MaxWidth(70));

            if (_buildEditorData.learningModel)
            {
                _buildEditorData.assessmentMode = false;
            }

            _buildEditorData.assessmentMode = GUILayout.Toggle(_buildEditorData.assessmentMode, "考核模式", GUILayout.MaxWidth(70));

            if (_buildEditorData.assessmentMode)
            {
                _buildEditorData.learningModel = false;
            }

            _buildEditorData.projectNameDate = GUILayout.Toggle(_buildEditorData.projectNameDate, "日期", GUILayout.MaxWidth(50));
            _buildEditorData.watermark = GUILayout.Toggle(_buildEditorData.watermark, "水印", GUILayout.MaxWidth(50));
            _buildEditorData.versionWatermark = _buildEditorData.watermark;
            _buildEditorData.updateToFtp =
                GUILayout.Toggle(_buildEditorData.updateToFtp, "打包后是否更新到FTP服务器", GUILayout.MaxWidth(170));
            _buildEditorData.versionSet = GUILayout.Toggle(_buildEditorData.versionSet, "版本打包设置");
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 服务器与打包信息

            EditorGUILayout.BeginHorizontal();
            if (_buildEditorData.updateToFtp)
            {
                EditorGUILayout.LabelField("服务器地址:", GUILayout.MaxWidth(70));
                _buildEditorData.ftpServerPath = EditorGUILayout.TextField(_buildEditorData.ftpServerPath);
                EditorGUILayout.LabelField("用户名:", GUILayout.MaxWidth(40));
                _buildEditorData.ftpUser = EditorGUILayout.TextField(_buildEditorData.ftpUser);
                EditorGUILayout.LabelField("密码:", GUILayout.MaxWidth(30));
                _buildEditorData.ftpPwd = EditorGUILayout.TextField(_buildEditorData.ftpPwd);
                EditorGUILayout.LabelField("存放目录:", GUILayout.MaxWidth(50));
                _buildEditorData.ftpRoot = EditorGUILayout.TextField(_buildEditorData.ftpRoot);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (_buildEditorData.versionSet)
            {
                _buildEditorData.versionWatermark =
                    GUILayout.Toggle(_buildEditorData.versionWatermark, "版本水印", GUILayout.MaxWidth(70));
                _buildEditorData.watermark = _buildEditorData.versionWatermark;
                _buildEditorData.versionDownLoad =
                    GUILayout.Toggle(_buildEditorData.versionDownLoad, "版本下载", GUILayout.MaxWidth(70));
                _buildEditorData.versionLoadingProgress = GUILayout.Toggle(_buildEditorData.versionLoadingProgress, "版本下载进度",
                    GUILayout.MaxWidth(90));
                _buildEditorData.versionSceneProgress =
                    GUILayout.Toggle(_buildEditorData.versionSceneProgress, "版本场景进度", GUILayout.MaxWidth(90));
                EditorGUILayout.LabelField("版本考核时间:", GUILayout.MaxWidth(70));
                _buildEditorData.versionAssessmentTime =
                    EditorGUILayout.IntField(_buildEditorData.versionAssessmentTime, GUILayout.MaxWidth(70));
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region 动态增加拷贝文件夹

            EditorGUILayout.BeginHorizontal();
            //增加拷贝数量
            if (GUILayout.Button("增加拷贝数量", GUILayout.MaxWidth(80)))
            {
                _buildEditorData.folderCopies.Add(new FolderCopy());
            }


            EditorGUILayout.EndHorizontal();


            _copyFolderCountScroll = EditorGUILayout.BeginScrollView(_copyFolderCountScroll);
            for (int i = 0; i < _buildEditorData.folderCopies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                //选择复制路径
                if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                {
                    _buildEditorData.folderCopies[i].copyFolderPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                }

                _buildEditorData.folderCopies[i].copyFolderPath =
                    EditorGUILayout.TextField(_buildEditorData.folderCopies[i].copyFolderPath, GUILayout.MaxHeight(20));


                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                _buildEditorData.folderCopies[i].pasteFolderPath =
                    EditorGUILayout.TextField(_buildEditorData.folderCopies[i].pasteFolderPath, GUILayout.MaxHeight(20));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                //增加拷贝数量
                if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                {
                    _buildEditorData.folderCopies.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            /*if (_buildData.folderCopies.Count > 0)
            {
                
                
                if (_buildData.copyFolderCount > _buildData.copyFolderPaths.Count)
                {
                    for (int i = 0; i < _buildData.copyFolderCount - _buildData.copyFolderPaths.Count; i++)
                    {
                        _buildData.copyFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _buildData.copyFolderPaths.Count - _buildData.copyFolderCount; i++)
                    {
                        _buildData.copyFolderPaths.RemoveAt(_buildData.copyFolderPaths.Count - 1);
                    }
                }

                if (_buildData.copyFolderCount > _buildData.pasteFolderPaths.Count)
                {
                    for (int i = 0; i < _buildData.copyFolderCount - _buildData.pasteFolderPaths.Count; i++)
                    {
                        _buildData.pasteFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _buildData.pasteFolderPaths.Count - _buildData.copyFolderCount; i++)
                    {
                        _buildData.pasteFolderPaths.RemoveAt(_buildData.pasteFolderPaths.Count - 1);
                    }
                }

                _copyFolderCountScroll = EditorGUILayout.BeginScrollView(_copyFolderCountScroll);
                for (int i = 0; i < _buildData.copyFolderPaths.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    //选择复制路径
                    if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                    {
                        _buildData.copyFolderPaths[i] = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    }

                    _buildData.copyFolderPaths[i] = EditorGUILayout.TextField(_buildData.copyFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));


                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                    _buildData.pasteFolderPaths[i] = EditorGUILayout.TextField(_buildData.pasteFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    //增加拷贝数量
                    if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                    {
                        _buildData.copyFolderPaths.Remove(_buildData.copyFolderPaths[i]);
                        _buildData.pasteFolderPaths.Remove(_buildData.pasteFolderPaths[i]);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }*/

            #endregion

            #region 打包

            // EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("开始打包", GUILayout.MaxHeight(40)))
            {
                //保存打包数据
                SaveData();
                //拷贝打包文件
                for (int i = 0; i < _buildEditorData.folderCopies.Count; i++)
                {
                    if (_buildEditorData.folderCopies[i].copyFolderPath != string.Empty &&
                        _buildEditorData.folderCopies[i].pasteFolderPath != string.Empty)
                    {
                        Copy(_buildEditorData.folderCopies[i].copyFolderPath,
                            ProjectPath() + "/" + _buildEditorData.folderCopies[i].pasteFolderPath);
                    }
                }

                BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), _buildEditorData.buildTarget,
                    BuildOptions.CompressWithLz4HC);
            }

            // EditorGUILayout.EndHorizontal();

            #endregion
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
                    Copy(item, destDirName + item.Substring(item.LastIndexOf("\\", StringComparison.Ordinal) + 1));
                }
            }
        }

        /// <summary>
        /// 获得打包的场景
        /// </summary>
        /// <returns></returns>
        private string[] FindEnableEditorScenes()
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
            //打包路径
            string path = "";
            path += _buildEditorData.exportPath + "/";
            string chinsesPath = "";
            //如果启动中文外壳
            if (_buildEditorData.chineseShell)
            {
                chinsesPath += _buildEditorData.exportCnProjectName;
                if (_buildEditorData.learningModel)
                {
                    chinsesPath += "-学习模式";
                }

                if (_buildEditorData.assessmentMode)
                {
                    chinsesPath += "-考核模式";
                }

                if (_buildEditorData.projectNameDate)
                {
                    chinsesPath += "-" + DateTime.Now.ToString("yyyy.MM.dd");
                }

                if (_buildEditorData.watermark)
                {
                    chinsesPath += "-水印";
                }
                else
                {
                    chinsesPath += "-无水印";
                }

                chinsesPath += "/";
            }

            path += chinsesPath;
            path += _buildEditorData.exportEnProjectName;

            /*if (_buildData.learningModel)
            {
                path += "-Study";
            }

            if (_buildData.assessmentMode)
            {
                path += "-Assessment";
            }

            if (_buildData.projectNameDate)
            {
                path += "-" + DateTime.Now.ToString("yyyy.MM.dd");
            }

            if (_buildData.watermark)
            {
                path += "-Watermark";
            }
            else
            {
                path += "-NoWatermark";
            }*/


            return path;
        }

        /// <summary>
        /// Build完成后的回调
        /// </summary>
        /// <param name="target">打包的目标平台</param>
        /// <param name="pathToBuiltProject">包体的完整路径</param>
        [PostProcessBuild(1)]
        public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
        {
            #pragma warning disable 0162
            return;
            Debug.Log("Build Success  输出平台: " + target + "  输出路径: " + pathToBuiltProject);

            //打开文件或文件夹
            System.Diagnostics.Process.Start(pathToBuiltProject);

            int index = pathToBuiltProject.LastIndexOf("/", StringComparison.Ordinal);

            Debug.Log("导出包体的目录 :" + pathToBuiltProject.Substring(0, index));
            _ftpOperation = new FtpOperation(_buildEditorData);
#if !UNITY_EDITOR
            if (_buildData.updateToFtp)
            {
                _ftpOperation.UploadFile();
                Debug.Log("FTP文件已经上传成功...");
            }
#endif
            GetWindow<CustomBuild>().Close();
            #pragma warning disable
        }
    }

    /// <summary>
    /// FTP工具
    /// </summary>
    public class FtpOperation
    {
        private CustomBuildEditorData _customBuildEditorData;

        public FtpOperation(CustomBuildEditorData customBuildEditorData)
        {
            _customBuildEditorData = customBuildEditorData;
        }

        /// <summary>
        /// 项目路径
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string ProjectPath()
        {
            //打包路径
            string path = "";
            path += _customBuildEditorData.exportPath + "/";
            string chinsesPath = "";
            //如果启动中文外壳
            if (_customBuildEditorData.chineseShell)
            {
                chinsesPath += _customBuildEditorData.exportCnProjectName;
                if (_customBuildEditorData.learningModel)
                {
                    chinsesPath += "-学习模式";
                }

                if (_customBuildEditorData.assessmentMode)
                {
                    chinsesPath += "-考核模式";
                }

                if (_customBuildEditorData.projectNameDate)
                {
                    chinsesPath += "-" + DateTime.Now.ToString("yyyy.MM.dd");
                }

                if (_customBuildEditorData.watermark)
                {
                    chinsesPath += "-水印";
                }
                else
                {
                    chinsesPath += "-无水印";
                }

                chinsesPath += "/";
            }

            path += chinsesPath + ProjectName();

            return path;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        private string ProjectName()
        {
            string projectName = _customBuildEditorData.exportEnProjectName;
            /*
            if (_customBuildData.learningModel)
            {
                projectName += "-Study";
            }

            if (_customBuildData.assessmentMode)
            {
                projectName += "-Assessment";
            }

            if (_customBuildData.projectNameDate)
            {
                projectName += "-" + DateTime.Now.ToString("yyyy.MM.dd");
            }

            if (_customBuildData.watermark)
            {
                projectName += "-Watermark";
            }
            else
            {
                projectName += "-NoWatermark";
            }
            */

            return projectName;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public void UploadFile()
        {
            Dictionary<string, List<string>> filePathInfos = new Dictionary<string, List<string>>();
            filePathInfos = GetFiles(ProjectPath(), ProjectPath(), filePathInfos);

            foreach (KeyValuePair<string, List<string>> pair in filePathInfos)
            {
                if (pair.Key != "")
                {
                    MakeDir(_customBuildEditorData.ftpServerPath, _customBuildEditorData.ftpUser, _customBuildEditorData.ftpPwd,
                        _customBuildEditorData.ftpRoot + "/" + ProjectName() + pair.Key);
                }
                else
                {
                    MakeDir(_customBuildEditorData.ftpServerPath, _customBuildEditorData.ftpUser, _customBuildEditorData.ftpPwd,
                        _customBuildEditorData.ftpRoot);
                    MakeDir(_customBuildEditorData.ftpServerPath, _customBuildEditorData.ftpUser, _customBuildEditorData.ftpPwd,
                        _customBuildEditorData.ftpRoot + "/" + ProjectName());
                }
            }

            foreach (KeyValuePair<string, List<string>> pair in GetFiles(ProjectPath(),
                ProjectPath(), filePathInfos))
            {
                foreach (string s in pair.Value)
                {
                    WebClient client = new WebClient();
                    Uri uri = new Uri("ftp://" + _customBuildEditorData.ftpServerPath + "/" + _customBuildEditorData.ftpRoot + "/" +
                                      ProjectName() + s);
                    client.Credentials = new NetworkCredential(_customBuildEditorData.ftpUser, _customBuildEditorData.ftpPwd);
                    client.UploadFileAsync(uri, "STOR", ProjectPath() + s);
                }
            }
        }

        /// <summary>
        ///在ftp服务器上创建文件目录
        /// </summary>
        /// <param name="dirName">文件目录</param>
        /// <returns></returns>
        public bool MakeDir(string path, string user, string pwd, string dirName)
        {
            try
            {
                string uri = ("ftp://" + path + "/" + dirName + "/");
                if (DirectoryIsExist(uri, user, pwd))
                {
                    // Debug.Log("已存在");
                    return true;
                }

                string url = "ftp://" + path + "/" + dirName;
                FtpWebRequest reqFtp = (FtpWebRequest) WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                // reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.Credentials = new NetworkCredential(user, pwd);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("因{0},无法下载" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断ftp上的文件目录是否存在
        /// </summary>
        /// <returns></returns>        
        private static bool DirectoryIsExist(string uri, string user, string pwd)
        {
            string[] value = GetFileList(uri, user, pwd);
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string[] GetFileList(string uri, string user, string pwd)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest) WebRequest.Create(uri);
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(user, pwd);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = reqFtp.GetResponse();
                StreamReader reader =
                    new StreamReader(response.GetResponseStream(),Encoding.UTF8);
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, List<string>> GetFiles(string rootPath, string path,
            Dictionary<string, List<string>> fileList)
        {
            string filename;
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            List<string> newFileList = new List<string>();
            foreach (FileInfo f in fil)
            {
                filename = f.FullName.Remove(0, rootPath.Length);
                if (!fileList.ContainsKey(path.Remove(0, rootPath.Length)))
                {
                    newFileList.Add(filename);
                    fileList.Add(path.Remove(0, rootPath.Length), newFileList);
                }

                newFileList.Add(filename);
            }

            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo d in dii)
            {
                GetFiles(rootPath, d.FullName, fileList);
            }

            return fileList;
        }
    }
}