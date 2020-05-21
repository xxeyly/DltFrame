using System;
using LitJson;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    public class PlatformInteractionManagerSvc : SvcBase
    {
        public static PlatformInteractionManagerSvc Instance;

        public AssessmentData assessmentData;

        public override void StartSvc()
        {
            Instance = GetComponent<PlatformInteractionManagerSvc>();
        }

        public override void InitSvc()
        {
            SendInitDataToServer();
        }

        string GetTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void SendInitDataToServer()
        {
            assessmentData.updateTime = GetTime();
            assessmentData.status = "1";
            ServerManageSvc.Instance.SendSaveSubject(JsonMapper.ToJson(assessmentData)); //向服务器传数据
        }

        public void SaveMoreData()
        {
            assessmentData.updateTime = GetTime();
            assessmentData.status = "1";
            ServerManageSvc.Instance.SendSaveSubject(JsonMapper.ToJson(assessmentData)); //向服务器传数据
        }
    }
}