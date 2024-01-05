using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HotFixRuntimeUpdatePanel : MonoBehaviour
{
    public GameObject initPanel;
    public GameObject localFileCheckPanel;
    public Slider localFileCheckSlider;
    public Text localFileCheckText;
    public GameObject downPanel;
    [LabelText("下载进度条")] public Slider downSliderProgress;
    [LabelText("下载进度")] public Text downTextProgress;
    [LabelText("下载速度")] public Text downTextSpeed;
    [LabelText("总下载大小")] public Text totalDownload;
    public GameObject networkPanel;

    private void Awake()
    {
        //表和本地检测
        HotFixRuntimeFileCheck.HotFixRuntimeTableDownStart += HotFixRuntimeFileCheck_HotFixRuntimeTableDownStart;
        HotFixRuntimeFileCheck.HotFixRuntimeTableDownOver += HotFixRuntimeFileCheck_HotFixRuntimeTableDownOver;
        HotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheck += HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheck;
        HotFixRuntimeFileCheck.HotFixRuntimeLocalFileCheckOver += HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheckOver;

        //下载
        HotFixRuntimeFileDown.HotFixRuntimeDownStart += HotFixRuntimeFileDown_HotFixRuntimeDownStart;
        HotFixRuntimeFileDown.HotFixRuntimeDownOver += HotFixRuntimeFileDown_HotFixRuntimeDownOver;
        HotFixRuntimeFileDown.HotFixRuntimeDownSpeed += HotFixRuntimeFileDown_HotFixRuntimeDownSpeed;
        HotFixRuntimeFileDown.HotFixRuntimeDownloadValue += HotFixRuntimeFileDown_HotFixRuntimeCurrentDownValue;

        //网络
        HotFixNetworking.NetworkingState += HotFixNetworking_NetworkingState;
    }

    private void HotFixNetworking_NetworkingState(bool state)
    {
        networkPanel.SetActive(!state);
    }


    private void HotFixRuntimeFileDown_HotFixRuntimeDownOver()
    {
        downPanel.SetActive(false);
    }

    private void HotFixRuntimeFileDown_HotFixRuntimeDownStart()
    {
        downPanel.SetActive(true);
    }

    private void HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheckOver()
    {
        localFileCheckPanel.SetActive(false);
    }

    private void HotFixRuntimeFileCheck_HotFixRuntimeTableDownOver()
    {
        initPanel.SetActive(false);
        localFileCheckPanel.SetActive(true);
    }

    private void HotFixRuntimeFileCheck_HotFixRuntimeTableDownStart()
    {
        initPanel.SetActive(true);
    }

    private void HotFixRuntimeFileDown_HotFixRuntimeCurrentDownValue(double current, double total)
    {
        totalDownload.text = HotFixGlobal.FileSizeString(current) + "/" + HotFixGlobal.FileSizeString(total);
        downSliderProgress.value = (float)(current / total);
        downTextProgress.text = (current / total * 100).ToString("0") + "/100";
    }

    private void HotFixRuntimeFileDown_HotFixRuntimeDownSpeed(float downSpeed)
    {
        downTextSpeed.text = HotFixGlobal.FileSizeString(downSpeed) + "/s";
    }

    private void HotFixRuntimeFileCheck_HotFixRuntimeLocalFileCheck(int currentCount, int maxCount)
    {
        localFileCheckSlider.value = (float)currentCount / maxCount;
        localFileCheckText.text = (int)(localFileCheckSlider.value * 100) + "/100";
    }
}