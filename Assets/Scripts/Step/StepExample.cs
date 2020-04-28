namespace Step
{
    public class StepExample : StepBase
    {
        protected override void EditingEvents()
        {
            SetSmallStepAction(0, 0, () => { ViewSvc.ShowView(typeof(GoodsPreparation.GoodsPreparation)); });
        }

        public override void InitEvent()
        {
        }

        protected override void FirstInit()
        {
            
        }
    }
}