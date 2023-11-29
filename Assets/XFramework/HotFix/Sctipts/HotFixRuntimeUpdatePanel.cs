using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotFixRuntimeUpdatePanel : MonoBehaviour
{
    private void Awake()
    {
        HotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck += HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheck;
        HotFixRuntimeFileDown.HotFixRuntimeDownSpeed += HotFixRuntimeFileDown_HotFixRuntimeDownSpeed;
        HotFixRuntimeFileDown.HotFixRuntimeCurrentDownValue += HotFixRuntimeFileDown_HotFixRuntimeCurrentDownValue;
        HotFixRuntimeFileDown.HotFixViewRuntimeTotalDownValue += HotFixRuntimeFileDown_HotFixViewRuntimeTotalDownValue;
    }

    private void HotFixRuntimeFileDown_HotFixViewRuntimeTotalDownValue(double downvalue)
    {
        Debug.Log("总的下载大小:" + HotFixGlobal.FileSizeString(downvalue));
    }

    private void HotFixRuntimeFileDown_HotFixRuntimeCurrentDownValue(double downvalue)
    {
        Debug.Log("当前下载大小:" + HotFixGlobal.FileSizeString(downvalue));
    }

    private void HotFixRuntimeFileDown_HotFixRuntimeDownSpeed(float downspeed)
    {
        Debug.Log("当前下载速度:" + HotFixGlobal.FileSizeString(downspeed));
    }

    private void HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheck(int currentcount, int maxcount)
    {
        Debug.Log("当前检测文件:" + currentcount + "/" + maxcount);
    }
}