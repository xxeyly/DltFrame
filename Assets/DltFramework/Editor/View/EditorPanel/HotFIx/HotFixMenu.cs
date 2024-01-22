using System;
using System.IO;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

#if HybridCLR

namespace DltFramework
{
    public class HotFixMenu : OdinMenuEditorWindow
    {
        private HotFixCollect _hotFixCollect = new HotFixCollect();
        private SceneHotFixConfig _sceneHotFixConfig = new SceneHotFixConfig();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            CreateInitDirectory();
            _sceneHotFixConfig.OnInit();
            _sceneHotFixConfig.OnLoadConfig();
            _hotFixCollect.OnLoadConfig();
            tree.Add("集合", _hotFixCollect);
            tree.Add("场景资源", _sceneHotFixConfig);

            return tree;
        }


        private void Update()
        {
            _sceneHotFixConfig.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _hotFixCollect.OnSaveConfig();
        }

        private void CreateInitDirectory()
        {
            if (!Directory.Exists("StreamingAssets/HotFix/HotFixCode"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFix/HotFixCode");
            }

            if (!Directory.Exists("StreamingAssets/HotFix/HotFixCodeConfig"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFix/HotFixCodeConfig");
            }

            if (!Directory.Exists("StreamingAssets/HotFix/HotFixView"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFix/HotFixView");
            }

            if (!Directory.Exists("StreamingAssets/HotFix/HotFixViewConfig"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFix/HotFixViewConfig");
            }

            if (!Directory.Exists("StreamingAssets/HotFix/Metadata"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFix/Metadata");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/Assembly"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/Assembly");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/AssemblyConfig"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/AssemblyConfig");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/GameRootStartAssetBundle"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/GameRootStartAssetBundle");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/GameRootStartAssetBundleConfig");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/HotFixAssetBundle"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/HotFixAssetBundle");
            }

            if (!Directory.Exists("StreamingAssets/HotFixRuntime/HotFixAssetBundleConfig"))
            {
                Directory.CreateDirectory("StreamingAssets/HotFixRuntime/HotFixAssetBundleConfig");
            }
        }
    }
}
#endif