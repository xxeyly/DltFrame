using System;
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
        private UnityWebRequest _request;
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
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestMethod"></param>
        /// <param name="requestData"></param>
        /// <param name="action"></param>
        /// <param name="errorAction"></param>
        public async void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            await HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction);
        }

        private async UniTask<string> HttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            UnityWebRequest webRequest = null;
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                case HttpRequestMethod.PUT:
                case HttpRequestMethod.DELETE:
                    webRequest = UnityWebRequest.Get(DataFrameComponent.StringBuilderString(url, DictionaryToString(requestData)));
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
                    errorAction.Invoke(DataFrameComponent.StringBuilderString(_request.url, ":", _request.error));
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text));
                }

                webRequest.Dispose();
            }

            return string.Empty;
        }


        private string DictionaryToString(Dictionary<string, string> parameter)
        {
            string content = String.Empty;
            foreach (KeyValuePair<string, string> pair in parameter)
            {
                if (content == string.Empty)
                {
                    content = DataFrameComponent.StringBuilderString(content, "?");
                }
                else
                {
                    content = DataFrameComponent.StringBuilderString(content, "&");
                }

                content = DataFrameComponent.StringBuilderString(content, pair.Key, "=", pair.Value);
            }


            return Regex.Unescape(content);
        }

        private async UniTask<string> UnityHttpWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "")
        {
            if (requestData.Length == 0)
            {
                requestData += requestMethod;
            }

            byte[] databyte = Encoding.UTF8.GetBytes(requestData);
            _request = new UnityWebRequest(url, requestMethod.ToString());
            _request.uploadHandler = new UploadHandlerRaw(databyte);
            _request.downloadHandler = new DownloadHandlerBuffer();
            _request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            await _request.SendWebRequest();

            if (_request.result == UnityWebRequest.Result.ProtocolError)
            {
                errorAction.Invoke(DataFrameComponent.StringBuilderString(_request.url, ":", _request.error));
            }
            else
            {
                action.Invoke(Regex.Unescape(_request.downloadHandler.text));
            }

            return String.Empty;
        }
    }
}