using UnityEngine;

namespace DltFramework
{
    public interface IAudioExtend
    {
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void PlayEffectAudio(string audioName);

        /// <summary>
        /// 获取音效长度
        /// </summary>
        /// <param name="audioName">音频名称</param>
        /// <returns></returns>
        public float GetEffectAudioLength(string audioName);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void PlayEffectAudio(AudioClip audioClip);

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void PlayTipAndDialogAudio(AudioClip audioClip);

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void PlayTipAndDialogAudio(string audioName);

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopEffectAudio();

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetEffectVolume(float volume);

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopTipAndDialogAudio();

        /// <summary>
        /// 暂停
        /// </summary>
        public void AudioPause();

        /// <summary>
        /// 继续
        /// </summary>
        public void AudioContinue();


        /// <summary>
        /// 切换音乐状态
        /// </summary>
        public void SwitchBackgroundState();

        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void PauseBackgroundAudio();

        public void StopBackgroundAudio();

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void PlayBackgroundAudio();

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void PlayBackgroundAudio(string audioName);
    }
}