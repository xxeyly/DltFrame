using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerAroundRotate : MonoBehaviour
{
    [LabelText("是否使用")] public bool isControl = true;
    [LabelText("是否视图限制")] public bool isViewLimit = true;
    [LabelText("目标物体")] public Transform targetTri;
    [LabelText("旋转物体")] public Transform rotateTri;
    [LabelText("距离范围")] public Vector2 distanceRange = new Vector2(2f, 10f);
    [Range(0f, 1f)] [LabelText("距离滑动值")] public float distanceSlidingValue = 0.5f;
    [LabelText("左右旋转速度")] public float leftAndRightRotateSpeed = 5f;
    [LabelText("上下旋转速度")] public float topAndDownRotateSpeed = 3f;
    [LabelText("垂直角度限定")] public Vector2 topAndDownRotateLimit = new Vector2(-20f, 80f);
    [LabelText("移动延迟")] public float moveDelay = 4f;
    [LabelText("鼠标滚轮速度")] public float mouseWheelSpeed = 0.5f;
    [LabelText("偏移")] public Vector3 offset;

    #region MonoBehaviour

    [Button]
    public void Init()
    {
        PosInit();
        nowDistanceSlidingValue = distanceSlidingValue;
        ReSetPos();
        isControl = true;
    }

    public void Init(Transform pos)
    {
        if (pos != null)
        {
            rotateTri.localPosition = pos.position;
            rotateTri.localEulerAngles = pos.eulerAngles;
        }

        Init();
    }

    private float _mouseX;
    private float _mouseY;
    Vector2 _oldPosition1;
    Vector2 _oldPosition2;

    void Update()
    {
        if (isControl)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER||UNITY_WEBGL
            if (Input.GetMouseButton(1))
            {
                _mouseX += Input.GetAxis("Mouse X") * leftAndRightRotateSpeed;
                _mouseY -= Input.GetAxis("Mouse Y") * topAndDownRotateSpeed;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                /*distanceSlidingValue -= Input.GetAxis("Mouse ScrollWheel") * mouseWheelSpeed;
                distanceSlidingValue = Mathf.Clamp(distanceSlidingValue, 0, 1);*/
            }

            /*if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    _mouseX += Input.GetTouch(0).deltaPosition.x / 9 * leftAndRightRotateSpeed;
                    _mouseY -= Input.GetTouch(0).deltaPosition.y / 9 * topAndDownRotateSpeed;
                }
            }
            else if (Input.touchCount > 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    var tempPosition1 = Input.GetTouch(0).position;
                    var tempPosition2 = Input.GetTouch(1).position;

                    if (isZoom(_oldPosition1, _oldPosition2, tempPosition1, tempPosition2))
                    {
                        if (distanceSlidingValue > 0)
                            distanceSlidingValue -= 0.015f;
                    }
                    else
                    {
                        if (distanceSlidingValue < 1f)
                            distanceSlidingValue += 0.015f;
                    }

                    _oldPosition1 = tempPosition1;
                    _oldPosition2 = tempPosition2;
                }
            }*/
#else
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                mouseX += Input.GetAxis("Mouse X") * 左右旋转速度;
                mouseY -= Input.GetAxis("Mouse Y") * 上下旋转速度;
            }
        }
        else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;

                if (isZoom(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    if (距离滑动条值 > 0)
                        距离滑动条值 -= 0.05f;
                }
                else
                {
                    if (距离滑动条值 < 1f)
                        距离滑动条值 += 0.05f;
                }

                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
#endif
        }
    }

    void LateUpdate()
    {
        if (!isControl)
        {
            return;
        }
        if (isViewLimit && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        _mouseY = ClampAngle(_mouseY, topAndDownRotateLimit.x, topAndDownRotateLimit.y);
        Quaternion toRotation = Quaternion.Euler(_mouseY, _mouseX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * (1 + moveDelay));

        if (targetTri != null)
        {
            Vector3 toPosition =rotateTri. transform.rotation * new Vector3(0.0f, 0.0f, -GetCurrentDistance()) + targetTri.position + offset;
            rotateTri.transform.position = toPosition;
        }
    }

    #endregion

    [HideInInspector] [LabelText("开始偏移")] public Vector3 startAngles;

    [HideInInspector] [LabelText("开始距离滑动值")]
    public float startDistanceSlidingValue;

    [HideInInspector] [LabelText("当前距离滑动值")]
    public float nowDistanceSlidingValue;

    public void PosInit()
    {
        startAngles = GetInspectorEuler(rotateTri);
        startDistanceSlidingValue = distanceSlidingValue;
    }

    /// <summary>
    /// 获取面板上的值
    /// </summary>
    /// <param name="mTransform"></param>
    /// <returns></returns>
    private Vector3 GetInspectorEuler(Transform mTransform)
    {
        Vector3 angle = mTransform.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        if (Vector3.Dot(mTransform.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }

            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }

        if (Vector3.Dot(mTransform.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }

            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }

        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }

        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }

        Vector3 vector3 = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
        return vector3;
    }

    /// <summary>
    /// 重置位置
    /// </summary>
    public void ReSetPos()
    {
        _mouseX = startAngles.y;
        _mouseY = startAngles.x;
        distanceSlidingValue = startDistanceSlidingValue;
    }

    /// <summary>
    /// 获得当前距离
    /// </summary>
    /// <returns></returns>
    float GetCurrentDistance()
    {
        nowDistanceSlidingValue = Mathf.Lerp(nowDistanceSlidingValue, distanceSlidingValue, Time.deltaTime * (1 + moveDelay));
        return distanceRange.x + (distanceRange.y - distanceRange.x) * nowDistanceSlidingValue;
    }

    #region Tools

    /// <summary>
    /// 放大/缩小
    /// </summary>
    /// <param name="oP1"></param>
    /// <param name="oP2"></param>
    /// <param name="nP1"></param>
    /// <param name="nP2"></param>
    /// <returns></returns>
    bool isZoom(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 限制旋转最大/最小值
    /// </summary>
    /// <param name="angle">当前</param>
    /// <param name="min">最小</param>
    /// <param name="max">最大</param>
    /// <returns></returns>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    #endregion
}