using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    [Serializable]
    public class EntitySvcDataInfo
    {
        [HideLabel] [HorizontalGroup("实体标签")] public string entityGroupTag;
        [HideLabel] [HorizontalGroup("实体组")] public List<EntityItem> entityGroup;
    }

    public class EntitySvc : SvcBase
    {
        public static EntitySvc Instance;
        [Searchable] [LabelText("场景所有实体")] public List<EntityItem> sceneEntity;

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("实体组")]
        public List<EntitySvcDataInfo> entitySvcDataInfos;

        public void TryAddEntity(EntityItem entityItem)
        {
            bool isCon = false;
            for (int i = 0; i < entityItem.entityTags.Count; i++)
            {
                foreach (EntitySvcDataInfo entitySvcDataInfo in entitySvcDataInfos)
                {
                    if (entitySvcDataInfo.entityGroupTag == entityItem.entityTags[i])
                    {
                        isCon = true;
                        entitySvcDataInfo.entityGroup.Add(entityItem);
                        break;
                    }
                }

                if (!isCon)
                {
                    entitySvcDataInfos.Add(new EntitySvcDataInfo()
                    {
                        entityGroupTag = entityItem.entityTags[i],
                        entityGroup = new List<EntityItem>() {entityItem}
                    });
                }
            }
        }

        public override void StartSvc()
        {
            Instance = GetComponent<EntitySvc>();
        }

        public override void InitSvc()
        {
            EntityInit();
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Medium)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            entitySvcDataInfos.Clear();
            sceneEntity = new List<EntityItem>(GameObject.FindObjectsOfType<EntityItem>());
            foreach (EntityItem entityItem in sceneEntity)
            {
                TryAddEntity(entityItem);
            }
        }

        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            List<EntityItem> entityGroup = null;
            foreach (EntitySvcDataInfo entitySvcDataInfo in entitySvcDataInfos)
            {
                if (entitySvcDataInfo.entityGroupTag == groupTag)
                {
                    entityGroup = entitySvcDataInfo.entityGroup;
                    break;
                }
            }

            if (entityGroup == null)
            {
                Debug.LogError($"实体组{groupTag}未定义");
                return;
            }

            //先显示
            foreach (EntityItem entityItem in entityGroup)
            {
                if (display)
                {
                    entityItem.Show();
                }
                else
                {
                    entityItem.Hide();
                }
            }

            if (hideOther)
            {
                //创建一个临时场景实体组
                List<EntityItem> tempSceEntityItems = new List<EntityItem>();
                foreach (EntityItem entityItem in sceneEntity)
                {
                    tempSceEntityItems.Add(entityItem);
                }

                //移除当前要显示的实体
                foreach (EntityItem entityItem in entityGroup)
                {
                    tempSceEntityItems.Remove(entityItem);
                }

                //隐藏剩余实体
                foreach (EntityItem tempSceEntityItem in tempSceEntityItems)
                {
                    tempSceEntityItem.Hide();
                }

                //清空
                tempSceEntityItems.Clear();
            }
        }
    }
}