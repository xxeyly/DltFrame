using System;
using System.Collections.Generic;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class WindowBaseAddTool : MonoBehaviour
{
    [LabelText("添加名称")] public string WindowBaseName;

    [GUIColor(0f, 1f, 0f),]
    [Button("添加", ButtonSizes.Large)]
    public void AddWindowBase()
    {
        gameObject.AddComponent(Type.GetType(WindowBaseName));
        DestroyImmediate(this);
    }
}