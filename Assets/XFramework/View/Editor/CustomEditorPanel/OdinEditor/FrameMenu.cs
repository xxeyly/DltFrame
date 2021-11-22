using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class FrameMenu : OdinMenuEditorWindow
    {
        private FrameMenu()
        {
        }

        [MenuItem("Xframe/框架界面")]
        private static void OpenWindow()
        {
            GetWindow<FrameMenu>().Show();
        }

        [MenuItem("Xframe/一键打包")]
        private static void StartBuild()
        {
            if (customBuild != null)
            {
                customBuild.StartBuild();
            }
        }

        [MenuItem("Xframe/监听生成")]
        private static void OnListenerGenerate()
        {
            ListenerSvcGenerateData listenerSvcGenerateData = DataSvc.GetObjectsInScene<ListenerSvcGenerateData>();
            if (listenerSvcGenerateData != null)
            {
                listenerSvcGenerateData.OnGenerate();
                Debug.Log("监听生成完毕");
            }
            else
            {
                Debug.LogWarning(" ListenerSvc未添加");
            }
        }

        [MenuItem("Xframe/框架生成")]
        private static void FrameBuild()
        {
            if (gameRootEditor != null)
            {
                gameRootEditor.Generate();
            }
        }

        [MenuItem("Xframe/打包异步场景")]
        private static void BuildAsyncScene()
        {
            if (sceneLoad == null)
            {
                SceneLoad tempSceneLoad = new SceneLoad();
                tempSceneLoad.OnInit();
                tempSceneLoad.BuildSyncScene();
            }
            else
            {
                sceneLoad.BuildSyncScene();
            }
        }

        //打包
        private static OdinCustomBuild customBuild = new OdinCustomBuild();

        //音频服务
        private AudioSvcEditor audioSvcEditor = new AudioSvcEditor();

        //框架配置
        private static GameRootEditor gameRootEditor;
        private GenerateBaseWindowEditor generateBaseWindowEditor = new GenerateBaseWindowEditor();
        private static SceneLoad sceneLoad;

        //可持久化
        RuntimeDataSvcEditor runtimeDataSvcEditor = new RuntimeDataSvcEditor();

        //资源服务
        ResSvcEditor resSvcEditor = new ResSvcEditor();

        //监听服务
        ListenerSvcEditor listenerSvcEditor = new ListenerSvcEditor();

        //场景服务
        SceneSvcEditor sceneSvcEditor = new SceneSvcEditor();

        //计时器服务
        TimeSvcEditor timeSvcEditor = new TimeSvcEditor();

        //视图服务
        ViewSvcEditor viewSvcEditor = new ViewSvcEditor();

        //实体服务
        EntitySvcEditor entitySvcEditor = new EntitySvcEditor();

        //流程服务
        CircuitSvcEditor circuitSvcEditor = new CircuitSvcEditor();

        //鼠标服务
        MouseSvcEditor mouseSvcEditor = new MouseSvcEditor();

        //生成配置
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            //框架配置
            gameRootEditor = new GameRootEditor(runtimeDataSvcEditor, resSvcEditor, audioSvcEditor, listenerSvcEditor, sceneSvcEditor, timeSvcEditor, entitySvcEditor, viewSvcEditor,
                circuitSvcEditor, mouseSvcEditor);
            ResourceUnification resourceUnification = new ResourceUnification();
            sceneLoad = new SceneLoad();
            sceneLoad.OnInit();
            customBuild.OnInit();
            gameRootEditor.OnInit();
            audioSvcEditor.OnInit();
            generateBaseWindowEditor.OnInit();
            resourceUnification.OnInit();
            customBuild.AfferentSceneLoad(sceneLoad);
            tree.Add("打包工具", customBuild);
            tree.Add("场景编辑", sceneLoad);
            tree.Add("框架服务", gameRootEditor);
            tree.Add("音频配置", audioSvcEditor);
            tree.Add("生成配置", generateBaseWindowEditor);
            tree.Add("资源统一化", resourceUnification);
            tree.Add("场景工具", new SceneTools());
            return tree;
        }

        private void OnDisable()
        {
            customBuild.OnDisable();
            audioSvcEditor.OnDisable();
            gameRootEditor.OnDisable();
            generateBaseWindowEditor.OnDisable();
            sceneLoad.OnDisable();
        }
    }
}