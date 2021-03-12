using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor
{
    public class ResourceUnification : BaseEditor
    {
        private int max = 1000;
        [ProgressBar(0, "max")] public int progressBar = 0;

        [Button(ButtonSizes.Medium)]
        [LabelText("资源统一化")]
        public void OnTest()
        {
            Debug.Log("资源统一化");
            max = 156;
        }

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }
    }
}