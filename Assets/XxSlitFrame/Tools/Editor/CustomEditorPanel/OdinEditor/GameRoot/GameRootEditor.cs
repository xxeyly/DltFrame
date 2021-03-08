using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.AudioSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.Listener;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.PersistentDataSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.ResSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.SceneSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.TimeSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.ViewSvc;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.GameRoot
{
    public class GameRootEditor : BaseEditor

    {
        private ConfigData.CustomScriptableObject _customScriptableObject;

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

        private GameRootEditorData _gameRootEditorData;

        public GameRootEditor(ConfigData.CustomScriptableObject customScriptableObject,
            PersistentDataSvcEditor persistentDataSvcEditor, ResSvcEditor resSvcEditor, AudioSvcEditor audioSvcEditor,
            ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, ViewSvcEditor viewSvcEditorSvc)
        {
            _customScriptableObject = customScriptableObject;
            this.persistentDataSvcEditor = persistentDataSvcEditor;
            this.resSvcEditor = resSvcEditor;
            this.audioSvcEditor = audioSvcEditor;
            this.listenerSvcEditorSvc = listenerSvcEditorSvc;
            this.customSceneSvc = customSceneSvc;
            this.timeSvcEditorSvc = timeSvcEditorSvc;
            this.viewSvcEditorSvc = viewSvcEditorSvc;
            OnCreateConfig();
            OnLoadConfig();
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
                resSvc.init = resSvcEditor.isInit;
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (resSvcEditor.Enabled)
            {
                GameObject resSvcObj = new GameObject("ResSvc");
                ResSvc resSvc = resSvcObj.AddComponent<ResSvc>();
                resSvcObj.transform.SetParent(gameRootStart.transform);
                resSvc.init = resSvcEditor.isInit;
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (audioSvcEditor.Enabled)
            {
                GameObject resSvcObj = new GameObject("AudioSvc");
                AudioSvc resSvc = resSvcObj.AddComponent<AudioSvc>();
                resSvc.init = audioSvcEditor.isInit;
                resSvc.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(_customScriptableObject.customAudioDataPath);
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (listenerSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("ListenerSvc");
                ListenerSvc resSvc = resSvcObj.AddComponent<ListenerSvc>();
                resSvc.init = listenerSvcEditorSvc.isInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (customSceneSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("SceneSvc");
                SceneSvc resSvc = resSvcObj.AddComponent<SceneSvc>();
                resSvc.init = customSceneSvc.isInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (timeSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("TimeSvc");
                TimeSvc resSvc = resSvcObj.AddComponent<TimeSvc>();
                resSvc.init = timeSvcEditorSvc.isInit;
                resSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(resSvc);
            }

            if (viewSvcEditorSvc.Enabled)
            {
                GameObject resSvcObj = new GameObject("ViewSvc");
                ViewSvc resSvc = resSvcObj.AddComponent<ViewSvc>();
                resSvc.init = viewSvcEditorSvc.isInit;
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
            _gameRootEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorData>(_customScriptableObject.customFrameDataPath);
            if (_gameRootEditorData == null)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameRootEditorData>(),
                    _customScriptableObject.customFrameDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _gameRootEditorData.persistentDataSvcEditor = persistentDataSvcEditor.Enabled;
            _gameRootEditorData.persistentDataSvcEditorInit = persistentDataSvcEditor.isInit;

            _gameRootEditorData.resSvcEditor = resSvcEditor.Enabled;
            _gameRootEditorData.resSvcEditorInit = resSvcEditor.isInit;

            _gameRootEditorData.audioSvcEditor = audioSvcEditor.Enabled;
            _gameRootEditorData.audioSvcEditorInit = audioSvcEditor.isInit;

            _gameRootEditorData.listenerSvcEditorSvc = listenerSvcEditorSvc.Enabled;
            _gameRootEditorData.listenerSvcEditorSvcInit = listenerSvcEditorSvc.isInit;

            _gameRootEditorData.customSceneSvc = customSceneSvc.Enabled;
            _gameRootEditorData.customSceneSvcInit = customSceneSvc.isInit;

            _gameRootEditorData.timeSvcEditorSvc = timeSvcEditorSvc.Enabled;
            _gameRootEditorData.timeSvcEditorSvcInit = timeSvcEditorSvc.isInit;

            _gameRootEditorData.viewSvcEditorSvc = viewSvcEditorSvc.Enabled;
            _gameRootEditorData.viewSvcEditorSvcInit = viewSvcEditorSvc.isInit;
            //标记脏区
            EditorUtility.SetDirty(_gameRootEditorData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            _gameRootEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorData>(_customScriptableObject.customFrameDataPath);

            persistentDataSvcEditor.Enabled = _gameRootEditorData.persistentDataSvcEditor;
            persistentDataSvcEditor.isInit = _gameRootEditorData.persistentDataSvcEditorInit;

            resSvcEditor.Enabled = _gameRootEditorData.resSvcEditor;
            resSvcEditor.isInit = _gameRootEditorData.resSvcEditorInit;

            audioSvcEditor.Enabled = _gameRootEditorData.audioSvcEditor;
            audioSvcEditor.isInit = _gameRootEditorData.audioSvcEditorInit;

            listenerSvcEditorSvc.Enabled = _gameRootEditorData.listenerSvcEditorSvc;
            listenerSvcEditorSvc.isInit = _gameRootEditorData.listenerSvcEditorSvcInit;

            customSceneSvc.Enabled = _gameRootEditorData.customSceneSvc;
            customSceneSvc.isInit = _gameRootEditorData.customSceneSvcInit;

            timeSvcEditorSvc.Enabled = _gameRootEditorData.timeSvcEditorSvc;
            timeSvcEditorSvc.isInit = _gameRootEditorData.timeSvcEditorSvcInit;

            viewSvcEditorSvc.Enabled = _gameRootEditorData.viewSvcEditorSvc;
            viewSvcEditorSvc.isInit = _gameRootEditorData.viewSvcEditorSvcInit;
        }
    }
}