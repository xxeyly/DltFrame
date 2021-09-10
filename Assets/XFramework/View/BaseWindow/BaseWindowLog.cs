using System.Runtime.CompilerServices;
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        public void Log(string message)
        {
            if (isLog)
            {
                Debug.Log(message);
            }
        }

        public void LogError(object message)
        {
            if (isLog)
            {
                Debug.LogError(message);
            }
        }
    }
}