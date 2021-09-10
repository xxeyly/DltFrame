using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace XFramework
{
    public class SceneLoad : BaseEditor
    {
        private SceneLoadEditorData _sceneLoadEditorData;

        [FolderPath] [LabelText("场景AssetBundle存放位置")]
        public string sceneAssetBundlePath;

        [LabelText("场景加载配置")] public List<SceneLoadEditorData.SceneInfo> sceneInfos;

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General
                .sceneLoadPath);
            if (_sceneLoadEditorData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SceneLoadEditorData>(),
                    General.sceneLoadPath);
                //读取数据
                _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General.sceneLoadPath);
            }
        }

        public override void OnSaveConfig()
        {
            _sceneLoadEditorData.sceneInfos = sceneInfos;
            _sceneLoadEditorData.sceneAssetBundlePath = sceneAssetBundlePath;
            SyncToUnity();
            //标记脏区
            EditorUtility.SetDirty(_sceneLoadEditorData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            sceneInfos = _sceneLoadEditorData.sceneInfos;
            sceneAssetBundlePath = _sceneLoadEditorData.sceneAssetBundlePath;
            SyncToUnity();
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }

        [LabelText("同步到Unity")]
        public void SyncToUnity()
        {
            EditorBuildSettingsScene[] editorBuildSettingsScenes = new EditorBuildSettingsScene[sceneInfos.Count];
            for (int i = 0; i < sceneInfos.Count; i++)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneInfos[i].sceneAsset);
                bool sceneEnable = false;
                switch (sceneInfos[i].sceneLoadType)
                {
                    case SceneLoadEditorData.SceneLoadType.不加载:
                        sceneEnable = false;
                        break;
                    case SceneLoadEditorData.SceneLoadType.同步:
                        sceneEnable = true;

                        break;
                    case SceneLoadEditorData.SceneLoadType.异步:
                        sceneEnable = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                editorBuildSettingsScenes[i] = new EditorBuildSettingsScene(scenePath, sceneEnable);
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes;
        }

        [Button(ButtonSizes.Medium)]
        [GUIColor(0, 1, 1)]
        [LabelText("打包异步场景")]
        public void BuildSyncScene()
        {
            // new BuildSceneAssetBundle().StartBuildAssetBundle();
            SyncToUnity();
            OnBuildChangeScene();
            EditorSceneFileConfig();
        }

        /// <summary>
        /// 打包已经更改的场景
        /// </summary>
        private void OnBuildChangeScene()
        {
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in sceneInfos)
            {
                if (sceneInfo.sceneLoadType == SceneLoadEditorData.SceneLoadType.异步)
                {
                    string currentSceneMd5 =
                        ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                    //场景已经更新了
                    if (currentSceneMd5 != sceneInfo.Md5)
                    {
                        Debug.Log("重新打包场景:" + sceneInfo.sceneAsset.name);
                        AgainBuildScene(sceneInfo.sceneAsset);
                        sceneInfo.Md5 = currentSceneMd5;
                    }
                }
            }
        }

        /// <summary>
        /// 重新打包场景
        /// </summary>
        /// <param name="sceneAsset"></param>
        private void AgainBuildScene(SceneAsset sceneAsset)
        {
            AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sceneAsset));
            importer.assetBundleName = sceneAsset.name;
            string assetBundleDirPath = _sceneLoadEditorData.sceneAssetBundlePath + "/" + sceneAsset.name + "Path";
            if (!Directory.Exists(_sceneLoadEditorData.sceneAssetBundlePath))
            {
                Directory.CreateDirectory(_sceneLoadEditorData.sceneAssetBundlePath);
            }

            if (!Directory.Exists(assetBundleDirPath))
            {
                Directory.CreateDirectory(assetBundleDirPath);
                AssetDatabase.Refresh();
            }

            BuildPipeline.BuildAssetBundles(assetBundleDirPath,
                BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
            importer.assetBundleName = String.Empty;
        }

        /// <summary>
        /// 编辑下载文件配置
        /// </summary>
        private void EditorSceneFileConfig()
        {
            //场景配置文件清空
            ResSvc.DownFile sceneFile =
                JsonMapper.ToObject<ResSvc.DownFile>(Resources.Load<TextAsset>("DownFile/SceneFileInfo").text);
            sceneFile.fileInfoList.Clear();
            //遍历所有异步场景,并存储
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in sceneInfos)
            {
                if (sceneInfo.sceneLoadType == SceneLoadEditorData.SceneLoadType.异步)
                {
                    string filePath = _sceneLoadEditorData.sceneAssetBundlePath.Replace("Assets/", "") + "/" +
                                      sceneInfo.sceneAsset.name + "Path" + "/" +
                                      DataSvc.AllCharToLower(sceneInfo.sceneAsset.name);
                    sceneFile.fileInfoList.Add(new ResSvc.DownFile.FileInfo()
                    {
                        fileName = DataSvc.AllCharToLower(sceneInfo.sceneAsset.name),
                        filePath = filePath,
                        fileSize = File.ReadAllBytes(Application.dataPath + "/" + filePath).Length
                    });
                }
            }

            //保存场景配置信息
            ResSvc.FileOperation.SaveTextToLoad(
                Application.dataPath + "/XFramework/Resources/DownFile/SceneFileInfo.json",
                JsonMapper.ToJson(sceneFile));
        }
    }
}