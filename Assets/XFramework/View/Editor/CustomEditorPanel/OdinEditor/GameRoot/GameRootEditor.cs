using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
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
                GameObject tempSvcObj = new GameObject("PersistentDataSvc");
                PersistentDataSvc tempSvc = tempSvcObj.AddComponent<PersistentDataSvc>();
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempSvc.frameInit = resSvcEditor.isFrameInit;
                tempSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (resSvcEditor.Enabled)
            {
                GameObject tempSvcObj = new GameObject("ResSvc");
                ResSvc tempSvc = tempSvcObj.AddComponent<ResSvc>();
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempSvc.frameInit = resSvcEditor.isFrameInit;
                tempSvc.sceneInit = resSvcEditor.isSceneInit;
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (audioSvcEditor.Enabled)
            {
                GameObject tempSvcObj = new GameObject("AudioSvc");
                AudioSvc tempSvc = tempSvcObj.AddComponent<AudioSvc>();
                tempSvc.frameInit = audioSvcEditor.isFrameInit;
                tempSvc.sceneInit = audioSvcEditor.isSceneInit;
                tempSvc.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (listenerSvcEditorSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("ListenerSvc");
                ListenerSvc tempSvc = tempSvcObj.AddComponent<ListenerSvc>();
                tempSvc.frameInit = listenerSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = listenerSvcEditorSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (customSceneSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("SceneSvc");
                SceneSvc tempSvc = tempSvcObj.AddComponent<SceneSvc>();
                tempSvc.frameInit = customSceneSvc.isFrameInit;
                tempSvc.sceneInit = customSceneSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (timeSvcEditorSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("TimeSvc");
                TimeSvc tempSvc = tempSvcObj.AddComponent<TimeSvc>();
                tempSvc.frameInit = timeSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = timeSvcEditorSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (entityBaseSvcEditorSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("EntitySvc");
                EntitySvc tempSvc = tempSvcObj.AddComponent<EntitySvc>();
                tempSvc.frameInit = entityBaseSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = entityBaseSvcEditorSvc.isSceneInit;
                tempSvc.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (viewSvcEditorSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("ViewSvc");
                ViewSvc tempSvc = tempSvcObj.AddComponent<ViewSvc>();
                tempSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
            }

            if (circuitSvcEditor.Enabled)
            {
                GameObject tempSvcObj = new GameObject("CircuitSvc");
                CircuitSvc tempSvc = tempSvcObj.AddComponent<CircuitSvc>();
                tempSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempSvc);
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