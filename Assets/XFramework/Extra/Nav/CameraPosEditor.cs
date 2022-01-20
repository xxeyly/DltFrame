using Sirenix.OdinInspector;
using UnityEditor;
using XFramework;


public class CameraPosEditor : Singleton<CameraPosEditor>
{
    [LabelText("相机位置配置文件")][InlineEditor()] public CameraPos cameraPos;
    [LabelText("保存名称")] public string cameraPosName;

    [Button("保存相机位置")]
    public void OnSave() 
    {
        CameraPos.CameraPosInfo cameraPosInfo = cameraPos.GetCameraPosInfoByName(cameraPosName);
        if (cameraPosInfo == null)
        {
            cameraPosInfo = new CameraPos.CameraPosInfo();
            cameraPos.cameraPosInfos.Add(cameraPosInfo);
        }

        cameraPosInfo.infoName = cameraPosName;
        cameraPosInfo.navPos = CameraControl.Instance.navMeshAgent.transform.position;
        cameraPosInfo.cameraRotate = CameraControl.Instance.currentCamera.transform.localEulerAngles;
        cameraPosInfo.cameraPos = CameraControl.Instance.currentCamera.transform.localPosition;
        cameraPosInfo.cameraFieldView = CameraControl.Instance.currentCamera.fieldOfView;
#if UNITY_EDITOR
        //标记脏区
        EditorUtility.SetDirty(cameraPos);
        // 保存所有修改
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}