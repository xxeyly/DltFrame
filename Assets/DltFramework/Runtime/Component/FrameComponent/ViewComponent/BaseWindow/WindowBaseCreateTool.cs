using System;
using System.Collections.Generic;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


public class WindowBaseCreateTool : MonoBehaviour
{
    [FolderPath(ParentFolder = "Assets/Scripts")] [LabelText("创建路径")]
    public string windowBaseCreatePath = "Assets/Scripts";

    [LabelText("创建名称")] [OnValueChanged("OnChangeWindowBaseName")] [InfoBox("$_createErrorTip")]
    public string windowBaseName;

    private string _createErrorTip = "创建的名称不能为空";

    private bool _isCreate;

    [GUIColor(0f, 1f, 0f),]
    [Button("创建", ButtonSizes.Large)]
    [EnableIf("_isCreate")]
    public void CreateWindowBase()
    {
#if UNITY_EDITOR
        TemplateMenuItem.OnCreateBaseWindowTemplate(windowBaseCreatePath, windowBaseName);
        WindowBaseAddTool windowBaseAddTool = gameObject.AddComponent<WindowBaseAddTool>();
        windowBaseAddTool.WindowBaseName = windowBaseName;
        DestroyImmediate(this);
#endif
    }


    public void OnChangeWindowBaseName()
    {
        name = windowBaseName;
        if (windowBaseName.Length <= 0)
        {
            _createErrorTip = "创建的名称不能为空";
            _isCreate = false;
        }
        else
        {
            List<string> baseWindowNames = new List<string>();
            List<Type> typeList = DataFrameComponent.List_GetSubclasses(typeof(BaseWindow));
            foreach (Type type in typeList)
            {
                baseWindowNames.Add(type.Name);
            }

            if (!baseWindowNames.Contains(windowBaseName))
            {
                _createErrorTip = "可以创建";
                _isCreate = true;
            }
            else
            {
                _createErrorTip = "创建的名称已存在";
                _isCreate = false;
            }
        }
    }
}