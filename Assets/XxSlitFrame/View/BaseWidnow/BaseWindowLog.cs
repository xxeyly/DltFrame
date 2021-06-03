using System.Runtime.CompilerServices;
using UnityEngine;

namespace XxSlitFrame.View
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