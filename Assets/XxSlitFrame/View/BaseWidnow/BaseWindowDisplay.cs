using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XxSlitFrame.Tools;

namespace XxSlitFrame.View
{
    partial class BaseWindow
    {
        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params GameObject[] hideObjArray)
        {
            foreach (GameObject hideObj in hideObjArray)
            {
                hideObj.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params MaskableGraphic[] hideObjArray)
        {
            foreach (MaskableGraphic hideObj in hideObjArray)
            {
                hideObj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params Selectable[] hideObjArray)
        {
            foreach (Selectable hideObj in hideObjArray)
            {
                hideObj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(true);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params MaskableGraphic[] showObjArray)
        {
            foreach (MaskableGraphic showObj in showObjArray)
            {
                showObj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params MaskableGraphic[] showObjArray)
        {
            foreach (MaskableGraphic showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }
      
    }
}