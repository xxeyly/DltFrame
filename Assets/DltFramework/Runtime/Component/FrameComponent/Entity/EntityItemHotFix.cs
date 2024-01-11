using Sirenix.OdinInspector;

namespace DltFramework
{
    public partial class EntityItem
    {
#if UNITY_EDITOR
        [GUIColor(0, 1, 0)]
        [HorizontalGroup("热更")]
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