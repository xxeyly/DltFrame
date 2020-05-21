using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

public class ScoreDetail : SingletonBaseWindow<ScoreDetail>
{
    private Dictionary<int, UnityAction> _scoreAction;
    private List<ScoreGridItem> _scoreGridItemList;
    private Button _checkDetails;
    private Text _text;

    protected override void OnlyOnceInit()
    {
        base.OnlyOnceInit();
        _scoreAction = new Dictionary<int, UnityAction>()
        {
            {
                0, () => { ViewSvc.ShowView(); }
            },
        };
        for (int i = 0; i < _scoreGridItemList.Count; i++)
        {
            _scoreGridItemList[i].Init();
        }

        float grade = 0;
        //结果 正确错误
        // SaveSuccess();
        //次数 是否操作
        //SaveOpCount();
        //每题分数 
        //SaveScore
        //总分数 
        // SaveTotalScore()
        //每题的分数
        /*for (int i = 0; i < PersistentDataSvc.fraction.Count; i++)
        {
            GreatIdeaSvc.Instance.SaveScore(i.ToString(), PersistentDataSvc.fraction[i].ToString());
            //总分数
            grade += PersistentDataSvc.fraction[i];
            GreatIdeaSvc.Instance.SaveSuccess(i.ToString(), (PersistentDataSvc.fraction[i] == PersistentDataSvc.fractionFullMarks[i] ? "1" : "0"));
            _scoreGridItemList[i].UpdateScore(PersistentDataSvc.fraction[i]);
        }

        //操作次数
        for (int i = 0; i < PersistentDataSvc.operationRecord.Count; i++)
        {
            GreatIdeaSvc.Instance.SaveOpCount(i.ToString(), PersistentDataSvc.operationRecord[i].ToString());
        }

        _text.text = grade.ToString();
        GreatIdeaSvc.Instance.SaveTotalScore(grade.ToString());
        GreatIdeaSvc.Instance.SaveMoreData();*/
    }

    public override void First()
    {
        base.First();
        Init();
    }

    public override void Init()
    {
    }

    protected override void InitView()
    {
        BindUi(ref _scoreGridItemList, "ScoreList/ScoreGrid");
        BindUi(ref _checkDetails, "CheckDetails");
        BindUi(ref _text, "Score/Text");
    }

    protected override void InitListener()
    {
        BindListener(_checkDetails, EventTriggerType.PointerClick, OnCheckDetails);
    }

    public void ImplementScoreEvent(int scoreIndex)
    {
        HideView();
        _scoreAction[scoreIndex].Invoke();
    }

    private void OnCheckDetails(BaseEventData targetObj)
    {
        HideView();
        _scoreAction[0].Invoke();
    }
}