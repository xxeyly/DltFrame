using UnityEngine;
using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.Tools.Svc.BaseSvc;

/// <summary>
/// 和unity交互的类
/// </summary>
/// 
/// <summary>
/// 解析init 
/// </summary>
[SerializeField]
public class PlayerData
{
    public string Labmark; //双方约定的实验标识

    public string Clazz; //班级

    //0805文档中删减
    //public string lid;
    public string Id; //本次操作记录的id 如果是练习就是练习的id,如果是自测就是自测的id，如果是考核就是考核的id(获取历史数据时使用 querySubjects)
    public string Mode; //打开模式 1作业 2考试 3练习
    public string Usetime; //实验操作限定用时,如果为0或为空表示不限时
    public string Pid; //本次操作记录的id 如果是练习就是练习的id,如果是自测就是自测的id，如果是考核就是考核的id
    public string Userid; //用户id
    public string Cnname; //姓名
    public string Loginname; //登录名
    public string Serverbaseurl; //平台的基础路径

    public string Sex; //性别

    //public string starttime;
    public string endtime; //实验限定时间，如果为空时不限时
}

/// <summary>
/// 存储数据大类
/// </summary>
[Serializable]
public class GeneralData
{
    public string pid; //本次操作记录的id 如果是练习就是练习的id,如果是自测就是自测的id，如果是考核就是考核的id
    public string zfs; //总分数，支持小数点前3位小数点后1位--------------------------------------------------------------需要面板填写
    public string jgfs; //及格分数，支持小数点前3位小数点后1位-----------------------------------------------------------需要面板填写
    public string creatTime; //考试开始时间
    public string updateTime; //最后更新时间，提交时获取当前时间即可
    public string useTime; //总用时
    public string score; //得分，支持小数点前3位小数点后1位
    public string status; //考试状态，默认0未完成,1完成，只有最后完成考试才提交该字段
    public string mode; //打开模式 1作业 2考试 3练习
    public List<InfoData> _infoData = new List<InfoData>();
}

/// <summary>
/// 存储数据小类
/// </summary>
[Serializable]
public class InfoData
{
    public string title; //题目或实验步骤名称-------------------------------------------------------------------------需要面板填写
    public string number; //题号或实验步骤，在本次实验或考试中不能重复------------------------------------------------需要面板填写
    public string type; //1.试题、2.操作步骤--------------------------------------------------------------------------需要面板填写
    public string op; //选项  "A:选项说明;B:选项说明"-----------------------------------------------------------------需要面板填写
    public string isSuccess; //结果是否正确,0不正确，1正确              非必填项 
    public string zqda; //题目的正确选项或正确操作步骤，多个直接用分号分隔  ”A:选项说明；B:选项说明….”-------------需要面板填写
    public string useTime; //本次操作的用时,整数？ = "0"             必填项 直接填0  数据上传时自动填0
    public string unit; //单位  ”秒”--------------------------------------------------------------------------------默认填秒
    public string opCount; //操作次数，整数？            非必填项     做对或做错都填1次或真实次数
    public string score; //本次操作的得分？             必填项
    public string otherData; //其他数据
    public string time; //提交时间
    public string fs; //题目或步骤的分数------------------------------------------------------------------------------需要面板填写
}

public class GreatIdeaSvc : SvcBase
{
    public static GreatIdeaSvc Instance;

    //数据总类
    public PlayerData pd = new PlayerData();

    //保存数据总类
    public GeneralData gd;


    //判定是否开启历史记录
    public bool isHistoryRunning;
    public int historyNum = 1;

    ///////////////////----------------面板编辑使用变量---------------------/////////////////////////////////////////////
    //是否显示服务器获取的信息
    public bool isShowPlayerData = false;

    //是否显示GeneralData不需要修改的信息
    public bool isShowGeneralData = false;

    //是否显示InfoData不需要修改的信息
    public bool isShowInfoData = false;

    //是否打开GeneralData下拉列表
    public bool isfoldout_GeneralData = true;

    //是否打开各步骤下拉列表
    public bool isfoldout_InfoData = true;

    /// <summary>
    /// 学习或者练习模式下进行初始化字符串的解析
    /// </summary>
    public void AnalyseLearnStateData(JsonData date)
    {
        pd.Labmark = date["labmark"].ToString();
        pd.Clazz = date["clazz"].ToString();
        //pd.lid= date["lid"].ToString();
        pd.Pid = date["pid"].ToString();
        gd.pid = date["pid"].ToString();
        pd.Id = date["id"].ToString();
        pd.Mode = date["mode"].ToString();
        pd.Usetime = date["usetime"].ToString();
        pd.Userid = date["userid"].ToString();
        pd.Cnname = date["cnname"].ToString();
        pd.Loginname = date["loginname"].ToString();
        pd.Serverbaseurl = date["serverbaseurl"].ToString();
        //pd.endtime = date["endtime"].ToString();

        gd.creatTime = GetTime();
    }

    /// <summary>
    /// 考核模式下进行初始化数据的解析
    /// </summary>
    public void AnalyseExamStateData(JsonData date)
    {
        pd.Labmark = date["labmark"].ToString();
        pd.Clazz = date["clazz"].ToString();
        //pd.lid = date["lid"].ToString();
        pd.Pid = date["pid"].ToString();
        gd.pid = date["pid"].ToString();
        pd.Id = date["id"].ToString();
        pd.Mode = date["mode"].ToString();
        pd.Usetime = date["usetime"].ToString();
        pd.Userid = date["userid"].ToString();
        pd.Cnname = date["cnname"].ToString();
        pd.Loginname = date["loginname"].ToString();
        pd.Serverbaseurl = date["serverbaseurl"].ToString();
        pd.endtime = date["endtime"].ToString();

        gd.creatTime = GetTime();
    }

    /// <summary>
    /// 历史记录下进行数据的解析
    /// </summary>
    public void AnalyseHistoryData(JsonData date)
    {
        pd.Labmark = date["labmark"].ToString();
        pd.Clazz = date["clazz"].ToString();
        //pd.lid = date["lid"].ToString();
        pd.Id = date["id"].ToString();
        pd.Mode = date["mode"].ToString();
        pd.Usetime = date["usetime"].ToString();
        pd.Pid = date["pid"].ToString();
        gd.pid = date["pid"].ToString();
        pd.Userid = date["userid"].ToString();
        pd.Cnname = date["cnname"].ToString();
        pd.Loginname = date["loginname"].ToString();
        pd.Serverbaseurl = date["serverbaseurl"].ToString();
        pd.endtime = date["endtime"].ToString();
    }

    /// <summary>
    /// 进行初始化试题信息结构的信息向平台传输
    /// </summary>
    public void GenerateALLQuestionData()
    {
        //{
        //  pid:”20183031222”,
        //  zfs:”150”,
        //  jgfs:”90.5”,
        //  list:[//本次实验或考试的所有步骤或题目信息
        //      {
        //          title:”病人防护”,
        //          number:”1”,
        //          type:”1”
        //      }
        //      ]
        //}
        JsonWriter writer2 = new JsonWriter();
        writer2.WriteObjectStart();
        writer2.WritePropertyName("pid");
        writer2.Write(gd.pid);
        writer2.WritePropertyName("zfs");
        writer2.Write(gd.zfs);
        writer2.WritePropertyName("jgfs");
        writer2.Write(gd.jgfs);
        writer2.WritePropertyName("list");
        int length = gd._infoData.Count;
        writer2.WriteArrayStart();
        for (int i = 0; i < length; i++)
        {
            writer2.WriteObjectStart();
            writer2.WritePropertyName("title");
            writer2.Write(gd._infoData[i].title);
            writer2.WritePropertyName("number");
            writer2.Write(gd._infoData[i].number);
            writer2.WritePropertyName("type");
            writer2.Write(gd._infoData[i].type);
            writer2.WritePropertyName("fs");
            writer2.Write(gd._infoData[i].fs);
            writer2.WriteObjectEnd();
        }

        writer2.WriteArrayEnd();
        writer2.WriteObjectEnd();
        // Debug.LogError(writer2);
        ServerManageSvc.Instance.SendInitSubjects(writer2.ToString()); //向服务器传数据
    }

    /// <summary>
    /// 获取历史记录模式下的回馈数据
    /// </summary>
    public void GetHistoryRecordData()
    {
        JsonWriter writer2 = new JsonWriter();
        writer2.WriteObjectStart();
        writer2.WritePropertyName("userid");
        writer2.Write(pd.Userid);
        writer2.WritePropertyName("labmark");
        writer2.Write(pd.Labmark);
        writer2.WritePropertyName("pid");
        //这是平台组相关数据要求
        writer2.Write("");
        writer2.WritePropertyName("mode");
        writer2.Write(pd.Mode);
        writer2.WritePropertyName("size");
        writer2.Write(historyNum);
        writer2.WriteObjectEnd();
        ServerManageSvc.Instance.GetHistoryData(writer2.ToString());
        isHistoryRunning = true;
        StartCoroutine(WaitReturn());
    }

    /// <summary>
    /// 获取历史记录下的协程等待函数
    /// </summary>
    IEnumerator WaitReturn()
    {
        while (isHistoryRunning)
        {
            if (ServerManageSvc.Instance.isGetHistoryInfo)
            {
                isHistoryRunning = false;
                ServerManageSvc.Instance.isGetHistoryInfo = false;
                AnalyseServerCallBackData(ServerManageSvc.Instance.HistoryJson);
            }

            yield return 0;
        }
    }

    /// <summary>
    /// 将回馈数据进行解析，并且填充到面板上
    /// </summary>
    void AnalyseServerCallBackData(string jsoninfo)
    {
        JsonData Data = JsonMapper.ToObject(jsoninfo);
        //解析数据是个测试活儿！
        gd.mode = Data["mode"].ToString();
        gd.pid = Data["id"].ToString();
        gd.useTime = Data["useTime"].ToString();
        // pd.score= Data["score"].ToString();
        gd.updateTime = Data["updateTime"].ToString();

        JsonData list = Data["list"].ToString();
        int callBackListCount = list.Count;
        gd._infoData = new List<InfoData>(callBackListCount);
        for (int i = 0; i < callBackListCount; i++)
        {
            gd._infoData[i].number = list[i]["number"].ToString();
            gd._infoData[i].useTime = list[i]["useTime"].ToString();
            gd._infoData[i].zqda = list[i]["zqda"].ToString();
            gd._infoData[i].time = list[i]["time"].ToString();
            gd._infoData[i].title = list[i]["title"].ToString();
            gd._infoData[i].op = list[i]["op"].ToString();
            gd._infoData[i].score = list[i]["score"].ToString();
            gd._infoData[i].opCount = list[i]["opCount"].ToString();
            gd._infoData[i].otherData = list[i]["otherData"].ToString();
        }
    }

    //回馈数据的接收判断
    public bool isHistoryDownOver()
    {
        return !isHistoryRunning;
    }

    ///////////////////////////////////////////////////////

    //保存选项
    public void SaveOp(string number, string op)
    {
        gd._infoData[int.Parse(number)].op = op;
    }

    //保存正误状态
    public void SaveSuccess(string number, string isSuccess)
    {
        gd._infoData[int.Parse(number)].isSuccess = isSuccess;
        //Debug.LogError (number+"--"+isSuccess);
    }

    //保存所用时间
    public void SaveUseTime(string number, string useTime)
    {
        gd._infoData[int.Parse(number)].useTime = useTime;
    }

    //保存次数
    public void SaveOpCount(string number, string opCount)
    {
        gd._infoData[int.Parse(number)].opCount = opCount;
    }

    //保存分数
    public void SaveScore(string number, string score)
    {
        gd._infoData[int.Parse(number)].score = score;
        //Debug.LogError (number+"--"+score);
    }

    //保存总分
    public void SaveTotalScore(string score)
    {
        Debug.Log("总分:" + score);
        gd.score = score;
        //Debug.LogError (number+"--"+score);
    }

    //保存其它的补充数据
    public void SaveOtherData(string number, string otherData)
    {
        gd._infoData[int.Parse(number)].otherData = otherData;
    }

    //提交一道题的数据
    public void SubmitData(string number)
    {
        JsonWriter writer2 = new JsonWriter();
        writer2.WriteObjectStart();

        writer2.WritePropertyName("pid");
        writer2.Write(gd.pid);

        writer2.WritePropertyName("createTime");
        writer2.Write(gd.creatTime);

        writer2.WritePropertyName("updateTime");
        gd.updateTime = GetTime();
        writer2.Write(gd.updateTime);

        writer2.WritePropertyName("useTime");
        writer2.Write(gd.useTime);

        writer2.WritePropertyName("score");
        writer2.Write(gd.score);

        writer2.WritePropertyName("status");
        gd.status = "0";
        writer2.Write(gd.status);

        writer2.WritePropertyName("list");

        writer2.WriteArrayStart();
        writer2.WriteObjectStart();
        int dex = int.Parse(number);

        writer2.WritePropertyName("number");
        writer2.Write(gd._infoData[dex].number);

        writer2.WritePropertyName("op");
        writer2.Write(gd._infoData[dex].op);

        writer2.WritePropertyName("isSuccess");
        writer2.Write(gd._infoData[dex].isSuccess);

        writer2.WritePropertyName("zqda");
        writer2.Write(gd._infoData[dex].zqda);

        writer2.WritePropertyName("useTime");
        if (gd._infoData[dex].useTime == "")
            gd._infoData[dex].useTime = "0";
        writer2.Write(gd._infoData[dex].useTime);

        writer2.WritePropertyName("opCount");
        writer2.Write(gd._infoData[dex].opCount);

        writer2.WritePropertyName("unit");
        writer2.Write(gd._infoData[dex].unit);

        writer2.WritePropertyName("score");
        writer2.Write(gd._infoData[dex].score);

        writer2.WritePropertyName("otherData");
        writer2.Write(gd._infoData[dex].otherData);

        writer2.WriteObjectEnd();
        writer2.WriteArrayEnd();

        writer2.WriteObjectEnd();
        //Debug.LogError (writer2);
        SaveOneData(writer2.ToString());
    }

    void SaveOneData(string jsoninfo)
    {
        ServerManageSvc.Instance.SendSaveSubject(jsoninfo);
    }


    /// <summary>
    /// 提交全部数据（项目数据最终提交）
    /// </summary>
    public void SaveMoreData()
    {
        JsonWriter writer2 = new JsonWriter();

        writer2.WriteObjectStart();

        writer2.WritePropertyName("pid");
        writer2.Write(gd.pid);

        writer2.WritePropertyName("createTime");
        writer2.Write(gd.creatTime);

        writer2.WritePropertyName("updateTime");
        gd.updateTime = GetTime();
        writer2.Write(gd.updateTime);

        writer2.WritePropertyName("useTime");
        writer2.Write(gd.useTime);

        writer2.WritePropertyName("score");
        writer2.Write(gd.score);

        writer2.WritePropertyName("status");
        gd.status = "1";
        writer2.Write(gd.status);

        writer2.WritePropertyName("list");
        writer2.WriteArrayStart();

        int length = gd._infoData.Count;
        for (int i = 0; i < length; i++)
        {
            writer2.WriteObjectStart();
            writer2.WritePropertyName("number");
            writer2.Write(gd._infoData[i].number);
            writer2.WritePropertyName("op");
            writer2.Write(gd._infoData[i].op);
            writer2.WritePropertyName("isSuccess");
            writer2.Write(gd._infoData[i].isSuccess);
            writer2.WritePropertyName("zqda");
            writer2.Write(gd._infoData[i].zqda);
            writer2.WritePropertyName("useTime");
            if (gd._infoData[i].useTime == "")
                gd._infoData[i].useTime = "0";
            writer2.Write(gd._infoData[i].useTime);
            writer2.WritePropertyName("opCount");
            writer2.Write(gd._infoData[i].opCount);
            writer2.WritePropertyName("unit");
            writer2.Write(gd._infoData[i].unit);
            writer2.WritePropertyName("score");
            writer2.Write(gd._infoData[i].score);
            writer2.WritePropertyName("otherData");
            writer2.Write(gd._infoData[i].otherData);
            writer2.WriteObjectEnd();
        }

        writer2.WriteArrayEnd();
        writer2.WriteObjectEnd();

        //Debug.Log(writer2);
        ServerManageSvc.Instance.SendSaveSubject(writer2.ToString()); //向服务器传数据
    }

    string GetTime()
    {
        string timestr = "";
        DateTime NowTime = DateTime.Now;
        timestr = NowTime.ToString("yyyy-MM-dd HH:mm:ss");
        return timestr;
    }

    public override void StartSvc()
    {
        Instance = GetComponent<GreatIdeaSvc>();
    }

    public override void InitSvc()
    {
    }
}