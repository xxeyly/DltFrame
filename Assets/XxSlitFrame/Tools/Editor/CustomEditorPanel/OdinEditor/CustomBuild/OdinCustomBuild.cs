using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject;
using CustomBuildData = XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject.CustomBuildData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomBuild
{
    public class OdinCustomBuild
    {
        private CustomScriptableObject.CustomScriptableObject _customScriptableObject;
        [LabelText("当前打包方式:")] public BuildTarget buildTarget;
        [LabelText("压缩类型")] public BuildOptions buildCompressType;

        [LabelText("当前打包存放路径")] [FolderPath(AbsolutePath = true)]
        public string buildPackagePath;

        [LabelText("中文名称")] [Tooltip("只做输出外壳使用")]
        public string exportCnProjectName;

        [LabelText("英文名称")] public string exportEnProjectName;

        [LabelText("使用中文输出外壳")] public bool chineseShell;
        [LabelText("文件拷贝输出路径")] public List<FolderCopy> folderCopy;

        [LabelText("开始打包")]
        [Button(ButtonSizes.Large)]
        public void StartBuild()
        {
            CopyFile();
            Build();
        }

        public OdinCustomBuild()
        {
            Debug.Log("加载配置");
            LoadConfig();
        }

        public OdinCustomBuild(CustomScriptableObject.CustomScriptableObject customScriptableObject)
        {
            _customScriptableObject = customScriptableObject;
            LoadConfig();
        }


        /// <summary>
        /// 打包
        /// </summary>
        private void Build()
        {
            CustomBuildData customBuildData =
                AssetDatabase.LoadAssetAtPath<CustomBuildData>(_customScriptableObject
                    .customBuildDataPath);
            if (customBuildData == null)
            {
                CustomBuildData tempCustomBuildData =
                    ScriptableObject.CreateInstance<CustomBuildData>();
                tempCustomBuildData.buildTarget = buildTarget;
                tempCustomBuildData.buildCompressType = buildCompressType;
                tempCustomBuildData.buildPackagePath = buildPackagePath;
                tempCustomBuildData.exportCnProjectName = exportCnProjectName;
                tempCustomBuildData.exportEnProjectName = exportEnProjectName;
                tempCustomBuildData.chineseShell = chineseShell;
                tempCustomBuildData.folderCopy = folderCopy;
                AssetDatabase.CreateAsset(tempCustomBuildData, _customScriptableObject.customBuildDataPath);
                customBuildData =
                    AssetDatabase.LoadAssetAtPath<CustomBuildData>(_customScriptableObject
                        .customBuildDataPath);
            }

            //标记脏区
            EditorUtility.SetDirty(customBuildData);
            // 保存所有修改
            AssetDatabase.SaveAssets();


            BuildPipeline.BuildPlayer(CustomBuildTools.FindEnableEditorScenes(),
                CustomBuildFileOperation.GetProjectPath(buildPackagePath, chineseShell, exportCnProjectName,
                    exportEnProjectName), buildTarget,
                buildCompressType);
        }

        private void LoadConfig()
        {
            CustomBuildData customBuildData =
                AssetDatabase.LoadAssetAtPath<CustomBuildData>(_customScriptableObject
                    .customBuildDataPath);
            if (customBuildData != null)
            {
                buildTarget = customBuildData.buildTarget;
                buildCompressType = customBuildData.buildCompressType;
                buildPackagePath = customBuildData.buildPackagePath;
                exportCnProjectName = customBuildData.exportCnProjectName;
                exportEnProjectName = customBuildData.exportEnProjectName;
                chineseShell = customBuildData.chineseShell;
                folderCopy = customBuildData.folderCopy;
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
    }
}