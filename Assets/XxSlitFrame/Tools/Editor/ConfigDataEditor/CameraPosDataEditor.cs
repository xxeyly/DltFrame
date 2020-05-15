using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(CameraPosData))]
    public class CameraPosDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CameraPosData cameraPosData
                = (CameraPosData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加", GUILayout.MaxHeight(30)))
            {
                cameraPosData.cameraPosInfosGroup.Add(new CameraPosInfo());
            }

            if (GUILayout.Button("删除", GUILayout.MaxHeight(30)))
            {
                cameraPosData.cameraPosInfosGroup.PopLast();
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < cameraPosData.cameraPosInfosGroup.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("位置信息", GUILayout.MaxWidth(50));
                cameraPosData.cameraPosInfosGroup[i].cameraPosType =
                    (CameraPosData.CameraPosType) EditorGUILayout.EnumPopup(cameraPosData.cameraPosInfosGroup[i].cameraPosType, GUILayout.MaxWidth(150));
                EditorGUILayout.LabelField("相机深度", GUILayout.MaxWidth(50));
                cameraPosData.cameraPosInfosGroup[i].cameraFieldView = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraFieldView, GUILayout.MaxWidth(20));
                EditorGUILayout.LabelField("寻路位置", GUILayout.MaxWidth(50));
                cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x, GUILayout.MaxWidth(60));

                EditorGUILayout.LabelField("相机位置", GUILayout.MaxWidth(50));
                cameraPosData.cameraPosInfosGroup[i].cameraPos.x = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraPos.x, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].cameraPos.y = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraPos.y, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].cameraPos.z = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraPos.z, GUILayout.MaxWidth(60));

                EditorGUILayout.LabelField("相机旋转", GUILayout.MaxWidth(50));
                cameraPosData.cameraPosInfosGroup[i].cameraRot.x = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraRot.x, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].cameraRot.y = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraRot.y, GUILayout.MaxWidth(60));
                cameraPosData.cameraPosInfosGroup[i].cameraRot.z = EditorGUILayout.FloatField(cameraPosData.cameraPosInfosGroup[i].cameraRot.z, GUILayout.MaxWidth(60));

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}