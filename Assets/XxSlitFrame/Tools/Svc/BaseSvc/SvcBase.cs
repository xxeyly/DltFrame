using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public abstract class SvcBase : MonoBehaviour, ISvc
    {
        [BoxGroup] [LabelText("框架初始化")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public bool frameInit;

        [BoxGroup] [LabelText("场景初始化")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public bool sceneInit;

        public abstract void StartSvc();


        public abstract void InitSvc();
    }
}