using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomBuild : EditorWindow
    {
        public CustomBuildData buildData;

        [MenuItem("xxslit/打包工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomBuild>();
            window.minSize = window.minSize;
            window.maxSize = new Vector2(600, 650);
            window.titleContent = new GUIContent() {image = null, text = "打包工具"};
            window.Show();
        }

        private void InitData()
        {
            if (buildData == null)
            {
                buildData = (CustomBuildData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/CustomBuildData.asset", typeof(CustomBuildData));
            }
            _buildTarget = buildData.buildTarget;
            _exportPath = buildData.exportPath;
            _exportProjectName = buildData.exportProjectName;
            _enableProjectNameDate = buildData.projectNameDate;
            _copyFolderPaths = buildData.copyFolderPaths;
            _pasteFolderPaths = buildData.pasteFolderPaths;
            _copyFolderCount = buildData.copyFolderCount;
            _updateToFtp = buildData.updateToFtp;
            _ftpServerPath = buildData.ftpServerPath;
            _ftpUser = buildData.ftpUser;
            _ftpPwd = buildData.ftpPwd;
            _ftpRoot = buildData.ftpRoot;
        }

        /// <summary>
        /// 平台
        /// </summary>
        private BuildTarget _buildTarget = BuildTarget.StandaloneWindows;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        private static string _exportPath;

        /// <summary>
        /// 项目名称
        /// </summary>
        private static string _exportProjectName;

        /// <summary>
        /// 项目日期
        /// </summary>
        private bool _enableProjectNameDate;

        /// <summary>
        /// 离线打包
        /// </summary>
        private bool _enableProjectOffline;

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        private List<string> _copyFolderPaths = new List<string>();

        /// <summary>
        ///粘贴文件夹
        /// </summary>
        private List<string> _pasteFolderPaths = new List<string>();

        /// <summary>
        /// 拷贝文件夹数量
        /// </summary>
        private int _copyFolderCount;

        private bool _initData;

        /// <summary>
        /// 服务器地址
        /// </summary>
        private static string _ftpServerPath;

        /// <summary>
        /// Ftp用户名
        /// </summary>
        private static string _ftpUser;

        /// <summary>
        /// Ftp用户名
        /// </summary>
        private static string _ftpPwd;

        /// <summary>
        /// Ftp根目录
        /// </summary>
        private static string _ftpRoot;

        Vector2 _copyFolderCountScroll = Vector2.zero;

        /// <summary>
        /// 更新到服务器
        /// </summary>
        private static bool _updateToFtp;


        private static FtpOperation _ftpOperation;

        public void OnGUI()
        {
            if (!_initData)
            {
                InitData();
                _initData = true;
            }

            #region 打包方式

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("选择当前项目的打包方式:", GUILayout.MaxWidth(130));
            _buildTarget = (BuildTarget) EditorGUILayout.EnumPopup(this._buildTarget, GUILayout.MaxWidth(150));

            #endregion

            #region 数据存储

            EditorGUILayout.LabelField("打包数据:", GUILayout.MaxWidth(60));
#pragma warning disable 618
            buildData = (CustomBuildData) EditorGUILayout.ObjectField(buildData, typeof(CustomBuildData), GUILayout.MaxWidth(150));
#pragma warning restore 618

            if (GUILayout.Button("加载打包数据", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)))
            {
                InitData();
            }
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 项目名称

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("项目名称:", GUILayout.MaxWidth(60));
            _exportProjectName = EditorGUILayout.TextField(_exportProjectName);
            EditorGUILayout.LabelField("日期", GUILayout.MaxWidth(25));
            _enableProjectNameDate = EditorGUILayout.Toggle("", _enableProjectNameDate, GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("WebPlayer离线", GUILayout.MaxWidth(90));
            _enableProjectOffline = EditorGUILayout.Toggle("", _enableProjectOffline, GUILayout.MaxWidth(10));
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包路径

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择打包路径", GUILayout.MaxWidth(80)))
            {
                _exportPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
            }

            _exportPath = EditorGUILayout.TextField(_exportPath, GUILayout.MaxWidth(520), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 动态增加拷贝文件夹

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("拷贝文件数量", GUILayout.MaxWidth(80));
            _copyFolderCount = EditorGUILayout.IntField(_copyFolderCount, GUILayout.MaxWidth(50));
            //增加拷贝数量
            if (GUILayout.Button("增加拷贝数量", GUILayout.MaxWidth(80)))
            {
                _copyFolderCount += 1;
            }

            _updateToFtp = GUILayout.Toggle(_updateToFtp, "打包后是否更新到FTP服务器");

            EditorGUILayout.EndHorizontal();

            #region 服务器信息

            EditorGUILayout.BeginHorizontal();
            if (_updateToFtp)
            {
                EditorGUILayout.LabelField("服务器地址:", GUILayout.MaxWidth(70));
                _ftpServerPath = EditorGUILayout.TextField(_ftpServerPath);
                EditorGUILayout.LabelField("用户名:", GUILayout.MaxWidth(40));
                _ftpUser = EditorGUILayout.TextField(_ftpUser);
                EditorGUILayout.LabelField("密码:", GUILayout.MaxWidth(30));
                _ftpPwd = EditorGUILayout.TextField(_ftpPwd);
                EditorGUILayout.LabelField("存放目录:", GUILayout.MaxWidth(50));
                _ftpRoot = EditorGUILayout.TextField(_ftpRoot);
            }

            EditorGUILayout.EndHorizontal();

            #endregion


            if (_copyFolderCount > 0)
            {
                if (_copyFolderCount > _copyFolderPaths.Count)
                {
                    for (int i = 0; i < _copyFolderCount - _copyFolderPaths.Count; i++)
                    {
                        _copyFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _copyFolderPaths.Count - _copyFolderCount; i++)
                    {
                        _copyFolderPaths.RemoveAt(_copyFolderPaths.Count - 1);
                    }
                }

                if (_copyFolderCount > _pasteFolderPaths.Count)
                {
                    for (int i = 0; i < _copyFolderCount - _pasteFolderPaths.Count; i++)
                    {
                        _pasteFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _pasteFolderPaths.Count - _copyFolderCount; i++)
                    {
                        _pasteFolderPaths.RemoveAt(_pasteFolderPaths.Count - 1);
                    }
                }

                _copyFolderCountScroll = EditorGUILayout.BeginScrollView(_copyFolderCountScroll);
                for (int i = 0; i < _copyFolderPaths.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    //选择复制路径
                    if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                    {
                        _copyFolderPaths[i] = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    }

                    this._copyFolderPaths[i] = EditorGUILayout.TextField(this._copyFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));


                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                    this._pasteFolderPaths[i] = EditorGUILayout.TextField(this._pasteFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    //增加拷贝数量
                    if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                    {
                        _copyFolderPaths.Remove(_copyFolderPaths[i]);
                        _pasteFolderPaths.Remove(_pasteFolderPaths[i]);
                        _copyFolderCount -= 1;
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
                SaveBuildData();
                for (int i = 0; i < _copyFolderCount; i++)
                {
                    if (_copyFolderPaths[i] != string.Empty && _pasteFolderPaths[i] != string.Empty)
                    {
                        Copy(_copyFolderPaths[i], ProjectPath() + "/" + _pasteFolderPaths[i]);
                    }
                }

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
                BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), _buildTarget, BuildOptions.CompressWithLz4HC);
                InitData();
#endif
            }


            EditorGUILayout.EndHorizontal();

            #endregion
        }

        private void OnDestroy()
        {
            //保存打包数据
            SaveBuildData();
            InitData();
        }

        private void SaveBuildData()
        {
            buildData.buildTarget = _buildTarget;
            if (_exportPath != string.Empty)
            {
                buildData.exportPath = _exportPath;
            }

            if (_exportProjectName != String.Empty)
            {
                buildData.exportProjectName = _exportProjectName;
            }

            buildData.projectNameDate = _enableProjectNameDate;
            buildData.copyFolderPaths = _copyFolderPaths;
            buildData.pasteFolderPaths = _pasteFolderPaths;
            buildData.copyFolderCount = _copyFolderCount;
            buildData.updateToFtp = _updateToFtp;
            buildData.ftpServerPath = _ftpServerPath;
            buildData.ftpUser = _ftpUser;
            buildData.ftpPwd = _ftpPwd;
            buildData.ftpRoot = _ftpRoot;
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
            string path = _exportPath + "/" + _exportProjectName;
            if (_enableProjectNameDate)
            {
                path += "--" + DateTime.Now.ToString("yy.MM.dd");
            }

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
            Debug.Log("Build Success  输出平台: " + target + "  输出路径: " + pathToBuiltProject);

            //打开文件或文件夹
            System.Diagnostics.Process.Start(pathToBuiltProject);

            int index = pathToBuiltProject.LastIndexOf("/", StringComparison.Ordinal);

            Debug.Log("导出包体的目录 :" + pathToBuiltProject.Substring(0, index));
            _ftpOperation = new FtpOperation
            {
                FilePath = _exportPath + "/" + _exportProjectName, FtpHost = "ftp://" + _ftpServerPath, FtpUserName = _ftpUser, FtpPassword = _ftpPwd,
                ProjectName = _exportProjectName,
                FileSavePath = _ftpRoot
            };
            if (_updateToFtp)
            {
                _ftpOperation.UploadFile();
                Debug.Log("FTP文件已经上传成功...");
            }
        }
    }

    /// <summary>
    /// FTP工具
    /// </summary>
    public class FtpOperation
    {
        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpHost;

        /// <summary>
        /// FTP登录名
        /// </summary>
        public string FtpUserName;

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FtpPassword;

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName;

        /// <summary>
        /// 项目存储路径
        /// </summary>
        public string FileSavePath;

        /// <summary>
        /// 上传文件
        /// </summary>
        public void UploadFile()
        {
            Dictionary<string, List<string>> filePathInfos = new Dictionary<string, List<string>>();
            filePathInfos = GetFiles(FilePath, FilePath, filePathInfos);

            foreach (KeyValuePair<string, List<string>> pair in filePathInfos)
            {
                if (pair.Key != "")
                {
                    MakeDir(FtpHost, FtpUserName, FtpPassword, FileSavePath + "/" + ProjectName + pair.Key);
                }
                else
                {
                    MakeDir(FtpHost, FtpUserName, FtpPassword, FileSavePath);
                    MakeDir(FtpHost, FtpUserName, FtpPassword, FileSavePath + "/" + ProjectName);
                }
            }

            foreach (KeyValuePair<string, List<string>> pair in GetFiles(FilePath, FilePath, filePathInfos))
            {
                foreach (string s in pair.Value)
                {
                    WebClient client = new WebClient();

                    Uri uri = new Uri(FtpHost + "/" + FileSavePath + "/" + ProjectName + s);
                    client.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                    client.UploadFileAsync(uri, "STOR", FilePath + s);
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
                string uri = (path + "/" + dirName + "/");
                if (DirectoryIsExist(uri, user, pwd))
                {
                    Debug.Log("已存在");
                    return true;
                }

                string url = path + "/" + dirName;
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
                StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
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

        private Dictionary<string, List<string>> GetFiles(string rootPath, string path, Dictionary<string, List<string>> fileList)
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