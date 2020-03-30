using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XxSlitFrame.Tools.Editor
{
    /// <summary>
    /// 动画数据
    /// </summary>
    public class CustomAnimatorData
    {
        /// <summary>
        /// 当前动画文件
        /// </summary>
        public string CurrentFbxPath;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string AnimatorName;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        public string ExportPath;

        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string ProfilePath;
    }

    //自定义动画编辑
    public class CustomAnimator : EditorWindow
    {
        [MenuItem("xxslit/动画工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomAnimator>();
            window.minSize = new Vector2(500, 100);
            window.maxSize = new Vector2(500, 100);
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

        /// <summary>
        /// 配置文件信息
        /// </summary>
        private List<AnimatorClipDataInfo> _animatorClipDataInfos;

        private ModelImporter _modelImporter;

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
            _currentFbx = (GameObject) EditorGUILayout.ObjectField(_currentFbx, typeof(GameObject), GUILayout.MaxWidth(150));
#pragma warning restore 618

            #endregion

            #region 项目名称

            EditorGUILayout.LabelField("Animator名称:", GUILayout.MaxWidth(80));
            _animatorName = EditorGUILayout.TextField(_animatorName);
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

            this._exportPath = EditorGUILayout.TextField(this._exportPath, GUILayout.MaxWidth(420), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择配置文件", GUILayout.MaxWidth(120)))
            {
                this._profilePath = EditorUtility.OpenFilePanel("选择配置文件", "", "");
            }

            this._profilePath = EditorGUILayout.TextField(this._profilePath, GUILayout.MaxWidth(420), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 分割动画并配置Animator数据

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("分割动画并配置Animator数据", GUILayout.MaxWidth(500), GUILayout.MaxHeight(40)))
            {
                BuildAnim();
            }

            EditorGUILayout.EndHorizontal();

            #endregion
        }


        private void BuildAnim()
        {
            #region 动画分段

            if (File.Exists(_profilePath))
            {
                _animatorClipDataInfos = JsonMapper.ToObject<List<AnimatorClipDataInfo>>(GetTextToLoad(_profilePath));
            }

            _modelImporter = (ModelImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(_currentFbx));
            _modelImporter.animationType = ModelImporterAnimationType.Generic;
            _modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
            ModelImporterClipAnimation[] animations = new ModelImporterClipAnimation[_animatorClipDataInfos.Count];

            for (int i = 0; i < _animatorClipDataInfos.Count; i++)
            {
                animations[i] = SetClipAnimation(_animatorClipDataInfos[i].animatorClipName, _animatorClipDataInfos[i].animatorClipFirstFrame,
                    _animatorClipDataInfos[i].animatorClipLastFrame,
                    _animatorClipDataInfos[i].animatorClipIsLoop);
            }

            _modelImporter.clipAnimations = animations;
            _modelImporter.SaveAndReimport();
            //动画文件下的所有文件
            List<Object> allAnimObject = new List<Object>(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_currentFbx)));
            List<string> allAnimClipName = GetAllAnimClipName(_animatorClipDataInfos);
            Dictionary<string, AnimationClip> animationClipDic = BuildingAnimationClips(allAnimObject, allAnimClipName);
            //筛选出动画文件
            for (int i = 0; i < allAnimObject.Count; i++)
            {
                Debug.Log(allAnimObject[i].name);
            }

            #endregion

            #region 动画控制器

            if (Directory.Exists(_exportPath) && _animatorName != "")
            {
                AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(_exportPath + "/" + _animatorName + ".controller");
                AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;

                for (int i = 0; i < _animatorClipDataInfos.Count; i++)
                {
                    //添加 参数
                    animatorController.AddParameter(_animatorClipDataInfos[i].animatorClipName, _animatorClipDataInfos[i].animatorClipStatesType);
                    //添加 片段
                    AnimatorState state = rootStateMachine.AddState(_animatorClipDataInfos[i].animatorClipName);
                    //设置动画
                    state.motion = animationClipDic[_animatorClipDataInfos[i].animatorClipName];
                    // 关联片段 
                    AnimatorStateTransition animatorStateTransition = rootStateMachine.AddAnyStateTransition(state);
                    //设置关联参数
                    animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, _animatorClipDataInfos[i].animatorClipName);
                    //设置持续时间
                    animatorStateTransition.duration = _animatorClipDataInfos[i].transitionDuration;
                }
            }

            #endregion
        }

        /// <summary>
        /// 初始化工具
        /// </summary>
        private void InitData()
        {
            if (File.Exists(Application.dataPath + "/Resources/CustomAnimatorData.json"))
            {
                CustomAnimatorData customAnimatorData =
                    JsonMapper.ToObject<CustomAnimatorData>(GetTextToLoad(Application.dataPath + "/Resources", "CustomAnimatorData.json"));
                _currentFbx = AssetDatabase.LoadAssetAtPath<GameObject>(customAnimatorData.CurrentFbxPath);
                _animatorName = customAnimatorData.AnimatorName;
                _exportPath = customAnimatorData.ExportPath;
                _profilePath = customAnimatorData.ProfilePath;
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

        public Dictionary<string, AnimationClip> BuildingAnimationClips(List<Object> allFbxObject, List<string> animClipNames)
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
        /// <param name="animatorClipDataInfos"></param>
        /// <returns></returns>
        private List<string> GetAllAnimClipName(List<AnimatorClipDataInfo> animatorClipDataInfos)
        {
            List<string> animNames = new List<string>();
            foreach (AnimatorClipDataInfo animatorClipDataInfo in animatorClipDataInfos)
            {
                animNames.Add(animatorClipDataInfo.animatorClipName);
            }

            return animNames;
        }

        #endregion

        private void OnDestroy()
        {
            #region 保存配置数据

            CustomAnimatorData customAnimatorData = new CustomAnimatorData
            {
                AnimatorName = _animatorName, ExportPath = _exportPath, ProfilePath = _profilePath, CurrentFbxPath = AssetDatabase.GetAssetPath(_currentFbx)
            };
            SaveTextToLoad(Application.dataPath + "/Resources", "CustomAnimatorData.json", EditorJsonUtility.ToJson(customAnimatorData));

            #endregion
        }

        #region 数据存储

        /// <summary>
        /// 保存文本信息到本地
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="information">保存信息</param>
        public static void SaveTextToLoad(string path, string fileName, string information)
        {
//            UnityEngine.Debug.Log(Path + "/" + FileName);

            if (Directory.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(information);
            sw.Close();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// 读取本地文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetTextToLoad(string path, string fileName)
        {
//            UnityEngine.Debug.Log(Path + "/" + FileName);
            if (Directory.Exists(path))
            {
            }
            else
            {
                Debug.LogError("文件不存在:" + path + "/" + fileName);
            }

            FileStream aFile = new FileStream(path + "/" + fileName, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            var textData = sr.ReadToEnd();
            sr.Close();
            return textData;
        }

        /// <summary>
        /// 读取本地文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetTextToLoad(string path)
        {
//            UnityEngine.Debug.Log(Path + "/" + FileName);
            if (File.Exists(path))
            {
            }
            else
            {
                Debug.LogError("文件不存在:" + path);
            }

            FileStream aFile = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);
            var textData = sr.ReadToEnd();
            sr.Close();
            return textData;
        }

        #endregion
    }

    /// <summary>
    /// 动画配置文件信息
    /// </summary>
    [Serializable]
    public class AnimatorClipDataInfo
    {
        /// <summary>
        /// 动画配置片段名称
        /// </summary>
        public string animatorClipName;


        /// <summary>
        /// 动画配置片段状态类型
        /// </summary>
        public AnimatorControllerParameterType animatorClipStatesType;

        /// <summary>
        /// 固定持续时间开关
        /// </summary>
        public bool fixedDuration;

        /// <summary>
        /// 动画过度时间
        /// </summary>
        public int transitionDuration;

        /// <summary>
        /// 动画配置片段开始帧
        /// </summary>
        public int animatorClipFirstFrame;

        /// <summary>
        /// 动画配置片段结束帧
        /// </summary>
        public int animatorClipLastFrame;

        /// <summary>
        /// 动画配置片段是否循环
        /// </summary>
        public bool animatorClipIsLoop;
    }
}