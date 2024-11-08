using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    /// <summary>
    /// 实体热更
    /// </summary>
    public partial class EntityItem
    {
        [HideInInspector] public bool HotFixAssetPathConfigIsExist;
#if UNITY_EDITOR
        [GUIColor(0, 1, 0)]
        [HorizontalGroup("热更")]
        [HideIf("HotFixAssetPathConfigIsExist")]
        [Button("添加热更组件", ButtonSizes.Large)]
        public void AddHotFixAssetPathConfigComponent()
        {
            if (!GetComponent<HotFixAssetPathConfig>())
            {
                gameObject.AddComponent<HotFixAssetPathConfig>();
            }

            GetComponent<HotFixAssetPathConfig>().SetPathAndApplyPrefab();
        }

        [GUIColor(0, 1, 0)]
        [HorizontalGroup("热更")]
        [ShowIf("HotFixAssetPathConfigIsExist")]
        [Button("移除热更组件", ButtonSizes.Large)]
        public void RemoveHotFixAssetPathConfigComponent()
        {
            if (GetComponent<HotFixAssetPathConfig>())
            {
                DestroyImmediate(GetComponent<HotFixAssetPathConfig>());
            }
        }
#endif
    }
}