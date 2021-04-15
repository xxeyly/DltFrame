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

        private GameRootEditorEditorData _gameRootEditorEditorData;

        public GameRootEditor(PersistentDataSvcEditor persistentDataSvcEditor, ResSvcEditor resSvcEditor,
            AudioSvcEditor audioSvcEditor,
            ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, ViewSvcEditor viewSvcEditorSvc)
        {
            this.persistentDataSvcEditor = persistentDataSvcEditor;
            this.resSvcEditor = resSvcEditor;
            this.audioSvcEditor = audioSvcEditor;
            this.listenerSvcEditorSvc = listenerSvcEditorSvc;
            this.customSceneSvc = customSceneSvc;
            this.timeSvcEditorSvc = timeSvcEditorSvc;
            this.viewSvcEditorSvc = viewSvcEditorSvc;
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
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
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
            _gameRootEditorEditorData.persistentDataSvcEditorInit = persistentDataSvcEditor.isInit;

            _gameRootEditorEditorData.resSvcEditor = resSvcEditor.Enabled;
            _gameRootEditorEditorData.resSvcEditorInit = resSvcEditor.isInit;

            _gameRootEditorEditorData.audioSvcEditor = audioSvcEditor.Enabled;
            _gameRootEditorEditorData.audioSvcEditorInit = audioSvcEditor.isInit;

            _gameRootEditorEditorData.listenerSvcEditorSvc = listenerSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.listenerSvcEditorSvcInit = listenerSvcEditorSvc.isInit;

            _gameRootEditorEditorData.customSceneSvc = customSceneSvc.Enabled;
            _gameRootEditorEditorData.customSceneSvcInit = customSceneSvc.isInit;

            _gameRootEditorEditorData.timeSvcEditorSvc = timeSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.timeSvcEditorSvcInit = timeSvcEditorSvc.isInit;

            _gameRootEditorEditorData.viewSvcEditorSvc = viewSvcEditorSvc.Enabled;
            _gameRootEditorEditorData.viewSvcEditorSvcInit = viewSvcEditorSvc.isInit;
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
            persistentDataSvcEditor.isInit = _gameRootEditorEditorData.persistentDataSvcEditorInit;

            resSvcEditor.Enabled = _gameRootEditorEditorData.resSvcEditor;
            resSvcEditor.isInit = _gameRootEditorEditorData.resSvcEditorInit;

            audioSvcEditor.Enabled = _gameRootEditorEditorData.audioSvcEditor;
            audioSvcEditor.isInit = _gameRootEditorEditorData.audioSvcEditorInit;

            listenerSvcEditorSvc.Enabled = _gameRootEditorEditorData.listenerSvcEditorSvc;
            listenerSvcEditorSvc.isInit = _gameRootEditorEditorData.listenerSvcEditorSvcInit;

            customSceneSvc.Enabled = _gameRootEditorEditorData.customSceneSvc;
            customSceneSvc.isInit = _gameRootEditorEditorData.customSceneSvcInit;

            timeSvcEditorSvc.Enabled = _gameRootEditorEditorData.timeSvcEditorSvc;
            timeSvcEditorSvc.isInit = _gameRootEditorEditorData.timeSvcEditorSvcInit;

            viewSvcEditorSvc.Enabled = _gameRootEditorEditorData.viewSvcEditorSvc;
            viewSvcEditorSvc.isInit = _gameRootEditorEditorData.viewSvcEditorSvcInit;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}