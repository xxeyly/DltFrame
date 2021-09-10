using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using XFramework;

namespace XFramework
{
    /// <summary>
    /// 动画控制管理器
    /// </summary>
    public class AnimatorControllerManager : StartSingleton
    {
        public static AnimatorControllerManager Instance;
        [LabelText("当前播放动画名称")] public string currentPlayAnim;
        [LabelText("动画是否切换")] public bool eventChange;

        [LabelText("所有动画控制器")] [SerializeField] [Searchable]
        private List<XAnimatorControllerBase> allAnimController = new List<XAnimatorControllerBase>();

        /// <summary>
        /// 动画控制器任务
        /// </summary>
        private int _animatorTimeTask;

        public override void StartSvc()
        {
            Instance = GetComponent<AnimatorControllerManager>();
        }

        public override void Init()
        {
            // allAnimController = new List<XAnimatorControllerBase>(FindObjectsOfType<XAnimatorControllerBase>());
            allAnimController = DataSvc.GetAllObjectsInScene<XAnimatorControllerBase>();
            foreach (XAnimatorControllerBase animatorControllerBase in allAnimController)
            {
                animatorControllerBase.StartSvc();
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        public void PlayAnim(string animType)
        {
            currentPlayAnim = animType;
            eventChange = true;
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="playProgress">播放进度</param>
        public void PlayAnim(string animType, float playProgress)
        {
            currentPlayAnim = animType;
            eventChange = true;

            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.enabled)
                {
                    controllerBase.PlayAnim(animType, playProgress);
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animSpeedProgress"></param>
        public void PlayAnim(string animType, AnimSpeedProgress animSpeedProgress)
        {
            currentPlayAnim = animType;
            eventChange = true;

            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.gameObject.activeInHierarchy)
                {
                    Debug.Log(controllerBase.name);
                    controllerBase.PlayAnim(animType, animSpeedProgress);
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animAction"></param>
        public int PlayAnim(string animType, UnityAction animAction)
        {
            currentPlayAnim = animType;
            eventChange = false;

            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);
            XAnimatorControllerBase fistController = GetPlayAnimFirstController(animType);

            _animatorTimeTask = TimeSvc.Instance.AddTimeTask(
                () =>
                {
                    if (fistController == null)
                    {
                        return;
                    }

                    if (fistController.GetAnimClipPlayOver())
                    {
                        animAction?.Invoke();
                    }
                }
                , "动画播放时间", GetPlayAnimFirstLength(animType));
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }

            return _animatorTimeTask;
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="listenerEventType"></param>
        public void PlayAnim(string animType, string listenerEventType)
        {
            currentPlayAnim = animType;
            eventChange = true;

            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);

            _animatorTimeTask = TimeSvc.Instance.AddTimeTask(() => { ListenerSvc.Instance.ExecuteEvent(listenerEventType); }, "动画播放时间", GetPlayAnimFirstLength(animType));
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }
        }

        public void StopAnimAction()
        {
            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnim();
            }
        }

        public void StopAllAnimAction()
        {
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnimTaskTime();
            }
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimFirstLength(string animType)
        {
            // float animLength = 0;
            XAnimatorControllerBase fistController = GetPlayAnimFirstController(animType);
            if (fistController != null)
            {
                return fistController.GetPlayAnimLength(animType);
            }

            return 0;
        }

        /// <summary>
        /// 获得首个能播放片段的控制器
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public XAnimatorControllerBase GetPlayAnimFirstController(string animType)
        {
            foreach (XAnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (animatorControllerBase.GetAnimState(animType) && animatorControllerBase.gameObject.activeInHierarchy)
                {
                    return animatorControllerBase;
                }
            }

            return null;
        }

        /// <summary>
        /// 获得动画状态
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public bool GetAnimState(string animType)
        {
            foreach (XAnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (animatorControllerBase.GetAnimState(animType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void PausePlay()
        {
            foreach (XAnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.PausePlay();
            }
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            foreach (XAnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.ContinuePlay();
            }
        }
    }
}