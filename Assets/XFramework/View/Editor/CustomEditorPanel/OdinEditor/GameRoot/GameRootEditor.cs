using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class GameRootEditor : BaseEditor

    {
        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("持久化")]
        public RuntimeDataSvcEditor RuntimeDataSvcEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("资源服务")]
        public ResSvcEditor resSvcEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("音频服务")]
        public AudioSvcEditor audioSvcEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("监听服务")]
        public ListenerSvcEditor listenerSvcEditorSvc;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("场景服务")]
        public SceneSvcEditor customSceneSvc;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("计时器服务")]
        public TimeSvcEditor timeSvcEditorSvc;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("视图服务")]
        public ViewSvcEditor viewSvcEditorSvc;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("实体服务")]
        public EntitySvcEditor entitySvcEditorSvc;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("流程服务")]
        public CircuitSvcEditor circuitSvcEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("鼠标服务")]
        public MouseSvcEditor mouseSvcEditor;


        private GameRootEditorEditorData _gameRootEditorEditorData;

        public GameRootEditor(RuntimeDataSvcEditor runtimeDataSvcEditor, ResSvcEditor resSvcEditor, AudioSvcEditor audioSvcEditor, ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, EntitySvcEditor entitySvcSvc, ViewSvcEditor viewSvcEditorSvc, CircuitSvcEditor circuitSvcEditor,
            MouseSvcEditor mouseSvcEditor)
        {
            this.RuntimeDataSvcEditor = runtimeDataSvcEditor;
            this.resSvcEditor = resSvcEditor;
            this.audioSvcEditor = audioSvcEditor;
            this.listenerSvcEditorSvc = listenerSvcEditorSvc;
            this.customSceneSvc = customSceneSvc;
            this.timeSvcEditorSvc = timeSvcEditorSvc;
            this.viewSvcEditorSvc = viewSvcEditorSvc;
            this.entitySvcEditorSvc = entitySvcSvc;
            this.circuitSvcEditor = circuitSvcEditor;
            this.mouseSvcEditor = mouseSvcEditor;
        }

        [BoxGroup("Generate")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        [LabelText("生成框架")]
        public void Generate()
        {
            if (GameObject.Find("GameRootStart"))
            {
                return;
            }

            GameObject gameRootStart = new GameObject("GameRootStart");
            Undo.RegisterCreatedObjectUndo(gameRootStart, "UndoCreate");
            GameRootStart tempGameRootStart = gameRootStart.AddComponent<GameRootStart>();
            tempGameRootStart.activeSvcBase = new List<SvcBase>();
            if (RuntimeDataSvcEditor.Enabled)
            {
                GameObject tempSvcObj = new GameObject("RuntimeDataSvc");
                RuntimeDataSvc tempSvc = tempSvcObj.AddComponent<RuntimeDataSvc>();
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

            if (entitySvcEditorSvc.Enabled)
            {
                GameObject tempSvcObj = new GameObject("EntitySvc");
                EntitySvc tempSvc = tempSvcObj.AddComponent<EntitySvc>();
                tempSvc.frameInit = entitySvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = entitySvcEditorSvc.isSceneInit;
                tempSvc.transform.SetParent(gameRootStart.transform);
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

            if (mouseSvcEditor.Enabled)
            {
                GameObject tempSvcObj = new GameObject("MouseSvc");
                MouseSvc tempSvc = tempSvcObj.AddComponent<MouseSvc>();
                tempSvc.frameInit = viewSvcEditorSvc.isFrameInit;
                tempSvc.sceneInit = viewSvcEditorSvc.isSceneInit;
                tempSvcObj.transform.SetParent(gameRootStart.transform);
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
        }

        [BoxGroup("Export")] [LabelText("导出路径")] [FolderPath(AbsolutePath = true)]
        public string ExportPath;

        [BoxGroup("Export")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
        [LabelText("导出框架")]
        public void ExportFrameToDesktop()
        {
            AssetDatabase.ExportPackage("Assets/" + "XFramework", ExportPath + "/XFramework.unitypackage", ExportPackageOptions.Recurse);
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
            _gameRootEditorEditorData.persistentDataSvcEditor = RuntimeDataSvcEditor.Enabled;
            _gameRootEditorEditorData.persistentDataSvcEditorFrameInit = RuntimeDataSvcEditor.isFrameInit;
            _gameRootEditorEditorData.persistentDataSvcEditorSceneInit = RuntimeDataSvcEditor.isSceneInit;

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

            _gameRootEditorEditorData.entitySvcEditorSvc = entitySvcEditorSvc.Enabled;
            _gameRootEditorEditorData.entityDataSvcEditorFrameInit = entitySvcEditorSvc.isFrameInit;
            _gameRootEditorEditorData.entityDataSvcEditorSceneInit = entitySvcEditorSvc.isSceneInit;

            _gameRootEditorEditorData.circuitSvcEditorSvc = circuitSvcEditor.Enabled;
            _gameRootEditorEditorData.circuitDataSvcEditorFrameInit = circuitSvcEditor.isFrameInit;
            _gameRootEditorEditorData.circuitDataSvcEditorSceneInit = circuitSvcEditor.isSceneInit;

            _gameRootEditorEditorData.mouseSvcEditorSvc = mouseSvcEditor.Enabled;
            _gameRootEditorEditorData.mouseSvcEditorFrameInit = mouseSvcEditor.isFrameInit;
            _gameRootEditorEditorData.mouseSvcEditorSceneInit = mouseSvcEditor.isSceneInit;

            //标记脏区
            EditorUtility.SetDirty(_gameRootEditorEditorData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            _gameRootEditorEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);

            RuntimeDataSvcEditor.Enabled = _gameRootEditorEditorData.persistentDataSvcEditor;
            RuntimeDataSvcEditor.isFrameInit = _gameRootEditorEditorData.persistentDataSvcEditorFrameInit;
            RuntimeDataSvcEditor.isSceneInit = _gameRootEditorEditorData.persistentDataSvcEditorSceneInit;

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

            entitySvcEditorSvc.Enabled = _gameRootEditorEditorData.entitySvcEditorSvc;
            entitySvcEditorSvc.isFrameInit = _gameRootEditorEditorData.entityDataSvcEditorFrameInit;
            entitySvcEditorSvc.isSceneInit = _gameRootEditorEditorData.entityDataSvcEditorSceneInit;

            circuitSvcEditor.Enabled = _gameRootEditorEditorData.circuitSvcEditorSvc;
            circuitSvcEditor.isFrameInit = _gameRootEditorEditorData.circuitDataSvcEditorFrameInit;
            circuitSvcEditor.isSceneInit = _gameRootEditorEditorData.circuitDataSvcEditorSceneInit;

            mouseSvcEditor.Enabled = _gameRootEditorEditorData.mouseSvcEditorSvc;
            mouseSvcEditor.isFrameInit = _gameRootEditorEditorData.mouseSvcEditorFrameInit;
            mouseSvcEditor.isSceneInit = _gameRootEditorEditorData.mouseSvcEditorSceneInit;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}