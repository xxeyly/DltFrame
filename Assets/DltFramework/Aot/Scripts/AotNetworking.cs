using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NetworkingState(bool downSpeed);


public class AotNetworking : MonoBehaviour
{
    public static NetworkingState NetworkingState;
    public float time = 0;
    public float timer = 1;
    List<IAotNetworking> _aotNetworkings = new List<IAotNetworking>();

    private void Start()
    {
        _aotNetworkings = AotGlobal.GetAllObjectsInScene<IAotNetworking>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
        {
            time = 0;
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    foreach (IAotNetworking aotNetworking in _aotNetworkings)
                    {
                        aotNetworking.NetworkingState(false);
                    }

                    // AotDebug.Log("断网了");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    foreach (IAotNetworking aotNetworking in _aotNetworkings)
                    {
                        aotNetworking.NetworkingState(true);
                    }

                    // AotDebug.Log("移动联网");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    foreach (IAotNetworking aotNetworking in _aotNetworkings)
                    {
                        aotNetworking.NetworkingState(true);
                    }

                    // AotDebug.Log("Wifi或者有线");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}