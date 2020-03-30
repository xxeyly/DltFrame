using UnityEditor;

namespace XxSlitFrame.View.Button.Editor
{
    /// <summary>
    /// Custom inspector for Object including derived classes.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawButtons();

            // Draw the rest of the inspector as usual
            DrawDefaultInspector();
        }
    }
}
