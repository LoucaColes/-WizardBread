using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Improvement
{
	void Start ()
    {
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.IMP_Water, Ev_WaterUpgraded);
        GlobalEventBoard.Instance.SubscribeToEvent(Events.Event.IMP_Accomadation, Ev_AccomadationUpgraded);
    }

    void OnDestroy()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Water, Ev_WaterUpgraded);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Accomadation, Ev_AccomadationUpgraded);
    }

    protected override void Unsub()
    {
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Water, Ev_WaterUpgraded);
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Accomadation, Ev_AccomadationUpgraded);
    }

    public void Ev_WaterUpgraded(object _data = null)
    {
        StageLevelUp();
        GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Water, Ev_WaterUpgraded);
    }

    public void Ev_AccomadationUpgraded(object _data = null)
    {
        if (_data != null)
        {
            UpgradeImprovementData data = _data as UpgradeImprovementData;
            if (data.Stage >= 2)
            {
                StageLevelUp();
                GlobalEventBoard.Instance.UnsubscribeToEvent(Events.Event.IMP_Accomadation, Ev_AccomadationUpgraded);
            }
        }
    }

    public override bool LevelUp()
    {
        GlobalEventBoard.Instance.AddRapidEvent(Events.Event.IMP_Food, new UpgradeImprovementData(m_stage));
        return base.LevelUp();
    }

    protected override void StageLevelUp()
    {
        base.StageLevelUp();
    }
}
