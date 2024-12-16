using System;
using System.Collections.Generic;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class SceneComponentSelectTool : MonoBehaviour
{
    [ValueDropdown("GetViewListOfBaseWindow")] [ShowInInspector] [LabelText("SceneComponent列表")] [SerializeField]
    public string viewType;

    private IEnumerable<string> GetViewListOfBaseWindow()
    {
        Type[] allType = typeof(SceneComponent).Assembly.GetTypes();
        List<string> baseWindowList = new List<string>();

        List<SceneComponent> sceneComponentList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>();
        List<Type> sceneComponentTypes = new List<Type>();
        foreach (SceneComponent sceneComponent in sceneComponentList)
        {
            if (!sceneComponentTypes.Contains(sceneComponent.GetType()))
            {
                sceneComponentTypes.Add(sceneComponent.GetType());
            }
        }

        foreach (Type type in allType)
        {
            if (type.BaseType == typeof(SceneComponent) && type != typeof(SceneComponentTemplate) && !sceneComponentTypes.Contains(type))
            {
                baseWindowList.Add(type.Name);
            }
        }

        return baseWindowList;
    }

    [Button("SceneComponent增加", ButtonSizes.Large)]
    [GUIColor(0, 1, 0)]
    public void AddSceneComponentToScene()
    {
        if (viewType == null)
        {
            return;
        }

        GameObject sceneComponentGameObject = new GameObject();
        sceneComponentGameObject.transform.SetParent(transform);
        sceneComponentGameObject.name = viewType;
        sceneComponentGameObject.AddComponent(Type.GetType(viewType));
        viewType = null;
    }
}