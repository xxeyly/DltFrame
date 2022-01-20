using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using XFramework;

/// <summary>
/// 相机控制
/// </summary>
[RequireComponent(typeof(ControllerSelfRotate))]
public class CameraControl : SceneComponent
{
    public static CameraControl Instance;
    public NavMeshAgent navMeshAgent;
    [HideInInspector] public Camera currentCamera;
    [LabelText("相机是否可以操作")] public bool isControl;
    [LabelText("UI遮挡")] public bool isViewOcclusion;
    [LabelText("移动")] public bool isMove;
    [LabelText("旋转")] public bool isRotate;
    [LabelText("升降")] public bool isUpAndDown;
    [LabelText("视野")] public bool isField;
    [LabelText("移动速度")] [SerializeField] private float moveSpeed = 1;
    [LabelText("旋转速度")] [SerializeField] private float rotationSpeed = 2;
#pragma warning disable 0649
    [LabelText("相机领域")] [SerializeField] private Vector2 cameraField;
    [LabelText("左右角度")] [SerializeField] private Vector2 leftAndRightLimit;
    [LabelText("上下角度")] [SerializeField] private Vector2 topAndDownLimit;

    [LabelText("相机高度")] [SerializeField] private Vector2 cameraHeightRange = Vector2.zero;
#pragma warning restore 0649
    private ControllerSelfRotate _controllerSelfRotate;

    public override void StartSvc()
    {
        Instance = GetComponent<CameraControl>();
    }

    public override void Init()
    {
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        currentCamera = GetComponentInChildren<Camera>();
        _controllerSelfRotate = GetComponent<ControllerSelfRotate>();
        _controllerSelfRotate.rotateSpeed = rotationSpeed;
        _controllerSelfRotate.leftAndRightLimit = leftAndRightLimit;
        _controllerSelfRotate.topAndDownLimit = topAndDownLimit;
        _controllerSelfRotate.SetRotateObj(currentCamera.transform);

        cameraField.x = 40;
        cameraField.y = 60;
        transform.position = Vector3.zero;
    }


    void LateUpdate()
    {
        if (isControl)
        {
            if (!ViewOcclusion())
            {
                TryScrollWheel();
                TryRotate();
            }

            TryMove();
            TryUpAndDown();
        }
    }

    /// <summary>
    /// 视图遮挡
    /// </summary>
    /// <returns></returns>
    private bool ViewOcclusion()
    {
        if (isViewOcclusion)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        else
        {
            return false;
        }
    }


    private void TryRotate()
    {
        if (!isRotate)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            _controllerSelfRotate.isOpenMouseOperation = true;
        }
        else
        {
            _controllerSelfRotate.isOpenMouseOperation = false;
        }
    }

    private void TryMove()
    {
        if (!isMove)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Math.Abs(v) > 0 || Math.Abs(h) > 0)
        {
            navMeshAgent.Move(
                currentCamera.transform.TransformDirection(new Vector3(h, 0, v) * (Time.deltaTime * moveSpeed)));
        }
    }

    private void TryUpAndDown()
    {
        if (!isUpAndDown)
        {
            return;
        }

        float height = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            height = -1 * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            height = 1 * moveSpeed;
        }
        else
        {
            height = 0;
        }

        if (height < 0)
        {
            if (currentCamera.transform.localPosition.y > cameraHeightRange.x)
            {
                currentCamera.transform.Translate(0, height * Time.deltaTime * moveSpeed, 0);
            }
        }

        if (height > 0)
        {
            if (currentCamera.transform.localPosition.y < cameraHeightRange.y)
            {
                currentCamera.transform.Translate(0, height * Time.deltaTime * moveSpeed, 0);
            }
        }
    }


    /// <summary>
    /// 改变相机的领域
    /// </summary>
    public void ChangeCameraField(float field)
    {
        transform.GetComponent<Camera>().fieldOfView = field;
    }


    /// <summary>
    /// 尝试更改相机的领域
    /// </summary>
    void TryScrollWheel()
    {
        if (isField)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentCamera.fieldOfView < cameraField.y)
                    currentCamera.fieldOfView += 1;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentCamera.fieldOfView > cameraField.x)
                    currentCamera.fieldOfView -= 1;
            }
        }
    }

    private int _navMeshAgentMoveTimeTask;
    private int _currentCameraMoveTimeTask;
    private int _currentCameraRotateTimeTask;
    private int _currentCameraReSetTimeTask;

    /// <summary>
    /// 相机移动
    /// </summary>
    /// <param name="targetPos">目标位置</param>
    /// <param name="targetRotate">目标旋转</param>
    /// <param name="targetHigh">相机高度</param>
    /// <param name="time">需要的时间</param>
    /// <param name="isControl">是否继续控制</param>
    /// <param name="fieldView">相机视野</param>
    /// <param name="action">执行事件</param>
    public void MoveTargetPos(Vector3 targetPos, Vector3 targetRotate, Vector3 targetHigh, float time,
        bool isControl, float fieldView, Action action)
    {
        StopMove();
        _controllerSelfRotate.enabled = false;
        this.isControl = false;
        navMeshAgent.enabled = false;
        if (time <= 0)
        {
            navMeshAgent.transform.localPosition = targetPos;
            currentCamera.transform.localEulerAngles = targetRotate;
            currentCamera.transform.localPosition = new Vector3(0, targetHigh.y, 0);
            _controllerSelfRotate.enabled = true;
            this.isControl = isControl;
            _controllerSelfRotate.SetRotateObj(currentCamera.transform);
            navMeshAgent.enabled = true;
            action.Invoke();
            currentCamera.fieldOfView = fieldView;
        }
        else
        {
            _navMeshAgentMoveTimeTask = TimeSvc.Instance.MoveTargetPos(navMeshAgent.transform, navMeshAgent.transform.position, targetPos, time, false);
            _currentCameraMoveTimeTask = TimeSvc.Instance.RotateTargetPos(currentCamera.transform, targetRotate, time, false);
            _currentCameraRotateTimeTask =
                TimeSvc.Instance.MoveTargetPos(currentCamera.transform, new Vector3(0, currentCamera.transform.localPosition.y, 0), new Vector3(0, targetHigh.y, 0), time, false);
            _currentCameraReSetTimeTask = TimeSvc.Instance.AddTimeTask(() =>
                {
                    _controllerSelfRotate.enabled = true;
                    this.isControl = isControl;
                    _controllerSelfRotate.SetRotateObj(currentCamera.transform);
                    navMeshAgent.enabled = true;
                    action.Invoke();
                    currentCamera.fieldOfView = fieldView;
                }, "校准位置",
                time + 0.02f);
        }
    }

    /// <summary>
    /// 移动到位置
    /// </summary>
    /// <param name="posName"></param>
    /// <param name="time"></param>
    /// <param name="isControl"></param>
    /// <param name="action"></param>
    public void MoveTargetPos(string posName, float time, bool isControl, Action action)
    {
        CameraPos.CameraPosInfo cameraPosInfo = CameraPosEditor.Instance.cameraPos.GetCameraPosInfoByName(posName);
        if (cameraPosInfo != null)
        {
            MoveTargetPos(cameraPosInfo.navPos, cameraPosInfo.cameraRotate, cameraPosInfo.cameraPos, time,
                isControl, cameraPosInfo.cameraFieldView, action);
        }
        else
        {
            Debug.LogWarning("未找到位置信息");
        }
    }

    /// <summary>
    /// 移动到位置
    /// </summary>
    /// <param name="posName"></param>
    /// <param name="time"></param>
    /// <param name="isControl"></param>
    /// <param name="action"></param>
    public void MoveTargetPos(string posName, Action action = null)
    {
        CameraPos.CameraPosInfo cameraPosInfo = CameraPosEditor.Instance.cameraPos.GetCameraPosInfoByName(posName);
        if (cameraPosInfo != null)
        {
            navMeshAgent.transform.localPosition = cameraPosInfo.navPos;
            currentCamera.transform.localPosition = cameraPosInfo.cameraPos;
            currentCamera.fieldOfView = cameraPosInfo.cameraFieldView;
            currentCamera.transform.localEulerAngles = cameraPosInfo.cameraRotate;
            _controllerSelfRotate.SetRotateObj(currentCamera.transform);
            action?.Invoke();
#if UNITY_EDITOR
            CameraPosEditor.Instance.cameraPosName = posName;
#endif
        }
        else
        {
            Debug.LogWarning("未找到位置信息");
        }
    }

    public void StopMove()
    {
        TimeSvc.Instance.DeleteTimeTask(_navMeshAgentMoveTimeTask);
        TimeSvc.Instance.DeleteTimeTask(_currentCameraMoveTimeTask);
        TimeSvc.Instance.DeleteTimeTask(_currentCameraRotateTimeTask);
        TimeSvc.Instance.DeleteTimeTask(_currentCameraReSetTimeTask);
    }

    /// <summary>
    /// 控制相机属性
    /// </summary>
    public void ControllerFunction(bool isMove, bool isRotate, bool isUpAndDown, bool isField)
    {
        this.isMove = isMove;
        this.isRotate = isRotate;
        this.isUpAndDown = isUpAndDown;
        this.isField = isField;
    }

    public void ControllerMove(bool isMove)
    {
        this.isMove = isMove;
    }

    public void ControllerRotate(bool isRotate)
    {
        this.isRotate = isRotate;
    }

    public void ControllerUpAndDown(bool isUpAndDown)
    {
        this.isUpAndDown = isUpAndDown;
    }

    public void ControllerField(bool isField)
    {
        this.isField = isField;
    }

    public void ChangeToRotateCamera(GameObject rotateCamera)
    {
        gameObject.SetActive(false);
        rotateCamera.SetActive(true);
        rotateCamera.transform.GetComponentInChildren<CameraControl>().Init();
    }
}