using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// 音乐组件
    /// </summary>
    public class AudioComponent : ComponentBase
    {
        public static AudioComponent Instance;
        private AudioSource _backgroundAudioSource;
        private AudioSource _effectAudioSource;
        private AudioSource _tipAndDialogAudioSource;

        [Searchable] [TableList(AlwaysExpanded = true)] [InlineEditor()] [Required("选择音频配置文件")] [LabelText("音频数据")]
        public AudioComponentData audioData;

        private Dictionary<string, AudioClip> _audioDlc;

        public override void FrameInitComponent()
        {
            Instance = GetComponent<AudioComponent>();
        }

        public override void EndComponent()
        {
        }

        public override void SceneInitComponent()
        {
            //创建音效组件
            if (_effectAudioSource == null)
            {
                _effectAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = false;
            }

            //创建提示与对话组件
            if (_tipAndDialogAudioSource == null)
            {
                _tipAndDialogAudioSource = gameObject.AddComponent<AudioSource>();
                _tipAndDialogAudioSource.playOnAwake = false;
            }

            //创建背景音乐组件
            if (_backgroundAudioSource == null)
            {
                _backgroundAudioSource = gameObject.AddComponent<AudioSource>();
                _effectAudioSource.playOnAwake = true;
                _backgroundAudioSource.volume = 0.5f;
                _backgroundAudioSource.loop = true;
            }

            //音效初始化
            _audioDlc = new Dictionary<string, AudioClip>();
            if (audioData != null)
            {
                foreach (AudioComponentData.AudioInfo audioDataInfo in audioData.audioInfos)
                {
                    if (!_audioDlc.ContainsKey(audioDataInfo.audioName) && audioDataInfo.audioClip != null)
                    {
                        _audioDlc.Add(audioDataInfo.audioName, audioDataInfo.audioClip);
                    }
                }
            }

            //播放背景音乐
            PlayBackgroundAudio();
        }


        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioType"></param>
        public void PlayEffectAudio(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                _effectAudioSource.clip = _audioDlc[audioName];
                _effectAudioSource.Play();
            }
        }

        public float GetEffectAudioLength(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                return _effectAudioSource.clip.length;
            }

            return -1;
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
            _effectAudioSource.clip = null;
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
            if (RuntimeDataComponent.Instance.audioState)
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
            PlayEffectAudio("背景音乐");
            _backgroundAudioSource.Pause();
            RuntimeDataComponent.Instance.audioState = false;
        }

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio()
        {
            if (_audioDlc.ContainsKey("背景音乐") && _audioDlc["背景音乐"] != null)
            {
                _backgroundAudioSource.clip = _audioDlc["背景音乐"];
                _backgroundAudioSource.Play();
                RuntimeDataComponent.Instance.audioState = true;
            }
            else
            {
                // Debug.LogError("没有指定背景音乐");
            }
        }
    }
}