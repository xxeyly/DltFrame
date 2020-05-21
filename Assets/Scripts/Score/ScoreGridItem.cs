using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.View;

public class ScoreGridItem : LocalBaseWindow
{
    private Text _topic;
    private Text _score;
    private Button _event;
    public float grade;

    [SerializeField] private int scoreIndex;
    [SerializeField] private string scoreContent;

    protected override void InitView()
    {
        BindUi(ref _score, "Score");
        BindUi(ref _topic, "Topic");
        _event = GetComponent<Button>();
    }

    protected override void InitListener()
    {
        BindListener(_event, EventTriggerType.PointerClick, OnEvent);
    }

    private void OnEvent(BaseEventData arg0)
    {
        ScoreDetail.Instance.ImplementScoreEvent(scoreIndex);
    }

    protected override void InitData()
    {
        _topic.text = scoreContent;
        _score.text = grade.ToString();
    }

    /// <summary>
    /// 更新成绩
    /// </summary>
    public void UpdateScore(float score)
    {
        _score.text = score.ToString();
    }
}