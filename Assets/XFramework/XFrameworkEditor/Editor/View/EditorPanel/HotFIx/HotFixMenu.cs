using Sirenix.OdinInspector.Editor;

namespace XFramework
{
    public class HotFixMenu : OdinMenuEditorWindow
    {
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("HotFix", new HotFixViewEditor());
            return tree;
        }
    }
}