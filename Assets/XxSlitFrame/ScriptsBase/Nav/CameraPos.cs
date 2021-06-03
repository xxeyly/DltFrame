using System;
using System.Collections;
using System.Collections.Generic;
using CameraTools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CameraPos : ScriptableObject
{
    [LabelText("相机位置")] [Searchable] [TableList(AlwaysExpanded = true)]
    public List<CameraPosInfo> cameraPosInfos;

    [Serializable]
    public class CameraPosInfo
    {
        [HideLabel] [HorizontalGroup("标记名称")] public string infoName;
        [HideLabel] [HorizontalGroup("寻路位置")] public Vector3 navPos;
        [HideLabel] [HorizontalGroup("相机旋转")] public Vector3 cameraRotate;
        [HideLabel] [HorizontalGroup("相机位置")] public Vector3 cameraPos;
        [HideLabel] [HorizontalGroup("相机视野")] public float cameraFieldView;
    }

    /// <summary>
    /// 返回相机信息
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CameraPosInfo GetCameraPosInfoByName(string name)
    {
        foreach (CameraPosInfo cameraPosInfo in cameraPosInfos)
        {
            if (cameraPosInfo.infoName == name)
            {
                return cameraPosInfo;
            }
        }

        return null;
    }
}