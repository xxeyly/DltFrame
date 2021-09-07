using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    partial class BaseWindow
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void SceneLoad(string sceneName)
        {
            SceneSvc.Instance.SceneLoad(sceneName);
        }
    }
}