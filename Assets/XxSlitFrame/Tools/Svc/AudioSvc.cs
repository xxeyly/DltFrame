using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 音乐类型
    /// </summary>
    public enum AudioType
    {
        /// <summary>
        /// 默认音乐类型
        /// </summary>
        [Header("默认音乐类型")] ENormal,

        /// <summary>
        /// 背景音乐
        /// </summary>
        [Header("背景音乐")] EBackground,

        /// <summary>
        /// 进入场景
        /// </summary>
        [Header("进入场景")] EEnterExperiment,

        /// <summary>
        /// 错误弹窗
        /// </summary>
        [Header("错误弹窗")] EErrorPopup,

        /// <summary>
        /// 提示弹窗
        /// </summary>
        [Header("提示弹窗")] ETipsPopup,

        /// <summary>
        /// 关闭
        /// </summary>
        [Header("关闭")] EClose,

        /// <summary>
        /// 关闭背景音乐
        /// </summary>
        [Header("关闭背景音乐")] ECloseBackground,

        /// <summary>
        /// 确定
        /// </summary>
        [Header("确定")] ESure,

        /// <summary>
        /// 跳步骤
        /// </summary>
        [Header("跳步骤")] ENextStep,

        /// <summary>
        /// 重做,下一步
        /// </summary>
        [Header("重做")] ERedo,

        /// <summary>
        /// 鼠标点击
        /// </summary>
        [Header("鼠标点击")] EClick,

        /// <summary>
        /// 显示物品
        /// </summary>
        DisplayObjects
    }

    /// <summary>
    /// 音乐服务
    /// </summary>
    public class AudioSvc : SvcBase<AudioSvc>
    {
        private AudioSource _backgroundAudioSource;
        private AudioSource _effectAudioSource;
        private AudioSource _tipAndDialogAudioSource;
        [Header("音乐列表")] public Dictionary<AudioType, AudioClip> audioDlc = new Dictionary<AudioType, AudioClip>();

        public override void InitSvc()
        {
            if (_effectAudioSource == null)
            {
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = false;
            }

            if (_tipAndDialogAudioSource == null)
            {
                _tipAndDialogAudioSource = gameObject.AddComponent<AudioSource>();
                _tipAndDialogAudioSource.playOnAwake = false;
            }

            if (_backgroundAudioSource == null)
            {
                _backgroundAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = true;
                _backgroundAudioSource.volume = 0.5f;
                _backgroundAudioSource.loop = true;
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioType"></param>
        public void PlayEffectAudio(AudioType audioType)
        {
            if (audioDlc.ContainsKey(audioType))
            {
                _effectAudioSource.clip = audioDlc[audioType];
                _effectAudioSource.Play();
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayEffectAudio(AudioClip audioClip)
        {
            _effectAudioSource.clip = audioClip;
            _effectAudioSource.Play();
        }

        public void PlayTipAndDialogAudio(AudioClip audioClip)
        {
            _tipAndDialogAudioSource.Stop();
            _tipAndDialogAudioSource.clip = audioClip;
            _tipAndDialogAudioSource.Play();
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopEffectAudio()
        {
            _effectAudioSource.Stop();
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopTipAndDialogAudio()
        {
            _tipAndDialogAudioSource.Stop();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _effectAudioSource.Pause();
            _backgroundAudioSource.Pause();
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            _effectAudioSource.UnPause();
            if (PersistentDataSvc.Instance.audioState)
            {
                _backgroundAudioSource.UnPause();
            }
        }


        /// <summary>
        /// 切换音乐状态
        /// </summary>
        public void SwitchBackgroundState()
        {
            if (_backgroundAudioSource.isPlaying)
            {
                PauseBackgroundAudio();
            }
            else
            {
                PlayBackgroundAudio();
            }
        }

        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void PauseBackgroundAudio()
        {
            PlayEffectAudio(AudioType.ECloseBackground);
            _backgroundAudioSource.Pause();
            PersistentDataSvc.Instance.audioState = false;
        }

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio()
        {
            _backgroundAudioSource.clip = audioDlc[AudioType.EBackground];
            _backgroundAudioSource.Play();
            PersistentDataSvc.Instance.audioState = true;
        }
    }
}