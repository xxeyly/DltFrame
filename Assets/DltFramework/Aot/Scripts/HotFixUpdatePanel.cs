using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Aot
{
    public class HotFixUpdatePanel : MonoBehaviour, IHotFixViewAndHotFixCode, IAotNetworking, IAotFilePathError
    {
        public Canvas canvas;
        [LabelText("背景")] public GameObject back;
        [LabelText("初始化面板")] public GameObject initPanel;
        [LabelText("下载面板")] public GameObject downPanel;
        [LabelText("下载进度条")] public Slider downSliderProgress;
        [LabelText("下载进度")] public Text downTextProgress;
        [LabelText("下载速度")] public Text downTextSpeed;
        [LabelText("总下载大小")] public Text totalDownload;
        [LabelText("网络状况")] public GameObject networkPanel;
        [LabelText("文件路径错误")] public GameObject FilePathErrorPanel;
        [LabelText("文件路径错误提示")] public Text FilePathErrorText;

        private void Start()
        {
            
        }

        public void HotFixViewAndHotFixCodeLocalIsUpdate(bool localIsUpdate)
        {
            if (localIsUpdate)
            {
                back.SetActive(true);
                initPanel.SetActive(true);
            }
        }

        public void HotFixViewAndHotFixCodeIsDown(bool down)
        {
            initPanel.SetActive(false);
            downPanel.SetActive(down);
        }

        public void HotFixViewAndHotFixCodeDownSpeed(float downSpeed)
        {
            downTextSpeed.text = AotGlobal.FileSizeString(downSpeed) + "/s";
        }

        public void HotFixViewAndHotFixCodeDownloadValue(double currentDownValue, double totalDownValue)
        {
            totalDownload.text = AotGlobal.FileSizeString(currentDownValue) + "/" + AotGlobal.FileSizeString(totalDownValue);
            downSliderProgress.value = (float)(currentDownValue / totalDownValue);
            downTextProgress.text = (currentDownValue / totalDownValue * 100).ToString("0") + "%";
        }

        public void NetworkingState(bool state)
        {
            networkPanel.SetActive(!state);
        }

        public void FilePathError(string error)
        {
            FilePathErrorPanel.SetActive(true);
            FilePathErrorText.text = "访问地址错误:" + error;
        }

        public void FilePathCorrect()
        {
            FilePathErrorPanel.SetActive(false);
        }
    }
}