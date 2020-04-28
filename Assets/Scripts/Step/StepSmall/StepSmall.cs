using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace Step.StepSmall
{
    public class StepSmall : LocalBaseWindow
    {
        [HideInInspector] public Toggle thisEvent;

        private Text _smallStepContent;

        //触发
        private bool _trigger;

        /// <summary>
        /// 当前小步骤索引
        /// </summary>
        public int currentSmallStepIndex;

        protected override void InitView()
        {
            thisEvent = GetComponent<Toggle>();
            BindUi(ref _smallStepContent, "SmallStepContent");
        }

        protected override void InitListener()
        {
            thisEvent.onValueChanged.AddListener(OnValueChanged);
            BindListener(thisEvent, EventTriggerType.PointerClick, OnEvent);
            BindListener(thisEvent, EventTriggerType.PointerEnter, OnThisEventEnter);
            BindListener(thisEvent, EventTriggerType.PointerExit, OnThisEventExit);
        }

        private void OnEvent(BaseEventData arg0)
        {
            if (!_trigger)
            {
                _trigger = true;
                ListenerSvc.Instance.ImplementListenerEvent(ListenerSvc.EventType.InvokeEventByStepIndex);
            }
        }

        private void OnThisEventExit(BaseEventData arg0)
        {
            if (PersistentDataSvc.Instance.currentStepSmallIndex == currentSmallStepIndex)
            {
            }
            else
            {
                _smallStepContent.color = Color.black;
            }
        }

        private void OnThisEventEnter(BaseEventData arg0)
        {
            if (PersistentDataSvc.Instance.currentStepSmallIndex == currentSmallStepIndex)
            {
            }
            else
            {
                _smallStepContent.color = Color.white;
            }
        }


        private void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                PersistentDataSvc.Instance.currentStepSmallIndex = currentSmallStepIndex;
                _smallStepContent.color = Color.white;
            }
            else
            {
                _trigger = false;
                _smallStepContent.color = Color.black;
            }
        }

        protected override void InitData()
        {
        }
    }
}