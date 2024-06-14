using System;
using System.Collections.Generic;
using UnityEngine;

namespace DltFramework
{
    public partial class EntityFrameComponent
    {
        public Dictionary<string, Dictionary<EntityItem, bool>> entityProcess = new Dictionary<string, Dictionary<EntityItem, bool>>();

        //释放流程池
        public void EntityReleaseProcess(string processName)
        {
            if (!entityProcess.ContainsKey(processName))
            {
                Debug.LogWarning(processName + "流程池不存在");
                return;
            }

            foreach (KeyValuePair<EntityItem, bool> pair in entityProcess[processName])
            {
                if (pair.Value)
                {
                    pair.Key.Hide();
                }
                else
                {
                    pair.Key.Show();
                }
            }

            entityProcess.Remove(processName);
        }

        /// <summary>
        /// 添加到流程池
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="entityItem"></param>
        /// <param name="display"></param>
        private void AddEntityToProcessPool(string processName, EntityItem entityItem, bool display)
        {
            if (!entityProcess[processName].ContainsKey(entityItem))
            {
                entityProcess[processName].Add(entityItem, display);
            }
            else
            {
                //正好相反,移除
                if (entityProcess[processName][entityItem] == !display)
                {
                    entityProcess[processName].Remove(entityItem);
                }

                if (entityProcess[processName].Count == 0)
                {
                    entityProcess.Remove(processName);
                }
            }
        }

        public void DisplayEntity(string processName, bool display,params string[] entityName)
        {
            if (!entityProcess.ContainsKey(processName))
            {
                entityProcess.Add(processName, new Dictionary<EntityItem, bool>());
            }

            for (int i = 0; i < entityName.Length; i++)
            {
                AddEntityToProcessPool(processName, GetEntity(entityName[i]), display);
            }

            DisplayEntity(display, entityName);
        }
    }
}