using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class ResourceUnification : BaseEditor
    {
#pragma warning disable 414
        [BoxGroup("替换字体")] [LabelText("替换字体")] [LabelWidth(60)]
        public Font changeFont;
#pragma warning restore 414
        [BoxGroup("替换字体")]
        [Button(ButtonSizes.Medium)]
        [LabelText("字体替换")]
        public void OnChangeFont()
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

        [BoxGroup("替换场景物体文字")] [LabelText("替换前文字")] [LabelWidth(60)]
        public string sceneReplaceBeforeName;

        [BoxGroup("替换场景物体文字")] [LabelText("替换后文字")] [LabelWidth(60)]
        public string sceneReplaceAfterName;

        [BoxGroup("替换场景物体文字")]
        [Button("替换文字")]
        public void OnReplaceSceneGameObjectName()
        {
            foreach (GameObject sceneObj in DataSvc.GetAllObjectsOnlyInScene())
            {
                string replace = sceneObj.name.Replace(sceneReplaceBeforeName, sceneReplaceAfterName);
                sceneObj.name = replace;
            }
        }

        [BoxGroup("替换Text文字")] [LabelText("替换前文字")] [LabelWidth(60)]
        public string textReplaceBeforeName;

        [BoxGroup("替换Text文字")] [LabelText("替换后文字")] [LabelWidth(60)]
        public string textReplaceAfterName;

        [BoxGroup("替换Text文字")]
        [Button("替换文字")]
        public void OnReplaceTextContent()
        {
            foreach (Text text in DataSvc.GetAllObjectsInScene<Text>())
            {
                string replace = text.text.Replace(textReplaceBeforeName, textReplaceAfterName);
                text.text = replace;
            }
            foreach (TextMeshProUGUI text in DataSvc.GetAllObjectsInScene<TextMeshProUGUI>())
            {
                string replace = text.text.Replace(textReplaceBeforeName, textReplaceAfterName);
                text.text = replace;
            }
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