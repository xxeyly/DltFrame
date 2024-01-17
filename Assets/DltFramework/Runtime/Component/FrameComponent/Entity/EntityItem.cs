using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [Serializable]
    public partial class EntityItem : MonoBehaviour
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

        public void Show()
        {
            if (isLog)
            {
                Log(entityName + ":" + "显示");
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
                Log(entityName + ":" + "隐藏");
            }

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        protected void EntityAllHide()
        {
            EntityFrameComponent.Instance.EntityAllHide();
        }

        /// <summary>
        /// 实体全部显示
        /// </summary>
        protected void EntityAllShow()
        {
            EntityFrameComponent.Instance.EntityAllShow();
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetEntity<T>(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity<T>(entityName);
        }

        public EntityItem GetEntity(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity(entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        protected void DisplayEntity(bool display, string entityName)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        protected void DisplayEntity(bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityNames);
        }

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        protected bool GetEntityState(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntityState(entityName);
        }
    }
}