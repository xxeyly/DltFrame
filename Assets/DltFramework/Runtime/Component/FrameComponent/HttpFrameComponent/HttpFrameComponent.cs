﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace DltFramework
{
    public class HttpFrameComponent : FrameComponent
    {
        public static HttpFrameComponent instance;
        private UnityWebRequest webRequest;
        [LabelText("是否联网")] public bool notReachable;

        /// <summary>
        /// Http请求模式
        /// </summary>
        public enum HttpRequestMethod
        {
            GET,
            PUT,
            DELETE,
            POST
        }

        private void Update()
        {
            notReachable = IsConnected();
        }


        /// <summary>
        /// 判断连接状态
        /// </summary>
        private bool IsConnected()
        {
            /*int dwFlag = new int();
            if (!HttpFrameComponentExtern.InternetGetConnectedState(ref dwFlag, 0))
            {
                if ((dwFlag & 0x14) == 0)
                {
                    return false;
                }
            }
            else
            {
                if ((dwFlag & 0x01) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x02) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x04) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x40) != 0)
                {
                    return true;
                }
            }*/

            return false;
        }


        public override void FrameInitComponent()
        {
            instance = this;
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
            instance = null;
        }


        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求方式</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="requestData">请求数据</param>
        public async void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "")
        {
            await UnityHttpWebRequest(url, requestMethod, action, errorAction, requestData);
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求模式</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="errorAction">错误执行事件</param>
        public async void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            await HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction);
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求模式</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="errorAction">错误执行事件</param>
        /// <returns></returns>
        public async UniTask<string> HttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                case HttpRequestMethod.PUT:
                case HttpRequestMethod.DELETE:
                    webRequest = UnityWebRequest.Get(DataFrameComponent.String_BuilderString(url, DictionaryToString(requestData)));
                    break;
                case HttpRequestMethod.POST:
                    WWWForm wwwForm = new WWWForm();
                    foreach (KeyValuePair<string, string> pair in requestData)
                    {
                        wwwForm.AddField(Regex.Unescape(pair.Key), Regex.Unescape(pair.Value));
                    }

                    webRequest = UnityWebRequest.Post(url, wwwForm);
                    break;
            }

            if (webRequest != null)
            {
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    errorAction.Invoke(DataFrameComponent.String_BuilderString(this.webRequest.url, ":", this.webRequest.error));
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text));
                }

                webRequest.Dispose();
            }

            return string.Empty;
        }

        /// <summary>
        /// 将Dictionary转换为字符串
        /// </summary>
        /// <param name="parameter">字符串字典</param>
        /// <returns></returns>
        private string DictionaryToString(Dictionary<string, string> parameter)
        {
            string content = String.Empty;
            foreach (KeyValuePair<string, string> pair in parameter)
            {
                if (content == string.Empty)
                {
                    content = DataFrameComponent.String_BuilderString(content, "?");
                }
                else
                {
                    content = DataFrameComponent.String_BuilderString(content, "&");
                }

                content = DataFrameComponent.String_BuilderString(content, pair.Key, "=", pair.Value);
            }


            return Regex.Unescape(content);
        }

        public async UniTask<string> UnityHttpWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "")
        {
            if (requestData.Length == 0)
            {
                requestData += requestMethod;
            }

            byte[] databyte = Encoding.UTF8.GetBytes(requestData);
            webRequest = new UnityWebRequest(url, requestMethod.ToString());
            webRequest.uploadHandler = new UploadHandlerRaw(databyte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                errorAction.Invoke(DataFrameComponent.String_BuilderString(webRequest.url, ":", webRequest.error));
            }
            else
            {
                action.Invoke(Regex.Unescape(webRequest.downloadHandler.text));
            }

            return String.Empty;
        }
    }
}