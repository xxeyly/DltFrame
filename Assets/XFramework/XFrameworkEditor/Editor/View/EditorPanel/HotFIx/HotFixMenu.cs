using System;
using System.IO;
using Sirenix.OdinInspector.Editor;
#if HybridCLR

namespace XFramework
{
    public class HotFixMenu : OdinMenuEditorWindow
    {
        public HotFixViewEditor HotFixViewEditor;
        private SceneHotfixAssetManager SceneHotfixAssetManager = new SceneHotfixAssetManager();
        private AssetBundleManager _assetBundleManager = new AssetBundleManager();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            CreateInitDirectory();
            HotFixViewEditor = new HotFixViewEditor();
            HotFixViewEditor.OnLoadConfig();
            SceneHotfixAssetManager.OnLoadConfig();
            _assetBundleManager.OnLoadConfig();
            tree.Add("HotFix", HotFixViewEditor);
            tree.Add("场景热更资源", SceneHotfixAssetManager);
            tree.Add("资源打包", _assetBundleManager);
            return tree;
        }

        private void Update()
        {
            SceneHotfixAssetManager.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HotFixViewEditor.OnSaveConfig();
            SceneHotfixAssetManager.OnSaveConfig();
            _assetBundleManager.OnSaveConfig();
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