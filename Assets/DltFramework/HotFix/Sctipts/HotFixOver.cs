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
            HotFixNetworking.networkStatusDetection = false;
            HotFixDebug.Log("开始加载原数据");
            LoadMetadataForAOTAssemblies();
            HotFixDebug.Log("开始加载Assembly-CSharp");
            LoadAssemblyCSharp();
            HotFixDebug.Log("开始加载游戏");
            LoadGameRootStart();
        }

        //加载原数据
        private static void LoadMetadataForAOTAssemblies()
        {
            if (!Directory.Exists(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/"))
            {
                Directory.CreateDirectory(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/Metadata/");
            }

            List<HotFixRuntimeDownConfig> metadataHotFixRuntimeDownConfigTable =
                JsonUtil.FromJson<List<HotFixRuntimeDownConfig>>(HotFixGlobal.GetTextToLoad(HotFixGlobal.GetDeviceStoragePath() + "/HotFix/MetadataConfig", "MetadataConfig.json"));

            List<string> metadataHotFixRuntimeDownConfigTableList = new List<string>();
            foreach (HotFixRuntimeDownConfig hotFixRuntimeDownConfig in metadataHotFixRuntimeDownConfigTable)
            {
                metadataHotFixRuntimeDownConfigTableList.Add(hotFixRuntimeDownConfig.name);
            }

            foreach (string metadata in metadataHotFixRuntimeDownConfigTableList)
            {
                byte[] dllBytes = File.ReadAllBytes($"{HotFixGlobal.GetDeviceStoragePath()}/{"HotFix/Metadata/" + metadata}");
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