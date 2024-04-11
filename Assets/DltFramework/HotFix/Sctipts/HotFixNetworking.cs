using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HotFix
{

    public class HotFixNetworking : MonoBehaviour
    {
        List<IHotFixNetworking> _aotNetworkings = new List<IHotFixNetworking>();
        private UnityWebRequest _webRequest;

        //开启网络状态检测
        public static bool networkStatusDetection = true;

        private void Start()
        {
            _aotNetworkings = HotFixGlobal.GetAllObjectsInScene<IHotFixNetworking>();
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