using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HotFixNetworking : MonoBehaviour
{
    public float time = 0;
    public float timer = 1;
    List<IHotFixNetworking> _hotFixNetworkings = new List<IHotFixNetworking>();

    private void Start()
    {
        _hotFixNetworkings = HotFixGlobal.GetAllObjectsInScene<IHotFixNetworking>();
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
                    foreach (IHotFixNetworking hotFixNetworking in _hotFixNetworkings)
                    {
                        hotFixNetworking.NetworkingState(false);
                    }

                    // HotFixDebug.Log("断网了");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    foreach (IHotFixNetworking hotFixNetworking in _hotFixNetworkings)
                    {
                        hotFixNetworking.NetworkingState(true);
                    }

                    // HotFixDebug.Log("移动联网");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    foreach (IHotFixNetworking hotFixNetworking in _hotFixNetworkings)
                    {
                        hotFixNetworking.NetworkingState(true);
                    }

                    // HotFixDebug.Log("Wifi或者有线");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}