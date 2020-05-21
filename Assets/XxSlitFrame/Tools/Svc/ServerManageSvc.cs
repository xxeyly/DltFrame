using System.Collections;
using System.Runtime.InteropServices;
using LitJson;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    public class ServerManageSvc : SvcBase
    {
        public static ServerManageSvc Instance;

        [Header("备用IP地址")] public string IpAddress;

        //Ip地址后缀，一般为固定路径
        private string InitSubjectsAddress = "api/initSubjects";
        private string SaveSubjectAddress = "api/saveSubject";
        private string QuerySubjectsAddress = "api/querySubjects";

        [Header("接收或测试用的json")] public string InitinfoJson;

        //是否使用本地测试json
        public bool isLocalTest;

        //--------------------------------------------------
        [HideInInspector]
        //是否与服务器连接
        public bool isConnectServer = false;

        [HideInInspector] public int InitSubjectsState = 0; //0:初始状态 1：上传成功 2：上传失败

        [HideInInspector] public bool isGetHistoryInfo; //查询历史记录是否返回
        [HideInInspector] public string HistoryJson; //历史记录数据

        //--------------------------------------------------
        //建立本地存储的相关机制进行流畅简易的判断执行
#pragma warning disable 414
        private string learnkey = "LearnMode";
        private string examKey = "ExamMode";
        private string recordKey = "RecordKey";
        private string learnContent = "学习启动";
        private string examContent = "考核启动";
        private string recordContent = "历史记录启动";
#pragma warning restore 414

        [HideInInspector] public string debugstr;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void loadComplete();
#endif
        //// Update is called once per frame
        void Update()
        {
            if (isLocalTest && Input.GetKeyDown(KeyCode.F9))
            {
                OpenType(InitinfoJson);
            }
        }

        #region 程序初始化获取数据信息

        /// <summary>
        /// 调取LoadComplete方法
        /// 通知html页面unity加载完成
        /// </summary>
        public void ToSendBaseMessage()
        {
            //Application.ExternalCall("loadComplete");
#if UNITY_WEBGL && !UNITY_EDITOR
        loadComplete();
        debugstr += "loadComplete is send\n";
#endif
        }

        /// <summary>
        /// 接收html函数的回调
        /// 返回实验信息
        /// </summary>
        /// <param name="info">html传回的json格式的数据</param>
        private void Init(string info)
        {
            InitinfoJson = info;
            isConnectServer = true;

            debugstr += "init is get \n";

            OpenType(InitinfoJson);
        }

        /// <summary>
        /// 解析打开模式
        /// </summary>
        public void OpenType(string jsoninfo)
        {
            JsonData Data = JsonMapper.ToObject(jsoninfo);
            JsonData jd = Data["data"];
            string mode = jd["mode"].ToString();
            IpAddress = jd["serverbaseurl"].ToString();

            switch (mode)
            {
                case "3": //学习模式，开启学习模式协程
                    GreatIdeaSvc.Instance.AnalyseLearnStateData(jd);
                    GreatIdeaSvc.Instance.GenerateALLQuestionData();
                    break;
                //考核模式
                case "2":
                    GreatIdeaSvc.Instance.AnalyseExamStateData(jd);
                    GreatIdeaSvc.Instance.GenerateALLQuestionData();
                    break;
                //練習模式
                case "1":
                case "5":
                    GreatIdeaSvc.Instance.AnalyseExamStateData(jd);
                    GreatIdeaSvc.Instance.GenerateALLQuestionData();
                    break;
                case "4": //历史记录模式
                    GreatIdeaSvc.Instance.AnalyseHistoryData(jd);
                    GreatIdeaSvc.Instance.GetHistoryRecordData();
                    break;
            }
        }

        #endregion

        #region 场景重置

        public void DestroyThisObj()
        {
            Destroy(this.gameObject);
        }

        #endregion

        #region 向服务器post数据

        /// <summary>
        /// 初始化所有题目或步骤
        /// </summary>
        /// <param name="jsoninfo">初始化json</param>
        public void SendInitSubjects(string jsoninfo)
        {
            if (isConnectServer)
            {
                StartCoroutine(PostInitSubjects(jsoninfo));
            }
        }

        /// <summary>
        /// 向服务器发送初始化消息
        /// </summary>
        /// <returns></returns>
        public IEnumerator PostInitSubjects(string jsoninfo)
        {
            int NoConnectTime = 1;
            string url = "http://" + IpAddress + InitSubjectsAddress;
            //string url = "http://192.168.1.5:8080/zf-yxpt/api/initSubjects";
            WWWForm forms = new WWWForm();
            forms.AddField("data", jsoninfo);

            debugstr += "initSubjects is post \n";

#pragma warning disable 618
            var save_wwwOneTime = new WWW(url, forms);
#pragma warning restore 618
            //inputAddress.value = url;
            yield return save_wwwOneTime;
            while (save_wwwOneTime.error != null && NoConnectTime < 5)
            {
                yield return save_wwwOneTime;
                Debug.Log("第" + NoConnectTime + "次未连接成功");
                NoConnectTime++;
            }

            if (save_wwwOneTime.error == null)
            {
                JsonData Data = JsonMapper.ToObject(save_wwwOneTime.text);
                string result = Data["result"].ToString();

                if (result == "true")
                {
                    InitSubjectsState = 1;
                    Debug.Log(Data["msg"].ToString());
                }
                else
                {
                    InitSubjectsState = 2;
                    isConnectServer = false;
                    Debug.Log(Data["msg"].ToString());
                }

                debugstr += "initSubjects is ok \n";
            }
            else
            {
                debugstr += "initSubjects error is " + save_wwwOneTime.error + " \n";
            }
        }

        /// <summary>
        /// 保存数据到服务器
        /// </summary>
        public void SendSaveSubject(string jsoninfo)
        {
            print("SendSaveSubject:" + jsoninfo);
            if (isConnectServer)
            {
                StartCoroutine(PostSaveSubject(jsoninfo));
            }
        }

        /// <summary>
        /// 向服务器发送要保存的结果
        /// </summary>
        /// <param name="jsoninfo"></param>
        /// <returns></returns>
        public IEnumerator PostSaveSubject(string jsoninfo)
        {
            int NoConnectTime = 1;
            string url = "http://" + IpAddress + SaveSubjectAddress;
            WWWForm forms = new WWWForm();
            forms.AddField("data", jsoninfo);

            debugstr += "saveSubject is post \n";

#pragma warning disable 618
            var save_wwwOneTime = new WWW(url, forms);
#pragma warning restore 618

            yield return save_wwwOneTime;
            Debug.Log(save_wwwOneTime.text + ":" + save_wwwOneTime.error);
            while (save_wwwOneTime.error != null && NoConnectTime < 5)
            {
                yield return save_wwwOneTime;
                Debug.Log("第" + NoConnectTime + "次未连接成功");
                NoConnectTime++;
            }

            if (save_wwwOneTime.error == null)
            {
                debugstr += "saveSubject is ok \n";
            }
            else
            {
                debugstr += "saveSubject error is " + save_wwwOneTime.error + " \n";
            }
        }

        #endregion

        #region 获取服务器数据

        public void GetHistoryData(string jsoninfo)
        {
            StartCoroutine(GetQuerySubjects(jsoninfo));
        }

        public IEnumerator GetQuerySubjects(string jsoninfo)
        {
            int NoConnectTime = 1;
            string url = "http://" + IpAddress + QuerySubjectsAddress;
            //string url = "http://192.168.1.5:8080/zf-yxpt/api/querySubjects";
            WWWForm forms = new WWWForm();
            forms.AddField("data", jsoninfo);

            debugstr += "querySubjects is post \n";

#pragma warning disable 618
            var save_wwwOneTime = new WWW(url, forms);
#pragma warning restore 618

            yield return save_wwwOneTime;

            while (save_wwwOneTime.error != null && NoConnectTime < 5)
            {
                yield return save_wwwOneTime;
                Debug.Log("第" + NoConnectTime + "次未连接成功");
                NoConnectTime++;
            }

            if (save_wwwOneTime.error == null)
            {
                isGetHistoryInfo = true;
                HistoryJson = save_wwwOneTime.text;

                debugstr += "querySubjects is ok \n";
            }
            else
            {
                Debug.Log("网络连接不稳");
                debugstr += "querySubjects error is " + save_wwwOneTime.error + " \n";
            }
        }

        #endregion

        public override void StartSvc()
        {
            Instance = GetComponent<ServerManageSvc>();
        }

        public override void InitSvc()
        {
            DontDestroyOnLoad(transform.gameObject);
            GameObject obj = GameObject.Find("Jia");
            if (obj == null)
            {
                this.gameObject.name = "Jia";
            }
            else
            {
                Destroy(this.gameObject);
            }


            GreatIdeaSvc.Instance = GetComponent<GreatIdeaSvc>();

            if (isLocalTest)
            {
                OpenType(InitinfoJson);
            }
            else
            {
                ToSendBaseMessage();
            }
        }
    }
}