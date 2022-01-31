#if UNITY_EDITOR

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
            _audioComponentEditor.OnDisable();
            customBuild.OnDisable();
            gameRootEditor.OnDisable();
            generateBaseWindowEditor.OnDisable();
            sceneLoad.OnDisable();
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Xframe/监听生成 &l")]
        private static void OnListenerGenerate()
        {
            ListenerComponentGenerateData listenerComponentGenerateData =
                DataComponent.GetObjectsInScene<ListenerComponentGenerateData>();
            if (listenerComponentGenerateData != null)
            {
                listenerComponentGenerateData.OnGenerate();
                Debug.Log("监听生成完毕");
            }
            else
            {
                Debug.LogWarning(" ListenerComponent未添加");
            }
        }

        //打包
        private static CustomBuild customBuild = new CustomBuild();

        //音频组件
        private AudioComponentEditor _audioComponentEditor = new AudioComponentEditor();

        //框架配置
        private static GameRootEditor gameRootEditor;
        private GenerateBaseWindowEditor generateBaseWindowEditor = new GenerateBaseWindowEditor();
        private static SceneLoad sceneLoad = new SceneLoad();

        //可持久化
        RuntimeDataComponentEditor _runtimeDataComponentEditor = new RuntimeDataComponentEditor();

        //资源组件
        ResComponentEditor _resComponentEditor = new ResComponentEditor();

        //下载组件
        DownComponentEditor _downComponentEditor = new DownComponentEditor();

        //监听组件
        ListenerComponentEditor _listenerComponentEditor = new ListenerComponentEditor();

        //场景组件
        SceneLoadComponentEditor _sceneLoadComponentEditor = new SceneLoadComponentEditor();

        //计时器组件
        TimeComponentEditor _timeComponentEditor = new TimeComponentEditor();

        //视图组件
        ViewComponentEditor _viewComponentEditor = new ViewComponentEditor();

        //实体组件
        EntityComponentEditor _entityComponentEditor = new EntityComponentEditor();

        //流程组件
        CircuitComponentEditor _circuitComponentEditor = new CircuitComponentEditor();

        //鼠标组件
        MouseComponentEditor _mouseComponentEditor = new MouseComponentEditor();

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
            gameRootEditor = new GameRootEditor(_runtimeDataComponentEditor, _resComponentEditor, _downComponentEditor,
                _audioComponentEditor, _listenerComponentEditor, _sceneLoadComponentEditor, _timeComponentEditor,
                _entityComponentEditor, _viewComponentEditor,
                _circuitComponentEditor, _mouseComponentEditor);
            sceneLoad.OnInit();
            customBuild.OnInit();
            gameRootEditor.OnInit();
            _audioComponentEditor.OnInit();
            generateBaseWindowEditor.OnInit();
            resourceUnification.OnInit();
            customBuild.AfferentSceneLoad(sceneLoad);
            tree.Add("打包工具", customBuild);
            tree.Add("场景编辑", sceneLoad);
            tree.Add("框架组件", gameRootEditor);
            tree.Add("音频配置", _audioComponentEditor);
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
#endif