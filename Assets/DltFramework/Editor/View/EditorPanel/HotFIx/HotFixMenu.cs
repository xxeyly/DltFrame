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
        public SceneAssetBundleRepeatAssetManager SceneAssetBundleRepeatAssetManager = new SceneAssetBundleRepeatAssetManager();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            _sceneHotFixConfig.OnInit();
            _sceneHotFixConfig.OnLoadConfig();
            _sceneHotFixConfig.OnLoadConfig();
            _hotFixCollect.OnInit();
            _hotFixCollect.OnLoadConfig();
            tree.Add("热更打包", _hotFixCollect);
            tree.Add("场景配置", _sceneHotFixConfig);
            tree.Add("场景重复资源", SceneAssetBundleRepeatAssetManager);
            return tree;
        }


        private void Update()
        {
            _sceneHotFixConfig.Update();
            // _hotFixCollect.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
#endif