﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.Svc
{
    [Serializable]
    public class EntityItem : StartSingleton
    {
        [LabelText("实体名称")] public string entityName;
        [LabelText("日志输出")] public bool isLog;
        [LabelText("实体标签")] public List<string> entityTags;
#if UNITY_EDITOR
        [LabelText("编辑器实体标签")] public List<string> editorEntityTags;
#endif

        [GUIColor(0, 1, 0)]
        [Button(ButtonSizes.Large)]
        [LabelText("设置为当前物体名称")]
        public void GetCurrentGameObjectName()
        {
            entityName = gameObject.name;
        }

        public override void StartSvc()
        {
        }

        public override void Init()
        {
        }

        public void Show()
        {
            if (isLog)
            {
                Debug.Log(entityName + ":" + "显示");
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (isLog)
            {
                Debug.Log(entityName + ":" + "隐藏");
            }

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }
}