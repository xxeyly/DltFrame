using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerRotate : MonoBehaviour
{
    public enum Axial
    {
        X,
        Y,
        Z
    }

    // Start is called before the first frame update
    [LabelText("是否开启旋转")] public bool isOpenMouseOperation;
    [LabelText("开启水平轴向")] public bool isHorizontal;
    [LabelText("开启垂直轴向")] public bool isVertical;
    [LabelText("目标物体")] [SerializeField] private Transform targetTri;
    private Quaternion _targetRotation;
    [LabelText("旋转速度")] public float rotateSpeed = 3;
    [LabelText("平滑过渡")] public bool smoothTransition;
    [LabelText("水平值")] public float mouseX;
    [LabelText("垂直值")] public float mouseY;
    [LabelText("水平反转")] public bool hReversal;
    [LabelText("垂直反转")] public bool vReversal;

    [LabelText("左右角度限定")] public Vector2 leftAndRightLimit;

    [LabelText("上下角度限定")] public Vector2 topAndDownLimit;

    [LabelText("水平旋转轴向")] public Axial mouseXAxial;
    [LabelText("垂直旋转轴向")] public Axial mouseYAxial;

    private float _inputX;
    private float _inputY;
    float _velocity = 0.0f;

    public void SetRotateObj(Transform target)
    {
        Vector3 inspectorEuler = GetInspectorEuler(target);
        targetTri = target;
        switch (mouseXAxial)
        {
            case Axial.X:
                if (leftAndRightLimit != Vector2.zero)
                {
                    mouseX = ClampAngle(inspectorEuler.x, leftAndRightLimit.x, leftAndRightLimit.y);
                }
                else
                {
                    mouseX = inspectorEuler.x;
                }

                break;
            case Axial.Y:
                if (leftAndRightLimit != Vector2.zero)
                {
                    mouseX = ClampAngle(inspectorEuler.y, leftAndRightLimit.x, leftAndRightLimit.y);
                }
                else
                {
                    mouseX = inspectorEuler.y;
                }

                break;
            case Axial.Z:
                if (leftAndRightLimit != Vector2.zero)
                {
                    mouseX = ClampAngle(inspectorEuler.z, leftAndRightLimit.x, leftAndRightLimit.y);
                }
                else
                {
                    mouseX = inspectorEuler.z;
                }

                break;
        }

        switch (mouseYAxial)
        {
            case Axial.X:
                if (topAndDownLimit != Vector2.zero)
                {
                    mouseY = ClampAngle(inspectorEuler.x, topAndDownLimit.x, topAndDownLimit.y);
                }
                else
                {
                    mouseY = inspectorEuler.x;
                }

                break;
            case Axial.Y:
                if (topAndDownLimit != Vector2.zero)
                {
                    mouseY = ClampAngle(inspectorEuler.y, topAndDownLimit.x, topAndDownLimit.y);
                }
                else
                {
                    mouseY = inspectorEuler.y;
                }

                break;
            case Axial.Z:
                if (topAndDownLimit != Vector2.zero)
                {
                    mouseY = ClampAngle(inspectorEuler.z, topAndDownLimit.x, topAndDownLimit.y);
                }
                else
                {
                    mouseY = inspectorEuler.z;
                }

                break;
        }
    }

    private void LateUpdate()
    {
        if (isOpenMouseOperation)
        {
            if (isHorizontal)
            {
                _inputX = Input.GetAxis("Mouse X");
            }
            else
            {
                _inputX = 0;
            }

            if (isVertical)
            {
                _inputY = Input.GetAxis("Mouse Y");
            }
            else
            {
                _inputY = 0;
            }
        }
        else
        {
            _inputX = Mathf.SmoothDamp(_inputX, 0, ref _velocity, Time.deltaTime);
            _inputY = Mathf.SmoothDamp(_inputY, 0, ref _velocity, Time.deltaTime);
        }

        if (hReversal)
        {
            mouseX += -_inputX * rotateSpeed;
        }
        else
        {
            mouseX -= -_inputX * rotateSpeed;
        }

        if (vReversal)
        {
            mouseY += _inputY * rotateSpeed;
        }
        else
        {
            mouseY -= _inputY * rotateSpeed;
        }

        if (leftAndRightLimit != Vector2.zero)
        {
            mouseX = ClampAngle(mouseX, leftAndRightLimit.x, leftAndRightLimit.y);
        }

        if (topAndDownLimit != Vector2.zero)
        {
            mouseY = ClampAngle(mouseY, topAndDownLimit.x, topAndDownLimit.y);
        }

        Vector3 nextPos = new Vector3();
        switch (mouseXAxial)
        {
            case Axial.X:
                nextPos.x = mouseX;
                break;
            case Axial.Y:
                nextPos.y = mouseX;

                break;
            case Axial.Z:
                nextPos.z = mouseX;

                break;
        }

        switch (mouseYAxial)
        {
            case Axial.X:
                nextPos.x = mouseY;
                break;
            case Axial.Y:
                nextPos.y = mouseY;

                break;
            case Axial.Z:
                nextPos.z = mouseY;

                break;
        }

        if (smoothTransition)
        {
            _targetRotation = Quaternion.Euler(nextPos);
            targetTri.rotation = Quaternion.Lerp(targetTri.rotation, _targetRotation,
                Time.deltaTime * rotateSpeed * rotateSpeed);
        }
        else
        {
            targetTri.localEulerAngles = nextPos;
        }
    }

    //function used to limit angles
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }

            if (angle > 360F)
            {
                angle -= 360F;
            }
        }

        return Mathf.Clamp(angle, min, max);
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
    /// 设置限定角度
    /// </summary>
    /// <param name="leftAndRight"></param>
    /// <param name="tpoAndDown"></param>
    public void SetRotateAngleLimit(Vector2 leftAndRight, Vector2 tpoAndDown)
    {
        leftAndRightLimit = leftAndRight;
        topAndDownLimit = tpoAndDown;
    }
}