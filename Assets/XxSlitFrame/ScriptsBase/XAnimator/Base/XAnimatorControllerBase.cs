using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.Svc;

namespace XAnimator.Base
{
    public enum AnimType
    {
        /// <summary>
        /// 洗手
        /// </summary>
        WashBasin,

        /// <summary>
        /// 向患者解说
        /// </summary>
        ExplainToPatient,

        /// <summary>
        /// 戴口罩
        /// </summary>
        WearMask,

        /// <summary>
        /// 戴手套
        /// </summary>
        WearGloves,

        /// <summary>
        /// 仰卧
        /// </summary>
        LieSupine,

        /// <summary>
        /// 俯卧
        /// </summary>
        Prostrate,

        /// <summary>
        /// 坐卧
        /// </summary>
        SitDown,

        /// <summary>
        /// 消毒棉球
        /// </summary>
        DisinfectionCottonBall,

        /// <summary>
        /// 消毒碘酊
        /// </summary>
        DisinfectionTinctureIodine,

        /// <summary>
        /// 消毒酒精
        /// </summary>
        DisinfectionAlcohol,

        /// <summary>
        /// 铺巾
        /// </summary>
        Scarf,

        /// <summary>
        /// 抽取利多卡因开始
        /// </summary>
        LidocaineExtractionStart,

        /// <summary>
        /// 抽取利多卡因
        /// </summary>
        LidocaineExtraction,

        /// <summary>
        /// 抽取利多卡因结束
        /// </summary>
        LidocaineExtractionEnd,

        /// <summary>
        /// 注射起皮丘
        /// </summary>
        PimpleInjection,

        /// <summary>
        /// 间歇性回抽开始
        /// </summary>
        BackPumpingStart,

        /// <summary>
        /// 间歇性回抽结束
        /// </summary>
        BackPumpingEnd,

        /// <summary>
        /// 穿刺-垂直刺入
        /// </summary>
        PunctureVerticalPuncture,

        /// <summary>
        /// 穿刺-旋转刺入
        /// </summary>
        PunctureRotaryPuncture,

        /// <summary>
        /// 穿刺-完成刺入
        /// </summary>
        PunctureCompletePuncture,

        /// <summary>
        /// 抽取骨髓液
        /// </summary>
        ExtractMarrowFluid,

        /// <summary>
        /// 取下注射器,插入针芯
        /// </summary>
        RemoveSyringeInsertPinCore,

        /// <summary>
        /// 拔针-拔出针
        /// </summary>
        NeedlePullingNeedlePulling,

        /// <summary>
        /// 拔针-盖纱布
        /// </summary>
        NeedlePullingCoverGauze,

        /// <summary>
        /// 拔针-撤洞巾 
        /// </summary>
        NeedlePullingHoleRemovingTowel,

        /// <summary>
        /// 拔针-粘胶布
        /// </summary>
        NeedlePullingAdhesiveTape,

        /// <summary>
        /// 涂片
        /// </summary>
        Smear,

        /// <summary>
        /// 抽取穿刺针
        /// </summary>
        PullOutPunctureNeedle,

        /// <summary>
        /// 连接注射器
        /// </summary>
        ConnectSyringe,
        /// <summary>
        /// 洗手
        /// </summary>
        WashBasinTwo,
    }

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
    public abstract class XAnimatorControllerBase : MonoBehaviour
    {
        private UnityEngine.Animator _animator;
        private List<string> _animationClips;

        public void StartSvc()
        {
            _animator = GetComponent<UnityEngine.Animator>();
            _animationClips = new List<string>();
            foreach (AnimationClip animationClip in _animator.runtimeAnimatorController.animationClips)
            {
                _animationClips.Add(animationClip.name);
            }
        }

        public void PlayAnim(AnimType animationType, float animProgress)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                _animator.speed = 0;
                _animator.Play(animationType.ToString(), 0, animProgress);
            }
        }

        public void PlayAnim(AnimType animationType, AnimSpeedProgress animSpeedProgress)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                _animator.speed = 0;
                if (animSpeedProgress == AnimSpeedProgress.End)
                {
                    _animator.Play(animationType.ToString(), 0, 1);
                }
                else if (animSpeedProgress == AnimSpeedProgress.Start)
                {
                    _animator.Play(animationType.ToString(), 0, normalizedTime: 0.01f);
                }
            }
        }

        private int _playAnimTimeTask;

        /// <summary>
        /// 播放动画+事件
        /// </summary>
        /// <param name="animationType"></param>
        /// <param name="eventAction"></param>
        /// <param name="delay"></param>
        public void PlayAnim(AnimType animationType, UnityAction eventAction, float delay = 0)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                PlayAnim(animationType);
                _playAnimTimeTask = TimeSvc.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType) + delay);
            }
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animationType"></param>
        public void PlayAnim(AnimType animationType)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                _animator.speed = 1;
                _animator.SetTrigger(animationType.ToString());
            }
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(AnimType animType)
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType.ToString())
                {
                    return item.length ;
                }
            }

            return -1;
        }

        /// <summary>
        /// 停止动画计时任务
        /// </summary>
        public void StopAnimatorTimeTask()
        {
            TimeSvc.Instance.DeleteTimeTask(_playAnimTimeTask);
        }

        public void StopAnimTaskTime()
        {
            TimeSvc.Instance.DeleteTimeTask(_playAnimTimeTask);
        }
    }
}