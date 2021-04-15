using System;
using System.Collections;
using System.Collections.Generic;
using CameraTools;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CameraPos : ScriptableObject
{
    [LabelText("相机位置")] public List<CameraPosInfo> cameraPosInfos;

    [Serializable]
    public class CameraPosInfo
    {
        public string infoName;
        [LabelText("寻路位置")] public Vector3 navPos;
        [LabelText("相机旋转")] public Vector3 cameraRotate;
        [LabelText("相机位置")] public Vector3 cameraPos;
        [LabelText("相机视野")] public float cameraFieldView;
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