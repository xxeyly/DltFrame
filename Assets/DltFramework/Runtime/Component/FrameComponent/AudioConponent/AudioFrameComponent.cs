using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CollectionNeverUpdated.Local

namespace DltFramework
{
    /// <summary>
    /// 音乐组件
    /// </summary>
    public class AudioFrameComponent : FrameComponent
    {
        public static AudioFrameComponent Instance;
        private AudioSource _backgroundAudioSource;
        private AudioSource _effectAudioSource;
        private AudioSource _tipAndDialogAudioSource;

        [Searchable] [TableList(AlwaysExpanded = true)] [InlineEditor()] [Required("选择音频配置文件")] [LabelText("音频数据")]
        public AudioComponentData audioData;

        [LabelText("背景音乐名称")] [ShowIf("@audioData !=null")] [ValueDropdown("GetAudioNameListOfAudioComponentData")]
        public string backgroundAudioName;

        private IEnumerable<string> GetAudioNameListOfAudioComponentData()
        {
            List<string> selfObj = new List<string>();
            if (audioData != null)
            {
                foreach (AudioComponentData.AudioInfo audioInfo in audioData.audioInfos)
                {
                    selfObj.Add(audioInfo.audioName);
                }
            }

            return selfObj;
        }

        [Button("修改音频名字")]
        public void ChangeAudioName()
        {
            foreach (AudioComponentData.AudioInfo audioInfo in audioData.audioInfos)
            {
                audioInfo.audioName = audioInfo.audioClip.name;
            }
        }

        private Dictionary<string, AudioClip> _audioDlc = new Dictionary<string, AudioClip>();


        public override void FrameInitComponent()
        {
            Instance = this;
            if (audioData != null)
            {
                foreach (AudioComponentData.AudioInfo audioInfo in audioData.audioInfos)
                {
                    _audioDlc.Add(audioInfo.audioName, audioInfo.audioClip);
                }
            }

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
        }


        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public async void PlayEffectAudio(string audioName)
        {
            await UniTask.NextFrame();
            if (_audioDlc.ContainsKey(audioName))
            {
                _effectAudioSource.volume = 1;
                _effectAudioSource.clip = _audioDlc[audioName];
                _effectAudioSource.Play();
            }
            else
            {
                Debug.LogWarning(audioName + "音效不存在");
            }
        }

        /// <summary>
        /// 获取音效长度
        /// </summary>
        /// <param name="audioName">音频名称</param>
        /// <returns></returns>
        public float GetEffectAudioLength(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                if (_effectAudioSource.clip != null)
                {
                    return _effectAudioSource.clip.length;
                }
            }

            return -1;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void PlayEffectAudio(AudioClip audioClip)
        {
            _effectAudioSource.clip = audioClip;
            _effectAudioSource.Play();
        }

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void PlayTipAndDialogAudio(AudioClip audioClip)
        {
            _tipAndDialogAudioSource.Stop();
            _tipAndDialogAudioSource.clip = audioClip;
            _tipAndDialogAudioSource.Play();
        }

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void PlayTipAndDialogAudio(string audioName)
        {
            if (_audioDlc.ContainsKey(audioName))
            {
                _tipAndDialogAudioSource.volume = 1;
                _tipAndDialogAudioSource.clip = _audioDlc[audioName];
                _tipAndDialogAudioSource.Play();
            }
            else
            {
                Debug.LogWarning(audioName + "音效不存在");
            }
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
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetEffectVolume(float volume)
        {
            _effectAudioSource.volume = volume;
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
            if (RuntimeDataFrameComponent.Instance.audioState)
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
            RuntimeDataFrameComponent.Instance.audioState = false;
        }

        public void StopBackgroundAudio()
        {
            _backgroundAudioSource.clip = null;
            _backgroundAudioSource.Stop();
        }

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio()
        {
            if (_audioDlc.ContainsKey(backgroundAudioName) && _audioDlc[backgroundAudioName] != null)
            {
                _backgroundAudioSource.clip = _audioDlc[backgroundAudioName];
                _backgroundAudioSource.Play();
                RuntimeDataFrameComponent.Instance.audioState = true;
            }
            else
            {
                // Debug.LogError("没有指定背景音乐");
            }
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void PlayBackgroundAudio(string audioName)
        {
            _backgroundAudioSource.clip = _audioDlc[audioName];
            _backgroundAudioSource.Play();
            RuntimeDataFrameComponent.Instance.audioState = true;
        }
    }
}