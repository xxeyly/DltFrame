using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using CustomBuildData = XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject.CustomBuildData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomBuild
{
#if UNITY_EDITOR

    public class OdinCustomBuild : BaseEditor
    {
        [LabelText("当前打包方式:")] public BuildTarget buildTarget;
        [LabelText("压缩类型")] public BuildOptions buildCompressType;

        [LabelText("当前打包存放路径")] [FolderPath(AbsolutePath = true)]
        public string buildPackagePath;

        [LabelText("中文名称")] [Tooltip("只做输出外壳使用")]
        public string exportCnProjectName;

        [LabelText("英文名称")] public string exportEnProjectName;

        [LabelText("使用中文输出外壳")] public bool chineseShell;
        [LabelText("文件拷贝输出路径")] public List<FolderCopy> folderCopy;

        [LabelText("自定义打包数据")] private CustomBuildData _customBuildData;

        [LabelText("开始打包")]
        [Button(ButtonSizes.Large)]
        public void StartBuild()
        {
            CopyFile();
            Build();
        }

        public OdinCustomBuild()
        {
            OnCreateConfig();
            OnLoadConfig();
        }

        /// <summary>
        /// 打包
        /// </summary>
        private void Build()
        {
            OnSaveConfig();
            BuildPipeline.BuildPlayer(CustomBuildTools.FindEnableEditorScenes(),
                CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName,
                    exportEnProjectName), buildTarget,
                buildCompressType);
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
                    CustomBuildFileOperation.Copy(folderCopy[i].copyFolderPath,
                        CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName,
                            exportEnProjectName) + "/" + folderCopy[i].pasteFolderPath);
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
            Debug.Log("打包成功....");
            Debug.Log("Build Success  输出平台: " + target + "  输出路径: " + pathToBuiltProject);
            //打开文件或文件夹
            System.Diagnostics.Process.Start(pathToBuiltProject);
            int index = pathToBuiltProject.LastIndexOf("/", StringComparison.Ordinal);
            Debug.Log("导出包体的目录 :" + pathToBuiltProject.Substring(0, index));
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
            _customBuildData.buildTarget = buildTarget;
            _customBuildData.buildCompressType = buildCompressType;
            _customBuildData.buildPackagePath = buildPackagePath;
            _customBuildData.exportCnProjectName = exportCnProjectName;
            _customBuildData.exportEnProjectName = exportEnProjectName;
            _customBuildData.chineseShell = chineseShell;
            _customBuildData.folderCopy = folderCopy;
            //标记脏区
            EditorUtility.SetDirty(_customBuildData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            buildTarget = _customBuildData.buildTarget;
            buildCompressType = _customBuildData.buildCompressType;
            buildPackagePath = _customBuildData.buildPackagePath;
            exportCnProjectName = _customBuildData.exportCnProjectName;
            exportEnProjectName = _customBuildData.exportEnProjectName;
            chineseShell = _customBuildData.chineseShell;
            folderCopy = _customBuildData.folderCopy;
        }
    }
#endif
}