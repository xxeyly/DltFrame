using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools
{
    public class SceneInit : Singleton<SceneInit>
    {
        protected override void Awake()
        {
            base.Awake();
            FindObjectOfType<ListenerSvc>().StartSvc();
            FindObjectOfType<ViewSvc>().StartSvc();
        }
    }
}