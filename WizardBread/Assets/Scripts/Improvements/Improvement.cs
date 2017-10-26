using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Improvement : MonoBehaviour
{
    public int m_stage = 1;
    public int m_maxStage = 3;

    public int m_remainingUpgrades = 0;

    public virtual bool LevelUp()
    {
        if (m_remainingUpgrades > 0)
        {
            m_stage++;
            m_remainingUpgrades--;
        }

        return !(m_remainingUpgrades == 0);
    }
    
    protected virtual void StageLevelUp()
    {
        if(m_stage + 1 < m_maxStage)
        {
            m_remainingUpgrades++;
        }
        else
        {
            Unsub();
        }
    }

    protected virtual void Unsub() { }

}
