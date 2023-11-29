using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotFixUpdatePanel : MonoBehaviour
{
    private void Awake()
    {
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeDownSpeed += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeDownSpeed;
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeCurrentDownValue += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeCurrentDownValue;
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeTotalDownValue += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeTotalDownValue;
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeTotalDownValue(double downvalue)
    {
        Debug.Log("总的下载大小:" + AotGlobal.FileSizeString(downvalue));
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeCurrentDownValue(double downvalue)
    {
        Debug.Log("当前下载大小:" + AotGlobal.FileSizeString(downvalue));
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeDownSpeed(float downspeed)
    {
        Debug.Log("当前下载速度:" + AotGlobal.FileSizeString(downspeed));
    }
}