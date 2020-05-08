using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "CameraPosData", menuName = "配置文件/相机位置数据", order = 1)]
    public class CameraPosData : ScriptableObject
    {
        /// <summary>
        /// 相机位置类型
        /// </summary>
        public enum CameraPosType
        {
            默认位置,
            位置1,
            位置2,
            位置3
        }

        /// <summary>
        /// 相机位置信息
        /// </summary>
        public CameraPosType currentCameraPosType;

        [Header("相机信息组")] public List<CameraPosInfo> cameraPosInfosGroup;

        /// <summary>
        /// 获得当前位置信息
        /// </summary>
        /// <param name="cameraPosType"></param>
        /// <returns></returns>
        public CameraPosInfo GetCameraPosInfoByCameraPosType(CameraPosType cameraPosType)
        {
            foreach (CameraPosInfo posInfo in cameraPosInfosGroup)
            {
                if (posInfo.cameraPosType == cameraPosType)
                {
                    return posInfo;
                }
            }

            return new CameraPosInfo();
        }

        public void SetCameraPosInfo(Vector3 navMeshAgentPos, Vector3 cameraPos, Vector3 cameraRot)
        {
            Debug.Log("记录位置信息");
            foreach (CameraPosInfo posInfo in cameraPosInfosGroup)
            {
                if (posInfo.cameraPosType == currentCameraPosType)
                {
                    posInfo.navMeshAgentPos = navMeshAgentPos;
                    posInfo.cameraPos = cameraPos;
                    posInfo.cameraRot = cameraRot;
                    break;
                }
            }
        }
    }

    [Serializable]
    public class CameraPosInfo
    {
        public CameraPosData.CameraPosType cameraPosType;

        /// <summary>
        /// 寻路位置
        /// </summary>
        public Vector3 navMeshAgentPos;

        /// <summary>
        /// 相机位置
        /// </summary>
        public Vector3 cameraPos;

        /// <summary>
        /// 相机旋转
        /// </summary>
        public Vector3 cameraRot;
    }
}