using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NetworkingState(bool downSpeed);


public class HotFixNetworking : MonoBehaviour
{
    public static NetworkingState NetworkingState;
    public float time = 0;
    public float timer = 1;

    void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
        {
            time = 0;
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    NetworkingState?.Invoke(false);
                    // Debug.Log("断网了");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    NetworkingState?.Invoke(true);
                    // Debug.Log("移动联网");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    NetworkingState?.Invoke(true);
                    // Debug.Log("Wifi或者有线");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}