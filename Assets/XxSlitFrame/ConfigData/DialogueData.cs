using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对话角色
/// </summary>
public enum Role
{
    Doctor = 0,
    Patient = 1
}

/// <summary>
/// 对话长短
/// </summary>
public enum Length
{
    EShort = 0,
    EIn = 1,
    ELong = 2
}

namespace XxSlitFrame.ConfigData
{
    [Serializable]
    public class DialogueData : ScriptableObject
    {
        [Header("对话片段")] public List<DialogDataInfoContainer> dataInfos;
    }

    [Serializable]
    public class DialogDataInfoContainer
    {
        [Header("对话内容组")] public List<DialogDataInfo> dialogDataInfos;
    }

    [Serializable]
    public class DialogDataInfo
    {
        [Header("对话角色")] public Role role;
        [Header("对话长度")] public Length length;
        [TextArea] [Header("对话内容")] public string dialogContent;
        [Header("对话音频")] public AudioClip dialogueAudioClip;
        [Header("对应时长")] public float dialogueLength;
    }
}