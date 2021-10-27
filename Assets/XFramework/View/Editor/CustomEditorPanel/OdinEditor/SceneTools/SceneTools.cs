using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace XFramework
{
    public class SceneTools : BaseEditor
    {
        [LabelText("场景工具路径")] public Transform sceneToolsRoot;

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
        }

        private void CheckSceneTools()
        {
            if (GameObject.Find("SceneTools") != null)
            {
                sceneToolsRoot = GameObject.Find("SceneTools").transform;
            }
            else
            {
                sceneToolsRoot = new GameObject("SceneTools").transform;
            }

        }

        [Button(ButtonSizes.Medium)]
        [LabelText("射线工具")]
        public void OnAddRayRenderTools()
        {
            CheckSceneTools();

            bool isLoad = sceneToolsRoot.GetComponentInChildren<RayRenderTools>();
            if (isLoad)
            {
                return;
            }

            GameObject rayRenderTools = new GameObject("RayRenderTools");
            rayRenderTools.AddComponent<RayRenderTools>();
            //设置父物体
            rayRenderTools.transform.parent = sceneToolsRoot;
        }

        [Button(ButtonSizes.Medium)]
        [LabelText("场景漫游")]
        public void OnAddSceneRoaming()
        {
            CheckSceneTools();

            bool isLoad = sceneToolsRoot.GetComponentInChildren<CameraControl>();
            if (isLoad)
            {
                return;
            }

            GameObject CameraTools = new GameObject("CameraTools");
            ControllerSelfRotate controllerSelfRotate = CameraTools.AddComponent<ControllerSelfRotate>();
            CameraControl cameraControl = CameraTools.AddComponent<CameraControl>();
            CameraTools.AddComponent<CameraPosEditor>();
            GameObject CameraPosition = new GameObject("CameraPosition");
            NavMeshAgent navMeshAgent = CameraPosition.AddComponent<NavMeshAgent>();
            GameObject MainCamera = new GameObject("Main Camera");
            MainCamera.AddComponent<Camera>();
            MainCamera.AddComponent<AudioListener>();
            MainCamera.tag = "MainCamera";
            //设置父物体
            MainCamera.transform.parent = CameraPosition.transform;
            CameraPosition.transform.parent = CameraTools.transform;
            CameraTools.transform.parent = sceneToolsRoot;
            //属性设置
            controllerSelfRotate.targetTri = MainCamera.transform;
            cameraControl.navMeshAgent = navMeshAgent;
        }

        [Button(ButtonSizes.Medium)]
        [LabelText("动画管理")]
        public void OnAddAnimManager()
        {
            CheckSceneTools();

            bool isLoad = sceneToolsRoot.GetComponentInChildren<AnimatorControllerManager>();
            if (isLoad)
            {
                return;
            }

            GameObject AnimatorControllerManager = new GameObject("AnimatorControllerManager");
            AnimatorControllerManager.AddComponent<AnimatorControllerManager>();
            AnimatorControllerManager.transform.parent = sceneToolsRoot;
        }

        [Button(ButtonSizes.Medium)]
        [LabelText("场景流程管理")]
        public void OnAddSceneCircuitManager()
        {
            CheckSceneTools();

            bool isLoad = sceneToolsRoot.GetComponentInChildren<SceneCircuitManager>();
            if (isLoad)
            {
                return;
            }

            GameObject sceneCircuitManager = new GameObject("SceneCircuitManager");
            sceneCircuitManager.AddComponent<SceneCircuitManager>();
            sceneCircuitManager.transform.parent = sceneToolsRoot;
        }
    }
}