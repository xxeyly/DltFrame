using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace XFramework
{
    public class FrameMenu : OdinMenuEditorWindow
    {
        [MenuItem("Xframe/框架")]
        private static void OpenWindow()
        {
            GetWindow<FrameMenu>().Show();
        }

        //打包
        private OdinCustomBuild customBuild;

        //音频服务
        private AudioSvcEditor audioSvcEditor;

        //框架配置
        private GameRootEditor gameRootEditor;
        private GenerateBaseWindowEditor generateBaseWindowEditor;
        private SceneLoad sceneLoad;

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            //打包
            customBuild = new OdinCustomBuild();
            //可持久化
            PersistentDataSvcEditor persistentDataSvcEditor = new PersistentDataSvcEditor();
            //资源服务
            ResSvcEditor resSvcEditor = new ResSvcEditor();
            //音频服务
            audioSvcEditor = new AudioSvcEditor();
            //监听服务
            ListenerSvcEditor listenerSvcEditor = new ListenerSvcEditor();
            //场景服务
            SceneSvcEditor sceneSvcEditor = new SceneSvcEditor();
            //计时器服务
            TimeSvcEditor timeSvcEditor = new TimeSvcEditor();
            //视图服务
            ViewSvcEditor viewSvcEditor = new ViewSvcEditor();
            //实体服务
            EntityBaseSvcEditor entityBaseSvcEditor = new EntityBaseSvcEditor();
            //流程服务
            CircuitSvcEditor circuitSvcEditor = new CircuitSvcEditor();
            //鼠标服务
            MouseSvcEditor mouseSvcEditor = new MouseSvcEditor();
            //生成配置
            generateBaseWindowEditor = new GenerateBaseWindowEditor();
            //框架配置
            gameRootEditor = new GameRootEditor(persistentDataSvcEditor, resSvcEditor, audioSvcEditor, listenerSvcEditor, sceneSvcEditor, timeSvcEditor, entityBaseSvcEditor, viewSvcEditor,
                circuitSvcEditor,mouseSvcEditor);
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