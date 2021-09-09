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
using XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.Svc.CircuitSvc;
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

        [Toggle("Enabled")] [LabelText("流程服务")]
        public CircuitSvcEditor circuitSvcEditor;

        private GameRootEditorEditorData _gameRootEditorEditorData;

        public GameRootEditor(PersistentDataSvcEditor persistentDataSvcEditor, ResSvcEditor resSvcEditor, AudioSvcEditor audioSvcEditor, ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, EntityBaseSvcEditor entityBaseSvcSvc, ViewSvcEditor viewSvcEditorSvc, CircuitSvcEditor circuitSvcEditor)
        {
            this.persistentDataSvcEditor = persistentDataSvcEditor;
            this.resSvcEditor = resSvcEditor;
            this.audioSvcEditor = audioSvcEditor;
            this.listenerSvcEditorSvc = listenerSvcEditorSvc;
            this.customSceneSvc = customSceneSvc;
            this.timeSvcEditorSvc = timeSvcEditorSvc;
            this.viewSvcEditorSvc = viewSvcEditorSvc;
            this.entityBaseSvcEditorSvc = entityBaseSvcSvc;
            this.circuitSvcEditor = circuitSvcEditor;
        }

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        [LabelText("生成框架")]
        public void Generate()
        {
            if (GameObject.Find("GameRootStart"))
            {
                return;
            }

            GameObject gameRootStart = new GameObject("GameRootStart");
            GameRootStart tempGameRootStart = gameRootStart.AddComponent<GameRootStart>();
            tempGameRootStart.activeSvcBase = new List<SvcBase>();
            if (persistentDataSvcEditor.Enabled)
            {
                GameObject timeSvcObj = new GameObject("PersistentDataSvc");
                PersistentDataSvc timeSvc = timeSvcObj.AddComponent<PersistentDataSvc>();
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                timeSvc.frameInit = resSvcEditor.isFrameInit;
                timeSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (resSvcEditor.Enabled)
            {
                GameObject timeSvcObj = new GameObject("ResSvc");
                ResSvc timeSvc = timeSvcObj.AddComponent<ResSvc>();
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                timeSvc.frameInit = resSvcEditor.isFrameInit;
                timeSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (audioSvcEditor.Enabled)
            {
                GameObject timeSvcObj = new GameObject("AudioSvc");
                AudioSvc timeSvc = timeSvcObj.AddComponent<AudioSvc>();
                timeSvc.frameInit = audioSvcEditor.isFrameInit;
                timeSvc.sceneInit = audioSvcEditor.isSceneInit;
                timeSvc.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (listenerSvcEditorSvc.Enabled)
            {
                GameObject timeSvcObj = new GameObject("ListenerSvc");
                ListenerSvc timeSvc = timeSvcObj.AddComponent<ListenerSvc>();
                timeSvc.frameInit = listenerSvcEditorSvc.isFrameInit;
                timeSvc.sceneInit = listenerSvcEditorSvc.isSceneInit;
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (customSceneSvc.Enabled)
            {
                GameObject timeSvcObj = new GameObject("SceneSvc");
                SceneSvc timeSvc = timeSvcObj.AddComponent<SceneSvc>();
                timeSvc.frameInit = customSceneSvc.isFrameInit;
                timeSvc.sceneInit = customSceneSvc.isSceneInit;
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (timeSvcEditorSvc.Enabled)
            {
                GameObject timeSvcObj = new GameObject("TimeSvc");
                TimeSvc timeSvc = timeSvcObj.AddComponent<TimeSvc>();
                timeSvc.frameInit = timeSvcEditorSvc.isFrameInit;
                timeSvc.sceneInit = timeSvcEditorSvc.isSceneInit;
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (entityBaseSvcEditorSvc.Enabled)
            {
                GameObject timeSvcObj = new GameObject("EntitySvc");
                EntitySvc timeSvc = timeSvcObj.AddComponent<EntitySvc>();
                timeSvc.frameInit = entityBaseSvcEditorSvc.isFrameInit;
                timeSvc.sceneInit = entityBaseSvcEditorSvc.isSceneInit;
                timeSvc.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (viewSvcEditorSvc.Enabled)
            {
                GameObject timeSvcObj = new GameObject("ViewSvc");
                ViewSvc timeSvc = timeSvcObj.AddComponent<ViewSvc>();
                timeSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                timeSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
            }

            if (circuitSvcEditor.Enabled)
            {
                GameObject timeSvcObj = new GameObject("CircuitSvc");
                CircuitSvc timeSvc = timeSvcObj.AddComponent<CircuitSvc>();
                timeSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                timeSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                timeSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(timeSvc);
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

            _gameRootEditorEditorData.circuitSvcEditorSvc = entityBaseSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.circuitDataSvcEditorFrameInit = entityBaseSvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.circuitDataSvcEditorSceneInit = entityBaseSvcEditorSvc.isSceneInit;

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

            circuitSvcEditor.Enabled = _gameRootEditorEditorData.entitySvcEditorSvc;
            circuitSvcEditor.isFrameInit = _gameRootEditorEditorData.entityDataSvcEditorFrameInit;
            circuitSvcEditor.isSceneInit = _gameRootEditorEditorData.entityDataSvcEditorSceneInit;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}