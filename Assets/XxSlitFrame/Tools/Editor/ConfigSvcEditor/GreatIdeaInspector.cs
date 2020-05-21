using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(GreatIdeaSvc))]
public class GreatIdeaInspector : Editor
{
    public GreatIdeaSvc greatidea;

    public override void OnInspectorGUI()
    {
        greatidea = (GreatIdeaSvc)target;

        MainEditor();
        PlayerDataEditor();
        GeneralDataEditor();
        InfoDataEditor();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    /// <summary>
    /// 主要变量绘制
    /// </summary>
    void MainEditor()
    {
        //greatidea.servermanage = EditorGUILayout.ObjectField("ServerManage（与服务器交互）", greatidea.servermanage, typeof(ServerManage), true) as ServerManage;

        greatidea.isHistoryRunning = EditorGUILayout.Toggle("当前是否是历史记录", greatidea.isHistoryRunning);
    }

    /// <summary>
    /// 服务器获取信息展示
    /// </summary>
    void PlayerDataEditor() {
        greatidea.isShowPlayerData = EditorGUILayout.Toggle("是否显示服务器获取的信息", greatidea.isShowPlayerData);

        if (greatidea.isShowPlayerData)
        {
            EditorGUILayout.LabelField("实验标识-labmark:", greatidea.pd.Labmark);
            EditorGUILayout.LabelField("班级-clazz:", greatidea.pd.Clazz);
            EditorGUILayout.LabelField("本次操作的id(历史数据时使用):", greatidea.pd.Id);
            EditorGUILayout.LabelField("打开模式-mode:", greatidea.pd.Mode);
            EditorGUILayout.LabelField("限定用时-usetime:", greatidea.pd.Usetime);
            EditorGUILayout.LabelField("本次操作记录的id-pid:", greatidea.pd.Pid);
            EditorGUILayout.LabelField("用户id-userid:", greatidea.pd.Userid);
            EditorGUILayout.LabelField("姓名-cnname:", greatidea.pd.Cnname);
            EditorGUILayout.LabelField("登录名-loginname:", greatidea.pd.Loginname);
            EditorGUILayout.LabelField("平台基础路径-serverbaseurl:", greatidea.pd.Serverbaseurl);
            EditorGUILayout.LabelField("性别-sex:", greatidea.pd.Sex);
            EditorGUILayout.LabelField("限定时间-endtime:", greatidea.pd.endtime);
        }
    }
    void GeneralDataEditor() {
        greatidea.isfoldout_GeneralData = EditorGUILayout.Foldout(greatidea.isfoldout_GeneralData, "服务器数据编辑");

        if (greatidea.isfoldout_GeneralData)
        {
            greatidea.isShowGeneralData = EditorGUILayout.Toggle("是否显示所有信息", greatidea.isShowGeneralData);

            greatidea.gd.zfs = EditorGUILayout.TextField("总分数-zfs", greatidea.gd.zfs);
            greatidea.gd.jgfs = EditorGUILayout.TextField("及格分数-jgfs", greatidea.gd.jgfs);

            if (greatidea.isShowGeneralData){
                EditorGUILayout.LabelField("本次操作记录的id-pid:", greatidea.gd.pid);
                EditorGUILayout.LabelField("考试开始时间-creatTime:", greatidea.gd.creatTime);
                EditorGUILayout.LabelField("最后更新时间-updateTime:", greatidea.gd.updateTime);
                EditorGUILayout.LabelField("总用时-useTime:", greatidea.gd.useTime);
                EditorGUILayout.LabelField("总得分-score:", greatidea.gd.score);
                EditorGUILayout.LabelField("考试状态-status:", greatidea.gd.status);
                EditorGUILayout.LabelField("打开模式-mode:", greatidea.gd.mode);
            }


        }
    }

    void InfoDataEditor() {
        greatidea.isfoldout_InfoData = EditorGUILayout.Foldout(greatidea.isfoldout_InfoData, "各步骤数据编辑", EditorStyles.foldoutPreDrop);

        if (greatidea.isfoldout_InfoData)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("添加步骤", GUILayout.Width(60)))
            {
                InfoData date = new InfoData();

                //date.title = "";
                date.number = "" + greatidea.gd._infoData.Count;
                //date.type = "";
                //date.op = "";
                //date.isSuccess = "";
                //date.zqda = "";
                //date.useTime = "";
                date.unit = "秒";
                //date.opCount = "";
                //date.score = "";
                //date.otherData = "";
                //date.time = "";
                //date.fs = "";

                greatidea.gd._infoData.Add(date);
                return;
            }

            if(GUILayout.Button("刷新题号", GUILayout.Width(60))){
                for (int i = 0; i < greatidea.gd._infoData.Count; i++)
                {
                    greatidea.gd._infoData[i].number = "" + i;
                }
            }

            if (GUILayout.Button("刷新类型", GUILayout.Width(60)))
            {
                for (int i = 0; i < greatidea.gd._infoData.Count; i++)
                {
                    greatidea.gd._infoData[i].type = "1";
                }
            }

            if (GUILayout.Button("刷新答案", GUILayout.Width(60)))
            {
                for (int i = 0; i < greatidea.gd._infoData.Count; i++)
                {
                    greatidea.gd._infoData[i].zqda = greatidea.gd._infoData[i].title;
                }
            }

            if (greatidea.isShowInfoData)
            {
                if (GUILayout.Button("隐藏不填信息", GUILayout.Width(80)))
                {
                    greatidea.isShowInfoData = false;
                }
            }
            else {
                if (GUILayout.Button("显示不填信息", GUILayout.Width(80)))
                {
                    greatidea.isShowInfoData = true;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            InfoDataShowList();

            GUILayout.Space(20);

            InfoDataNoShowList();
        }
    }

    /// <summary>
    /// 需要面板修改的数据列表
    /// </summary>
    void InfoDataShowList()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(8);
        EditorGUILayout.LabelField("题号", GUILayout.Width(25));
        GUILayout.Space(25);
        EditorGUILayout.LabelField("名称", GUILayout.Width(25));
        GUILayout.Space(23);
        EditorGUILayout.LabelField("类型", GUILayout.Width(25));
        GUILayout.Space(25);
        EditorGUILayout.LabelField("选项", GUILayout.Width(25));
        GUILayout.Space(55);
        EditorGUILayout.LabelField("答案", GUILayout.Width(25));
        GUILayout.Space(25);
        EditorGUILayout.LabelField("单位", GUILayout.Width(25));
        //GUILayout.Space(0);
        EditorGUILayout.LabelField("总分", GUILayout.Width(25));
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        for (int i = 0; i < greatidea.gd._infoData.Count; i++)
        {
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            greatidea.gd._infoData[i].number = EditorGUILayout.TextField(greatidea.gd._infoData[i].number, GUILayout.Width(20));
            greatidea.gd._infoData[i].title = EditorGUILayout.TextField(greatidea.gd._infoData[i].title, GUILayout.Width(80));
            greatidea.gd._infoData[i].type = EditorGUILayout.TextField(greatidea.gd._infoData[i].type, GUILayout.Width(20));
            greatidea.gd._infoData[i].op = EditorGUILayout.TextField(greatidea.gd._infoData[i].op, GUILayout.Width(80));
            greatidea.gd._infoData[i].zqda = EditorGUILayout.TextField(greatidea.gd._infoData[i].zqda, GUILayout.Width(80));
            greatidea.gd._infoData[i].unit = EditorGUILayout.TextField(greatidea.gd._infoData[i].unit, GUILayout.Width(20));
            greatidea.gd._infoData[i].fs = EditorGUILayout.TextField(greatidea.gd._infoData[i].fs, GUILayout.Width(30));


            if (GUILayout.Button("插入", GUILayout.Width(35)))
            {
                InfoData date = new InfoData();

                date.unit = "秒";

                greatidea.gd._infoData.Insert(i, date);

                return;
            }

            GUI.enabled = i > 0;

            if (GUILayout.Button("上移", GUILayout.Width(35)))
            {
                InfoData d = greatidea.gd._infoData[i];
                greatidea.gd._infoData[i] = greatidea.gd._infoData[i - 1];
                greatidea.gd._infoData[i - 1] = d;
                return;
            }

            GUI.enabled = i < greatidea.gd._infoData.Count - 1;

            if (GUILayout.Button("下移", GUILayout.Width(35)))
            {
                InfoData d = greatidea.gd._infoData[i];
                greatidea.gd._infoData[i] = greatidea.gd._infoData[i + 1];
                greatidea.gd._infoData[i + 1] = d;
                return;
            }

            GUI.enabled = true;

            if (GUILayout.Button("删除", GUILayout.Width(35)))
            {
                greatidea.gd._infoData.RemoveAt(i);
                return;
            }

            GUILayout.EndHorizontal();
        }

    }

    /// <summary>
    /// 不需要面板修改的数据列表
    /// </summary>
    void InfoDataNoShowList() {

        GUI.color = Color.gray;
        if (greatidea.isShowInfoData)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.LabelField("题号", GUILayout.Width(25));
            GUILayout.Space(20);
            EditorGUILayout.LabelField("步骤名称", GUILayout.Width(50));
            GUILayout.Space(32);
            EditorGUILayout.LabelField("结果", GUILayout.Width(25));
            GUILayout.Space(15);
            EditorGUILayout.LabelField("用时", GUILayout.Width(25));
            GUILayout.Space(15);
            EditorGUILayout.LabelField("次数", GUILayout.Width(25));
            GUILayout.Space(5);
            EditorGUILayout.LabelField("实际得分", GUILayout.Width(50));
            GUILayout.Space(15);
            EditorGUILayout.LabelField("提交时间", GUILayout.Width(50));
            GUILayout.Space(50);
            EditorGUILayout.LabelField("其他数据", GUILayout.Width(50));
            GUILayout.EndHorizontal();

            //public string isSuccess;//结果是否正确,0不正确，1正确？
            //public string useTime;//本次操作的用时,整数？
            //public string opCount;//操作次数，整数？
            //public string score;//本次操作的得分？
            //public string otherData;//其他数据
            //public string time;//提交时间

            for (int i = 0; i < greatidea.gd._infoData.Count; i++)
            {
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.TextField(greatidea.gd._infoData[i].number, GUILayout.Width(20));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].title, GUILayout.Width(100));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].isSuccess, GUILayout.Width(40));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].useTime, GUILayout.Width(40));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].opCount, GUILayout.Width(40));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].score, GUILayout.Width(40));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].time, GUILayout.Width(100));
                EditorGUILayout.TextField(greatidea.gd._infoData[i].otherData, GUILayout.Width(100));

                GUILayout.EndHorizontal();
            }
        }

        GUI.color = Color.white;
    }
}
