using UnityEditor;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomStepInitData : EditorWindow
    {
        StepInitData _stepInitData;
        Vector2 _scrollPos = Vector2.zero;

        [MenuItem("xxslit/步骤数据")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomStepInitData>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(900, 900);
            window.titleContent = new GUIContent() {image = null, text = "步骤数据"};
            window.Show();
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("音频配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _stepInitData = (StepInitData) EditorGUILayout.ObjectField(_stepInitData, typeof(StepInitData));
#pragma warning restore 618
            EditorGUILayout.EndHorizontal();

            #endregion

            if (_stepInitData != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加"))
                {
                    _stepInitData.stepInitDataInfoGroups.Add(new StepInitDataInfo());
                }

                EditorGUILayout.EndHorizontal();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _stepInitData.stepInitDataInfoGroups.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("大步骤", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].bigIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].bigIndex);
                    EditorGUILayout.LabelField("小步骤", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].smallIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].smallIndex);
                    EditorGUILayout.LabelField("语音索引", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].tipIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].tipIndex);
                    EditorGUILayout.LabelField("物品组索引", GUILayout.MaxWidth(60));
                    _stepInitData.stepInitDataInfoGroups[i].propGroupIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].propGroupIndex);
                    EditorGUILayout.LabelField("相机位置", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].cameraPosType =
                        (CameraPosData.CameraPosType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].cameraPosType);
                    EditorGUILayout.LabelField("动画类型", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].animType = (AnimType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].animType);
                    EditorGUILayout.LabelField("动画进度", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].animSpeedProgress =
                        (AnimSpeedProgress) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].animSpeedProgress);
                    if (GUILayout.Button("删除步骤", GUILayout.MaxWidth(80)))
                    {
                        _stepInitData.stepInitDataInfoGroups.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();

                EditorUtility.SetDirty(_stepInitData);
            }
        }
    }
}