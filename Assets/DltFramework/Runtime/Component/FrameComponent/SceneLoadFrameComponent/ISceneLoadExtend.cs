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
        protected void SceneLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneMode">加载模式</param>
        protected void SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single);
    }
}