using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DltFramework
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

    /// <summary>
    /// 动画控制器基类
    /// </summary>
    public abstract class AnimatorControllerBase : EntityItem
    {
        [LabelText("动画控制器")] private Animator _animator;

        public void Init()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void PausePlay()
        {
            _animator.speed = 0;
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void ContinuePlay()
        {
            _animator.speed = 1;
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName">动画名称</param>
        /// <param name="animProgress">动画进度</param>
        /// <param name="animSpeed">动画速度</param>
        public async UniTask PlayAnim(string animName, float animProgress, float animSpeed = 1)
        {
            _animator.speed = animSpeed;
            animProgress = Math.Clamp(animProgress, 0.01f, 1);
            _animator.Play(animName, 0, animProgress);
            //动画总时长*剩余动画进度/动画速度
            if (animSpeed == 0)
            {
                await U_AddTask(animName, 0);
            }
            else
            {
                await U_AddTask(animName, GetPlayAnimLength(animName) * (1 - animProgress) / animSpeed);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName">动画名称</param>
        /// <param name="animSpeedProgress">动画进度</param>
        public async UniTask PlayAnim(string animName, AnimSpeedProgress animSpeedProgress)
        {
            if (animSpeedProgress == AnimSpeedProgress.Start)
            {
                await PlayAnim(animName, 0, 0);
            }
            else if (animSpeedProgress == AnimSpeedProgress.End)
            {
                await PlayAnim(animName, 1, 0);
            }
        }

        /// <summary>
        /// 获得当前动画播放是否完毕
        /// </summary>
        /// <returns></returns>
        public bool GetAnimClipPlayOver()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.90f && _animator.gameObject.activeSelf;
        }


        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(string animType)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType)
                {
                    return item.length / GetPlayAnimPlaySpeed();
                }
            }

            return -1;
        }

        /// <summary>
        /// 获得动画播放速度
        /// </summary>
        /// <returns></returns>
        public float GetPlayAnimPlaySpeed()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).speed;
        }

        /// <summary>
        /// 获得动画状态
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public bool GetAnimState(string animType)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
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
        }
    }
}