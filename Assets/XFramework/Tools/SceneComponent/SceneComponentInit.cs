using UnityEngine;
using XFramework;

public abstract class SceneComponentInit : MonoBehaviour, ISceneComponent
{
    public void StartComponent()
    {
    }

    public abstract void InitComponent();
}