using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [LabelText("场景重复实体")]
    [Serializable]
    public struct SceneRepeatEntity
    {
        [LabelText("实体名称")] public string entityName;
        [LabelText("实体列表")] public List<GameObject> entityList;
    }

    public partial class EntityFrameComponent : FrameComponent
    {
        public static EntityFrameComponent Instance;
        [Searchable] [LabelText("场景所有实体")] public List<EntityItem> sceneEntity;
        [LabelText("场景中重复名实体")] public List<SceneRepeatEntity> sceneRepeatEntityList;

        public GameObject Instantiate(GameObject instantiate)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate);
            InstantiateInit(tempInstantiate);
            return tempInstantiate;
        }

        public GameObject Instantiate(GameObject instantiate, Transform parent, bool world)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate, parent, world);
            InstantiateInit(tempInstantiate);

            return tempInstantiate;
        }

        private void InstantiateInit(GameObject instantiate)
        {
            foreach (EntityItem entityItem in instantiate.transform.GetComponentsInChildren<EntityItem>())
            {
                entityItem.AddToEntityList();
            }

            foreach (AnimatorControllerBase animatorControllerBase in instantiate.transform.GetComponentsInChildren<AnimatorControllerBase>())
            {
                animatorControllerBase.AddToAnimatorControllerList();
            }
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<EntityFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            EntityInit();
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }

        [Button("场景道具初始化", ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            sceneEntity.Clear();
            List<EntityItem> tempEntity;
            //首场景,加载全部
            if (GameRootStart.Instance.loadScene.name == String.Empty)
            {
                tempEntity = DataFrameComponent.Hierarchy_GetAllObjectsInScene<EntityItem>();
            }
            else
            {
                tempEntity = DataFrameComponent.Hierarchy_GetAllObjectsInScene<EntityItem>(GameRootStart.Instance.loadScene.name);
            }

            foreach (EntityItem entityItem in tempEntity)
            {
                if (!sceneEntity.Contains(entityItem))
                {
                    sceneEntity.Add(entityItem);
                }
            }
        }


        [Button("查找场景中重复实体", ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityRepeat()
        {
            sceneRepeatEntityList.Clear();
            Dictionary<string, List<GameObject>> temp = new Dictionary<string, List<GameObject>>();
            List<EntityItem> tempEntity = DataFrameComponent.Hierarchy_GetAllObjectsInScene<EntityItem>();
            foreach (EntityItem entityItem in tempEntity)
            {
                if (!temp.ContainsKey(entityItem.entityName))
                {
                    temp.Add(entityItem.entityName, new List<GameObject>() { entityItem.gameObject });
                }
                else
                {
                    temp[entityItem.entityName].Add(entityItem.gameObject);
                }
            }

            foreach (KeyValuePair<string, List<GameObject>> pair in temp)
            {
                if (pair.Value.Count > 1)
                {
                    sceneRepeatEntityList.Add(new SceneRepeatEntity() { entityName = pair.Key, entityList = pair.Value });
                }
            }
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
            foreach (EntityItem entityItem in sceneEntity)
            {
                entityItem.Show();
            }
        }

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetEntity<T>(string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
                {
                    return entityItem.GetComponent<T>();
                }
            }

            return default(T);
        }

        /// <summary>
        /// 根据实体名称获得第一个实体
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public EntityItem GetEntity(string entityName)
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
        public void DisplayEntity(bool display, string entityName)
        {
            foreach (EntityItem entityItem in sceneEntity)
            {
                if (entityItem.entityName == entityName)
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
            }
        }

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        public void DisplayEntity(bool display, params string[] entityNames)
        {
            foreach (string entityName in entityNames)
            {
                foreach (EntityItem entityItem in sceneEntity)
                {
                    if (entityName == entityItem.entityName)
                    {
                        if (display)
                        {
                            entityItem.Show();
                        }
                        else
                        {
                            entityItem.Hide();
                        }

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
        public bool GetEntityState(string entityName)
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

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entityName"></param>
        public void RemoveEntity(string entityName)
        {
            for (int i = 0; i < sceneEntity.Count; i++)
            {
                if (sceneEntity[i].entityName == entityName)
                {
                    sceneEntity.RemoveAt(i);
                    break;
                }
            }
        }
    }
}