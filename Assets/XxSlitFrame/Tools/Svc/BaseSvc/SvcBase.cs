using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public abstract class SvcBase : MonoBehaviour, ISvc
    {
        [BoxGroup] [LabelText("初始化")][ToggleLeft] public bool init;
        public abstract void StartSvc();


        public abstract void InitSvc();
    }
}