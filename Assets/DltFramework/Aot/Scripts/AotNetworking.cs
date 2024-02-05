using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AotNetworking : MonoBehaviour
{
    public float time = 0;
    public float timer = 1;
    List<IAotNetworking> _aotNetworkings = new List<IAotNetworking>();

    private UnityWebRequest _webRequest;

    private void Start()
    {
        _aotNetworkings = AotGlobal.GetAllObjectsInScene<IAotNetworking>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
        {
            time = 0;
            if (PingIsHaveNet("https://www.baidu.com"))
            {
                foreach (var aotNetworking in _aotNetworkings)
                {
                    aotNetworking.NetworkingState(true);
                }
            }
            else
            {
                foreach (var aotNetworking in _aotNetworkings)
                {
                    aotNetworking.NetworkingState(false);
                }
            }
        }
    }

    public bool PingIsHaveNet(string url)
    {
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

        try
        {
            PingReply ret = ping.Send(url);
            if (ret.Status != IPStatus.Success)
            {
                // AotDebug.Log("未联网");
                return false;
            }
            else
            {
                // AotDebug.Log("已联网");
                return true;
            }
        }
        catch (Exception e)
        {
            // AotDebug.Log("Ping URL 失败");
            return false;
        }
    }
}