using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Model.ConfigData.Editor
{
    /// <summary>
    /// 框架生成服务配置数据
    /// </summary>
    [Serializable]
    public class GameRootEditorEditorData : ScriptableObject
    {
        //是否激活
        [LabelText("持久化服务")] public bool persistentDataSvcEditor;
        [LabelText("资源服务")] public bool resSvcEditor;
        [LabelText("音频服务")] public bool audioSvcEditor;
        [LabelText("监听服务")] public bool listenerSvcEditorSvc;
        [LabelText("场景服务")] public bool customSceneSvc;
        [LabelText("计时器服务")] public bool timeSvcEditorSvc;
        [LabelText("视图服务")] public bool viewSvcEditorSvc;

        [LabelText("实体服务")] public bool entitySvcEditorSvc;

        //是否开启框架初始化
        [LabelText("资源框架初始化")] public bool resSvcEditorFrameInit;
        [LabelText("音频框架初始化")] public bool audioSvcEditorFrameInit;
        [LabelText("监听框架初始化")] public bool listenerSvcEditorFrameInit;
        [LabelText("场景框架初始化")] public bool customSceneSvcEditorFrameInit;
        [LabelText("计时器框架初始化")] public bool timeSvcEditorFrameInit;
        [LabelText("视图框架初始化")] public bool viewSvcEditorFrameInit;
        [LabelText("持久化框架初始化")] public bool persistentDataSvcEditorFrameInit;
        [LabelText("实体框架初始化")] public bool entityDataSvcEditorFrameInit;

        //是否开启场景初始化
        [LabelText("资源场景初始化")] public bool resSvcEditorSceneInit;
        [LabelText("音频场景初始化")] public bool audioSvcEditorSceneInit;
        [LabelText("监听场景初始化")] public bool listenerSvcEditorSceneInit;
        [LabelText("场景场景初始化")] public bool customSceneSvcEditorSceneInit;
        [LabelText("计时器场景初始化")] public bool timeSvcEditorSceneInit;
        [LabelText("视图场景初始化")] public bool viewSvcEditorSceneInit;
        [LabelText("持久化场景初始化")] public bool persistentDataSvcEditorSceneInit;
        [LabelText("实体场景初始化")] public bool entityDataSvcEditorSceneInit;
    }
}