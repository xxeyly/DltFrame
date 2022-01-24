using System;
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            audioSvcEditor.OnDisable();
            customBuild.OnDisable();
            gameRootEditor.OnDisable();
            generateBaseWindowEditor.OnDisable();
            sceneLoad.OnDisable();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Xframe/监听生成 &l")]
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

        //打包
        private static CustomBuild customBuild = new CustomBuild();

        //音频服务
        private AudioSvcEditor audioSvcEditor = new AudioSvcEditor();

        //框架配置
        private static GameRootEditor gameRootEditor;
        private GenerateBaseWindowEditor generateBaseWindowEditor = new GenerateBaseWindowEditor();
        private static SceneLoad sceneLoad = new SceneLoad();

        //可持久化
        RuntimeDataSvcEditor runtimeDataSvcEditor = new RuntimeDataSvcEditor();

        //资源服务
        ResSvcEditor resSvcEditor = new ResSvcEditor();

        //下载服务
        DownSvcEditor downSvcEditor = new DownSvcEditor();

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

        //资源统一化
        ResourceUnification resourceUnification = new ResourceUnification();

        //动画工具
        AnimTools animTools = new AnimTools();

        //生成配置
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            //框架配置
            gameRootEditor = new GameRootEditor(runtimeDataSvcEditor, resSvcEditor, downSvcEditor, audioSvcEditor, listenerSvcEditor, sceneSvcEditor, timeSvcEditor, entitySvcEditor, viewSvcEditor,
                circuitSvcEditor, mouseSvcEditor);
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
            tree.Add("动画工具", animTools);
            return tree;
        }


        private void OnDisable()
        {
        }
    }
}