using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HotFixUpdatePanel : MonoBehaviour
{
    [LabelText("背景")] public GameObject back;
    [LabelText("初始化面板")] public GameObject initPanel;
    [LabelText("下载面板")] public GameObject downPanel;
    [LabelText("下载进度条")] public Slider downSliderProgress;
    [LabelText("下载进度")] public Text downTextProgress;
    [LabelText("下载速度")] public Text downTextSpeed;
    [LabelText("总下载大小")] public Text totalDownload;
    [LabelText("网络状况")] public GameObject networkPanel;

    private void Awake()
    {
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeLocalIsUpdate += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeLocalIsUpdate;
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeDownSpeed += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeDownSpeed;
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeIsDown += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeIsDown;
        HotFixViewAndHotFixCodeCheck.HotFixViewAndHotFixCodeDownloadValue += HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeCurrentDownValue;
        AotNetworking.NetworkingState += AotNetworking_NetworkingState;
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeLocalIsUpdate(bool localIsUpdate)
    {
        if (localIsUpdate)
        {
            back.SetActive(true);
            initPanel.SetActive(true);
        }
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeCurrentDownValue(double currentDownValue, double totalDownValue)
    {
        totalDownload.text = AotGlobal.FileSizeString(currentDownValue) + "/" + AotGlobal.FileSizeString(totalDownValue);
        downSliderProgress.value = (float)(currentDownValue / totalDownValue);
        downTextProgress.text = (currentDownValue / totalDownValue * 100).ToString("0") + "/100";
    }

    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeIsDown(bool down)
    {
        initPanel.SetActive(false);
        downPanel.SetActive(down);
    }

    private void AotNetworking_NetworkingState(bool state)
    {
        networkPanel.SetActive(!state);
    }


    private void HotFixViewAndHotFixCodeCheck_HotFixViewAndHotFixCodeDownSpeed(float downSpeed)
    {
        downTextSpeed.text = AotGlobal.FileSizeString(downSpeed) + "/s";
    }
}