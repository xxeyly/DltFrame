using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;

namespace CameraTools
{
    /// <summary>
    /// 相机控制
    /// </summary>
    [RequireComponent(typeof(ControllerRotate))]
    public class CameraControl : StartSingleton
    {
        public static CameraControl Instance;
        public NavMeshAgent navMeshAgent;
        [HideInInspector] public Camera currentCamera;
        [LabelText("相机是否可以操作")] public bool isControl;
        [LabelText("移动速度")] [SerializeField] private float moveSpeed = 1;
        [LabelText("旋转速度")] [SerializeField] private float rotationSpeed = 2;
#pragma warning disable 0649
        [LabelText("相机领域")] [SerializeField] private Vector2 cameraField;
        [LabelText("左右角度")] [SerializeField] private Vector2 leftAndRightLimit;
        [LabelText("上下角度")] [SerializeField] private Vector2 topAndDownLimit;

        [LabelText("相机高度")] [SerializeField] private Vector2 cameraHeightRange = Vector2.zero;
#pragma warning restore 0649
        private ControllerRotate _controllerRotate;

        public override void StartSvc()
        {
            Instance = GetComponent<CameraControl>();
        }

        public override void Init()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            currentCamera = GetComponentInChildren<Camera>();
            _controllerRotate = GetComponent<ControllerRotate>();
            _controllerRotate.rotateSpeed = rotationSpeed;
            _controllerRotate.leftAndRightLimit = leftAndRightLimit;
            _controllerRotate.topAndDownLimit = topAndDownLimit;
            _controllerRotate.SetRotateObj(currentCamera.transform);

            cameraField.x = 40;
            cameraField.y = 60;
        }


        void LateUpdate()
        {
            if (isControl)
            {
                TryScrollWheel();
                TryMove();
                TryRotate();
            }
        }

        private void TryRotate()
        {
            if (Input.GetMouseButton(1))
            {
                _controllerRotate.isOpenMouseOperation = true;
            }
            else
            {
                _controllerRotate.isOpenMouseOperation = false;
            }
        }

        void TryMove()
        {
            float height;
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

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Math.Abs(v) > 0 || Math.Abs(h) > 0)
            {
                // Debug.Log("开始移动");
                navMeshAgent.Move(
                    currentCamera.transform.TransformDirection(new Vector3(h, 0, v) * (Time.deltaTime * moveSpeed)));
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
            _controllerRotate.enabled = false;
            this.isControl = false;
            navMeshAgent.enabled = false;
            _navMeshAgentMoveTimeTask = TimeSvc.Instance.MoveTargetPos(navMeshAgent.transform,
                navMeshAgent.transform.position, targetPos, time,
                false);
            _currentCameraMoveTimeTask =
                TimeSvc.Instance.RotateTargetPos(currentCamera.transform, targetRotate, time, false);
            _currentCameraRotateTimeTask = TimeSvc.Instance.MoveTargetPos(currentCamera.transform,
                new Vector3(0, currentCamera.transform.localPosition.y, 0), new Vector3(0, targetHigh.y, 0), time,
                false);
            _currentCameraReSetTimeTask = TimeSvc.Instance.AddTimeTask(() =>
                {
                    _controllerRotate.enabled = true;
                    this.isControl = isControl;
                    _controllerRotate.SetRotateObj(currentCamera.transform);
                    navMeshAgent.enabled = true;
                    action.Invoke();
                    currentCamera.fieldOfView = fieldView;
                }, "校准位置",
                time + 0.02f);
        }

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

        public void StopMove()
        {
            TimeSvc.Instance.DeleteTimeTask(_navMeshAgentMoveTimeTask);
            TimeSvc.Instance.DeleteTimeTask(_currentCameraMoveTimeTask);
            TimeSvc.Instance.DeleteTimeTask(_currentCameraRotateTimeTask);
            TimeSvc.Instance.DeleteTimeTask(_currentCameraReSetTimeTask);
        }

        private void OnDestroy()
        {
            StopMove();
            _controllerRotate.SetRotateObj(currentCamera.transform);
        }
    }
}