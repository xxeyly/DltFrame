using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;
using Object = System.Object;

namespace XxSlitFrame.Tools.Svc
{
    [Serializable]
    public class EntitySvcDataInfo
    {
        [HideLabel] [HorizontalGroup("实体标签")] public string entityGroupTag;
        [HideLabel] [HorizontalGroup("实体组")] public List<EntityItem> entityGroup;
    }

    [Serializable]
    public class EditorEntitySvcDataInfo
    {
        [HideLabel] [HorizontalGroup("实体标签")] public string entityGroupTag;
        [HideLabel] [HorizontalGroup("实体组")] public List<EntityItem> entityGroup;

        [HideLabel]  [Button("仅显示当前组")]
        public void OnOnlyShow()
        {
          UnityEngine.Object.FindObjectOfType<EntitySvc>().DisplayEditorEntityGroup(entityGroupTag, true, true);
        }
    }

    public partial class EntitySvc : SvcBase
    {
        public static EntitySvc Instance;
        [Searchable] [LabelText("场景所有实体")] public List<EntityItem> sceneEntity;

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("实体组")]
        public List<EntitySvcDataInfo> entitySvcDataInfos;

        [TableList(AlwaysExpanded = true, DrawScrollView = false)] [LabelText("编辑器实体组")]
        public List<EditorEntitySvcDataInfo> editorEntitySvcDataInfos;

        public void TryAddEntity(EntityItem entityItem)
        {
            //读取添加的每个实体的所有实体组
            for (int i = 0; i < entityItem.entityTags.Count; i++)
            {
                bool isCon = false;
                foreach (EntitySvcDataInfo entitySvcDataInfo in entitySvcDataInfos)
                {
                    if (entitySvcDataInfo.entityGroupTag == entityItem.entityTags[i])
                    {
                        isCon = true;
                        entitySvcDataInfo.entityGroup.Add(entityItem);
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
#if UNITY_EDITOR
            for (int i = 0; i < entityItem.editorEntityTags.Count; i++)
            {
                bool isCon = false;
                foreach (EditorEntitySvcDataInfo editorEntitySvcDataInfo in editorEntitySvcDataInfos)
                {
                    if (editorEntitySvcDataInfo.entityGroupTag == entityItem.editorEntityTags[i])
                    {
                        isCon = true;
                        editorEntitySvcDataInfo.entityGroup.Add(entityItem);
                    }
                }

                if (!isCon)
                {
                    editorEntitySvcDataInfos.Add(new EditorEntitySvcDataInfo()
                    {
                        entityGroupTag = entityItem.editorEntityTags[i],
                        entityGroup = new List<EntityItem>() {entityItem}
                    });
                }
            }
#endif
        }

        public override void StartSvc()
        {
            Instance = GetComponent<EntitySvc>();
        }

        public override void InitSvc()
        {
            EntityInit();
        }

        public override void EndSvc()
        {
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Medium)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            entitySvcDataInfos.Clear();
#if UNITY_EDITOR
            editorEntitySvcDataInfos.Clear();
#endif
            // sceneEntity = new List<EntityItem>(GameObject.FindObjectsOfType<EntityItem>());
            sceneEntity = DataSvc.GetAllObjectsInScene<EntityItem>();
            foreach (EntityItem entityItem in sceneEntity)
            {
                TryAddEntity(entityItem);
            }
        }


        /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="display"></param>
        /// <param name="groupTag"></param>
        public void DisplayEntityGroup(bool display, params string[] groupTag)
        {
            foreach (string groupName in groupTag)
            {
                DisplayEntityGroup(groupName, display, false);
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
        }  /// <summary>
        /// 实体组控制
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="display"></param>
        /// <param name="hideOther"></param>
        public void DisplayEditorEntityGroup(string groupTag, bool display, bool hideOther = false)
        {
            List<EntityItem> entityGroup = null;
            foreach (EditorEntitySvcDataInfo entitySvcDataInfo in editorEntitySvcDataInfos)
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

        public List<EntityItem> GetEntityItemByEntityGroupName(string groupName)
        {
            foreach (EntitySvcDataInfo entitySvcDataInfo in entitySvcDataInfos)
            {
                if (entitySvcDataInfo.entityGroupTag == groupName)
                {
                    return entitySvcDataInfo.entityGroup;
                }
            }

            return null;
        }

        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        public void EntityAllHide()
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                entityItem.Hide();
            }
        }

        /// <summary>
        /// 实体全部显示
        /// </summary>
        public void EntityAllShow()
        {
            foreach (EntitySvcDataInfo entitySvcDataInfo in entitySvcDataInfos)
            {
                //先显示
                foreach (EntityItem entityItem in entitySvcDataInfo.entityGroup)
                {
                    entityItem.Show();
                }
            }
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetFirstEntityItemByName<T>(string entityName) where T : MonoBehaviour
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.GetComponent<T>();
                }
            }

            return null;
        }

        public EntityItem GetFirstEntityItemByName(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.GetComponent<EntityItem>();
                }
            }

            return null;
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    entityItem.gameObject.SetActive(display);
                }
            }
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        public void DisplayEntityByEntityName(bool display, params string[] entityNames)
        {
            foreach (string entityName in entityNames)
            {
                foreach (EntityItem entityItem in sceneEntity)
                {
                    if (entityName == entityItem.entityName)
                    {
                        entityItem.gameObject.SetActive(display);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool GetFirstEntityStateByEntityName(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.gameObject.activeSelf;
                }
            }

            return false;
        }
    }
}