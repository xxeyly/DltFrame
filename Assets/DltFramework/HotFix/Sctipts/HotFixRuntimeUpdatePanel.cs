using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    public class HotFixRuntimeUpdatePanel : MonoBehaviour, IHotFixRuntimeFileCheck, IHotFixRuntimeFileDown, IHotFixNetworking
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

        public void HotFixRuntimeTableDownStart()
        {
            initPanel.SetActive(true);
        }

        public void HotFixRuntimeTableDownOver()
        {
            initPanel.SetActive(false);
            localFileCheckPanel.SetActive(true);
        }

        public void HotFixRuntimeLocalFileCheck(int currentCount, int maxCount)
        {
            localFileCheckSlider.value = (float)currentCount / maxCount;
            localFileCheckText.text = (int)(localFileCheckSlider.value * 100) + "/100";
        }

        public void HotFixRuntimeLocalFileCheckOver()
        {
            localFileCheckPanel.SetActive(false);
        }

        public void HotFixRuntimeDownStart()
        {
            downPanel.SetActive(true);
        }

        public void HotFixRuntimeDownSpeed(float downSpeed)
        {
            downTextSpeed.text = HotFixGlobal.FileSizeString(downSpeed) + "/s";
        }

        public void HotFixRuntimeDownloadValue(double current, double total)
        {
            totalDownload.text = HotFixGlobal.FileSizeString(current) + "/" + HotFixGlobal.FileSizeString(total);
            downSliderProgress.value = (float)(current / total);
            downTextProgress.text = (current / total * 100).ToString("0") + "/100";
        }

        public void HotFixRuntimeDownOver()
        {
            downPanel.SetActive(false);
        }

        public void NetworkingState(bool state)
        {
            Debug.Log("网络状态：" + state);
            networkPanel.SetActive(!state);
        }
    }
}