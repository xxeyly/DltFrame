using System;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using XxSlitFrame.Model.ConfigData.Editor;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor
{
    public class BuildSceneAssetBundle
    {
        private BuildSceneAssetBundleEditorData _buildSceneAssetBundleEditorData;
        private SceneLoadEditorData _sceneLoadEditorData;

        /// <summary>
        /// 加载旧的打包数据
        /// </summary>
        private void LoadData()
        {
            _buildSceneAssetBundleEditorData =
                AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleEditorData>(General
                    .buildSceneAssetBundleDataPath);
            if (_buildSceneAssetBundleEditorData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildSceneAssetBundleEditorData>(),
                    General.buildSceneAssetBundleDataPath);
                _buildSceneAssetBundleEditorData =
                    AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleEditorData>(General
                        .buildSceneAssetBundleDataPath);
            }

            _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General.sceneLoadPath);
        }

        /// <summary>
        /// 开始打包场景文件
        /// </summary>
        public void StartBuildAssetBundle()
        {
            LoadData();
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in _sceneLoadEditorData.sceneInfos)
            {
                if (sceneInfo.sceneLoadType != SceneLoadEditorData.SceneLoadType.异步)
                {
                    continue;
                }

                // bool isBuild = false;
                foreach (BuildSceneAssetBundleEditorData.SceneAssetBundleInfo sceneAssetBundleInfo in
                    _buildSceneAssetBundleEditorData.sceneAssetBundleInfos)
                {
                    //包含打包过的场景
                    if (sceneInfo.sceneAsset == sceneAssetBundleInfo.sceneAsset)
                    {
                        //两个文件的Md5不一样
                        if (ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset)) ==
                            sceneAssetBundleInfo.Md5)
                        {
                            // isBuild = true;
                        }
                        else
                        {
                            sceneAssetBundleInfo.Md5 =
                                ResSvc.FileOperation.GetMD5HashFromFile(
                                    AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                            sceneAssetBundleInfo.sceneAbPath =
                                _sceneLoadEditorData.sceneAssetBundlePath + "/" +
                                DataSvc.AllCharToLower(sceneInfo.sceneAsset.name);
                        }

                        break;
                    }
                }


                bool isCon = false;
                foreach (BuildSceneAssetBundleEditorData.SceneAssetBundleInfo sceneAssetBundleInfo in
                    _buildSceneAssetBundleEditorData.sceneAssetBundleInfos)
                {
                    if (sceneAssetBundleInfo.sceneAsset == sceneInfo.sceneAsset)
                    {
                        isCon = true;
                        break;
                    }
                }

                if (!isCon)
                {
                    Debug.Log("增加新场景");
                    _buildSceneAssetBundleEditorData.sceneAssetBundleInfos.Add(
                        new BuildSceneAssetBundleEditorData.SceneAssetBundleInfo()
                        {
                            sceneAsset = sceneInfo.sceneAsset,
                            Md5 =
                                ResSvc.FileOperation.GetMD5HashFromFile(
                                    AssetDatabase.GetAssetPath(sceneInfo.sceneAsset)),
                            sceneAbPath =
                                _sceneLoadEditorData.sceneAssetBundlePath + "/" +
                                DataSvc.AllCharToLower(sceneInfo.sceneAsset.name)
                        });
                }

                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                importer.assetBundleName = sceneInfo.sceneAsset.name;
                BuildPipeline.BuildAssetBundles(_sceneLoadEditorData.sceneAssetBundlePath, BuildAssetBundleOptions.ChunkBasedCompression,
                    BuildTarget.WebGL);
                importer.assetBundleName = String.Empty;
                // AssetDatabase.RemoveAssetBundleName(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset), true);
                /*//已经打包过,不参与打包
                if (!isBuild)
                {
                    // Create the array of bundle build details.
                    AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

                    buildMap[0].assetBundleName = sceneInfo.sceneAsset.name;

                    string[] enemyAssets = new string[1];
                    enemyAssets[0] = AssetDatabase.GetAssetPath(sceneInfo.sceneAsset);
                    buildMap[0].assetNames = enemyAssets;
                    BuildPipeline.BuildAssetBundles(_sceneLoadEditorData.sceneAssetBundlePath, buildMap,
                        BuildAssetBundleOptions.None,
                        BuildTarget.WebGL);
                }*/
            }

            _buildSceneAssetBundleEditorData =
                AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleEditorData>(General
                    .buildSceneAssetBundleDataPath);
            //标记脏区
            EditorUtility.SetDirty(_buildSceneAssetBundleEditorData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
            AssetDatabase.RemoveUnusedAssetBundleNames();
            EditorDownFileConfig();
        }

        /// <summary>
        /// 编辑下载文件配置
        /// </summary>
        private void EditorDownFileConfig()
        {
            ResSvc.DownFile downFile =
                JsonMapper.ToObject<ResSvc.DownFile>(Resources.Load<TextAsset>("DownFile/DownFileInfo").text);
            foreach (BuildSceneAssetBundleEditorData.SceneAssetBundleInfo sceneAssetBundleInfo in
                _buildSceneAssetBundleEditorData.sceneAssetBundleInfos)
            {
                bool isCon = false;
                foreach (ResSvc.DownFile.FileInfo fileInfo in downFile.fileInfoList)
                {
                    //查找到相同文件
                    if (fileInfo.fileName == DataSvc.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name))
                    {
                        isCon = true;
                        break;
                    }
                }

                if (!isCon)
                {
                    downFile.fileInfoList.Add(new ResSvc.DownFile.FileInfo()
                    {
                        fileName = DataSvc.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name),
                        filePath = _sceneLoadEditorData.sceneAssetBundlePath.Replace("Assets/", "") + "/" +
                                   DataSvc.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name),
                        fileSize = File.ReadAllBytes(sceneAssetBundleInfo.sceneAbPath).Length
                    });
                }
            }

            ResSvc.FileOperation.SaveTextToLoad(
                Application.dataPath + "/XxSlitFrame/Resources/DownFile/DownFileInfo.json",
                JsonMapper.ToJson(downFile));
        }
    }
}