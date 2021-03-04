using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomBuild;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.GameRoot;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.AudioSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.Listener;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.ResSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.SceneSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.TimeSvc;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.ViewSvc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor
{
    public class FrameMenu : OdinMenuEditorWindow
    {
        [MenuItem("My Game/My Editor")]
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

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            //数据配置
            CustomScriptableObject.CustomScriptableObject customScriptableObject =
                new CustomScriptableObject.CustomScriptableObject();
            //打包
            customBuild = new OdinCustomBuild(customScriptableObject);
            //资源服务
            ResSvcEditor resSvcEditor = new ResSvcEditor();
            //音频服务
            audioSvcEditor = new AudioSvcEditor(customScriptableObject);
            //监听服务
            ListenerSvcEditor listenerSvcEditor = new ListenerSvcEditor();
            //场景服务
            SceneSvcEditor sceneSvcEditor = new SceneSvcEditor();
            //计时器服务
            TimeSvcEditor timeSvcEditor = new TimeSvcEditor();
            //视图服务
            ViewSvcEditor viewSvcEditor = new ViewSvcEditor();
            //框架配置
            gameRootEditor = new GameRootEditor(customScriptableObject, resSvcEditor, audioSvcEditor,
                listenerSvcEditor, sceneSvcEditor,
                timeSvcEditor, viewSvcEditor);
            tree.Add("打包工具", customBuild);
            tree.Add("框架服务", gameRootEditor);
            tree.Add("音频配置", audioSvcEditor);
            tree.Add("配置文件", customScriptableObject);
            return tree;
        }

        private void OnDisable()
        {
            customBuild.OnDisable();
            audioSvcEditor.OnDisable();
            gameRootEditor.OnDisable();
        }
    }
}