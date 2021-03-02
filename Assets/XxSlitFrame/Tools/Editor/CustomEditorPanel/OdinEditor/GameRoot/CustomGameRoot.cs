using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.GameRoot
{
    public class CustomGameRoot
    {
        private CustomScriptableObject.CustomScriptableObject _customScriptableObject;

        [Toggle("Enabled")] [LabelText("音频服务")]
        public CustomAudioSvc _customAudioSvc;

        public CustomGameRoot(CustomScriptableObject.CustomScriptableObject customScriptableObject,
            CustomAudioSvc customAudioSvc)
        {
            _customScriptableObject = customScriptableObject;
            _customAudioSvc = customAudioSvc;
        }
    }
}