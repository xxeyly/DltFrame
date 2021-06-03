using UnityEngine;
using XxSlitFrame.View;
using XxSlitFrame.Tools.Svc;

//引入开始
using UnityEngine.UI;
using UnityEngine.EventSystems;

//引入结束
public class DownProgress : BaseWindow
{
    //变量声明开始
    private Slider _barSlider;
    private Text _title;
    private Text _loadingText;

    private Button _anyKeyContinue;

    //变量声明结束
    //下载进度计时任务
    private int _downTimeTask;

    //当前下载的文件
    private DownSvc.DownData _downData;

    public override void Init()
    {
    }

    /// <summary>
    /// 显示下载进度
    /// </summary>
    /// <param name="fileName"></param>
    private void OnShowDownLoadProgress(string fileName)
    {
        _downTimeTask = AddTimeTask(() => { UpdateDownProgress(fileName); }, "获得下载进度", 0.1f, 0);
    }

    /// <summary>
    /// 更新下载进度
    /// </summary>
    /// <param name="fileName"></param>
    private void UpdateDownProgress(string fileName)
    {
        _downData = DownSvc.Instance.GetDownSvcDataByFileName(fileName);
        if (_downData == null)
        {
            return;
        }

        _title.text = _downData.downName;
        _barSlider.value = (float) _downData.downCurrentSize / _downData.downTotalSize;
        _loadingText.text = _downData.downCurrentSize / 1024 / 1024 + "M" + "/" +
                            _downData.downTotalSize / 1024 / 1024 + "M";
        if (_downData.downOver)
        {
            _barSlider.value = 1;
            _loadingText.text = _downData.downTotalSize / 1024 / 1024 + "M" + "/" +
                                _downData.downTotalSize / 1024 / 1024 + "M";
            Debug.Log("下载完毕");
            DeleteTimeTask(_downTimeTask);
            HideObj(_barSlider);
            ShowObj(_anyKeyContinue);
        }
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _barSlider, "BarSlider");
        BindUi(ref _title, "BarSlider/Title");
        BindUi(ref _loadingText, "BarSlider/LoadingText");
        BindUi(ref _anyKeyContinue, "AnyKeyContinue");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_anyKeyContinue, EventTriggerType.PointerClick, OnAnyKeyContinueClick);
        //变量绑定结束
        ListenerSvc.AddListenerEvent<string>("OnShowDownLoadProgress", OnShowDownLoadProgress);
    }

    //变量方法开始
    private void OnAnyKeyContinueClick(BaseEventData targetObj)
    {
        if (_downData != null)
        {
            SceneSvc.SceneLoad(_downData.downName);
            HideView();
        }
    }

    //变量方法结束
}