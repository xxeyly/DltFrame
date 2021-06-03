using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    partial class BaseWindow
    {
        protected AudioSvc AudioSvc;
        protected ResSvc ResSvc;
        protected ViewSvc ViewSvc;
        protected SceneSvc SceneSvc;
        protected PersistentDataSvc PersistentDataSvc;
        protected TimeSvc TimeSvc;
        protected ListenerSvc ListenerSvc;
        protected MouseSvc MouseSvc;
        protected EntitySvc EntitySvc;

        private void SvcInit()
        {
            AudioSvc = AudioSvc.Instance;
            ResSvc = ResSvc.Instance;
            ViewSvc = ViewSvc.Instance;
            SceneSvc = SceneSvc.Instance;
            TimeSvc = TimeSvc.Instance;
            PersistentDataSvc = PersistentDataSvc.Instance;
            ListenerSvc = ListenerSvc.Instance;
            MouseSvc = MouseSvc.Instance;
            EntitySvc = EntitySvc.Instance;
        }
    }
}