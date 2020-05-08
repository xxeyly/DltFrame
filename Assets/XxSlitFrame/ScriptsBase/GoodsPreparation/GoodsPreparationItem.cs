using UnityEngine;
using UnityEngine.UI;
using XxSlitFrame.View;

namespace GoodsPreparation
{
    public class GoodsPreparationItem : LocalBaseWindow
    {
        private Toggle _toggle;
        private Image _itemIcon;
        private Text _itemContent;
        [Header("物品ID")] public int itemId;
        [Header("是否是满的")] public bool isFull;
        [Header("层数")] public int layoutInt;

        protected override void InitView()
        {
            _toggle = GetComponent<Toggle>();
            BindUi(ref _itemIcon, "ItemIcon");
            BindUi(ref _itemContent, "ItemContent");
        }

        protected override void InitListener()
        {
        }

        protected override void InitData()
        {
        }

        /// <summary>
        /// 获得当前图片
        /// </summary>
        /// <returns></returns>
        public Sprite GetCurrentSpr()
        {
            return _itemIcon.sprite;
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        /// <param name="display"></param>
        public void DisPlayContent(bool display)
        {
            if (display)
            {
                ShowObj(_itemContent.gameObject, _itemIcon.gameObject);
            }
            else
            {
                HideObj(_itemContent.gameObject, _itemIcon.gameObject);
            }
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="spr"></param>
        /// <param name="content"></param>
        /// <param name="itemId"></param>
        /// <param name="layout"></param>
        public void SetContent(Sprite spr, string content, int itemId, int layout)
        {
            _itemIcon.sprite = spr;
            _itemContent.text = content;
            this.itemId = itemId;
            layoutInt = layout;
        }
    }
}