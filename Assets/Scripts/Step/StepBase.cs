using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace Step
{
    public abstract class StepBase : SingletonBaseWindow<StepBase>
    {
        private List<StepBig.StepBig> _bigStepList;
        public Dictionary<StepBig.StepBig, List<StepSmall.StepSmall>> bigStepDic;
        protected Dictionary<int, UnityAction> smallStepActionEvent;
        private Dictionary<int, int> _stepCountDic = new Dictionary<int, int>();
        protected abstract void EditingEvents();
        public abstract void InitEvent();
        protected abstract void FirstInit();

        public override void First()
        {
            base.First();
            FirstInit();
            smallStepActionEvent = new Dictionary<int, UnityAction>();
            EditingEvents();
            OpenCurrentStep();
            InvokeEventByStepIndex();
        }

        /// <summary>
        /// 根据步骤索引执行事件
        /// </summary>
        private void InvokeEventByStepIndex()
        {
            if (smallStepActionEvent.ContainsKey(PersistentDataSvc.currentStepBigIndex * 100 + PersistentDataSvc.currentStepSmallIndex))
            {
                InitEvent();
                smallStepActionEvent[PersistentDataSvc.currentStepBigIndex * 100 + PersistentDataSvc.currentStepSmallIndex].Invoke();
            }
            else
            {
                Debug.Log("事件没有定义:当前大步骤索引:" + PersistentDataSvc.currentStepBigIndex + "当前小步骤索引:" + PersistentDataSvc.currentStepSmallIndex);
            }
        }

        protected override void OnlyOnceInit()
        {
            base.OnlyOnceInit();
            bigStepDic = new Dictionary<StepBig.StepBig, List<StepSmall.StepSmall>>();
            _stepCountDic = new Dictionary<int, int>();
            //步骤绑定
            foreach (StepBig.StepBig stepBig in _bigStepList)
            {
                stepBig.Init();
                stepBig.SetStepBase(this);
                bigStepDic.Add(stepBig, stepBig.StepSmallList);
            }

            //步骤个数统计
            foreach (KeyValuePair<StepBig.StepBig, List<StepSmall.StepSmall>> pair in bigStepDic)
            {
                _stepCountDic.Add(pair.Key.currentStepIndex, pair.Value.Count - 1);
            }
        }

        public override void Init()
        {
        }

        protected override void InitView()
        {
            BindUi(ref _bigStepList, "BigStepList");
        }

        protected override void InitListener()
        {
            ListenerSvc.AddListenerEvent(ListenerSvc.EventType.InvokeEventByStepIndex, InvokeEventByStepIndex);
            ListenerSvc.AddListenerEvent(ListenerSvc.EventType.SkipToNext, SkipToNext);
        }

        /// <summary>
        /// 打开当前步骤
        /// </summary>
        private void OpenCurrentStep()
        {
            foreach (StepBig.StepBig stepBig in _bigStepList)
            {
                if (stepBig.currentStepIndex == PersistentDataSvc.currentStepBigIndex)
                {
                    stepBig.ShowCurrentBigStep();
                }
                else
                {
                    stepBig.HideCurrentBigStep();
                }
            }
        }

        /// <summary>
        /// 设置当前步骤的事件
        /// </summary>
        /// <param name="bigIndex"></param>
        /// <param name="smallIndex"></param>
        /// <param name="action"></param>
        protected void SetSmallStepAction(int bigIndex, int smallIndex, UnityAction action)
        {
            if (!smallStepActionEvent.ContainsKey(bigIndex * 100 + smallIndex))
            {
                smallStepActionEvent.Add(bigIndex * 100 + smallIndex, action);
            }
            else
            {
                Debug.Log("该步骤已经定义:" + bigIndex + ":" + smallIndex);
            }
        }

        /// <summary>
        /// 跳到下一步
        /// </summary>
        private void SkipToNext()
        {
            //小于大步骤上限个数
            if (PersistentDataSvc.currentStepBigIndex < _stepCountDic.Count)
            {
                if (PersistentDataSvc.currentStepSmallIndex < _stepCountDic[PersistentDataSvc.currentStepBigIndex])
                {
                    PersistentDataSvc.currentStepSmallIndex += 1;
                }
                else
                {
                    Debug.Log("达到上限了,进行下一个大步骤");
                    PersistentDataSvc.currentStepBigIndex += 1;
                    PersistentDataSvc.currentStepSmallIndex = 0;
                }

                OpenCurrentStep();
                InvokeEventByStepIndex();
            }
            else
            {
                Debug.LogError("超出步骤上限");
            }
        }
    }
}