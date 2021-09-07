using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using XxSlitFrame.Tools.Svc;

public class ControllerCenterRotate : MonoBehaviour
{
    [BoxGroup("基础")] [LabelText("开启")] public bool playing = true;
    [BoxGroup("基础")] [LabelText("目标物体")] public Transform centerTarget;
    [BoxGroup("基础")] [LabelText("旋转物体")] public Transform rotateTarget;

    [BoxGroup("基础")] [LabelText("目标偏移")] public Vector3 targetOffset;
    private float _velocity;
    [LabelText("目标距离")] private float targetDistance;
    private float x = 0.0f;
    private float y = 0f;

    #region 缩放

    [BoxGroup("缩放")] [LabelText("是否可缩放")] public bool canManualZoom = true;
    [BoxGroup("缩放")] [LabelText("距离")] public float distance = 100.0f;
    [BoxGroup("缩放")] [LabelText("缩放速度")] public float movSpeedScroll = 15f;
    [BoxGroup("缩放")] [LabelText("最近距离")] public float nearLimit = 0.8f;
    [BoxGroup("缩放")] [LabelText("最远距离")] public float farLimit = 4f;
    float smoothTime = 0.3f;

    #endregion

    #region 旋转

    [BoxGroup("旋转")] [LabelText("是否可旋转")] public bool canManualRotate = true;
    [BoxGroup("旋转")] [LabelText("旋转按键")] public KeyCode rotateCode;
    [BoxGroup("旋转")] [LabelText("仅水平旋转")] public bool onlyX = false;
    [BoxGroup("旋转")] [LabelText("水平旋转速度")] public float xSpeed = 250.0f;
    [BoxGroup("旋转")] [LabelText("垂直旋转速度")] public float ySpeed = 120.0f;

    [BoxGroup("旋转")] [LabelText("垂直旋转最小角度")]
    public float yMinLimit = -20f;

    [BoxGroup("旋转")] [LabelText("垂直旋转最大角度")]
    public float yMaxLimit = 80f;

    [BoxGroup("旋转")] [LabelText("水平旋转最小角度")]
    public float xMinLimit = -360;

    [BoxGroup("旋转")] [LabelText("水平旋转最大角度")]
    public float xMaxLimit = 360;

    #endregion

    #region 移动

    [BoxGroup("移动")] [LabelText("是否可移动")] public bool canManualMove = true;
    [BoxGroup("移动")] [LabelText("移动反转")] public bool moveReversal = true;
    [BoxGroup("移动")] [LabelText("移动按键")] public KeyCode moveCode;
    [BoxGroup("移动")] [LabelText("移动速度")] public float movSpeed = 0.005f;

    [BoxGroup("移动")] [LabelText("本地X限定")] public Vector2 xTargetLimit;

    [BoxGroup("移动")] [LabelText("本地Y限定")] public Vector2 yTargetLimit;

    [BoxGroup("移动")] [LabelText("本地Z限定")] public Vector2 zTargetLimit;

    [BoxGroup("移动")] [LabelText("目标默认世界位置")]
    public Vector3 targetWorldDefaultPos;

    [BoxGroup("移动")] [LabelText("目标默认局部位置")]
    public Vector3 targetSelfDefaultPos;

    [BoxGroup("移动")] [LabelText("移动增量")] public Vector3 movingIncrement;
    [BoxGroup("移动")] [LabelText("X轴前限制")] public bool selfLeftDirectionLimit;
    [BoxGroup("移动")] [LabelText("X轴后限制")] public bool selfRightDirectionLimit;
    [BoxGroup("移动")] [LabelText("Y轴前限制")] public bool selfUpDirectionLimit;
    [BoxGroup("移动")] [LabelText("Y轴后限制")] public bool selfDownDirectionLimit;
    [BoxGroup("移动")] [LabelText("Z轴前限制")] public bool selfForwardDirectionLimit;
    [BoxGroup("移动")] [LabelText("Z轴后限制")] public bool selfBackDirectionLimit;
    private Vector3 _limitValue;
    float _yDis, _xDis;
    private bool _firstMove;
    private Vector3 _oldMousePos;
    private Vector3 _currentMousePos;
    private Vector3 _oldTargetPos;

    #endregion


    private void Awake()
    {
        Init();
    }

    private Vector3 _startPos;

    public void Init()
    {
        if (centerTarget == null)
        {
            centerTarget = Instantiate(new GameObject()).transform;
        }

        targetWorldDefaultPos = centerTarget.position;
        targetSelfDefaultPos = centerTarget.localPosition;

        distance = Vector3.Distance(centerTarget.position, rotateTarget.transform.position);
        targetDistance = Vector3.Distance(centerTarget.position, rotateTarget.transform.position);
        _startPos = DataSvc.GetInspectorEuler(rotateTarget.transform);
        x = _startPos.y;
        y = _startPos.x;
    }


    void Update()
    {
        if (playing)
        {
            MouseFunction();
            x = ClampAngle(x, xMinLimit, xMaxLimit);
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            if (canManualRotate)
            {
                Quaternion rotation = Quaternion.Euler(y, x, 0);

                rotateTarget.transform.rotation = Quaternion.Lerp(rotateTarget.transform.rotation, rotation, Time.deltaTime * 4);
                rotateTarget.transform.position =
                    rotateTarget.transform.rotation * new Vector3(0.0f, 0.0f, -distance) + centerTarget.position + targetOffset;
            }
        }
    }

    #region 鼠标操作

    /// <summary>
    /// 鼠标控制相机
    /// </summary>
    private void MouseFunction()
    {
        //旋转
        if (Input.GetKey(rotateCode) && canManualRotate)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

            if (onlyX == false)
            {
                y -= Input.GetAxis("Mouse Y") * xSpeed * 0.02f;
            }
        }

        if (Input.GetKey(moveCode) && canManualMove)
        {
            _limitValue = centerTarget.localPosition;
            _currentMousePos = Input.mousePosition;
            if (!_firstMove)
            {
                _oldMousePos = _currentMousePos;
                _firstMove = true;
                return;
            }

            if (_oldMousePos != _currentMousePos)
            {
                _yDis = (_currentMousePos.y - _oldMousePos.y) * movSpeed;
                _xDis = (_currentMousePos.x - _oldMousePos.x) * movSpeed;
                _oldMousePos = _currentMousePos;
                Vector3 cameraValue = DataSvc.GetInspectorEuler(rotateTarget.transform);
                Quaternion rot = Quaternion.Euler(cameraValue.x, cameraValue.y, 0);
                if (moveReversal)
                {
                    _xDis = -_xDis;
                    _yDis = -_yDis;
                }

                //目标世界坐标
                Vector3 targetWorldPos = rot * new Vector3(_xDis, _yDis, 0) + centerTarget.position;
                //目标局部坐标
                Vector3 targetSelfPos;
                //不包含父物体
                if (centerTarget.parent == null)
                {
                    //世界坐标等于局部坐标
                    targetSelfPos = targetWorldPos;
                }
                else
                {
                    //将世界坐标转换局部坐标
                    targetSelfPos = centerTarget.parent.InverseTransformPoint(targetWorldPos);
                }

                //局部坐标增加限制
                targetSelfPos = LimitTargetPos(targetSelfPos);
                if (targetSelfPos.x < _limitValue.x && selfRightDirectionLimit)
                {
                    targetSelfPos = new Vector3(_limitValue.x, targetSelfPos.y, targetSelfPos.z);
                }

                if (targetSelfPos.x > _limitValue.x && selfLeftDirectionLimit)
                {
                    targetSelfPos = new Vector3(_limitValue.x, targetSelfPos.y, targetSelfPos.z);
                }

                if (targetSelfPos.y < _limitValue.y && selfDownDirectionLimit)
                {
                    targetSelfPos = new Vector3(targetSelfPos.x, _limitValue.y, targetSelfPos.z);
                }

                if (targetSelfPos.y > _limitValue.y && selfUpDirectionLimit)
                {
                    targetSelfPos = new Vector3(targetSelfPos.x, _limitValue.y, targetSelfPos.z);
                }

                if (targetSelfPos.z < _limitValue.z && selfBackDirectionLimit)
                {
                    targetSelfPos = new Vector3(targetSelfPos.x, targetSelfPos.y, _limitValue.z);
                }

                if (targetSelfPos.z > _limitValue.z && selfForwardDirectionLimit)
                {
                    targetSelfPos = new Vector3(targetSelfPos.x, targetSelfPos.y, _limitValue.z);
                }

                centerTarget.localPosition = targetSelfPos;
                movingIncrement = targetSelfPos - _oldTargetPos;
                _oldTargetPos = targetSelfPos;
            }
            else
            {
                movingIncrement = Vector3.zero;
            }
        }
        else
        {
            movingIncrement = Vector3.zero;
            _firstMove = false;
            _yDis = 0;
            _xDis = 0;
            _currentMousePos = Vector3.zero;
            _oldMousePos = Vector3.zero;
        }

        //镜头远近
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetDistance -= Input.GetAxis("Mouse ScrollWheel") * movSpeedScroll;
            targetDistance = Mathf.Clamp(targetDistance, nearLimit, farLimit);
        }

        if (canManualZoom)
        {
            distance = Mathf.SmoothDamp(distance, targetDistance, ref _velocity, smoothTime);
            distance = Mathf.Clamp(distance, nearLimit, farLimit);
        }
    }

    #endregion

    /// <summary>
    /// 限定角度
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// 限制移动
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector3 LimitTargetPos(Vector3 pos)
    {
        float posx = Mathf.Clamp(pos.x, xTargetLimit.x, xTargetLimit.y);
        float posy = Mathf.Clamp(pos.y, yTargetLimit.x, yTargetLimit.y);
        float posz = Mathf.Clamp(pos.z, zTargetLimit.x, zTargetLimit.y);

        return new Vector3(posx, posy, posz);
    }

    /// <summary>
    /// 开启移动
    /// </summary>
    public void Play()
    {
        playing = true;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void Stop()
    {
        playing = false;
    }
}