using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.ConfigData
{
#if UNITY_EDITOR

    public class BuildSceneAssetBundle
    {
        private BuildSceneAssetBundleData _buildSceneAssetBundleData;
        private SceneLoadData _sceneLoadData;

        private void LoadData()
        {
            _buildSceneAssetBundleData =
                AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleData>(General
                    .buildSceneAssetBundleDataPath);
            if (_buildSceneAssetBundleData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildSceneAssetBundleData>(),
                    General.buildSceneAssetBundleDataPath);
                _buildSceneAssetBundleData =
                    AssetDatabase.LoadAssetAtPath<BuildSceneAssetBundleData>(General
                        .buildSceneAssetBundleDataPath);
            }

            _sceneLoadData = AssetDatabase.LoadAssetAtPath<SceneLoadData>(General.sceneLoadPath);
        }

        public void StartBuildAssetBundle()
        {
            LoadData();
            foreach (SceneLoadData.SceneInfo sceneInfo in _sceneLoadData.sceneInfos)
            {
                if (sceneInfo.sceneLoadType != SceneLoadData.SceneLoadType.异步)
                {
                    continue;
                }

                bool isBuild = false;
                foreach (BuildSceneAssetBundleData.SceneAssetBundleInfo sceneAssetBundleInfo in
                    _buildSceneAssetBundleData.sceneAssetBundleInfos)
                {
                    //包含打包过的场景
                    if (sceneInfo.sceneAsset == sceneAssetBundleInfo.sceneAsset)
                    {
                        //两个文件的Md5不一样
                        if (ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(sceneInfo.sceneAsset)) ==
                            sceneAssetBundleInfo.Md5)
                        {
                            isBuild = true;
                        }
                        else
                        {
                            sceneAssetBundleInfo.Md5 =
                                ResSvc.FileOperation.GetMD5HashFromFile(
                                    AssetDatabase.GetAssetPath(sceneInfo.sceneAsset));
                            Debug.Log("Md5不同");
                        }

                        break;
                    }
                }


                bool isCon = false;
                foreach (BuildSceneAssetBundleData.SceneAssetBundleInfo sceneAssetBundleInfo in
                    _buildSceneAssetBundleData.sceneAssetBundleInfos)
                {
                    if (sceneAssetBundleInfo.sceneAsset == sceneInfo.sceneAsset)
                    {
                        isCon = true;
                        break;
                    }
                }

                if (!isCon)
                {
                    Debug.Log("增加新场景");
                    _buildSceneAssetBundleData.sceneAssetBundleInfos.Add(
                        new BuildSceneAssetBundleData.SceneAssetBundleInfo()
                        {
                            sceneAsset = sceneInfo.sceneAsset,
                            Md5 =
                                ResSvc.FileOperation.GetMD5HashFromFile(
                                    AssetDatabase.GetAssetPath(sceneInfo.sceneAsset))
                        });
                }

                //标记脏区
                EditorUtility.SetDirty(_buildSceneAssetBundleData);
                // 保存所有修改
                AssetDatabase.SaveAssets();

                //已经打包过,不参与打包
                if (!isBuild)
                {
                    // Create the array of bundle build details.
                    AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

                    buildMap[0].assetBundleName = sceneInfo.sceneAsset.name;

                    string[] enemyAssets = new string[1];
                    enemyAssets[0] = AssetDatabase.GetAssetPath(sceneInfo.sceneAsset);
                    buildMap[0].assetNames = enemyAssets;
                    BuildPipeline.BuildAssetBundles(_sceneLoadData.sceneAssetBundlePath, buildMap,
                        BuildAssetBundleOptions.None,
                        BuildTarget.WebGL);
                }
            }
        }
    }
#endif
}