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
                // ViewSvc.ShowView(typeof(GoodsPreparation.GoodsPreparation));
                // SceneSvc.SceneLoad(0);
            });
            SetSmallStepAction(0, 1, () =>
            {
                Debug.Log("啊是否达到");

                ListenerSvc.ExecuteEvent(ListenerEventType.CameraMoveToTargetPos, CameraPosData.CameraPosType.位置2);
                // ViewSvc.ShowView(typeof(GoodsPreparation.GoodsPreparation));
                // SceneSvc.SceneLoad("End");
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