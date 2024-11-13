using UnityEngine;

namespace DltFramework
{
    public interface IAudioExtend
    {
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void A_PlayEffectAudio(string audioName);

        /// <summary>
        /// 获取音效长度
        /// </summary>
        /// <param name="audioName">音频名称</param>
        /// <returns></returns>
        public float A_GetEffectAudioLength(string audioName);

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void A_PlayEffectAudio(AudioClip audioClip);

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioClip">音频Clip </param>
        public void A_PlayTipAndDialogAudio(AudioClip audioClip);

        /// <summary>
        /// 播放提示音
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void A_PlayTipAndDialogAudio(string audioName);

        /// <summary>
        /// 停止音效
        /// </summary>
        public void A_StopEffectAudio();

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public void A_SetEffectVolume(float volume);

        /// <summary>
        /// 停止音效
        /// </summary>
        public void A_StopTipAndDialogAudio();

        /// <summary>
        /// 暂停
        /// </summary>
        public void A_AudioPause();

        /// <summary>
        /// 继续
        /// </summary>
        public void A_AudioContinue();


        /// <summary>
        /// 切换音乐状态
        /// </summary>
        public void A_SwitchBackgroundState();

        /// <summary>
        /// 暂停背景音乐播放
        /// </summary>
        public void A_PauseBackgroundAudio();
        /// <summary>
        /// 停止背景音乐播放
        /// </summary>
        public void A_StopBackgroundAudio();

        /// <summary>
        /// 开始背景音乐播放
        /// </summary>
        public void A_PlayBackgroundAudio();

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioName">音频名称</param>
        public void A_PlayBackgroundAudio(string audioName);
    }
}