using System;
using System.Collections.Generic;
using LitJson;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Windows;

namespace XFramework
{
    public class SceneLoad : BaseEditor
    {
        private SceneLoadEditorData _sceneLoadEditorData;

        [FolderPath] [LabelText("场景AssetBundle存放位置")]
        public string sceneAssetBundlePath;
        [LabelText("当前打包方式:")] public General.BuildTargetPlatform buildTargetPlatform;
        [LabelText("场景加载配置")] public List<SceneLoadEditorData.SceneInfo> sceneInfos;
        string platformPath = string.Empty;

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General.sceneLoadPath);
            if (_sceneLoadEditorData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SceneLoadEditorData>(), General.sceneLoadPath);
                //读取数据
                _sceneLoadEditorData = AssetDatabase.LoadAssetAtPath<SceneLoadEditorData>(General.sceneLoadPath);
            }
        }

        public override void OnSaveConfig()
        {
            _sceneLoadEditorData.sceneInfos = sceneInfos;
            _sceneLoadEditorData.sceneAssetBundlePath = sceneAssetBundlePath;
            _sceneLoadEditorData.buildTargetPlatform = buildTargetPlatform;

            SyncToUnity();
            //标记脏区
            EditorUtility.SetDirty(_sceneLoadEditorData);
        }

        public override void OnLoadConfig()
        {
            sceneInfos = _sceneLoadEditorData.sceneInfos;
            switch (buildTargetPlatform)
            {
                case General.BuildTargetPlatform.StandaloneWindows64:
                    platformPath = "Win";

                    break;
                case General.BuildTargetPlatform.WebGL:
                    platformPath = "WebGl";

                    break;
                case General.BuildTargetPlatform.Android:
                    platformPath = "Android";

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sceneAssetBundlePath = _sceneLoadEditorData.sceneAssetBundlePath;
            buildTargetPlatform = _sceneLoadEditorData.buildTargetPlatform;
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
                    case SceneLoadType.不加载:
                        sceneEnable = false;
                        break;
                    case SceneLoadType.同步:
                        sceneEnable = true;

                        break;
                    case SceneLoadType.异步:
                        sceneEnable = true;
                        break;
                    case SceneLoadType.下载同步:
                        sceneEnable = false;
                        break;
                    case SceneLoadType.下载异步:
                        sceneEnable = false;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                editorBuildSettingsScenes[i] = new EditorBuildSettingsScene(scenePath, sceneEnable);
            }

            //同步到Unity需要打包的场景
            EditorBuildSettings.scenes = editorBuildSettingsScenes;
            //记录场景加载方式
            SceneSvc.SceneFile sceneFile = new SceneSvc.SceneFile();
            sceneFile.sceneInfoList = new List<SceneSvc.SceneFile.SceneInfo>();
            for (int i = 0; i < sceneInfos.Count; i++)
            {
                SceneSvc.SceneFile.SceneInfo sceneInfo = new SceneSvc.SceneFile.SceneInfo();
                sceneInfo.sceneName = sceneInfos[i].sceneAsset.name;
                sceneInfo.sceneLoadType = (SceneSvc.SceneFile.SceneLoadType) sceneInfos[i].sceneLoadType;
                sceneFile.sceneInfoList.Add(sceneInfo);
            }

            //保存到文件
            FileOperation.SaveTextToLoad(Application.dataPath + "/XFramework/Resources/DownFile/SceneLoadInfo.json",
                JsonMapper.ToJson(sceneFile));
        }

        [Button(ButtonSizes.Medium)]
        [GUIColor(0, 1, 1)]
        [LabelText("打包异步场景")]
        public void BuildSyncScene()
        {
            SyncToUnity();
            OnBuildChangeScene();
            AssetDatabase.Refresh();
            EditorSceneFileConfig();
        }

        [Button(ButtonSizes.Medium)]
        [GUIColor(0, 1, 1)]
        [LabelText("清空文件信息")]
        public void ClearSceneMd5Data()
        {
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in sceneInfos)
            {
                sceneInfo.Md5 = String.Empty;
            }

            FileOperation.SaveTextToLoad(Application.dataPath + "/XFramework/Resources/DownFile/SceneFileInfo.json",
                JsonMapper.ToJson(new ResSvc.DownFile {fileInfoList = new List<ResSvc.DownFile.FileInfo>()}));
        }

        /// <summary>
        /// 打包已经更改的场景
        /// </summary>
        private void OnBuildChangeScene()
        {
            foreach (SceneLoadEditorData.SceneInfo sceneInfo in sceneInfos)
            {
                if (sceneInfo.sceneLoadType == SceneLoadType.下载同步 || sceneInfo.sceneLoadType == SceneLoadType.下载异步)
                {
                    string currentSceneMd5 =
                        FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
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
            string assetBundleDirPath = _sceneLoadEditorData.sceneAssetBundlePath + "/" + platformPath + "/" +
                                        sceneAsset.name + "Path";
            if (!Directory.Exists(_sceneLoadEditorData.sceneAssetBundlePath + "/" + platformPath))
            {
                Directory.CreateDirectory(_sceneLoadEditorData.sceneAssetBundlePath + "/" + platformPath);
            }

            if (!Directory.Exists(assetBundleDirPath))
            {
                Directory.CreateDirectory(assetBundleDirPath);
                AssetDatabase.Refresh();
            }

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

            Debug.Log(assetBundleDirPath);
            BuildPipeline.BuildAssetBundles(assetBundleDirPath,
                BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
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
                if (sceneInfo.sceneLoadType == SceneLoadType.下载同步 || sceneInfo.sceneLoadType == SceneLoadType.下载异步)
                {
                    string filePath =
                        (_sceneLoadEditorData.sceneAssetBundlePath + "/" + platformPath).Replace("Assets/", "") + "/" +
                        sceneInfo.sceneAsset.name + "Path" + "/" +
                        DataSvc.AllCharToLower(sceneInfo.sceneAsset.name);
                    sceneFile.fileInfoList.Add(new ResSvc.DownFile.FileInfo()
                    {
                        fileOriginalName = sceneInfo.sceneAsset.name,
                        fileName = DataSvc.AllCharToLower(sceneInfo.sceneAsset.name),
                        filePath = filePath,
                        fileSize = File.ReadAllBytes(Application.dataPath + "/" + filePath).Length,
                        fileMd5 = FileOperation.GetMD5HashFromFile(Application.dataPath + "/" + filePath)
                    });
                }
            }

            //保存场景配置信息
            FileOperation.SaveTextToLoad(
                Application.dataPath + "/XFramework/Resources/DownFile/SceneFileInfo.json",
                JsonMapper.ToJson(sceneFile));
        }

        [LabelText("场景加载方式")]
        public enum SceneLoadType
        {
            不加载,
            同步,
            异步,
            下载同步,
            下载异步
        }
    }
}