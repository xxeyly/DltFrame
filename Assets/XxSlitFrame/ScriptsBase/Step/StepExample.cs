using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace Step
{
    public class StepExample : StepBase
    {
        protected override void EditingEvents()
        {
            SetSmallStepAction(0, 0, () =>
            {
                ListenerSvc.ExecuteEvent(ListenerEventType.CameraMoveToTargetPos, CameraPosData.CameraPosType.位置1);
            });
            SetSmallStepAction(0, 1, () => { SceneSvc.SceneLoad("End"); });
        }

        public override void InitEvent()
        {
        }

        protected override void FirstInit()
        {
        }
    }
}