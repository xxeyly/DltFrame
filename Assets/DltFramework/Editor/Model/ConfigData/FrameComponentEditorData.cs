﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public class FrameComponentEditorData : ScriptableObject
    {
        [LabelText("包导入地址")] public  string componentPackageServerPath = @"127.0.0.1\Import";
    }
}