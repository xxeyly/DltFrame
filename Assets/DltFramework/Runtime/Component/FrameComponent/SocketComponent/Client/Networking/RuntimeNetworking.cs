using System.Collections;
using System.Collections.Generic;
using DltFramework;
using UnityEngine;
using UnityEngine.Networking;

public class RuntimeNetworking : SceneComponent
{
    public List<IRuntimeNetworking> _aotNetworkings;

    //开启网络状态检测
    public bool networkStatusDetection = true;
    private UnityWebRequest _webRequest;

    public override void StartComponent()
    {
        _aotNetworkings = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRuntimeNetworking>();
        StartCoroutine(Networking());
    }

    public override void EndComponent()
    {
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