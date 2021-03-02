using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomBuild;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.GameRoot;
using XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc;

public class OdinMenu : OdinMenuEditorWindow
{
    [MenuItem("My Game/My Editor")]
    private static void OpenWindow()
    {
        GetWindow<OdinMenu>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        CustomScriptableObject customScriptableObject = new CustomScriptableObject();
        CustomAudioSvc customAudioSvc = new CustomAudioSvc(customScriptableObject);
        tree.Add("打包工具", new OdinCustomBuild(customScriptableObject));
        tree.Add("游戏根目录", new CustomGameRoot(customScriptableObject, customAudioSvc));
        tree.Add("音频配置", customAudioSvc);
        tree.Add("配置文件", customScriptableObject);
        return tree;
    }
}