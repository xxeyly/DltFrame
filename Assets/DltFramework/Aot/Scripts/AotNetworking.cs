using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Aot
{
    public class AotNetworking : MonoBehaviour
    {
        List<IAotNetworking> _aotNetworkings = new List<IAotNetworking>();
        private UnityWebRequest _webRequest;

        //开启网络状态检测
        public static bool networkStatusDetection = true;

        private void Start()
        {
            _aotNetworkings = AotGlobal.GetAllObjectsInScene<IAotNetworking>();
            StartCoroutine(Networking());
        }

        IEnumerator Networking()
        {
            yield return new WaitForSeconds(1f);
            _webRequest = UnityWebRequest.Get("https://www.baidu.com");
            yield return _webRequest.SendWebRequest();
            if (_webRequest.responseCode != 200)
            {
                foreach (var aotNetworking in _aotNetworkings)
                {
                    aotNetworking.NetworkingState(false);
                }
            }
            else
            {
                foreach (var aotNetworking in _aotNetworkings)
                {
                    aotNetworking.NetworkingState(true);
                }
            }

            if (networkStatusDetection)
            {
                StartCoroutine(Networking());
            }
        }
    }
}