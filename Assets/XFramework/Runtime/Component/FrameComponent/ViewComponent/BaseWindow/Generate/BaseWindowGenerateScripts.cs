using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;


namespace XFramework
{
    /// <summary>
    /// BaseWindow属性
    /// </summary>
    [Serializable]
    public struct GenerateAttributesTypeGroup
    {
        [HorizontalGroup("属性类型")] [HideLabel] public GenerateAttributesType generateAttributesType;
        [HorizontalGroup("属性名称")] [HideLabel] public string attributesName;
        [HorizontalGroup("属性描述")] [HideLabel] public string attributesDescription;
    }
    
  
    /// <summary>
    /// 生成属性类型
    /// </summary>
    public enum GenerateAttributesType
    {
        @int,
        @float,
        @string,
        @Sprite,
        @bool,
        @GameObject,
        @Transform,
        @Camera,
        @Color,
        @Texture,
        [LabelText("List<int>")] @List_int,
        [LabelText("List<float>")] @List_float,
        [LabelText("List<string>")] @List_string,
        [LabelText("List<Sprite>")] @List_Sprite,
        [LabelText("List<bool>")] @List_bool,
        [LabelText("List<GameObject>")] @List_GameObject,
        [LabelText("List<Transform>")] @List_Transform,
        [LabelText("List<Camera>")] @List_Camera,
        [LabelText("List<Color>")] @List_Color,
        [LabelText("List<Texture>")] @List_Texture,
    }

    public class BaseWindowGenerateScripts : ViewGenerateScripts
    {
        #region 子类数据生成

        [BoxGroup("属性生成")] [LabelText("生成属性类名称")]
        public string generateAttributesStructName;

        [BoxGroup("属性生成")] [TableList] [LabelText("生成属性类组")]
        public List<GenerateAttributesTypeGroup> generateAttributesTypeGroups;

        [TabGroup("UI", "属性")] [LabelText("UI变量名称")] [ReadOnly]
        public List<string> allCustomAttributes;

        #endregion

        protected override void GenerateCustomData()
        {
            base.GenerateCustomData();
            allCustomAttributes = new List<string>();
            if (generateAttributesTypeGroups.Count < 1)
            {
                return;
            }

            string dataName = String.Empty;
            if (generateAttributesStructName == String.Empty)
            {
                dataName = GetComponent<BaseWindow>().GetType().ToString();
            }
            else
            {
                dataName = generateAttributesStructName;
            }

            allCustomAttributes.Add(Indents(4) + "[Serializable]" + LineFeed);
            allCustomAttributes.Add(Indents(4) + "public struct " + dataName + "Data" + LineFeed);
            allCustomAttributes.Add(Indents(4) + "{" + LineFeed);

            allUiVariableName.Add(Indents(4) + "[TableList]" + "[LabelText(\"数据\")]" + "public List<" + dataName + "Data" + "> " +
                                  DataFrameComponent.FirstCharToLower(dataName) + "Data" + Semicolon);
            AddUsing("using Sirenix.OdinInspector;");
            AddUsing("using UnityEngine;");
            allCustomAttributes.Add(Indents(8) + "[HorizontalGroup(" + "\"索引\")]" + Indents(1) + "[HideLabel]" + " public int itemIndex;" + LineFeed);

            foreach (GenerateAttributesTypeGroup generateAttributesTypeGroup in generateAttributesTypeGroups)
            {
                string temp = String.Empty;
                temp += Indents(8);
                temp += "[HorizontalGroup(" + "\"" + generateAttributesTypeGroup.attributesDescription + "\")]" + Indents(1);
                temp += "[HideLabel]" + Indents(1);
                string generateAttributesType = generateAttributesTypeGroup.generateAttributesType.ToString().Replace("@", "");
                if (generateAttributesType.Contains("_"))
                {
                    generateAttributesType = generateAttributesTypeGroup.generateAttributesType.ToString().Replace("_", "<") + ">";
                }

                temp += "public" + Indents(1) + generateAttributesType + Indents(1) + generateAttributesTypeGroup.attributesName +
                        Semicolon;
                temp += LineFeed;
                allCustomAttributes.Add(temp);
            }

            allCustomAttributes.Add(Indents(4) + "}" + LineFeed);
        }

        protected override string CustomReplaceScriptContent(string currentScriptsContent)
        {
            return ReplaceScriptContent(currentScriptsContent, allCustomAttributes, GenerateBaseWindowData.startCustomAttributesStart, GenerateBaseWindowData.endCustomAttributesStart);
        }
    }
}