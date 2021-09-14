using UnityEngine;

namespace XFramework
{
    public abstract class CircuitBaseData : MonoBehaviour
    {
        /// <summary>
        /// 注册
        /// </summary>
        public abstract void StartCircuit();

        /// <summary>
        /// 释放
        /// </summary>
        public abstract void EndCircuit();
    }
}