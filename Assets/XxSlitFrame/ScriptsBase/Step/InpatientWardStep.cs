using UnityEngine;
using XxSlitFrame.Tools.Svc;

namespace Step
{
    public class InpatientWardStep : StepBase
    {
        protected override void EditingEvents()
        {
            SetSmallStepAction(0, 0, () => { Debug.Log("用物准备"); });
        }

        public override void InitEvent()
        {
            TimeSvc.DeleteTimeTask();
            ViewSvc.HideView();
            ListenerSvc.ExecuteEvent(ListenerEventType.PropInit);
        }

        protected override void FirstInit()
        {
        }
    }
}