using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    /// <summary>
    /// 视图组件
    /// </summary>
    public class ViewFrameComponent : FrameComponent
    {
        public static ViewFrameComponent Instance;

        [LabelText("视图类型与视图窗口的键值对")] 
        public Dictionary<Type, BaseWindow> activeViewDlc = new Dictionary<Type, BaseWindow>();


        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameSceneInitComponent()
        {
            //获得当前场景中的所有视图
            List<BaseWindow> loadSceneBaseWindow = DataFrameComponent.Hierarchy_GetAllObjectsInScene<BaseWindow>(GameRootStart.Instance.loadScene.name);
            foreach (BaseWindow window in loadSceneBaseWindow)
            {
                //子类或者已经存在的视图不添加
                if (!window.GetComponent<ChildBaseWindow>() && !activeViewDlc.ContainsKey(window.viewType))
                {
                    //添加到视图字典中
                    activeViewDlc.Add(window.viewType, window);
                    //视图初始化
                    window.ViewStartInit();
                }
            }
        }

        public override void FrameSceneEndComponent()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in activeViewDlc)
            {
                pair.Value.OnViewDestroy();
            }

            activeViewDlc.Clear();
        }

        public void RemoveView(Type viewType)
        {
            if (activeViewDlc.ContainsKey(viewType))
            {
                activeViewDlc.Remove(viewType);
            }
        }


        public override void FrameEndComponent()
        {
        }

        //视图实例化
        public GameObject Instantiate(GameObject instantiate, Transform parent, bool world)
        {
            GameObject tempInstantiate = GameObject.Instantiate(instantiate, parent, world);
            InstantiateInit(tempInstantiate);
            return tempInstantiate;
        }

        private void InstantiateInit(GameObject instantiate)
        {
            BaseWindow baseWindow = instantiate.GetComponent<BaseWindow>();
            if (!activeViewDlc.ContainsKey(baseWindow.viewType))
            {
                activeViewDlc.Add(baseWindow.viewType, baseWindow);
                baseWindow.ViewStartInit();
            }
        }

        /// <summary>
        /// 获得视图是否存在
        /// </summary>
        /// <returns></returns>
        public bool GetViewExistence(Type view)
        {
            if (activeViewDlc != null && activeViewDlc.ContainsKey(view))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool GetViewState(Type view)
        {
            if (activeViewDlc != null && activeViewDlc.ContainsKey(view))
            {
                return activeViewDlc[view].GetDisplay();
            }

            return false;
        }
        
        public BaseWindow GetView(Type view)
        {
            if (activeViewDlc != null && activeViewDlc.ContainsKey(view))
            {
                return activeViewDlc[view];
            }

            return null;
        }

        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentActiveViewCount()
        {
            int currentActiveViewCount = 0;
            foreach (KeyValuePair<Type, BaseWindow> pair in activeViewDlc)
            {
                if (pair.Value.GetDisplay())
                {
                    currentActiveViewCount += 1;
                }
            }

            return activeViewDlc.Count - currentActiveViewCount;
        }

        /// <summary>
        /// 获得当前场景中视图的数量
        /// </summary>
        public int GetCurrentSceneViewCount()
        {
            return activeViewDlc.Count;
        }

        #region 显示视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void ShowView(Type type)
        {
            activeViewDlc[type].DisPlay(true);
            activeViewDlc[type].Init();
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        public void ShowView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                ShowView(viewType);
            }
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void HideView(Type type)
        {
            BaseWindow baseWindow = activeViewDlc[type];
            if (GetViewState(type))
            {
                baseWindow.OnViewHide();
                baseWindow.DisPlay(false);
            }
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        public void HideView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                HideView(viewType);
            }
        }

        public void HideAllView()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in activeViewDlc)
            {
                if (pair.Value.GetViewShowType() == ViewShowType.Activity)
                {
                    HideView(pair.Key);
                }
            }
        }

        #endregion
    }
}