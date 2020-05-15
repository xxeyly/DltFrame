using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace GoodsPreparation
{
    public class GoodsPreparation : SingletonBaseWindow<GoodsPreparation>
    {
        private List<Toggle> _goodsPreparationItems;
        private Button _treatmentVehicleTopEvent;
        private Button _treatmentVehicleDownEvent;
        private Image _dragGoodsPreparationItem;
        private Button _sure;
        private GameObject _errorTips;

        [Header("物品准备图片")] public List<Sprite> goodsPreparationSprs;

        [Header("物品准备内容")] public List<String> goodsPreparationContents;
        [Header("物品准备层")] public List<int> goodsPreparationLayouts;
        private List<TreatmentVehicleItem> _treatmentVehicleItems;
        private Text _errorTipsContent;

        //当前操作的物品
        private GoodsPreparationItem _currentOperationItem;

        /// <summary>
        /// 当前按下的物品
        /// </summary>
        private GoodsPreparationItem _currentDownItem;

        //当前放置的物品
        private TreatmentVehicleEvent _treatmentVehicleEvent;
        [SerializeField] [Header("错误物品")] public int errorItemIndex;

        public override void Init()
        {
            foreach (TreatmentVehicleItem treatmentVehicleItem in _treatmentVehicleItems)
            {
                HideObj(treatmentVehicleItem.gameObject);
                treatmentVehicleItem.Init();
            }

            for (int i = 0; i < _goodsPreparationItems.Count; i++)
            {
                _goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().isFull = true;
                _goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().DisPlayContent(true);
            }

            HideObj(_errorTips);
        }

        protected override void OnlyOnceInit()
        {
            base.OnlyOnceInit();
            for (int i = 0; i < _goodsPreparationItems.Count; i++)
            {
                _goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().Init();
                _goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().SetContent(goodsPreparationSprs[i], goodsPreparationContents[i], i, goodsPreparationLayouts[i]);
            }
        }

        protected override void InitView()
        {
            BindUi(ref _goodsPreparationItems, "GoodsPreparationItems");
            BindUi(ref _treatmentVehicleTopEvent, "TreatmentVehicle/TreatmentVehicleTopEvent");
            BindUi(ref _treatmentVehicleDownEvent, "TreatmentVehicle/TreatmentVehicleDownEvent");
            BindUi(ref _dragGoodsPreparationItem, "DragGoodsPreparationItem");
            BindUi(ref _sure, "Sure");
            BindUi(ref _errorTips, "ErrorTips");
            BindUi(ref _errorTipsContent, "ErrorTips/ErrorTipsContent");
            BindUi(ref _treatmentVehicleItems, "TreatmentVehicle/TreatmentVehicleItems");
        }

        protected override void InitListener()
        {
            BindListener(_sure, EventTriggerType.PointerClick, OnSure);
            BindListener(SelectableConverter(_goodsPreparationItems), EventTriggerType.PointerDown, OnStartClassifyItem);
            BindListener(SelectableConverter(_goodsPreparationItems), EventTriggerType.PointerUp, OnStopClassifyItem);

            BindListener(_treatmentVehicleTopEvent, EventTriggerType.PointerEnter, OnTreatmentVehicleEventEnter);
            BindListener(_treatmentVehicleDownEvent, EventTriggerType.PointerEnter, OnTreatmentVehicleEventEnter);
            BindListener(_treatmentVehicleTopEvent, EventTriggerType.PointerExit, OnTreatmentVehicleEventExit);
            BindListener(_treatmentVehicleDownEvent, EventTriggerType.PointerExit, OnTreatmentVehicleEventExit);
        }

        private void OnTreatmentVehicleEventEnter(BaseEventData arg0)
        {
            //手上有拖拽的物品
            if (_currentOperationItem != null)
            {
                _treatmentVehicleEvent = (arg0 as PointerEventData)?.pointerEnter.GetComponent<TreatmentVehicleEvent>();
                if (_treatmentVehicleEvent != null) _treatmentVehicleEvent.GetComponent<Image>().color = Color.white;
            }
        }

        private void OnTreatmentVehicleEventExit(BaseEventData arg0)
        {
            ((PointerEventData) arg0).pointerEnter.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            //手上有拖拽的物品
            if (_currentOperationItem != null)
            {
                _treatmentVehicleEvent = null;
            }
        }

        /// <summary>
        /// 停止拖拽
        /// </summary>
        /// <param name="arg0"></param>
        private void OnStopClassifyItem(BaseEventData arg0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                MouseSvc.StopUiFollowingMouse();
                HideObj(_dragGoodsPreparationItem.gameObject);
                GoodsPreparationItem currentOperationItem = null;
                if (arg0.selectedObject != null && arg0.selectedObject.GetComponent<GoodsPreparationItem>() != null)
                {
                    currentOperationItem = arg0.selectedObject.GetComponent<GoodsPreparationItem>();
                }

                //说明拖拽结束时,停留在了放置区域
                if (_treatmentVehicleEvent != null)
                {
                    //可能点击了鼠标右键
                    if (currentOperationItem == null)
                    {
                        for (int i = 0; i < _goodsPreparationItems.Count; i++)
                        {
                            if (_goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().itemId == _currentOperationItem.itemId)
                            {
                                _goodsPreparationItems[i].GetComponent<GoodsPreparationItem>().DisPlayContent(true);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //如果放置的位置正确
                        if (_currentOperationItem != null && _treatmentVehicleEvent.layout == _currentOperationItem.layoutInt)
                        {
                            _currentOperationItem.isFull = false;
                            for (int i = 0; i < _treatmentVehicleItems.Count; i++)
                            {
                                if (_treatmentVehicleItems[i].correspondingArticlesIndex == _currentOperationItem.itemId)
                                {
                                    _treatmentVehicleItems[i].gameObject.SetActive(true);
                                    break;
                                }
                            }

                            if (_treatmentVehicleEvent != null)
                            {
                                _treatmentVehicleEvent.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                            }
                        }
                        else
                        {
                            //放置错误
                            ShowErrorTips("放置错误");
                            if (currentOperationItem != null)
                            {
                                currentOperationItem.isFull = true;
                                currentOperationItem.DisPlayContent(true);
                            }
                        }
                    }
                }
                else
                {
                    if (currentOperationItem != null && currentOperationItem.isFull)
                    {
                        currentOperationItem.DisPlayContent(true);
                    }
                }

                _currentOperationItem = null;
                _treatmentVehicleEvent = null;
            }
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="arg0"></param>
        private void OnStartClassifyItem(BaseEventData arg0)
        {
            if (Input.GetMouseButton(0))
            {
                //当前格子物品
                GoodsPreparationItem currentOperationItem = null;
                if (arg0.selectedObject != null && arg0.selectedObject.GetComponent<GoodsPreparationItem>() != null)
                {
                    currentOperationItem = arg0.selectedObject.GetComponent<GoodsPreparationItem>();
                }

                if (currentOperationItem != null)
                {
                    if (currentOperationItem.isFull && currentOperationItem.itemId != errorItemIndex)
                    {
                        arg0.selectedObject.GetComponent<Toggle>().isOn = true;
                        ShowObj(_dragGoodsPreparationItem.gameObject);
                        _dragGoodsPreparationItem.sprite = arg0.selectedObject.GetComponent<GoodsPreparationItem>().GetCurrentSpr();
                        currentOperationItem.DisPlayContent(false);
                        MouseSvc.StartUiFollowingMouse(_dragGoodsPreparationItem.gameObject);
                        //赋值给鼠标
                        _currentOperationItem = currentOperationItem;
                    }
                    else
                    {
                        ShowErrorTips("当前实验不需要当前物品");
                    }
                }
            }
        }

        private void OnSure(BaseEventData targetObj)
        {
            bool allCorrect = true;
            foreach (TreatmentVehicleItem treatmentVehicleItem in _treatmentVehicleItems)
            {
                if (!treatmentVehicleItem.gameObject.activeInHierarchy)
                {
                    allCorrect = false;
                    break;
                }
            }

            if (allCorrect)
            {
                MouseSvc.StopUiFollowingMouse();
                ListenerSvc.ExecuteEvent(ListenerEventType.SkipToNext);
            }
            else
            {
                ShowErrorTips("还有物品未进行分类");
            }
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="errorContent"></param>
        private void ShowErrorTips(string errorContent)
        {
            ShowObj(_errorTips);
            _errorTipsContent.text = errorContent;
            AddTimeTask(() => { HideObj(_errorTips); }, "显示错误", General.ViewErrorTime);
        }

        /// <summary>
        /// 用物还原
        /// </summary>
        /// <param name="itemID"></param>
        public void ItemRecovery(int itemID)
        {
            foreach (Toggle goodsPreparationItem in _goodsPreparationItems)
            {
                GoodsPreparationItem temPreparationItem = goodsPreparationItem.GetComponent<GoodsPreparationItem>();
                if (temPreparationItem.itemId == itemID)
                {
                    ShowObj(goodsPreparationItem.transform.Find("ItemIcon").gameObject, goodsPreparationItem.transform.Find("ItemContent").gameObject);
                    temPreparationItem.isFull = true;
                    return;
                }
            }
        }
    }
}