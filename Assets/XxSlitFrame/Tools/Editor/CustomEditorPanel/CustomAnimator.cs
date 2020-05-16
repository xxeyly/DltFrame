using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;
using Object = UnityEngine.Object;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    //自定义动画编辑
    public class CustomAnimator : EditorWindow
    {
        [MenuItem("xxslit/动画工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomAnimator>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(900, 900);
            window.titleContent = new GUIContent() {image = null, text = "动画工具"};
            window.Show();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private bool _initData;

        /// <summary>
        /// 当前动画文件
        /// </summary>
        private GameObject _currentFbx;

        /// <summary>
        /// 项目名称
        /// </summary>
        private string _animatorName;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        private string _exportPath;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string _profilePath;

        public AnimatorClipData animatorClipData;

        private ModelImporter _modelImporter;

        private CustomAnimatorClipConfig _customAnimatorClipConfig;
        Vector2 _scrollPos = Vector2.zero;


        private void OnGUI()
        {
            if (!_initData)
            {
                InitData();
                _initData = true;
            }

            #region 动画Fbx源文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("动画Fbx源文件:", GUILayout.MaxWidth(100));
#pragma warning disable 618
            _currentFbx = (GameObject) EditorGUILayout.ObjectField(_currentFbx, typeof(GameObject), GUILayout.MaxWidth(250));
#pragma warning restore 618

            #endregion

            #region 项目名称

            EditorGUILayout.LabelField("Animator名称:", GUILayout.MaxWidth(80));
            _animatorName = EditorGUILayout.TextField(_animatorName, GUILayout.MaxWidth(470));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包路径

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择动画存放路径", GUILayout.MaxWidth(120)))
            {
                string projectPath = Application.dataPath;
                string openFolderPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                if (openFolderPath.Contains(projectPath))
                {
                    //转换为相对路径
                    openFolderPath = openFolderPath.Remove(0, projectPath.Length - 6);
                    this._exportPath = openFolderPath;
                }
            }

            this._exportPath = EditorGUILayout.TextField(this._exportPath, GUILayout.MaxWidth(780));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("动画配置数据:", GUILayout.MaxWidth(120));
#pragma warning disable 618
            animatorClipData = (AnimatorClipData) EditorGUILayout.ObjectField(animatorClipData, typeof(AnimatorClipData), GUILayout.MaxWidth(400));
#pragma warning restore 618

            if (GUILayout.Button("分割动画并配置Animator数据", GUILayout.MaxWidth(500)))
            {
                BuildAnim();
            }

            #endregion

            EditorGUILayout.EndHorizontal();

            #region 动画帧

            EditorGUILayout.BeginHorizontal();
            if (animatorClipData == null)
            {
                return;
            }

            if (GUILayout.Button("增加关键帧"))
            {
                animatorClipData.animatorClipDataInfos.Add(new AnimatorClipData.AnimatorClipDataInfo() {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
            }

            EditorGUILayout.EndHorizontal();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            for (int i = 0; i < animatorClipData.animatorClipDataInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("动画属性名称", GUILayout.MaxWidth(70));
                animatorClipData.animatorClipDataInfos[i].animatorClipName = EditorGUILayout.TextField(animatorClipData.animatorClipDataInfos[i].animatorClipName);
                EditorGUILayout.LabelField("属性类型", GUILayout.MaxWidth(50));
                animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType =
                    (AnimatorControllerParameterType) EditorGUILayout.EnumPopup(animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType, GUILayout.MaxWidth(100));
                EditorGUILayout.LabelField("固定过渡持续时间", GUILayout.MaxWidth(100));
                animatorClipData.animatorClipDataInfos[i].fixedDuration = EditorGUILayout.Toggle(animatorClipData.animatorClipDataInfos[i].fixedDuration, GUILayout.MaxWidth(10));
                EditorGUILayout.LabelField("持续过度时间", GUILayout.MaxWidth(70));
                animatorClipData.animatorClipDataInfos[i].transitionDuration =
                    EditorGUILayout.FloatField(animatorClipData.animatorClipDataInfos[i].transitionDuration, GUILayout.MaxWidth(30));
                EditorGUILayout.LabelField("循环", GUILayout.MaxWidth(30));
                animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop =
                    EditorGUILayout.Toggle(animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop, GUILayout.MaxWidth(10));
                EditorGUILayout.LabelField("开始帧", GUILayout.MaxWidth(40));
                animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame = EditorGUILayout.IntField(animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame);
                EditorGUILayout.LabelField("结束帧", GUILayout.MaxWidth(40));
                animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame = EditorGUILayout.IntField(animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame);
                if (GUILayout.Button("增加关键帧"))
                {
                    animatorClipData.animatorClipDataInfos.Insert(i + 1,
                        new AnimatorClipData.AnimatorClipDataInfo() {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
                }

                if (GUILayout.Button("删除关键帧"))
                {
                    animatorClipData.animatorClipDataInfos.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            #endregion

            EditorUtility.SetDirty(_customAnimatorClipConfig);
        }


        private void BuildAnim()
        {
            #region 动画分段

            if (_currentFbx != null)
            {
                _modelImporter = (ModelImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_currentFbx));
                if (_modelImporter != null)
                {
                    _modelImporter.animationType = ModelImporterAnimationType.Generic;
                    _modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
                    ModelImporterClipAnimation[] animations = new ModelImporterClipAnimation[animatorClipData.animatorClipDataInfos.Count];

                    for (int i = 0; i < animatorClipData.animatorClipDataInfos.Count; i++)
                    {
                        animations[i] = SetClipAnimation(animatorClipData.animatorClipDataInfos[i].animatorClipName,
                            animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame,
                            animatorClipData.animatorClipDataInfos[i].animatorClipLastFrame,
                            animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop);
                    }

                    _modelImporter.clipAnimations = animations;
                    _modelImporter.SaveAndReimport();
                    //动画文件下的所有文件
                    List<Object> allAnimObject = new List<Object>(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_currentFbx)));
                    List<string> allAnimClipName = GetAllAnimClipName();
                    Dictionary<string, AnimationClip> animationClipDic = BuildingAnimationClips(allAnimObject, allAnimClipName);

                    #endregion

                    #region 动画控制器

                    if (Directory.Exists(_exportPath) && _animatorName != "")
                    {
                        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(_exportPath + "/" + _animatorName + ".controller");
                        AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;

                        for (int i = 0; i < animatorClipData.animatorClipDataInfos.Count; i++)
                        {
                            //添加 参数
                            animatorController.AddParameter(animatorClipData.animatorClipDataInfos[i].animatorClipName,
                                animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType);
                            //添加 片段
                            AnimatorState state = rootStateMachine.AddState(animatorClipData.animatorClipDataInfos[i].animatorClipName);
                            //设置动画
                            state.motion = animationClipDic[animatorClipData.animatorClipDataInfos[i].animatorClipName];
                            // 关联片段 
                            AnimatorStateTransition animatorStateTransition = rootStateMachine.AddAnyStateTransition(state);
                            //设置关联参数
                            animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, animatorClipData.animatorClipDataInfos[i].animatorClipName);
                            //设置持续时间
                            animatorStateTransition.duration = animatorClipData.animatorClipDataInfos[i].transitionDuration;
                        }
                    }

                    #endregion
                }
            }
            else
            {
                Debug.LogError("当前Fbx文件没有选择");
            }
        }

        /// <summary>
        /// 初始化工具
        /// </summary>
        private void InitData()
        {
            //配置文件数据
            _customAnimatorClipConfig =
                (CustomAnimatorClipConfig) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/CustomAnimatorClipConfig.asset", typeof(CustomAnimatorClipConfig));
            if (_customAnimatorClipConfig != null)
            {
                _currentFbx = _customAnimatorClipConfig.currentFbx;
                _animatorName = _customAnimatorClipConfig.animatorName;
                _exportPath = _customAnimatorClipConfig.exportPath;
                _profilePath = _customAnimatorClipConfig.profilePath;
            }
            else
            {
                AssetDatabase.CreateAsset(CreateInstance<CustomAnimatorClipConfig>(), "Assets" + "/" + "XxSlitFrame/Config/CustomAnimatorClipConfig.asset");
                Debug.LogError("当前配置文件不存在,已经创建");
            }
        }

        #region 设置动画片段

        private ModelImporterClipAnimation SetClipAnimation(string clipName, int firstFrame, int lastFrame, bool isLoop)
        {
            ModelImporterClipAnimation clip = new ModelImporterClipAnimation {name = clipName, firstFrame = firstFrame, lastFrame = lastFrame, loop = isLoop};
            if (isLoop)
            {
                clip.wrapMode = WrapMode.Loop;
            }
            else
            {
                clip.wrapMode = WrapMode.Default;
            }

            return clip;
        }

        #endregion

        #region 构建动画片段键值对

        private Dictionary<string, AnimationClip> BuildingAnimationClips(List<Object> allFbxObject, List<string> animClipNames)
        {
            Dictionary<string, AnimationClip> animationClipDic = new Dictionary<string, AnimationClip>();
            AnimationClip animationClip;
            foreach (string animClipName in animClipNames)
            {
                foreach (Object objectClip in allFbxObject)
                {
                    if (objectClip.name == animClipName)
                    {
                        animationClip = objectClip as AnimationClip;
                        animationClipDic.Add(animClipName, animationClip);
                        break;
                    }
                }
            }

            return animationClipDic;
        }

        /// <summary>
        /// 获得所有动画片段的名字
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllAnimClipName()
        {
            List<string> animNames = new List<string>();
            foreach (AnimatorClipData.AnimatorClipDataInfo animatorClipDataInfo in animatorClipData.animatorClipDataInfos)
            {
                animNames.Add(animatorClipDataInfo.animatorClipName);
            }

            return animNames;
        }

        #endregion

        private void OnDestroy()
        {
            #region 保存配置数据

            _customAnimatorClipConfig.currentFbx = _currentFbx;
            _customAnimatorClipConfig.animatorName = _animatorName;
            _customAnimatorClipConfig.exportPath = _exportPath;
            _customAnimatorClipConfig.profilePath = _profilePath;

            #endregion
        }
    }
}