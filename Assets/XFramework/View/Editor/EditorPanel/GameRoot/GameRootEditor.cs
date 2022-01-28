using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class GameRootEditor : BaseEditor

    {
        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("持久化")]
        public RuntimeDataComponentEditor RuntimeDataComponentEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("资源组件")]
        public ResComponentEditor ResComponentEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("下载组件")]
        public DownComponentEditor DownComponentEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("音频组件")]
        public AudioComponentEditor AudioComponentEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("监听组件")]
        public ListenerComponentEditor ListenerComponentEditorComponent;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("场景组件")]
        public SceneLoadComponentEditor CustomSceneLoadComponent;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("计时器组件")]
        public TimeComponentEditor TimeComponentEditorComponent;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("视图组件")]
        public ViewComponentEditor ViewComponentEditorComponent;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("实体组件")]
        public EntityComponentEditor EntityComponentEditorComponent;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("流程组件")]
        public CircuitComponentEditor CircuitComponentEditor;

        [BoxGroup("Generate")] [Toggle("Enabled")] [LabelText("鼠标组件")]
        public MouseComponentEditor MouseComponentEditor;


        private GameRootEditorEditorData _gameRootEditorEditorData;

        public GameRootEditor(RuntimeDataComponentEditor runtimeDataComponentEditor, ResComponentEditor resComponentEditor, DownComponentEditor downComponentEditor,
            AudioComponentEditor audioComponentEditor, ListenerComponentEditor listenerComponentEditorComponent,
            SceneLoadComponentEditor customSceneLoadComponent, TimeComponentEditor timeComponentEditorComponent, EntityComponentEditor entityComponentComponent,
            ViewComponentEditor viewComponentEditorComponent, CircuitComponentEditor circuitComponentEditor,
            MouseComponentEditor mouseComponentEditor)
        {
            this.RuntimeDataComponentEditor = runtimeDataComponentEditor;
            this.ResComponentEditor = resComponentEditor;
            this.DownComponentEditor = downComponentEditor;
            this.AudioComponentEditor = audioComponentEditor;
            this.ListenerComponentEditorComponent = listenerComponentEditorComponent;
            this.CustomSceneLoadComponent = customSceneLoadComponent;
            this.TimeComponentEditorComponent = timeComponentEditorComponent;
            this.ViewComponentEditorComponent = viewComponentEditorComponent;
            this.EntityComponentEditorComponent = entityComponentComponent;
            this.CircuitComponentEditor = circuitComponentEditor;
            this.MouseComponentEditor = mouseComponentEditor;
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
            tempGameRootStart.activeComponentBase = new List<ComponentBase>();


            if (RuntimeDataComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("RuntimeDataComponent");
                RuntimeDataComponent tempComponent = tempComponentObj.AddComponent<RuntimeDataComponent>();
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempComponent.frameInit = RuntimeDataComponentEditor.isFrameInit;
                tempComponent.sceneInit = RuntimeDataComponentEditor.isSceneInit;
                tempComponent.componentIndex = RuntimeDataComponentEditor.componentIndex;
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (ResComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("ResComponent");
                ResComponent tempComponent = tempComponentObj.AddComponent<ResComponent>();
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempComponent.frameInit = ResComponentEditor.isFrameInit;
                tempComponent.sceneInit = ResComponentEditor.isSceneInit;
                tempComponent.componentIndex = ResComponentEditor.componentIndex;

                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (DownComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("DownComponent");
                DownComponent tempComponent = tempComponentObj.AddComponent<DownComponent>();
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempComponent.frameInit = ResComponentEditor.isFrameInit;
                tempComponent.sceneInit = ResComponentEditor.isSceneInit;
                tempComponent.sceneInit = ResComponentEditor.isSceneInit;
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (AudioComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("AudioComponent");
                AudioComponent tempComponent = tempComponentObj.AddComponent<AudioComponent>();
                tempComponent.frameInit = AudioComponentEditor.isFrameInit;
                tempComponent.sceneInit = AudioComponentEditor.isSceneInit;
                tempComponent.componentIndex = AudioComponentEditor.componentIndex;
                tempComponent.audioData =
                    AssetDatabase.LoadAssetAtPath<AudioComponentData>(General.customAudioDataPath);
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (ListenerComponentEditorComponent.Enabled)
            {
                GameObject tempComponentObj = new GameObject("ListenerComponent");
                ListenerComponent tempComponent = tempComponentObj.AddComponent<ListenerComponent>();
                tempComponent.frameInit = ListenerComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = ListenerComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = ListenerComponentEditorComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (CustomSceneLoadComponent.Enabled)
            {
                GameObject tempComponentObj = new GameObject("SceneLoadComponent");
                SceneLoadComponent tempComponent = tempComponentObj.AddComponent<SceneLoadComponent>();
                tempComponent.frameInit = CustomSceneLoadComponent.isFrameInit;
                tempComponent.sceneInit = CustomSceneLoadComponent.isSceneInit;
                tempComponent.componentIndex = CustomSceneLoadComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (TimeComponentEditorComponent.Enabled)
            {
                GameObject tempComponentObj = new GameObject("TimeComponent");
                TimeComponent tempComponent = tempComponentObj.AddComponent<TimeComponent>();
                tempComponent.frameInit = TimeComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = TimeComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = TimeComponentEditorComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (EntityComponentEditorComponent.Enabled)
            {
                GameObject tempComponentObj = new GameObject("EntityComponent");
                EntityComponent tempComponent = tempComponentObj.AddComponent<EntityComponent>();
                tempComponent.frameInit = EntityComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = EntityComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = EntityComponentEditorComponent.componentIndex;
                tempComponent.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (CircuitComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("CircuitComponent");
                CircuitComponent tempComponent = tempComponentObj.AddComponent<CircuitComponent>();
                tempComponent.frameInit = ViewComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = ViewComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = ViewComponentEditorComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (MouseComponentEditor.Enabled)
            {
                GameObject tempComponentObj = new GameObject("MouseComponent");
                MouseComponent tempComponent = tempComponentObj.AddComponent<MouseComponent>();
                tempComponent.frameInit = ViewComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = ViewComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = ViewComponentEditorComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }

            if (ViewComponentEditorComponent.Enabled)
            {
                GameObject tempComponentObj = new GameObject("ViewComponent");
                ViewComponent tempComponent = tempComponentObj.AddComponent<ViewComponent>();
                tempComponent.frameInit = ViewComponentEditorComponent.isFrameInit;
                tempComponent.sceneInit = ViewComponentEditorComponent.isSceneInit;
                tempComponent.componentIndex = ViewComponentEditorComponent.componentIndex;
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                tempGameRootStart.activeComponentBase.Add(tempComponent);
            }
        }

        [BoxGroup("Export")] [LabelText("导出路径")] [FolderPath(AbsolutePath = true)]
        public string ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

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
            _gameRootEditorEditorData = AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);
            if (_gameRootEditorEditorData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameRootEditorEditorData>(), General.customFrameDataPath);
                _gameRootEditorEditorData = AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _gameRootEditorEditorData.persistentDataComponentEditor = RuntimeDataComponentEditor.Enabled;
            _gameRootEditorEditorData.persistentDataComponentEditorFrameInit = RuntimeDataComponentEditor.isFrameInit;
            _gameRootEditorEditorData.persistentDataComponentEditorSceneInit = RuntimeDataComponentEditor.isSceneInit;

            _gameRootEditorEditorData.resComponentEditor = ResComponentEditor.Enabled;
            _gameRootEditorEditorData.resComponentEditorFrameInit = ResComponentEditor.isFrameInit;
            _gameRootEditorEditorData.resComponentEditorSceneInit = ResComponentEditor.isSceneInit;

            _gameRootEditorEditorData.downComponentEditor = DownComponentEditor.Enabled;
            _gameRootEditorEditorData.downComponentEditorFrameInit = DownComponentEditor.isFrameInit;
            _gameRootEditorEditorData.downComponentEditorSceneInit = DownComponentEditor.isSceneInit;

            _gameRootEditorEditorData.audioComponentEditor = AudioComponentEditor.Enabled;
            _gameRootEditorEditorData.audioComponentEditorFrameInit = AudioComponentEditor.isFrameInit;
            _gameRootEditorEditorData.audioComponentEditorSceneInit = AudioComponentEditor.isSceneInit;

            _gameRootEditorEditorData.listenerComponentEditorComponent = ListenerComponentEditorComponent.Enabled;
            _gameRootEditorEditorData.listenerComponentEditorFrameInit = ListenerComponentEditorComponent.isFrameInit;
            _gameRootEditorEditorData.listenerComponentEditorSceneInit = ListenerComponentEditorComponent.isSceneInit;

            _gameRootEditorEditorData.customSceneComponent = CustomSceneLoadComponent.Enabled;
            _gameRootEditorEditorData.customSceneComponentEditorFrameInit = CustomSceneLoadComponent.isFrameInit;
            _gameRootEditorEditorData.customSceneComponentEditorSceneInit = CustomSceneLoadComponent.isSceneInit;

            _gameRootEditorEditorData.timeComponentEditorComponent = TimeComponentEditorComponent.Enabled;
            _gameRootEditorEditorData.timeComponentEditorFrameInit = TimeComponentEditorComponent.isFrameInit;
            _gameRootEditorEditorData.timeComponentEditorSceneInit = TimeComponentEditorComponent.isSceneInit;

            _gameRootEditorEditorData.viewComponentEditorComponent = ViewComponentEditorComponent.Enabled;
            _gameRootEditorEditorData.viewComponentEditorFrameInit = ViewComponentEditorComponent.isFrameInit;
            _gameRootEditorEditorData.viewComponentEditorSceneInit = ViewComponentEditorComponent.isSceneInit;

            _gameRootEditorEditorData.entityComponentEditorComponent = EntityComponentEditorComponent.Enabled;
            _gameRootEditorEditorData.entityDataComponentEditorFrameInit = EntityComponentEditorComponent.isFrameInit;
            _gameRootEditorEditorData.entityDataComponentEditorSceneInit = EntityComponentEditorComponent.isSceneInit;

            _gameRootEditorEditorData.circuitComponentEditorComponent = CircuitComponentEditor.Enabled;
            _gameRootEditorEditorData.circuitDataComponentEditorFrameInit = CircuitComponentEditor.isFrameInit;
            _gameRootEditorEditorData.circuitDataComponentEditorSceneInit = CircuitComponentEditor.isSceneInit;

            _gameRootEditorEditorData.mouseComponentEditorComponent = MouseComponentEditor.Enabled;
            _gameRootEditorEditorData.mouseComponentEditorFrameInit = MouseComponentEditor.isFrameInit;
            _gameRootEditorEditorData.mouseComponentEditorSceneInit = MouseComponentEditor.isSceneInit;

            //标记脏区
            EditorUtility.SetDirty(_gameRootEditorEditorData);
        }

        public override void OnLoadConfig()
        {
            _gameRootEditorEditorData =
                AssetDatabase.LoadAssetAtPath<GameRootEditorEditorData>(General.customFrameDataPath);

            RuntimeDataComponentEditor.Enabled = _gameRootEditorEditorData.persistentDataComponentEditor;
            RuntimeDataComponentEditor.isFrameInit = _gameRootEditorEditorData.persistentDataComponentEditorFrameInit;
            RuntimeDataComponentEditor.isSceneInit = _gameRootEditorEditorData.persistentDataComponentEditorSceneInit;

            DownComponentEditor.Enabled = _gameRootEditorEditorData.downComponentEditor;
            DownComponentEditor.isFrameInit = _gameRootEditorEditorData.downComponentEditorFrameInit;
            DownComponentEditor.isSceneInit = _gameRootEditorEditorData.downComponentEditorSceneInit;

            ResComponentEditor.Enabled = _gameRootEditorEditorData.resComponentEditor;
            ResComponentEditor.isFrameInit = _gameRootEditorEditorData.resComponentEditorFrameInit;
            ResComponentEditor.isSceneInit = _gameRootEditorEditorData.resComponentEditorSceneInit;

            AudioComponentEditor.Enabled = _gameRootEditorEditorData.audioComponentEditor;
            AudioComponentEditor.isFrameInit = _gameRootEditorEditorData.audioComponentEditorFrameInit;
            AudioComponentEditor.isSceneInit = _gameRootEditorEditorData.audioComponentEditorSceneInit;

            ListenerComponentEditorComponent.Enabled = _gameRootEditorEditorData.listenerComponentEditorComponent;
            ListenerComponentEditorComponent.isFrameInit = _gameRootEditorEditorData.listenerComponentEditorFrameInit;
            ListenerComponentEditorComponent.isSceneInit = _gameRootEditorEditorData.listenerComponentEditorSceneInit;

            CustomSceneLoadComponent.Enabled = _gameRootEditorEditorData.customSceneComponent;
            CustomSceneLoadComponent.isFrameInit = _gameRootEditorEditorData.customSceneComponentEditorFrameInit;
            CustomSceneLoadComponent.isSceneInit = _gameRootEditorEditorData.customSceneComponentEditorSceneInit;

            TimeComponentEditorComponent.Enabled = _gameRootEditorEditorData.timeComponentEditorComponent;
            TimeComponentEditorComponent.isFrameInit = _gameRootEditorEditorData.timeComponentEditorFrameInit;
            TimeComponentEditorComponent.isSceneInit = _gameRootEditorEditorData.timeComponentEditorSceneInit;

            ViewComponentEditorComponent.Enabled = _gameRootEditorEditorData.viewComponentEditorComponent;
            ViewComponentEditorComponent.isFrameInit = _gameRootEditorEditorData.viewComponentEditorFrameInit;
            ViewComponentEditorComponent.isSceneInit = _gameRootEditorEditorData.viewComponentEditorSceneInit;

            EntityComponentEditorComponent.Enabled = _gameRootEditorEditorData.entityComponentEditorComponent;
            EntityComponentEditorComponent.isFrameInit = _gameRootEditorEditorData.entityDataComponentEditorFrameInit;
            EntityComponentEditorComponent.isSceneInit = _gameRootEditorEditorData.entityDataComponentEditorSceneInit;

            CircuitComponentEditor.Enabled = _gameRootEditorEditorData.circuitComponentEditorComponent;
            CircuitComponentEditor.isFrameInit = _gameRootEditorEditorData.circuitDataComponentEditorFrameInit;
            CircuitComponentEditor.isSceneInit = _gameRootEditorEditorData.circuitDataComponentEditorSceneInit;

            MouseComponentEditor.Enabled = _gameRootEditorEditorData.mouseComponentEditorComponent;
            MouseComponentEditor.isFrameInit = _gameRootEditorEditorData.mouseComponentEditorFrameInit;
            MouseComponentEditor.isSceneInit = _gameRootEditorEditorData.mouseComponentEditorSceneInit;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}