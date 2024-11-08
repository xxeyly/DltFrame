using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    /// <summary>
    /// 实体
    /// </summary>
    [Serializable]
    public partial class EntityItem : ExtendMonoBehaviour
    {
        [LabelText("实体名称")] public string entityName;
        [LabelText("描述名称")] public string descriptionName;
        [LabelText("日志输出")] public bool isLog;
        [LabelText("实体标签")] public List<string> entityTags;
#if UNITY_EDITOR
        [LabelText("编辑器实体标签")] public List<string> editorEntityTags;
#endif

        [GUIColor(0, 1, 0)]
        [Button("设置为当前物体名称", ButtonSizes.Large)]
        public void SetCurrentGameObjectName()
        {
            entityName = gameObject.name;
        }

        /// <summary>
        /// 添加到实体列表
        /// </summary>
        public void AddToEntityList()
        {
            if (EntityFrameComponent.Instance == null)
            {
                return;
            }

            if (!EntityFrameComponent.Instance.sceneEntity.Contains(this))
            {
                EntityFrameComponent.Instance.sceneEntity.Add(this);
            }
            else
            {
                // Debug.Log(name + "已添加");
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
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

        /// <summary>
        /// 隐藏
        /// </summary>
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