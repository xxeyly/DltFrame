using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.AudioSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.Listener;
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
        private CustomScriptableObject.CustomScriptableObject _customScriptableObject;

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

        public GameRootEditor(CustomScriptableObject.CustomScriptableObject customScriptableObject,
            ResSvcEditor resSvcEditor, AudioSvcEditor audioSvcEditor, ListenerSvcEditor listenerSvcEditorSvc,
            SceneSvcEditor customSceneSvc, TimeSvcEditor timeSvcEditorSvc, ViewSvcEditor viewSvcEditorSvc)
        {
            _customScriptableObject = customScriptableObject;
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
            if (resSvcEditor.Enabled)
            {
                GameObject customResSvcObj = new GameObject("CustomResSvc");
                ResSvc tempCustomResSvc = customResSvcObj.AddComponent<ResSvc>();
                customResSvcObj.transform.SetParent(gameRootStart.transform);
                tempCustomResSvc.init = resSvcEditor.isInit;
                tempGameRootStart.activeSvcBase.Add(tempCustomResSvc);
            }

            if (audioSvcEditor.Enabled)
            {
                GameObject customAudioSvcObj = new GameObject("AudioSvc");
                AudioSvc tempCustomAudioSvc = customAudioSvcObj.AddComponent<AudioSvc>();
                tempCustomAudioSvc.init = audioSvcEditor.isInit;
                tempCustomAudioSvc.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(_customScriptableObject.customAudioDataPath);
                customAudioSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempCustomAudioSvc);
            }

            if (listenerSvcEditorSvc.Enabled)
            {
                GameObject customListenerSvcObj = new GameObject("CustomListenerSvc");
                ListenerSvc tempCustomListenerSvc = customListenerSvcObj.AddComponent<ListenerSvc>();
                tempCustomListenerSvc.init = listenerSvcEditorSvc.isInit;
                customListenerSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempCustomListenerSvc);
            }

            if (customSceneSvc.Enabled)
            {
                GameObject customResSceneObj = new GameObject("CustomSceneSvc");
                SceneSvc tempCustomSceneSvc = customResSceneObj.AddComponent<SceneSvc>();
                tempCustomSceneSvc.init = customSceneSvc.isInit;
                customResSceneObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempCustomSceneSvc);
            }

            if (timeSvcEditorSvc.Enabled)
            {
                GameObject customResTimeObj = new GameObject("CustomTimeSvc");
                TimeSvc tempCustomTimeSvc = customResTimeObj.AddComponent<TimeSvc>();
                tempCustomTimeSvc.init = timeSvcEditorSvc.isInit;
                customResTimeObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempCustomTimeSvc);
            }

            if (viewSvcEditorSvc.Enabled)
            {
                GameObject customViewSvcObj = new GameObject("CustomViewSvc");
                ViewSvc tempCustomViewSvc = customViewSvcObj.AddComponent<ViewSvc>();
                tempCustomViewSvc.init = viewSvcEditorSvc.isInit;
                customViewSvcObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeSvcBase.Add(tempCustomViewSvc);
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