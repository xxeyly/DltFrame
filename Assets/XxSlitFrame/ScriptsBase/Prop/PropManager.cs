using System;
using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace Prop
{
    public class PropManager : StartSingleton<PropManager>
    {
        [Header("物品")] private Dictionary<PropItemData.PropType, GameObject> _propDic;
        [Header("物品列表")] private List<PropItem> _propItems;

        public PropItemData propItemData;

        /// <summary>
        /// 道具初始化
        /// </summary>
        private void PropInit()
        {
            foreach (PropItem propItem in _propItems)
            {
                /*if (propItem.highlightEffect != null)
                {
                    propItem.highlightEffect.SetHighlighted(false);
                }*/

                if (propItem.GetComponent<Collider>())
                {
                    propItem.GetComponent<Collider>().enabled = false;
                }

                propItem.gameObject.SetActive(propItem.hiding);
            }
        }

        /// <summary>
        /// 隐藏物体
        /// </summary>
        /// <param name="propType"></param>
        public void HideObject(params PropItemData.PropType[] propType)
        {
            for (int i = 0; i < propType.Length; i++)
            {
                if (_propDic.ContainsKey(propType[i]))
                {
                    _propDic[propType[i]].SetActive(false);
                }
            }
        }

        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="propType"></param>
        public void ShowObject(params PropItemData.PropType[] propType)
        {
            for (int i = 0; i < propType.Length; i++)
            {
                if (_propDic.ContainsKey(propType[i]))
                {
                    _propDic[propType[i]].SetActive(true);
                }
            }
        }

        public override void Init()
        {
            base.Init();
            _propDic = new Dictionary<PropItemData.PropType, GameObject>();
            _propItems = new List<PropItem>(GameObject.FindObjectsOfType<PropItem>());
            foreach (PropItem propItem in _propItems)
            {
                propItem.StartSvc();
                if (!_propDic.ContainsKey(propItem.propType))
                {
                    _propDic.Add(propItem.propType, propItem.gameObject);
                    if (propItem.GetComponent<Collider>())
                    {
                        propItem.GetComponent<Collider>().enabled = false;
                    }
                }
                else
                {
                    Debug.Log(propItem.gameObject);
                }
            }

            ListenerSvc.Instance.AddListenerEvent(ListenerSvc.EventType.PropInit, PropInit);
            ListenerSvc.Instance.AddListenerEvent<int>(ListenerSvc.EventType.PropShowGroup, PropShowGroup);
        }

        private void PropShowGroup(int groupIndex)
        {
            PropItemData.PropItemGroupInfo propItemGroupInfo = new PropItemData.PropItemGroupInfo();

            for (int i = 0; i < propItemData.groupInfos.Count; i++)
            {
                if (propItemData.groupInfos[i].@group == groupIndex)
                {
                    propItemGroupInfo = propItemData.groupInfos[i];
                }
            }

            for (int i = 0; i < propItemGroupInfo.propItemGroupInfo.Count; i++)
            {
                if (propItemGroupInfo.propItemGroupInfo[i].display)
                {
                    ShowObject(propItemGroupInfo.propItemGroupInfo[i].propTypes);
                }
                else
                {
                    HideObject(propItemGroupInfo.propItemGroupInfo[i].propTypes);
                }
            }
        }

        /// <summary>
        /// 根据物品类型,获得物品
        /// </summary>
        /// <param name="propType"></param>
        public GameObject GetObjByPropType(PropItemData.PropType propType)
        {
            if (_propDic.ContainsKey(propType))
            {
                return _propDic[propType];
            }

            return null;
        }

        /// <summary>
        /// 物体高亮并且开启触发
        /// </summary>
        /// <param name="propType"></param>
        /// <returns></returns>
        public bool HighlightObjByPropType(PropItemData.PropType propType)
        {
            if (_propDic.ContainsKey(propType))
            {
                PropItem propItem = _propDic[propType].GetComponent<PropItem>();
                if (propItem != null)
                {
                    // propItem.highlightEffect.enabled = true;
                    /*propItem.highlightEffect.highlighted = true;
                    propItem.highlightEffect.RenderOccluders();*/
                    ;
                    /*_propHighTaskTime = TimeSvc.Instance.AddSwitchTask(new List<UnityAction>()
                    {
                        () => { propItem.highlightEffect.enabled = false; },
                        () => { propItem.highlightEffect.enabled = true; },
                    }, "物体高亮", 0.5f, 0);*/
                }

                if (!_propDic[propType].activeSelf)
                {
                    _propDic[propType].gameObject.SetActive(true);
                }

                if (propItem.GetComponent<Collider>())
                {
                    propItem.GetComponent<Collider>().enabled = true;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 隐藏物体高亮
        /// </summary>
        /// <param name="propType"></param>
        /// <returns></returns>
        public bool HideHighlightObjByPropType(PropItemData.PropType propType)
        {
            if (_propDic.ContainsKey(propType))
            {
                PropItem propItem = _propDic[propType].GetComponent<PropItem>();
                if (propItem != null)
                {
                    // propItem.highlightEffect.enabled = false;
                    /*propItem.highlightEffect.highlighted = false;
                    propItem.highlightEffect.RenderOccluders();*/
                }

                if (propItem.GetComponent<Collider>())
                {
                    propItem.GetComponent<Collider>().enabled = false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 触发开关
        /// </summary>
        /// <param name="propType"></param>
        /// <param name="trigger"></param>
        public bool SwitchTriggerByPropType(PropItemData.PropType propType, bool trigger)
        {
            if (_propDic.ContainsKey(propType) && _propDic[propType].GetComponent<Collider>())
            {
                _propDic[propType].GetComponent<Collider>().enabled = trigger;
                return true;
            }

            return false;
        }
    }
}