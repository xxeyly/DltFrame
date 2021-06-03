using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Model.ConfigData;
using XxSlitFrame.Model.ConfigData.Editor;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.Tools.Svc.BaseSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.AudioSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.EntitySvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.Listener;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.PersistentDataSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.ResSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.SceneSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.TimeSvc;
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.ViewSvc;

namespace XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.GameRoot
{
    public class GameRootEditor : BaseEditor

    {
        [Toggle("Enabled")] [LabelText("持久化")] public PersistentDataSvcEditor persistentDataSvcEditor;

        [Toggle("Enabled")] [LabelText("资源服务")]
        public ResSvcEditor resSvcEditor;

        [Toggle("Enabled")] [LabelText("音频服务")]
        public AudioSvcEditor audioSvcEditor;

        [Toggle("Enabled")] [LabelText("监听服务")]
        public ListenerSvcEditor listenerSvcEditorSvc;

        [Toggle("Enabled")] [LabelText("场景服务")]
        public SceneSvcEditor customSceneSvc;

        [Toggle("Enabled")] [LabelText("计时器服务")]
        public TimeSvcEditor timeSvcEditorSvc;

        [Toggle("Enabled")] [LabelText("视图服务")]
        public ViewSvcEditor viewSvcEditorSvc;

        [Toggle("Enabled")] [LabelText("实体服务")]
        public EntityBaseSvcEditor entityBaseSvcEditorSvc;

        private GameRootEditorEditorData _gameRootEditorEditorData;

        public GameRootEditor(PersistentDataSvcEditor persistentDataSvcEditor, ResSvcEditor resSvcEditor, AudioSvcEditor audioSvcEditor, ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, EntityBaseSvcEditor entityBaseSvcSvc, ViewSvcEditor viewSvcEditorSvc)
        {
            this.persistentDataSvcEditor = persistentDataSvcEditor;
            this.resSvcEditor = resSvcEditor;
            this.audioSvcEditor = audioSvcEditor;
            this.listenerSvcEditorSvc = listenerSvcEditorSvc;
            this.customSceneSvc = customSceneSvc;
            this.timeSvcEditorSvc = timeSvcEditorSvc;
            this.viewSvcEditorSvc = viewSvcEditorSvc;
            this.entityBaseSvcEditorSvc = entityBaseSvcSvc;
        }

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        [LabelText("生成预制体")]
        public void Generate()
        {
            GameObject gameRootStart = new GameObject("GameRootStart");
            GameRootStart tempGameRootStart = gameRootStart.AddComponent<GameRootStart>();
            tempGameRootStart.activeSvcBase = new List<SvcBase>();
            if (persistentDataSvcEditor.Enabled)
            {
                GameObject resSvcObj = new GameObject("PersistentDataSvc");
                PersistentDataSvc resSvc = resSvcObj.AddComponent<PersistentDataSvc>();
                resSvcObj.transform.SetParent(gameRootStart.transform);
                resSvc.frameInit = resSvcEditor.isFrameInit;
                resSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (resSvcEditor.Enabled)
            {
                GameObject resSvcObj = new GameObject("ResSvc");
                ResSvc resSvc = resSvcObj.AddComponent<ResSvc>();
                resSvcObj.transform.SetParent(gameRootStart.transform);
                resSvc.frameInit = resSvcEditor.isFrameInit;
                resSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (audioSvcEditor.Enabled)
            {
                GameObject resSvcObj = new GameObject("AudioSvc");
                AudioSvc resSvc = resSvcObj.AddComponent<AudioSvc>();
                resSvc.frameInit = audioSvcEditor.isFrameInit;
                resSvc.sceneInit = audioSvcEditor.isSceneInit;
                resSvc.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (listenerSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("ListenerSvc");
                ListenerSvc resSvc = resSvcObj.AddComponent<ListenerSvc>();
                resSvc.frameInit = listenerSvcEditorSvc.isFrameInit;
                resSvc.sceneInit = listenerSvcEditorSvc.isSceneInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (customSceneSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("SceneSvc");
                SceneSvc resSvc = resSvcObj.AddComponent<SceneSvc>();
                resSvc.frameInit = customSceneSvc.isFrameInit;
                resSvc.sceneInit = customSceneSvc.isSceneInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (timeSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("TimeSvc");
                TimeSvc resSvc = resSvcObj.AddComponent<TimeSvc>();
                resSvc.frameInit = timeSvcEditorSvc.isFrameInit;
                resSvc.sceneInit = timeSvcEditorSvc.isSceneInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (entityBaseSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("EntitySvc");
                EntitySvc entitySvc = resSvcObj.AddComponent<EntitySvc>();
                entitySvc.frameInit = entityBaseSvcEditorSvc.isFrameInit;
                entitySvc.sceneInit = entityBaseSvcEditorSvc.isSceneInit;
                entitySvc.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(entitySvc);
            }

            if (viewSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("ViewSvc");
                ViewSvc resSvc = resSvcObj.AddComponent<ViewSvc>();
                resSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                resSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }
        }

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _gameRootEditorEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);
            if (_gameRootEditorEditorData == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameRootEditorEditorData>(),
                    General.customFrameDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _gameRootEditorEditorData.persistentDataSvcEditor = persistentDataSvcEditor.Enabled;
            _gameRootEditorEditorData.persistentDataSvcEditorFrameInit = persistentDataSvcEditor.isFrameInit;
            _gameRootEditorEditorData.persistentDataSvcEditorSceneInit = persistentDataSvcEditor.isSceneInit;

            _gameRootEditorEditorData.resSvcEditor = resSvcEditor.Enabled;
            _gameRootEditorEditorData.resSvcEditorFrameInit = resSvcEditor.isFrameInit;
            _gameRootEditorEditorData.resSvcEditorSceneInit = resSvcEditor.isSceneInit;

            _gameRootEditorEditorData.audioSvcEditor = audioSvcEditor.Enabled;
            _gameRootEditorEditorData.audioSvcEditorFrameInit = audioSvcEditor.isFrameInit;
            _gameRootEditorEditorData.audioSvcEditorSceneInit = audioSvcEditor.isSceneInit;

            _gameRootEditorEditorData.listenerSvcEditorSvc = listenerSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.listenerSvcEditorFrameInit = listenerSvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.listenerSvcEditorSceneInit = listenerSvcEditorSvc.isSceneInit;

            _gameRootEditorEditorData.customSceneSvc = customSceneSvc.Enabled;
            _gameRootEditorEditorData.customSceneSvcEditorFrameInit = customSceneSvc.isFrameInit;
            _gameRootEditorEditorData.customSceneSvcEditorSceneInit = customSceneSvc.isSceneInit;

            _gameRootEditorEditorData.timeSvcEditorSvc = timeSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.timeSvcEditorFrameInit = timeSvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.timeSvcEditorSceneInit = timeSvcEditorSvc.isSceneInit;

            _gameRootEditorEditorData.viewSvcEditorSvc = viewSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.viewSvcEditorFrameInit = viewSvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.viewSvcEditorSceneInit = viewSvcEditorSvc.isSceneInit;

            _gameRootEditorEditorData.entitySvcEditorSvc = entityBaseSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.entityDataSvcEditorFrameInit = entityBaseSvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.entityDataSvcEditorSceneInit = entityBaseSvcEditorSvc.isSceneInit;
            //标记脏区
            EditorUtility.SetDirty(_gameRootEditorEditorData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            _gameRootEditorEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);

            persistentDataSvcEditor.Enabled = _gameRootEditorEditorData.persistentDataSvcEditor;
            persistentDataSvcEditor.isFrameInit = _gameRootEditorEditorData.persistentDataSvcEditorFrameInit;
            persistentDataSvcEditor.isSceneInit = _gameRootEditorEditorData.persistentDataSvcEditorSceneInit;

            resSvcEditor.Enabled = _gameRootEditorEditorData.resSvcEditor;
            resSvcEditor.isFrameInit = _gameRootEditorEditorData.resSvcEditorFrameInit;
            resSvcEditor.isSceneInit = _gameRootEditorEditorData.resSvcEditorSceneInit;

            audioSvcEditor.Enabled = _gameRootEditorEditorData.audioSvcEditor;
            audioSvcEditor.isFrameInit = _gameRootEditorEditorData.audioSvcEditorFrameInit;
            audioSvcEditor.isSceneInit = _gameRootEditorEditorData.audioSvcEditorSceneInit;

            listenerSvcEditorSvc.Enabled = _gameRootEditorEditorData.listenerSvcEditorSvc;
            listenerSvcEditorSvc.isFrameInit = _gameRootEditorEditorData.listenerSvcEditorFrameInit;
            listenerSvcEditorSvc.isSceneInit = _gameRootEditorEditorData.listenerSvcEditorSceneInit;

            customSceneSvc.Enabled = _gameRootEditorEditorData.customSceneSvc;
            customSceneSvc.isFrameInit = _gameRootEditorEditorData.customSceneSvcEditorFrameInit;
            customSceneSvc.isSceneInit = _gameRootEditorEditorData.customSceneSvcEditorSceneInit;

            timeSvcEditorSvc.Enabled = _gameRootEditorEditorData.timeSvcEditorSvc;
            timeSvcEditorSvc.isFrameInit = _gameRootEditorEditorData.timeSvcEditorFrameInit;
            timeSvcEditorSvc.isSceneInit = _gameRootEditorEditorData.timeSvcEditorSceneInit;

            viewSvcEditorSvc.Enabled = _gameRootEditorEditorData.viewSvcEditorSvc;
            viewSvcEditorSvc.isFrameInit = _gameRootEditorEditorData.viewSvcEditorFrameInit;
            viewSvcEditorSvc.isSceneInit = _gameRootEditorEditorData.viewSvcEditorSceneInit;

            entityBaseSvcEditorSvc.Enabled = _gameRootEditorEditorData.entitySvcEditorSvc;
            entityBaseSvcEditorSvc.isFrameInit = _gameRootEditorEditorData.entityDataSvcEditorFrameInit;
            entityBaseSvcEditorSvc.isSceneInit = _gameRootEditorEditorData.entityDataSvcEditorSceneInit;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}