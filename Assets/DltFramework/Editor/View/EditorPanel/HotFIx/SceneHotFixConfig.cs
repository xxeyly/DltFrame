using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    public class SceneHotFixConfig : BaseEditor
    {
#pragma warning disable CS0414 // 字段已被赋值，但它的值从未被使用
        [LabelText("场景资源文件是否存在")] private bool isExistsNormalSceneAssetBundleAsset;
#pragma warning restore CS0414 // 字段已被赋值，但它的值从未被使用

        [ShowIf("isExistsNormalSceneAssetBundleAsset")] [HorizontalGroup("NormalSceneAssetBundleAsset")] [LabelText("正常场景配置")] [InlineEditor()] [OnValueChanged("OnNormalSceneAssetBundleAssetChanged")] [AssetList]
        public NormalSceneAssetBundleAsset NormalSceneAssetBundleAsset;

        [ShowIf("@this.isExistsNormalSceneAssetBundleAsset")]
        [EnableIf("@this.NormalSceneAssetBundleAsset!=null")]
        [GUIColor(1, 0, 0)]
        [Button("删除", ButtonSizes.Medium)]
        public void RemoveNormalSceneAssetBundleAsset()
        {
            if (NormalSceneAssetBundleAsset != null)
            {
                UnityEditor.AssetDatabase.DeleteAsset("Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }
        }
#pragma warning disable CS0414 // 字段已被赋值，但它的值从未被使用
        [LabelText("创建场景资源文件")] private bool isCreateNormalSceneAssetBundleAsset;
#pragma warning restore CS0414 // 字段已被赋值，但它的值从未被使用
        [Button("创建场景热更配置")]
        [ShowIf("@isCreateNormalSceneAssetBundleAsset")]
        public void CreateSceneAssetBundleAsset()
        {
            if (!Directory.Exists("Assets/Config/SceneHotfixAsset/"))
            {
                Directory.CreateDirectory("Assets/Config/SceneHotfixAsset/");
                UnityEditor.AssetDatabase.Refresh();
            }

            UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NormalSceneAssetBundleAsset>(), "Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        private void OnNormalSceneAssetBundleAssetChanged()
        {
            if (NormalSceneAssetBundleAsset != null)
            {
                if (NormalSceneAssetBundleAsset.name != SceneManager.GetActiveScene().name)
                {
                    List<string> allScenePath = DataFrameComponent.Path_GetSpecifyTypeOnlyInAssets("unity");
                    for (int i = 0; i < allScenePath.Count; i++)
                    {
                        if (DataFrameComponent.Path_GetPathFileNameDontContainFileType(allScenePath[i]) == NormalSceneAssetBundleAsset.name)
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(allScenePath[i]);
                        }
                    }
                }
            }
        }


        public void Update()
        {
            GetSceneAssetBundleAsset();
        }

        [LabelText("获得当前场景热更配置")]
        public void GetSceneAssetBundleAsset()
        {
            NormalSceneAssetBundleAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<NormalSceneAssetBundleAsset>("Assets/Config/SceneHotfixAsset/" + SceneManager.GetActiveScene().name + ".asset");
            if (NormalSceneAssetBundleAsset == null)
            {
                isExistsNormalSceneAssetBundleAsset = false;
                isCreateNormalSceneAssetBundleAsset = true;
            }
            else
            {
                isExistsNormalSceneAssetBundleAsset = true;
                isCreateNormalSceneAssetBundleAsset = false;
            }
        }

        private void ViewSort()
        {
            List<BaseWindow> sceneAllBaseWindow = DataFrameComponent.Hierarchy_GetAllObjectsInScene<BaseWindow>();
            List<BaseWindow> sortBaseWindow = new List<BaseWindow>();

            for (int i = 0; i < sceneAllBaseWindow.Count; i++)
            {
                foreach (BaseWindow baseWindow in sceneAllBaseWindow)
                {
                    if (baseWindow.GetSceneLayerIndex() == i)
                    {
                        sortBaseWindow.Add(baseWindow);
                    }
                }
            }

            //UI层排序
            foreach (BaseWindow baseWindow in sortBaseWindow)
            {
                if (!baseWindow.GetComponent<ChildBaseWindow>())
                {
                    baseWindow.SetSetSiblingIndex();
                }
            }
        }

        public override void OnDisable()
        {
        }

        public override void OnCreateConfig()
        {
        }

        public override void OnSaveConfig()
        {
        }

        public override void OnLoadConfig()
        {
        }

        public override void OnInit()
        {
            GetSceneAssetBundleAsset();
        }
    }
}