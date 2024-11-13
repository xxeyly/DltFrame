using System;
using System.Collections.Generic;

namespace DltFramework
{
    public interface IHttpExtend
    {
        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求方式</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="requestData">请求数据</param>
        public void H_SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Action<string> action, Action<string> errorAction, string requestData = "");

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求模式</param>
        /// <param name="requestData">请求数据</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="errorAction">错误执行事件</param>
        public void H_SendHttpUnityWebRequest(string url, HttpFrameComponent.HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction);
    }
}