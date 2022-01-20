using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerCameraAxisRotate : MonoBehaviour
{
    [LabelText("开启操作")]
    public bool isOperation;

    /// <summary>
    /// 缓存鼠标坐标
    /// </summary>
    private Vector3 _localMousePoint;
    [LabelText("目标位置")]
    public Transform rotateTarget;

    [LabelText("当前相机")] public Camera sceneCamera;
    [BoxGroup("旋转")] [LabelText("旋转按键")] public KeyCode rotateCode = KeyCode.Mouse1;

    void Update()
    {
        if (Input.GetKey(rotateCode))
        {
            if (isOperation)
            {
                if (_localMousePoint == Vector3.zero)
                {
                    _localMousePoint = Input.mousePosition;
                }

                OnMouseLeftHold();
            }
        }

        if (Input.GetKeyUp(rotateCode))
        {
            _localMousePoint = Vector3.zero;
        }
    }


    /// <summary>
    /// 鼠标左键按住
    /// </summary>
    private void OnMouseLeftHold()
    {
        //鼠标左键，代表XY轴旋转
        if (_localMousePoint != Input.mousePosition)
        {
            XYRotate(Input.mousePosition - _localMousePoint);

            _localMousePoint = Input.mousePosition;
        }
    }

    /// <summary>
    /// XY旋转
    /// </summary>
    /// <param name="offset">偏移量</param>
    public void XYRotate(Vector3 offset)
    {
        /*应用相机轴*/
        rotateTarget.Rotate(sceneCamera.transform.up, -offset.x * 0.1f, Space.World);
        rotateTarget.Rotate(sceneCamera.transform.right, offset.y * 0.1f, Space.World);
    }
}