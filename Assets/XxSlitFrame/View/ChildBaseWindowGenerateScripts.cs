using System;
using UnityEngine;

namespace XxSlitFrame.View
{
#if UNITY_EDITOR

    public class ChildBaseWindowGenerateScripts : BaseWindowGenerateScripts
    {
        protected override string GetScriptsPath()
        {
            Type scriptType = GetComponent<ChildBaseWindow>().GetType();
            string[] scriptNameSplit = scriptType.ToString().Split(new char[] {'.'});
            string scriptName = scriptNameSplit[scriptNameSplit.Length - 1];
            string scriptPath = GetPath(scriptName);
            return scriptPath;
        }

        protected override Transform GetWindow()
        {
            return transform;
        }
    }
#endif
}