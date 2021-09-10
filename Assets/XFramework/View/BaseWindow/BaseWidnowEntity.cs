using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            EntitySvc.Instance.DisplayEntityGroup(groupTag, display, hideOther);
        }

        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEntityGroup(bool display, params string[] groupTag)
        {
            EntitySvc.Instance.DisplayEntityGroup(display, groupTag);
        }

        public List<EntityItem> GetEntityItemByEntityGroupName(string groupName)
        {
            return EntitySvc.Instance.GetEntityItemByEntityGroupName(groupName);
        }

        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        public void EntityAllHide()
        {
            EntitySvc.Instance.EntityAllHide();
        }

        /// <summary>
        /// 实体全部显示
        /// </summary>
        public void EntityAllShow()
        {
            EntitySvc.Instance.EntityAllShow();
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFirstEntityItemByName<T>(string entityName) where T : MonoBehaviour
        {
            return EntitySvc.Instance.GetFirstEntityItemByName<T>(entityName);
        }

        public EntityItem GetFirstEntityItemByName(string entityName)
        {
            return EntitySvc.Instance.GetFirstEntityItemByName(entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, string entityName)
        {
            EntitySvc.Instance.DisplayEntityByEntityName(display, entityName);
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, params string[] entityNames)
        {
            EntitySvc.Instance.DisplayEntityByEntityName(display, entityNames);
        }

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool GetFirstEntityStateByEntityName(string entityName)
        {
            return EntitySvc.Instance.GetFirstEntityStateByEntityName(entityName);
        }
    }
}