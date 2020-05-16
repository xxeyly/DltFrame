namespace Step
{
    public class StepExampleTwo : StepBase

    {
        protected override void EditingEvents()
        {
            SetSmallStepAction(0, 0, () => { SceneSvc.SceneLoad(0); });
            SetSmallStepAction(0, 1, () => { });
        }

        public override void InitEvent()
        {
        }

        protected override void FirstInit()
        {
        }
    }
}