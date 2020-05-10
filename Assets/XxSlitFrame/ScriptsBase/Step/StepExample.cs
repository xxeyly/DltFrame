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
                ListenerSvc.ImplementListenerEvent(ListenerSvc.EventType.CameraMoveToTargetPos, CameraPosData.CameraPosType.位置1);
                // ViewSvc.ShowView(typeof(GoodsPreparation.GoodsPreparation));
            });
            SetSmallStepAction(0, 1, () =>
            {
                ListenerSvc.ImplementListenerEvent(ListenerSvc.EventType.CameraMoveToTargetPos, CameraPosData.CameraPosType.位置2);
                // ViewSvc.ShowView(typeof(GoodsPreparation.GoodsPreparation));
            });
        }

        public override void InitEvent()
        {
        }

        protected override void FirstInit()
        {
        }
    }
}