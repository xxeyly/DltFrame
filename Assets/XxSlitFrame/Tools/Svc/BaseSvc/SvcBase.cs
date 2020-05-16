using UnityEngine;

namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public abstract class SvcBase : MonoBehaviour, ISvc
    {
        public abstract void StartSvc();


        public abstract void InitSvc();
    }
}