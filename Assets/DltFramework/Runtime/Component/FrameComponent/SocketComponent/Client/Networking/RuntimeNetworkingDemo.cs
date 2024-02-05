using DltFramework;
using UnityEngine;

public class RuntimeNetworkingDemo : SceneComponent, IRuntimeNetworking
{
   
    public void NetworkingState(bool state)
    {
        if (!state)
        {
            Debug.Log("网络异常");
        }
    }

    public override void StartComponent()
    {
        Debug.Log(GetComponent<IRuntimeNetworking>());
    }

    public override void EndComponent()
    {
    }
}