#if UNITY_EDITOR

using System;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace XFramework
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
            _buildSceneAssetBundleEditorData = AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleEditorData>(General.buildSceneAssetBundleDataPath);
            if (_buildSceneAssetBundleEditorData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildSceneAssetBundleEditorData>(), General.buildSceneAssetBundleDataPath);
                _buildSceneAssetBundleEditorData = AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleEditorData>(General.buildSceneAssetBundleDataPath);
            }

            _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General.sceneLoadEditorPath);
        }

        /// <summary>
        /// 开始打包场景文件
        /// </summary>
        public void StartBuildAssetBundle()
        {
            LoadData();
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in _sceneLoadEditorData.sceneInfos)
            {
                if (sceneInfo.sceneLoadType != General.SceneLoadType.异步)
                {
                    continue;
                }

                // bool isBuild = false;
                foreach (BuildSceneAssetBundleEditorData.SceneAssetBundleInfo sceneAssetBundleInfo in _buildSceneAssetBundleEditorData.sceneAssetBundleInfos)
                {
                    //包含打包过的场景
                    if (sceneInfo.sceneAsset == sceneAssetBundleInfo.sceneAsset)
                    {
                        //两个文件的Md5不一样
                        if (FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset)) == sceneAssetBundleInfo.Md5)
                        {
                            // isBuild = true;
                        }
                        else
                        {
                            sceneAssetBundleInfo.Md5 = FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                            sceneAssetBundleInfo.sceneAbPath = _sceneLoadEditorData.sceneAssetBundlePath + "/" + DataComponent.AllCharToLower(sceneInfo.sceneAsset.name);
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
                            Md5 = FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset)),
                            sceneAbPath = _sceneLoadEditorData.sceneAssetBundlePath + "/" + DataComponent.AllCharToLower(sceneInfo.sceneAsset.name)
                        });
                }

                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                importer.assetBundleName = sceneInfo.sceneAsset.name;
                BuildPipeline.BuildAssetBundles(_sceneLoadEditorData.sceneAssetBundlePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
                importer.assetBundleName = String.Empty;
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
            //场景加载数据
            DownFileData downFileData = AssetDatabase.LoadAssetAtPath<DownFileData>(General.downFilePath);

            if (downFileData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SceneLoadData>(), General.downFilePath);
                //读取数据
                downFileData = AssetDatabase.LoadAssetAtPath<DownFileData>(General.downFilePath);
            }

            foreach (BuildSceneAssetBundleEditorData.SceneAssetBundleInfo sceneAssetBundleInfo in _buildSceneAssetBundleEditorData.sceneAssetBundleInfos)
            {
                bool isCon = false;
                foreach (DownFileData.DownFile.FileInfo fileInfo in downFileData.downFile.fileInfoList)
                {
                    //查找到相同文件
                    if (fileInfo.fileName == DataComponent.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name))
                    {
                        isCon = true;
                        break;
                    }
                }

                if (!isCon)
                {
                    downFileData.downFile.fileInfoList.Add(new DownFileData.DownFile.FileInfo()
                    {
                        fileName = DataComponent.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name),
                        filePath = _sceneLoadEditorData.sceneAssetBundlePath.Replace("Assets/", "") + "/" +
                                   DataComponent.AllCharToLower(sceneAssetBundleInfo.sceneAsset.name),
                        fileSize = File.ReadAllBytes(sceneAssetBundleInfo.sceneAbPath).Length
                    });
                }
            }
            
        }
    }
}
#endif