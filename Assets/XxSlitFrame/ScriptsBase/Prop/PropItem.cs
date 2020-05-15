using System.Collections.Generic;
// using HighlightPlus;
using UnityEngine;
using UnityEngine.Events;
using XAnimator.Base;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace Prop
{
    public class PropItem : StartSingleton
    {
        public static PropItem Instance;

        [Header("物品类型")] public PropItemData.PropType propType;
        // [Header("高亮物体")] public HighlightEffect highlightEffect;

        [SerializeField] private Dictionary<PropItemData.PropType, UnityAction> actionDic;
        [Header("不隐藏")] public bool hiding;

        public override void StartSvc()
        {
            Instance = GetComponent<PropItem>();
            Init();
        }

        public override void Init()
        {
            /*if (GetComponent<HighlightEffect>())
            {
                highlightEffect = GetComponent<HighlightEffect>();
            }*/

            actionDic = new Dictionary<PropItemData.PropType, UnityAction>
            {
                {
                    PropItemData.PropType.拔针注射器, () =>
                    {
                        Debug.Log("拔针:注射器");
                        /*AnimatorControllerManager.Instance.PlayAnim(AnimType.PullOutSyringe, () => { ListenerSvc.Instance.ImplementListenerEvent(ListenerEventType.SkipToNext); });
                        PropManager.Instance.HideHighlightObjByPropType(PropItemData.PropType.拔针注射器);*/
                    }
                },
                {
                    PropItemData.PropType.消毒手臂, () => { Debug.Log("消毒手臂"); }
                }
            };
        }

        private void OnMouseDown()
        {
            actionDic[propType]?.Invoke();
        }
    }
}