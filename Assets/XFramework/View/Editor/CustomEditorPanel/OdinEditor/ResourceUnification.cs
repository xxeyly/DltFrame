using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class ResourceUnification : BaseEditor
    {
#pragma warning disable 414
        [HorizontalGroup()] [LabelText("替换字体")][LabelWidth(100)]
        public Font changeFont;
#pragma warning restore 414
        [HorizontalGroup()]
        [Button(ButtonSizes.Medium)]
        [LabelText("字体替换")]
        public void OnTest()
        {
            if (changeFont == null)
            {
                return;
            }

            List<Text> sceneAllText = DataSvc.GetAllObjectsInScene<Text>();
            foreach (Text text in sceneAllText)
            {
                text.font = changeFont;
            }

            Debug.Log("场景字体替换完毕:" + sceneAllText.Count);
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

        public override void OnInit()
        {
        }
    }
}