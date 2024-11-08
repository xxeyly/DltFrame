using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    public partial class ExtendMonoBehaviour : MonoBehaviour, IEntityExtend, IViewExtend, IAudioExtend, IUniTaskExtend, ISceneLoadExtend, IHttpExtend
    {
        #region 实体

        public void EntityAllHide()
        {
            EntityFrameComponent.Instance.EntityAllHide();
        }

        public void EntityAllShow()
        {
            EntityFrameComponent.Instance.EntityAllShow();
        }

        public T GetEntity<T>(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity<T>(entityName);
        }

        public EntityItem GetEntity(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity(entityName);
        }

        public void DisplayEntity(bool display, string entityName)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityName);
        }

        public void DisplayEntity(bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityNames);
        }

        public void DisplayEntity(string processName, bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntity(processName, display, entityNames);
        }

        public void EntityReleaseProcess(string processName)
        {
            EntityFrameComponent.Instance.EntityReleaseProcess(processName);
        }

        public bool GetEntityState(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntityState(entityName);
        }

        public void RemoveEntity(string entityName)
        {
            EntityFrameComponent.Instance.RemoveEntity(entityName);
        }

        public GameObject Instantiate(GameObject instantiateObj)
        {
            return EntityFrameComponent.Instance.Instantiate(instantiateObj);
        }

        public GameObject Instantiate(GameObject instantiateObj, Transform parent, bool world)
        {
            return EntityFrameComponent.Instance.Instantiate(instantiateObj, parent, world);
        }

        #endregion

        #region 视图

        public bool GetViewExistence(Type view)
        {
            return ViewFrameComponent.Instance.GetViewExistence(view);
        }

        public bool GetViewState(Type view)
        {
            return ViewFrameComponent.Instance.GetViewState(view);
        }

        public BaseWindow GetView(Type view)
        {
            return ViewFrameComponent.Instance.GetView(view);
        }

        public int GetCurrentActiveViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentActiveViewCount();
        }

        public int GetCurrentSceneViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentSceneViewCount();
        }

        public void ShowView(Type type)
        {
            ViewFrameComponent.Instance.ShowView(type);
        }

        public void ShowView(params Type[] types)
        {
            ViewFrameComponent.Instance.ShowView(types);
        }

        public void HideView(Type type)
        {
            ViewFrameComponent.Instance.HideView(type);
        }

        public void HideView(params Type[] types)
        {
            ViewFrameComponent.Instance.HideView(types);
        }

        public void HideAllView()
        {
            ViewFrameComponent.Instance.HideAllView();
        }

        #endregion

        #region 音效

        public void PlayEffectAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayEffectAudio(audioName);
        }

        public float GetEffectAudioLength(string audioName)
        {
            return AudioFrameComponent.Instance.GetEffectAudioLength(audioName);
        }

        public void PlayEffectAudio(AudioClip audioClip)
        {
            AudioFrameComponent.Instance.PlayEffectAudio(audioClip);
        }

        public void PlayTipAndDialogAudio(AudioClip audioClip)
        {
            AudioFrameComponent.Instance.PlayTipAndDialogAudio(audioClip);
        }

        public void PlayTipAndDialogAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayTipAndDialogAudio(audioName);
        }

        public void StopEffectAudio()
        {
            AudioFrameComponent.Instance.StopEffectAudio();
        }

        public void SetEffectVolume(float volume)
        {
            AudioFrameComponent.Instance.SetEffectVolume(volume);
        }

        public void StopTipAndDialogAudio()
        {
            AudioFrameComponent.Instance.StopTipAndDialogAudio();
        }

        public void AudioPause()
        {
            AudioFrameComponent.Instance.Pause();
        }

        public void AudioContinue()
        {
            AudioFrameComponent.Instance.Continue();
        }

        public void SwitchBackgroundState()
        {
            AudioFrameComponent.Instance.SwitchBackgroundState();
        }

        public void PauseBackgroundAudio()
        {
            AudioFrameComponent.Instance.PauseBackgroundAudio();
        }

        public void StopBackgroundAudio()
        {
            AudioFrameComponent.Instance.StopBackgroundAudio();
        }

        public void PlayBackgroundAudio()
        {
            AudioFrameComponent.Instance.PlayBackgroundAudio();
        }

        public void PlayBackgroundAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayBackgroundAudio(audioName);
        }

        #endregion

        #region 计时器

        public async UniTask AddSceneTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            await UniTaskFrameComponent.Instance.AddSceneTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        public UniTask AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        public void RemoveTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }

        public void RemoveSceneTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveSceneTask(taskName);
        }

        #endregion

        #region 场景加载

        async void ISceneLoadExtend.SceneLoad(string sceneName, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        async void ISceneLoadExtend.SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        #endregion

        #region Http请求

        public async void SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "")
        {
            await HttpFrameComponent.instance.UnityHttpWebRequest(url, requestMethod, action, errorAction, requestData);
        }

        public async void SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action,
            Action<string> errorAction)
        {
            await HttpFrameComponent.instance.HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction);
        }

        #endregion
    }
}