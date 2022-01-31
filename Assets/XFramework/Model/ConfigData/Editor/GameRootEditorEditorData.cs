#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 框架生成组件配置数据
    /// </summary>
    [Serializable]
    public class GameRootEditorEditorData : ScriptableObject
    {
        //是否激活
        [LabelText("持久化组件")] public bool persistentDataComponentEditor;
        [LabelText("资源组件")] public bool resComponentEditor;
        [LabelText("下载组件")] public bool downComponentEditor;
        [LabelText("音频组件")] public bool audioComponentEditor;
        [LabelText("监听组件")] public bool listenerComponentEditorComponent;
        [LabelText("场景组件")] public bool customSceneComponent;
        [LabelText("计时器组件")] public bool timeComponentEditorComponent;
        [LabelText("视图组件")] public bool viewComponentEditorComponent;
        [LabelText("实体组件")] public bool entityComponentEditorComponent;
        [LabelText("流程组件")] public bool circuitComponentEditorComponent;
        [LabelText("流程组件")] public bool mouseComponentEditorComponent;

        //是否开启框架初始化
        [LabelText("资源框架初始化")] public bool resComponentEditorFrameInit;
        [LabelText("下载框架初始化")] public bool downComponentEditorFrameInit;
        [LabelText("音频框架初始化")] public bool audioComponentEditorFrameInit;
        [LabelText("监听框架初始化")] public bool listenerComponentEditorFrameInit;
        [LabelText("场景框架初始化")] public bool customSceneComponentEditorFrameInit;
        [LabelText("计时器框架初始化")] public bool timeComponentEditorFrameInit;
        [LabelText("视图框架初始化")] public bool viewComponentEditorFrameInit;
        [LabelText("持久化框架初始化")] public bool persistentDataComponentEditorFrameInit;
        [LabelText("实体框架初始化")] public bool entityDataComponentEditorFrameInit;
        [LabelText("流程框架初始化")] public bool circuitDataComponentEditorFrameInit;
        [LabelText("鼠标框架初始化")] public bool mouseComponentEditorFrameInit;

        //是否开启场景初始化
        [LabelText("资源场景初始化")] public bool resComponentEditorSceneInit;
        [LabelText("下载场景初始化")]  public bool downComponentEditorSceneInit;
        [LabelText("音频场景初始化")] public bool audioComponentEditorSceneInit;
        [LabelText("监听场景初始化")] public bool listenerComponentEditorSceneInit;
        [LabelText("场景场景初始化")] public bool customSceneComponentEditorSceneInit;
        [LabelText("计时器场景初始化")] public bool timeComponentEditorSceneInit;
        [LabelText("视图场景初始化")] public bool viewComponentEditorSceneInit;
        [LabelText("持久化场景初始化")] public bool persistentDataComponentEditorSceneInit;
        [LabelText("实体场景初始化")] public bool entityDataComponentEditorSceneInit;
        [LabelText("流程场景初始化")] public bool circuitDataComponentEditorSceneInit;
        [LabelText("鼠标场景初始化")] public bool mouseComponentEditorSceneInit;
    }
}
#endif
