using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    public class GameRootEditorData : ScriptableObject
    {
        [LabelText("持久化服务")] public bool persistentDataSvcEditor;
        [LabelText("资源服务")] public bool resSvcEditor;
        [LabelText("音频服务")] public bool audioSvcEditor;
        [LabelText("监听服务")] public bool listenerSvcEditorSvc;
        [LabelText("场景服务")] public bool customSceneSvc;
        [LabelText("计时器服务")] public bool timeSvcEditorSvc;
        [LabelText("视图服务")] public bool viewSvcEditorSvc;

        [LabelText("资源服务初始化")] public bool resSvcEditorInit;
        [LabelText("音频服务初始化")] public bool audioSvcEditorInit;
        [LabelText("监听服务初始化")] public bool listenerSvcEditorSvcInit;
        [LabelText("场景服务初始化")] public bool customSceneSvcInit;
        [LabelText("计时器服务初始化")] public bool timeSvcEditorSvcInit;
        [LabelText("视图服务初始化")] public bool viewSvcEditorSvcInit;
        [LabelText("持久化服务初始化")] public bool persistentDataSvcEditorInit;
    }
}