using Sirenix.OdinInspector;

namespace XFramework
{
    public abstract partial class SceneComponentInit : SerializedMonoBehaviour, ISceneComponent
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

        public virtual void StartComponent()
        {
        }

        public abstract void InitComponent();
    }
}