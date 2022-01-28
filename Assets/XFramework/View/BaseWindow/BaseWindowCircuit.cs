using System;

namespace XFramework
{
    public partial class BaseWindow
    {
        /// <summary>
        /// 开始流程
        /// </summary>
        /// <param name="circuitBaseData"></param>
        public void StartCircuit(Type circuitBaseData)
        {
            CircuitComponent.Instance.StartCircuit(circuitBaseData);
        }

        /// <summary>
        /// 结束流程
        /// </summary>
        public void EndCircuit()
        {
            CircuitComponent.Instance.EndCircuit();
        }
    }
}