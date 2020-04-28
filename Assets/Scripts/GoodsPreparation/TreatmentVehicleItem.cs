using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.View;

namespace GoodsPreparation
{
    /// <summary>
    /// 用物车物品
    /// </summary>
    public class TreatmentVehicleItem : LocalBaseWindow
    {
        private Button _event;
        [Header("对应物品序列")] public int correspondingArticlesIndex;

        protected override void InitView()
        {
            _event = GetComponent<Button>();
        }

        protected override void InitListener()
        {
            BindListener(_event, EventTriggerType.PointerClick, OnRecovery);
        }

        private void OnRecovery(BaseEventData arg0)
        {
            HideObj(this.gameObject);
            GoodsPreparation.Instance.ItemRecovery(correspondingArticlesIndex);
        }

        protected override void InitData()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}