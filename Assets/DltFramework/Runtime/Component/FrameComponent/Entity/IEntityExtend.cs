using UnityEngine;

namespace DltFramework
{
    public interface IEntityExtend
    {
        /// <summary>
        /// 实体全部隐藏
        /// </summary>
        void EntityAllHide();

        /// <summary>
        /// 实体全部显示
        /// </summary>
        void EntityAllShow();

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetEntity<T>(string entityName);

        /// <summary>
        /// 根据名称返回第一个Entity类型
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <returns></returns>
        EntityItem GetEntity(string entityName);

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="display"></param>
        void DisplayEntity(bool display, string entityName);

        /// <summary>
        /// 根据实体名称显示或隐藏
        /// </summary>
        /// <param name="entityNames"></param>
        /// <param name="display"></param>
        void DisplayEntity(bool display, params string[] entityNames);

        /// <summary>
        /// 根据进程名称显示或隐藏
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <param name="display">显示或隐藏</param>
        /// <param name="entityNames">实体名称</param>
        void DisplayEntity(string processName, bool display, params string[] entityNames);

        /// <summary>
        /// 根据进程名称释放实体
        /// </summary>
        /// <param name="processName"></param>
        void EntityReleaseProcess(string processName);

        /// <summary>
        /// 获得实体的状态
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        bool GetEntityState(string entityName);
        
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entityName">实体名称</param>
        public void RemoveEntity(string entityName);
        /// <summary>
        /// 实体实例化
        /// </summary>
        /// <param name="instantiateObj">实例化对象</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject instantiateObj);

        /// <summary>
        /// 实体实例化
        /// </summary>
        /// <param name="instantiateObj">实例化对象</param>
        /// <param name="parent">父物体</param>
        /// <param name="world">是否在世界坐标系下</param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject instantiateObj, Transform parent, bool world);
    }
}