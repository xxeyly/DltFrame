using Sirenix.OdinInspector;

namespace XFramework
{
    public partial class BaseWindow
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

            GetComponent<HotFixAssetPathConfig>().SetPath();
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