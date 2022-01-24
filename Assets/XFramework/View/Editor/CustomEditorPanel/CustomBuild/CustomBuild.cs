using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Windows;

namespace XFramework
{
#if UNITY_EDITOR

    public class CustomBuild : BaseEditor
    {
        [LabelText("当前打包方式:")] public General.BuildTargetPlatform buildTargetPlatform;
        [LabelText("压缩类型")] public BuildOptions buildCompressType;

        [LabelText("当前打包存放路径")] [FolderPath(AbsolutePath = true)]
        public string buildPackagePath;

        [LabelText("中文名称")] [Tooltip("只做输出外壳使用")]
        public string exportCnProjectName;

        [LabelText("英文名称")] public string exportEnProjectName;

        [LabelText("使用中文输出外壳")] public bool chineseShell;
        [LabelText("文件拷贝输出路径")] public List<FolderCopy> folderCopy;

        [LabelText("自定义打包数据")] private CustomBuildData _customBuildData;

        private SceneLoad _sceneLoad;

        [LabelText("开始打包")]
        [Button(ButtonSizes.Large)]
        public void StartBuild()
        {
            _sceneLoad.BuildSyncScene();
            Build();
            CopySceneFile();
            if (buildTargetPlatform == General.BuildTargetPlatform.WebGL)
            {
                CopyFile();
            }
        }

        /// <summary>
        /// 打包
        /// </summary>
        private void Build()
        {
            OnSaveConfig();
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = CustomBuildTools.FindEnableEditorScenes();
            string outPath = CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName, exportEnProjectName);
            BuildTarget buildTarget = BuildTarget.NoTarget;
            switch (buildTargetPlatform)
            {
                case General.BuildTargetPlatform.StandaloneWindows64:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    break;
                case General.BuildTargetPlatform.WebGL:
                    buildTarget = BuildTarget.WebGL;
                    break;
                case General.BuildTargetPlatform.Android:
                    buildTarget = BuildTarget.Android;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log(buildTarget);
            if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
            {
                Debug.Log("PC路径修正");
                outPath += ".exe";
            }
            else if (buildTarget == BuildTarget.Android)
            {
                Debug.Log("Android路径修正");
                outPath += ".apk";
            }

            Debug.Log(outPath);
            buildPlayerOptions.locationPathName = outPath;
            buildPlayerOptions.target = buildTarget;
            buildPlayerOptions.options = buildCompressType;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed" + outPath);
            }
        }

        private void CopySceneFile()
        {
            if (_sceneLoad.sceneAssetBundlePath == string.Empty)
            {
                Debug.LogError("拷贝场景文件路径不正确");
                return;
            }

            //场景配置文件清空
            ResSvc.DownFile sceneFile =
                JsonMapper.ToObject<ResSvc.DownFile>(Resources.Load<TextAsset>("DownFile/SceneFileInfo").text);
            //打包后场景文件地址
            string sceneLoadAssetBundlePath =
                CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName,
                    exportEnProjectName) + "/" + _sceneLoad.sceneAssetBundlePath.Replace("Assets/", "");
            if (Directory.Exists(sceneLoadAssetBundlePath))
            {
                Directory.Delete(sceneLoadAssetBundlePath);
            }
            else
            {
                // Debug.Log("地址不存在:" + sceneLoadAssetBundlePath);
            }

            foreach (ResSvc.DownFile.FileInfo fileInfo in sceneFile.fileInfoList)
            {
                string buildTargetPlatformPath = String.Empty;
                switch (buildTargetPlatform)
                {
                    case General.BuildTargetPlatform.StandaloneWindows64:
                        buildTargetPlatformPath = "";
                        break;
                    case General.BuildTargetPlatform.WebGL:
                        break;
                    case General.BuildTargetPlatform.Android:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                string copyAssetPath = "Assets/" + fileInfo.filePath.Replace("/" + fileInfo.fileName, "");
                string pastePath = CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell,
                                       exportCnProjectName, exportEnProjectName) + buildTargetPlatformPath + "/" +
                                   fileInfo.filePath.Replace("/" + fileInfo.fileName, "");

                CustomBuildFileOperation.Copy(copyAssetPath, pastePath);
            }
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        private void CopyFile()
        {
            for (int i = 0; i < folderCopy.Count; i++)
            {
                if (folderCopy[i].copyFolderPath != string.Empty && folderCopy[i].pasteFolderPath != string.Empty)
                {
                    string buildTargetPlatformPath = String.Empty;
                    switch (buildTargetPlatform)
                    {
                        case General.BuildTargetPlatform.StandaloneWindows64:
                            buildTargetPlatformPath = "";
                            break;
                        case General.BuildTargetPlatform.WebGL:
                            break;
                        case General.BuildTargetPlatform.Android:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    string pastePath = CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName,
                        exportEnProjectName) + buildTargetPlatformPath + "/" + folderCopy[i].pasteFolderPath;

                    CustomBuildFileOperation.Copy(folderCopy[i].copyFolderPath, pastePath);
                }
            }
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
        }

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _customBuildData = AssetDatabase.LoadAssetAtPath<CustomBuildData>(General.customBuildDataPath);
            if (_customBuildData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomBuildData>(),
                    General.customBuildDataPath);
                //读取数据
                _customBuildData = AssetDatabase.LoadAssetAtPath<CustomBuildData>(General
                    .customBuildDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _customBuildData.buildTargetPlatform = buildTargetPlatform;
            _customBuildData.buildCompressType = buildCompressType;
            _customBuildData.buildPackagePath = buildPackagePath;
            _customBuildData.exportCnProjectName = exportCnProjectName;
            _customBuildData.exportEnProjectName = exportEnProjectName;
            _customBuildData.chineseShell = chineseShell;
            _customBuildData.folderCopy = folderCopy;
            //标记脏区
            EditorUtility.SetDirty(_customBuildData);
        }

        public override void OnLoadConfig()
        {
            buildTargetPlatform = _customBuildData.buildTargetPlatform;
            buildCompressType = _customBuildData.buildCompressType;
            buildPackagePath = _customBuildData.buildPackagePath;
            exportCnProjectName = _customBuildData.exportCnProjectName;
            exportEnProjectName = _customBuildData.exportEnProjectName;
            chineseShell = _customBuildData.chineseShell;
            folderCopy = _customBuildData.folderCopy;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }

        public void AfferentSceneLoad(SceneLoad sceneLoad)
        {
            _sceneLoad = sceneLoad;
        }
    }
#endif
}