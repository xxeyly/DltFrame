using CameraTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using XAnimator.Base;

namespace XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.SceneTools
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

        [Button(ButtonSizes.Medium)]
        [LabelText("射线工具")]
        public void OnAddRayRenderTools()
        {
            if (sceneToolsRoot == null)
            {
                return;
            }

            bool isLoad = sceneToolsRoot.GetComponentInChildren<RayRenderTools>();
            if (isLoad)
            {
                return;
            }

            GameObject RayRenderTools = new GameObject("RayRenderTools");
            RayRenderTools.AddComponent<RayRenderTools>();
            //设置父物体
            RayRenderTools.transform.parent = sceneToolsRoot;
        }

        [Button(ButtonSizes.Medium)]
        [LabelText("场景漫游")]
        public void OnAddSceneRoaming()
        {
            if (sceneToolsRoot == null)
            {
                return;
            }

            bool isLoad = sceneToolsRoot.GetComponentInChildren<CameraControl>();
            if (isLoad)
            {
                return;
            }

            GameObject CameraTools = new GameObject("CameraTools");
            ControllerRotate controllerRotate = CameraTools.AddComponent<ControllerRotate>();
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
            controllerRotate.targetTri = MainCamera.transform;
            cameraControl.navMeshAgent = navMeshAgent;
        }

        [Button(ButtonSizes.Medium)]
        [LabelText("动画管理")]
        public void OnAddAnimManager()
        {
            if (sceneToolsRoot == null)
            {
                return;
            }

            bool isLoad = sceneToolsRoot.GetComponentInChildren<AnimatorControllerManager>();
            if (isLoad)
            {
                return;
            }

            GameObject AnimatorControllerManager = new GameObject("AnimatorControllerManager");
            AnimatorControllerManager.AddComponent<AnimatorControllerManager>();
            AnimatorControllerManager.transform.parent = sceneToolsRoot;
        }
    }
}