using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework
{
    /// <summary>
    /// 播放动画进化
    /// </summary>
    public enum AnimSpeedProgress
    {
        None,
        Start,
        End
    }

    public enum AnimValueType
    {
        None,
        Progress,
        Speed,
        Delay
    }

    /// <summary>
    /// 动画控制器基类
    /// </summary>
    public abstract class AnimatorControllerBase : MonoBehaviour
    {
        protected UnityEngine.Animator animator;
        private List<AnimatorControllerParameter> _allParameter;

        public virtual void StartSvc()
        {
            animator = GetComponent<UnityEngine.Animator>();
            _allParameter = new List<AnimatorControllerParameter>(animator.parameters);
        }

        private bool ContainsParameter(string parameterName)
        {
            foreach (AnimatorControllerParameter animatorControllerParameter in _allParameter)
            {
                if (animatorControllerParameter.name == parameterName)
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
            animator.speed = 0;
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            animator.speed = 1;
        }

        public void PlayAnim(string animationType, float animValue, AnimValueType animValueType = AnimValueType.Progress)
        {
            if (ContainsParameter(animationType))
            {
                switch (animValueType)
                {
                    case AnimValueType.Progress:
                        animator.speed = 0;
                        if (animValue >= 1f)
                        {
                            animValue = 0.99f;
                        }

                        animator.Play(animationType, 0, animValue);
                        break;
                    case AnimValueType.Speed:
                        animator.speed = animValue;
                        animator.Play(animationType, 0);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(animValueType), animValueType, null);
                }
            }
        }

        public void PlayAnim(string animationType, AnimSpeedProgress animSpeedProgress)
        {
            if (ContainsParameter(animationType))
            {
                animator.speed = 0;
                if (animSpeedProgress == AnimSpeedProgress.End)
                {
                    animator.Play(animationType, 0, 0.99f);
                }
                else if (animSpeedProgress == AnimSpeedProgress.Start)
                {
                    Debug.Log(animationType);
                    animator.Play(animationType, 0, normalizedTime: 0.01f);
                }
            }
        }

        private int _playAnimTimeTask;

        /// <summary>
        /// 播放动画+事件
        /// </summary>
        /// <param name="animationType"></param>
        /// <param name="eventAction"></param>
        /// <param name="animValue"></param>
        public int PlayAnim(string animationType, UnityAction eventAction, float animValue, AnimValueType animValueType = AnimValueType.Progress)
        {
            if (ContainsParameter(animationType))

            {
                switch (animValueType)
                {
                    case AnimValueType.Progress:
                        PlayAnim(animationType, animValue, animValueType);
                        return _playAnimTimeTask = TimeSvc.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType) * (1 - animValue));
                    case AnimValueType.Speed:
                        PlayAnim(animationType, animValue, animValueType);
                        return _playAnimTimeTask = TimeSvc.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType) / animValue);
                    case AnimValueType.Delay:
                        PlayAnim(animationType);
                        return _playAnimTimeTask = TimeSvc.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType) + animValue);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(animValueType), animValueType, null);
                }
            }

            return 0;
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animationType"></param>
        public void PlayAnim(string animationType)
        {
            if (ContainsParameter(animationType))
            {
                animator.speed = 1;
                // Debug.Log(animator.name + ":" + animationType);
                animator.SetTrigger(animationType);
            }
        }

        /// <summary>
        /// 获得当前动画播放是否完毕
        /// </summary>
        /// <returns></returns>
        public bool GetAnimClipPlayOver()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && animator.gameObject.activeSelf &&
                   !AnimatorControllerManager.Instance.eventChange;
        }

        /// <summary>
        /// 停止播放动画
        /// </summary>
        public void StopAnim()
        {
            animator.speed = 0;
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(string animType)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType)
                {
                    return item.length;
                }
            }

            return -1;
        }

        /// <summary>
        /// 获得动画状态
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public bool GetAnimState(string animType)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType)
                {
                    return true;
                }
            }

            return false;
        }

        public void StopAnimTaskTime()
        {
            StopAnim();
            TimeSvc.Instance.DeleteTimeTask(_playAnimTimeTask);
        }
    }
}