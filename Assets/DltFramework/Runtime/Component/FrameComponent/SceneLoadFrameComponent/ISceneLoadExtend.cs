using UnityEngine.SceneManagement;

namespace DltFramework
{
    public interface ISceneLoadExtend
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneMode">加载模式</param>
        public void S_SceneLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneMode">加载模式</param>
        public void S_SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single);
        /// <summary>
        /// 退出
        /// </summary>
        public void S_SceneEsc();
    }
}