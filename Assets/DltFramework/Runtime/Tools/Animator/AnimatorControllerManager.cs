using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DltFramework
{
    /// <summary>
    /// 动画控制管理器
    /// </summary>
    public class AnimatorControllerManager : SceneComponent
    {
        [LabelText("当前播放动画名称")] public string currentPlayAnim;

        [LabelText("所有动画控制器")] [SerializeField] [Searchable]
        public List<AnimatorControllerBase> allAnimController = new List<AnimatorControllerBase>();

        /// <summary>
        /// 动画控制器任务
        /// </summary>
        private string _animatorTimeTask;

        public override void StartComponent()
        {
            allAnimController = DataFrameComponent.Hierarchy_GetAllObjectsInScene<AnimatorControllerBase>();
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
            {
                animatorControllerBase.Init();
            }
        }

        public override void EndComponent()
        {
        }

        [LabelText("添加动画控制器")]
        [AddListenerEvent]
        private void AddAnimatorController(AnimatorControllerBase animatorControllerBase)
        {
            if (!allAnimController.Contains(animatorControllerBase))
            {
                allAnimController.Add(animatorControllerBase);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="playProgress">播放进度</param>
        [AddListenerEvent]
        private async UniTask PlayAnim(string animName, float playProgress, float animSpeed = 1)
        {
            currentPlayAnim = animName;
            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.gameObject.activeInHierarchy)
                {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    controllerBase.PlayAnim(animName, playProgress, animSpeed);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                }
            }

            await U_AddTask(animName, GetPlayAnimFirstLength(animName));
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="animSpeedProgress"></param>
        public void PlayAnim(string animName, AnimSpeedProgress animSpeedProgress)
        {
            currentPlayAnim = animName;

            foreach (AnimatorControllerBase controllerBase in allAnimController)
            {
                try
                {
                    if (controllerBase.gameObject.activeInHierarchy)
                    {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                        controllerBase.PlayAnim(animName, animSpeedProgress);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e + ":" + controllerBase.name);
                }
            }
        }


        private bool GetAnimControllerPlayerOver()
        {
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (!animatorControllerBase.GetAnimClipPlayOver())
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        private float GetPlayAnimFirstLength(string animType)
        {
            // float animLength = 0;
            AnimatorControllerBase fistController = GetPlayAnimFirstController(animType);
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
        public AnimatorControllerBase GetPlayAnimFirstController(string animType)
        {
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
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
            foreach (AnimatorControllerBase animatorControllerBase in allAnimController)
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
            foreach (AnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.PausePlay();
            }
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            foreach (AnimatorControllerBase xAnimatorControllerBase in allAnimController)
            {
                xAnimatorControllerBase.ContinuePlay();
            }
        }
    }
}