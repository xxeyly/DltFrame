using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;

public class RayRenderTools : StartSingleton
{
    public static RayRenderTools Instance;
    [LabelText("触发键")] public KeyCode mouseKeyCode;
    [LabelText("射线相机")] public Camera currentRayCamera;
    [LabelText("双击触发间隔时间")] public float doubleClickTime;
    private int _doubleClickTimeTask;

    /// <summary>
    /// 点击次数
    /// </summary>
    private int _doubleClickCount;

    private int _rayTimeTask;

    public override void StartSvc()
    {
        Instance = GetComponent<RayRenderTools>();
        OpenRayTest(PrintRayObject, LayerMask.GetMask("Tooth"), true);
        TimeSvc.Instance.AddTimeTask(() =>
        {
            Debug.Log(1);
            
        }, "", 0.01f,0);
    }

    public override void Init()
    {
    }

    public void PrintRayObject(GameObject obj)
    {
        Debug.Log(obj);
    }

    /// <summary>
    /// 开始射线测试
    /// </summary>
    public void OpenRayTest(Action<GameObject> testAction, LayerMask currentOperationMask, bool doubleClick = false)
    {
        _rayTimeTask = TimeSvc.Instance.AddTimeTask(() =>
        {
            if (Input.GetKeyDown(mouseKeyCode))
            {
                SendRay(testAction, currentOperationMask, doubleClick);
            }
        }, "射线检测人物", Time.fixedTime, 0);
    }

    private void SendRay(Action<GameObject> testAction, LayerMask currentOperationMask, bool doubleClick)
    {
        Ray ray = currentRayCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, currentOperationMask))
        {
            if (doubleClick)
            {
                _doubleClickCount += 1;
                Debug.Log(_doubleClickCount);
                //第一次点击,开启计时任务,时间内再次点击可触发事件
                if (_doubleClickCount == 1)
                {
                    _doubleClickTimeTask =
                        TimeSvc.Instance.AddTimeTask(() => { _doubleClickCount -= 1; }, "双击倒计时", doubleClickTime);
                }
                else if (_doubleClickCount == 2)
                {
                    TimeSvc.Instance.DeleteTimeTask(_doubleClickTimeTask);
                    testAction.Invoke(hit.collider.gameObject);
                    _doubleClickCount = 0;
                }
            }
            else
            {
                //单击直接执行事件
                testAction.Invoke(hit.collider.gameObject);
            }
        }
    }

    [Button]
    public void CloseRayTest()
    {
        TimeSvc.Instance.DeleteTimeTask(_rayTimeTask);
    }
}