﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    public class FrameMenu : OdinMenuEditorWindow
    {
        private FrameMenu()
        {
        }

        //添加宏定义
        [Obsolete("Obsolete")]
        private static void AddMacro(string macroName)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            //获取当前平台已有的宏定义
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            //添加想要的宏定义
            var symbolsList = symbols.Split(';').ToList();
            if (symbolsList.Contains(macroName))
            {
                return;
            }

            symbolsList.Add(macroName);
            symbols = string.Join(";", symbolsList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
        }

        //删除宏定义
        [Obsolete("Obsolete")]
        private static void RemoveMacro(string macroName)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            //获取当前平台已有的宏定义
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            //添加想要的宏定义
            var symbolsList = symbols.Split(';').ToList();
            if (!symbolsList.Contains(macroName))
            {
                return;
            }

            symbolsList.Remove(macroName);
            symbols = string.Join(";", symbolsList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
        }

        [MenuItem("DltFrame/框架界面")]
        private static void OpenWindow()
        {
            GetWindow<FrameMenu>().Show();
        }

        [MenuItem("DltFrame/监听生成(手动) &l")]
        private static void OnListenerGenerate()
        {
            GenerateListenerComponent.GenerateListener();
            Debug.Log("监听生成结束!");
        }

        [MenuItem("DltFrame/切换监听[自动|手动]生成")]
        private static void OnListenerAutoGenerateOn()
        {
            //开启
            AutoGenerateListenerConfig autoGenerateListenerConfig = AssetDatabase.LoadAssetAtPath<AutoGenerateListenerConfig>("Assets/Config/" + "AutoGenerateListenerConfig.asset");
            autoGenerateListenerConfig.isAuto = !autoGenerateListenerConfig.isAuto;
            EditorUtility.SetDirty(autoGenerateListenerConfig);
            if (autoGenerateListenerConfig.isAuto)
            {
                Debug.Log("切换到自动生成");
            }
            else
            {
                Debug.Log("切换到手动生成");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        [MenuItem("DltFrame/生成框架 &F")]
        public static void Generate()
        {
            GameObject gameRootStart = GameObject.Find("GameRootStart");
            if (gameRootStart != null)
            {
                DestroyImmediate(gameRootStart);
            }

            gameRootStart = new GameObject("GameRootStart");
            GameRootStart tempGameRootStart = gameRootStart.AddComponent<GameRootStart>();
            //添加框架组件
            Undo.RegisterCreatedObjectUndo(gameRootStart, "UndoCreate");
            for (int i = 0; i < RuntimeGlobal.frameComponentType.Count; i++)
            {
                GameObject tempComponentObj = new GameObject(RuntimeGlobal.frameComponentType[i].Name);
                tempComponentObj.transform.SetParent(gameRootStart.transform);
                FrameComponent tempComponent = (FrameComponent)tempComponentObj.AddComponent(RuntimeGlobal.frameComponentType[i]);
                tempComponent.frameInitIndex = i;
            }
        }

#if !HybridCLR
        [MenuItem("DltFrame/开启热更功能")]
        public static void OpenHotFix()
        {
            AddMacro("HybridCLR");
        }
#endif

#if HybridCLR
        [MenuItem("DltFrame/关闭热更功能")]
        [Obsolete("Obsolete")]
        public static void CloseHotFix()
        {
            RemoveMacro("HybridCLR");
        }
#endif

#endif

#if HybridCLR
        [MenuItem("DltFrame/热更界面")]
        private static void OpenHotFixWindow()
        {
            GetWindow<HotFixMenu>().Show();
        }
#endif
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _audioComponentEditor.OnDisable();
            gameRootEditor.OnDisable();
            _frameImportComponent.OnDisable();
            AssetDatabase.SaveAssets();
        }

        //音频组件
        private AudioComponentEditor _audioComponentEditor = new AudioComponentEditor();

        //框架配置
        private static GameRootEditor gameRootEditor = new GameRootEditor();

        //资源统一化
        ResourceUnification resourceUnification = new ResourceUnification();

        //动画工具
        AnimTools animTools = new AnimTools();
        FrameImportComponent _frameImportComponent = new FrameImportComponent();

        //生成配置
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            _audioComponentEditor.OnInit();
            resourceUnification.OnInit();
            _frameImportComponent.OnInit();
            gameRootEditor.OnInit();
            tree.Add("导出框架", gameRootEditor);
            tree.Add("音频配置", _audioComponentEditor);
            tree.Add("资源统一化", resourceUnification);
            tree.Add("动画工具", animTools);
            tree.Add("框架组件", _frameImportComponent);
            return tree;
        }
    }
}