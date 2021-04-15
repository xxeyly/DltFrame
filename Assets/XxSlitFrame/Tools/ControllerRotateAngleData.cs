using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class ControllerRotateAngleData : MonoBehaviour
{
    [LabelText("左右角度限定")] public Vector2 leftAndRightLimit;

    [LabelText("上下角度限定")] public Vector2 topAndDownLimit;
}