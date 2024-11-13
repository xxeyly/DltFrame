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

        public void E_EntityAllHide()
        {
            EntityFrameComponent.Instance.EntityAllHide();
        }

        public void E_EntityAllShow()
        {
            EntityFrameComponent.Instance.EntityAllShow();
        }

        public T E_GetEntity<T>(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity<T>(entityName);
        }

        public EntityItem E_GetEntity(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntity(entityName);
        }

        public void E_DisplayEntity(bool display, string entityName)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityName);
        }

        public void E_DisplayEntity(bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntity(display, entityNames);
        }

        public void E_DisplayEntity(string processName, bool display, params string[] entityNames)
        {
            EntityFrameComponent.Instance.DisplayEntity(processName, display, entityNames);
        }

        public void E_EntityReleaseProcess(string processName)
        {
            EntityFrameComponent.Instance.EntityReleaseProcess(processName);
        }

        public bool E_GetEntityState(string entityName)
        {
            return EntityFrameComponent.Instance.GetEntityState(entityName);
        }

        public void E_RemoveEntity(string entityName)
        {
            EntityFrameComponent.Instance.RemoveEntity(entityName);
        }

        public GameObject E_Instantiate(GameObject instantiateObj)
        {
            return EntityFrameComponent.Instance.Instantiate(instantiateObj);
        }

        public GameObject E_Instantiate(GameObject instantiateObj, Transform parent, bool world)
        {
            return EntityFrameComponent.Instance.Instantiate(instantiateObj, parent, world);
        }

        #endregion

        #region 视图

        public bool V_GetViewExistence(Type view)
        {
            return ViewFrameComponent.Instance.GetViewExistence(view);
        }

        public bool V_GetViewState(Type view)
        {
            return ViewFrameComponent.Instance.GetViewState(view);
        }

        public BaseWindow V_GetView(Type view)
        {
            return ViewFrameComponent.Instance.GetView(view);
        }

        public int V_GetCurrentActiveViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentActiveViewCount();
        }

        public int V_GetCurrentSceneViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentSceneViewCount();
        }

        public void V_ShowView(Type type)
        {
            ViewFrameComponent.Instance.ShowView(type);
        }

        public void V_ShowView(params Type[] types)
        {
            ViewFrameComponent.Instance.ShowView(types);
        }

        public void V_HideView(Type type)
        {
            ViewFrameComponent.Instance.HideView(type);
        }

        public void V_HideView(params Type[] types)
        {
            ViewFrameComponent.Instance.HideView(types);
        }

        public void V_HideAllView()
        {
            ViewFrameComponent.Instance.HideAllView();
        }

        #endregion

        #region 音效

        public void A_PlayEffectAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayEffectAudio(audioName);
        }

        public float A_GetEffectAudioLength(string audioName)
        {
            return AudioFrameComponent.Instance.GetEffectAudioLength(audioName);
        }

        public void A_PlayEffectAudio(AudioClip audioClip)
        {
            AudioFrameComponent.Instance.PlayEffectAudio(audioClip);
        }

        public void A_PlayTipAndDialogAudio(AudioClip audioClip)
        {
            AudioFrameComponent.Instance.PlayTipAndDialogAudio(audioClip);
        }

        public void A_PlayTipAndDialogAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayTipAndDialogAudio(audioName);
        }

        public void A_StopEffectAudio()
        {
            AudioFrameComponent.Instance.StopEffectAudio();
        }

        public void A_SetEffectVolume(float volume)
        {
            AudioFrameComponent.Instance.SetEffectVolume(volume);
        }

        public void A_StopTipAndDialogAudio()
        {
            AudioFrameComponent.Instance.StopTipAndDialogAudio();
        }

        public void A_AudioPause()
        {
            AudioFrameComponent.Instance.Pause();
        }

        public void A_AudioContinue()
        {
            AudioFrameComponent.Instance.Continue();
        }

        public void A_SwitchBackgroundState()
        {
            AudioFrameComponent.Instance.SwitchBackgroundState();
        }

        public void A_PauseBackgroundAudio()
        {
            AudioFrameComponent.Instance.PauseBackgroundAudio();
        }

        public void A_StopBackgroundAudio()
        {
            AudioFrameComponent.Instance.StopBackgroundAudio();
        }

        public void A_PlayBackgroundAudio()
        {
            AudioFrameComponent.Instance.PlayBackgroundAudio();
        }

        public void A_PlayBackgroundAudio(string audioName)
        {
            AudioFrameComponent.Instance.PlayBackgroundAudio(audioName);
        }

        #endregion

        #region 计时器

        public async UniTask U_AddSceneTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            await UniTaskFrameComponent.Instance.AddSceneTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        public UniTask U_AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        public void U_RemoveTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }

        public void U_RemoveSceneTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveSceneTask(taskName);
        }

        #endregion

        #region 场景加载

        public async void S_SceneLoad(string sceneName, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        public async void S_SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        public void S_SceneEsc()
        {
            SceneLoadFrameComponent.Instance.SceneEsc();
        }

        #endregion

        #region Http请求

        public async void H_SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "")
        {
            await HttpFrameComponent.instance.UnityHttpWebRequest(url, requestMethod, action, errorAction, requestData);
        }

        public async void H_SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action,
            Action<string> errorAction)
        {
            await HttpFrameComponent.instance.HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction);
        }

        #endregion
    }
}