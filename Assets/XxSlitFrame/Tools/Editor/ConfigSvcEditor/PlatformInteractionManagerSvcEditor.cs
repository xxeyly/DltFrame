using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.ConfigSvcEditor
{
    [CustomEditor(typeof(PlatformInteractionManagerSvc))]
    public class PlatformInteractionManagerSvcEditor : UnityEditor.Editor
    {
        private PlatformInteractionManagerSvc _platformInteractionManagerSvc;
        private AssessmentData _assessmentData;
        Vector2 _scrollPos = Vector2.zero;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _platformInteractionManagerSvc = (PlatformInteractionManagerSvc) target;
            if (_platformInteractionManagerSvc.assessmentData != null)
            {
                _assessmentData = _platformInteractionManagerSvc.assessmentData;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("总分数:", GUILayout.MaxWidth(50));
                _assessmentData.zfs = EditorGUILayout.TextField(_assessmentData.zfs);
                EditorGUILayout.LabelField("及格分数:", GUILayout.MaxWidth(60));
                _assessmentData.jgfs = EditorGUILayout.TextField(_assessmentData.jgfs);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("添加步骤", GUILayout.Width(60)))
                {
                    _assessmentData.list.Add(new TopicInfoData());
                }

                if (GUILayout.Button("刷新题号", GUILayout.Width(60)))
                {
                }

                if (GUILayout.Button("刷新类型", GUILayout.Width(60)))
                {
                }

                if (GUILayout.Button("刷新答案", GUILayout.Width(60)))
                {
                }

                EditorGUILayout.EndHorizontal();
                if (_assessmentData.list.Count >= 1)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("题号:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("名称:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("类型:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("选项:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("答案:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("单位:", GUILayout.MaxWidth(60));
                    EditorGUILayout.LabelField("总分:", GUILayout.MaxWidth(60));

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();

                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _assessmentData.list.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _assessmentData.list[i].number = EditorGUILayout.TextField(_assessmentData.list[i].number);
                    _assessmentData.list[i].title = EditorGUILayout.TextField(_assessmentData.list[i].title);
                    _assessmentData.list[i].type = EditorGUILayout.TextField(_assessmentData.list[i].type);
                    _assessmentData.list[i].op = EditorGUILayout.TextField(_assessmentData.list[i].op);
                    _assessmentData.list[i].zqda = EditorGUILayout.TextField(_assessmentData.list[i].zqda);
                    _assessmentData.list[i].unit = EditorGUILayout.TextField(_assessmentData.list[i].unit);
                    _assessmentData.list[i].fs = EditorGUILayout.TextField(_assessmentData.list[i].fs);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}