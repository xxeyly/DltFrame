using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.View.CustomInspector.Editor
{
    /// <summary>
    /// 定义对带有 `CustomLabelAttribute` 特性的字段的面板内容的绘制行为。
    /// </summary>
    [CustomPropertyDrawer(typeof(XLabeTextAttribute))]
    public class XLabeTextDrawer : PropertyDrawer
    {
        private GUIContent _label = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_label == null)
            {
                string name = (attribute as XLabeTextAttribute).name;
                _label = new GUIContent(name);
            }

            EditorGUI.PropertyField(position, property, _label);
        }
    }
}