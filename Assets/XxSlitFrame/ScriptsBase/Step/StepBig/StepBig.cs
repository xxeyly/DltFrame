using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace Step.StepBig
{
    public class StepBig : LocalBaseWindow
    {
        private List<StepSmall.StepSmall> _stepSmallList;
        private bool _currentOpenState;
        private ScrollRect _scrollRect;
        private Button _openBigStep;
        private Button _closeBigStep;
        private Button _thisEvent;
        public int currentStepIndex;
        private StepBase _stepBase;
        public List<StepSmall.StepSmall> StepSmallList => _stepSmallList;

        public void SetStepBase(StepBase stepBase)
        {
            _stepBase = stepBase;
        }

        protected override void InitView()
        {
            _thisEvent = GetComponent<Button>();
            BindUi(ref _openBigStep, "OpenBigStep");
            BindUi(ref _closeBigStep, "CloseBigStep");
            BindUi(ref _scrollRect, "Scroll View");
            BindUi(ref _stepSmallList, "Scroll View/Viewport/Content");
        }

        protected override void InitListener()
        {
            BindListener(_thisEvent, EventTriggerType.PointerClick, OnThisEvent);
            BindListener(_thisEvent, EventTriggerType.PointerEnter, OnThisEventEnter);
            BindListener(_thisEvent, EventTriggerType.PointerExit, OnThisEventExit);
            BindListener(_openBigStep, EventTriggerType.PointerClick, OnOpenBigStep);
            BindListener(_closeBigStep, EventTriggerType.PointerClick, OnCloseBigStep);
        }

        private void OnThisEventExit(BaseEventData arg0)
        {
            if (PersistentDataSvc.Instance.currentStepBigIndex == currentStepIndex)
            {
            }
            else
            {
                _thisEvent.image.color = new Color(1, 1, 1, 0.7f);
            }
        }

        private void OnThisEventEnter(BaseEventData arg0)
        {
            if (PersistentDataSvc.Instance.currentStepBigIndex == currentStepIndex)
            {
            }
            else
            {
                _thisEvent.image.color = new Color(1, 1, 1, 1f);
            }
        }

        private void OnThisEvent(BaseEventData arg0)
        {
            if (_currentOpenState)
            {
                LocalOpenCurrentBigStep();
                ShowAllStepList();
            }
            else
            {
                LocalCloseCurrentBigStep();
                HideAllStepList();
                OpenCurrentSmallStep();
            }
        }

        protected override void InitData()
        {
            foreach (StepSmall.StepSmall stepSmall in _stepSmallList)
            {
                stepSmall.Init();
            }
        }

        private void OnOpenBigStep(BaseEventData targetObj)
        {
            LocalOpenCurrentBigStep();
            ShowAllStepList();
            _thisEvent.image.color = new Color(1, 1, 1, 1f);
        }

        private void OnCloseBigStep(BaseEventData targetObj)
        {
            LocalCloseCurrentBigStep();
            HideAllStepList();
            OpenCurrentSmallStep();
        }

        /// <summary>
        /// 显示 ScrollView
        /// </summary>
        private void ShowScrollView()
        {
            ShowObj(_scrollRect.gameObject);
        }

        /// <summary>
        /// 隐藏 ScrollView
        /// </summary>
        private void HideScrollView()
        {
            HideObj(_scrollRect.gameObject);
        }

        /// <summary>
        /// 隐藏当前大步骤
        /// </summary>
        public void HideCurrentBigStep()
        {
            HideObj(gameObject);
            LocalOpenCurrentBigStep();
        }

        /// <summary>
        /// 显示当前大步骤
        /// </summary>
        public void ShowCurrentBigStep()
        {
            ShowObj(gameObject);
            LocalCloseCurrentBigStep();
            OpenCurrentSmallStep();
        }

        /// <summary>
        /// 打开当前大步骤
        /// </summary>
        private void LocalOpenCurrentBigStep()
        {
            HideScrollView();
            HideObj(_openBigStep.gameObject);
            ShowObj(_closeBigStep.gameObject);
            _currentOpenState = false;
        }

        /// <summary>
        /// 打开当前步骤
        /// </summary>
        private void LocalCloseCurrentBigStep()
        {
            ShowScrollView();
            HideObj(_closeBigStep.gameObject);
            ShowObj(_openBigStep.gameObject);
            _currentOpenState = true;
        }

        /// <summary>
        /// 全局关闭步骤
        /// </summary>
        private void GlobalCloseCurrentStep()
        {
            HideScrollView();
            HideObj(_openBigStep.gameObject, _closeBigStep.gameObject);
            ShowObj(gameObject);
            _currentOpenState = false;
            _thisEvent.image.color = new Color(1, 1, 1, 0.7f);
        }

        /// <summary>
        /// 显示全部列表
        /// </summary>
        private void ShowAllStepList()
        {
            foreach (KeyValuePair<StepBig, List<StepSmall.StepSmall>> pair in _stepBase.bigStepDic)
            {
                //是当前物体
                if (pair.Key == this)
                {
                }
                else
                {
                    pair.Key.GlobalCloseCurrentStep();
                }
            }
        }

        /// <summary>
        /// 显示全部列表
        /// </summary>
        private void HideAllStepList()
        {
            foreach (KeyValuePair<StepBig, List<StepSmall.StepSmall>> pair in _stepBase.bigStepDic)
            {
                //是当前物体
                if (pair.Key == this)
                {
                }
                else
                {
                    HideObj(pair.Key.gameObject);
                }
            }
        }

        /// <summary>
        /// 打开当前小步骤
        /// </summary>
        private void OpenCurrentSmallStep()
        {
            //如果当前打开的还是当前步骤
            if (PersistentDataSvc.Instance.currentStepBigIndex == currentStepIndex)
            {
                foreach (StepSmall.StepSmall stepSmall in _stepSmallList)
                {
                    if (stepSmall.currentSmallStepIndex == PersistentDataSvc.Instance.currentStepSmallIndex)
                    {
                        stepSmall.thisEvent.isOn = true;
                    }
                }
            }
            else
            {
                foreach (StepSmall.StepSmall stepSmall in _stepSmallList)
                {
                    if (stepSmall.currentSmallStepIndex == 0)
                    {
                        stepSmall.thisEvent.isOn = true;
                        PersistentDataSvc.Instance.currentStepBigIndex = currentStepIndex;
                        PersistentDataSvc.Instance.currentStepSmallIndex = 0;
                        PersistentDataSvc.Instance.currentStepSmallSmallIndex = 0;
                        ListenerSvc.Instance.ExecuteEvent(ListenerEventType.InvokeEventByStepIndex);
                    }
                }
            }
        }
    }
}