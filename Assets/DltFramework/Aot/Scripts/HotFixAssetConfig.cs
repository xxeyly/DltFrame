using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aot
{
    [Serializable]
    public class HotFixAssetConfig
    {
        [LabelText("名称")] public string name;
        [LabelText("Md5")] public string md5;
        [LabelText("大小")] public string size;
        [LabelText("路径")] public string path;
    }
}