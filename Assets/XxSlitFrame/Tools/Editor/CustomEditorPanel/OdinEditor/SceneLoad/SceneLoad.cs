using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.SceneLoad
{
    public class SceneLoad : BaseEditor
    {
        private SceneLoadData _sceneLoadData;

        [FolderPath] [LabelText("场景AssetBundle存放位置")]
        public string sceneAssetBundlePath;

        [LabelText("场景加载配置")] public List<SceneLoadData.SceneInfo> sceneInfos;


        public SceneLoad()
        {
            OnCreateConfig();
            OnLoadConfig();
        }


        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _sceneLoadData = AssetDatabase.LoadAssetAtPath<SceneLoadData>(General
                .sceneLoadPath);
            if (_sceneLoadData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SceneLoadData>(), General.sceneLoadPath);
                //读取数据
                _sceneLoadData = AssetDatabase.LoadAssetAtPath<SceneLoadData>(General.sceneLoadPath);
            }
        }

        public override void OnSaveConfig()
        {
            _sceneLoadData.sceneInfos = sceneInfos;
            _sceneLoadData.sceneAssetBundlePath = sceneAssetBundlePath;
            SyncToUnity();
            //标记脏区
            EditorUtility.SetDirty(_sceneLoadData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            sceneInfos = _sceneLoadData.sceneInfos;
            sceneAssetBundlePath = _sceneLoadData.sceneAssetBundlePath;
            SyncToUnity();
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
                    case SceneLoadData.SceneLoadType.不加载:
                        sceneEnable = false;
                        break;
                    case SceneLoadData.SceneLoadType.同步:
                        sceneEnable = true;

                        break;
                    case SceneLoadData.SceneLoadType.异步:
                        sceneEnable = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                editorBuildSettingsScenes[i] = new EditorBuildSettingsScene(scenePath, sceneEnable);
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes;
        }

        [Button]
        public void Start()
        {
            new BuildSceneAssetBundle().StartBuildAssetBundle();
        }
    }
}