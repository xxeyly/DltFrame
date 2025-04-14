using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if HybridCLR
using HybridCLR;
#endif
using UnityEditor;
using UnityEngine;

namespace HotFix
{
    public class HotFixOver
    {
        public static void Over()
        {
            DeleteCacheFile();
            HotFixNetworking.networkStatusDetection = false;
            HotFixDebug.Log("开始加载原数据");
            LoadMetadataForAOTAssemblies();
            HotFixDebug.Log("开始加载Assembly-CSharp");
            LoadAssemblyCSharp();
            HotFixDebug.Log("开始加载OtherAssembly-CSharp");
            LoadOtherAssemblyCSharp();
            HotFixDebug.Log("开始加载游戏");
            LoadGameRootStart();
        }

        /// <summary>
        /// 删除缓存文件
        /// </summary>
        private static void DeleteCacheFile()
        {
            Debug.Log("删除缓存文件");
            List<string> cacheFilePath = HotFixGlobal.Path_GetSpecifyTypeOnlyInAssets("Cache");
            for (int i = 0; i < cacheFilePath.Count; i++)
            {
                File.Delete(cacheFilePath[i]);
            }
        }

        /// <summary>
        /// 加载元数据
        /// </summary>
        private static void LoadMetadataForAOTAssemblies()
        {
            //如果元文件夹不存在,创建
            if (!Directory.Exists(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/Metadata/"))
            {
                Directory.CreateDirectory(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/Metadata/");
            }

            //获得元数据配置表
            List<HotFixRuntimeDownConfig> metadataHotFixRuntimeDownConfigTable =
                JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/MetadataConfig", "MetadataConfig.json"));
            //元数据列表管理
            List<string> metadataHotFixRuntimeDownConfigTableList = new List<string>();
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in metadataHotFixRuntimeDownConfigTable)
            {
                metadataHotFixRuntimeDownConfigTableList.Add(hotFixRuntimeDownConfig.name);
            }

            //加载元数据
            foreach (string metadata in metadataHotFixRuntimeDownConfigTableList)
            {
                byte[] dllBytes = File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/{"HotFixRuntime/Metadata/" + metadata}");
#if HybridCLR
                LoadImageErrorCode err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
                HotFixDebug.Log($"LoadMetadataForAOTAssembly:{metadata}. ret:{err}");
#endif
            }
        }

        //加载Assembly-CSharp数据
        private static void LoadAssemblyCSharp()
        {
#if !UNITY_EDITOR
        Assembly.Load(File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/HotFixRuntime/Assembly/Assembly-CSharp.dll.bytes"));
#else

#endif
        }

        //加载OtherAssembly-CSharp数据
        private static void LoadOtherAssemblyCSharp()
        {
#if !UNITY_EDITOR
            //获得OtherAssembly
            List<HotFixRuntimeDownConfig> otherAssemblyHotFixRuntimeDownConfigTable =
                JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFixRuntime/OtherAssemblyConfig", "OtherAssemblyConfig.json"));
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in otherAssemblyHotFixRuntimeDownConfigTable)
            {
                Assembly.Load(File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/HotFixRuntime/OtherAssembly/" + hotFixRuntimeDownConfig.name));
            }
#else

#endif
        }

        //LoadGameRootStart
        private static void LoadGameRootStart()
        {
            GameObject gameRootStart = null;
#if UNITY_EDITOR
            gameRootStart = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/GameRootStart.prefab");
#else
            gameRootStart = AssetBundle.LoadFromFile(HotFixGlobal.GetDeviceStoragePath() + "/" + "HotFixRuntime/GameRootStartAssetBundle/gamerootstart").LoadAsset<GameObject>("GameRootStart");
#endif
            Object.Instantiate(gameRootStart);
        }
    }
}