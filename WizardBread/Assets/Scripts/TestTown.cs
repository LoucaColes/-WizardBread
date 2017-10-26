using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTown : MonoBehaviour
{
    public List<Improvement> m_improvements = new List<Improvement>();

    public bool trigger = false;
	void Update ()
    {
        if(trigger)
        {
            trigger = false;
            GlobalEventBoard.Instance.AddRapidEvent(Events.Event.IMP_Accomadation, new UpgradeImprovementData(3));
            GlobalEventBoard.Instance.AddRapidEvent(Events.Event.IMP_Water);
        }
	}

    void LateUpdate()
    {
        bool complete = false;
        do
        {
            foreach (Improvement item in m_improvements)
            {
                complete = !item.LevelUp();
            }
        } while (!complete);
    }
}
