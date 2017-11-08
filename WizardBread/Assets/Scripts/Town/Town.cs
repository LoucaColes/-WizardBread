using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public enum ImprovementTags
    {
        FOOD,
        ACCOMADATION,
        WATER,
        ENERGY,
        TRANSPORT,
        EDUCATION,
        COMMERCE,
        SERVICES,
        RECREATION,
        TOURIST,
        Count
    }

    public enum State
    {
        Waiting,
        Simulating, 
        Pause
    }

    public static Town m_instance;

    public bool m_complete = false;

    public int m_townSelfEsteem;

    public int m_population;
    private int m_populationLevel = 1;
    public List<int> m_populationLevels = new List<int>();

    public int m_esteemCoefficient;
    private int m_esteemLevel = 1;
    public List<int> m_esteemLevels = new List<int>();

    public State m_state = State.Waiting;

    // Use this for initialization
    private void Start()
    {
        CreateInstance();
        ResetTown();
    }

    //Functionality
    private void CreateInstance()
    {
        if (!m_instance)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetTown()
    {
        m_complete = false;

        m_buttonsActivated.Clear();
        for(int iter = 0; iter <= m_numButtons - 1; iter++)
        {
            m_buttonsActivated.Add(false);
        }
        m_order.Clear();

        m_population = m_populationLevels[0];
        m_esteemCoefficient = m_esteemLevels[0];
        m_townSelfEsteem = m_population * m_esteemCoefficient;
        foreach (Improvement improv in m_activeImprovements)
        {
            improv.Clear();
        }
        m_activeImprovements.Clear();
    }

    //Improvement Handling
    public List<ImprovementTags> m_order = new List<ImprovementTags>();
    public List<Improvement> m_allImprovements = new List<Improvement>();

    private List<Improvement> m_activeImprovements = new List<Improvement>();
    private List<ChangePackage> m_changeBuffer = new List<ChangePackage>();
    public List<ImprovementTags> m_updateBuffer = new List<ImprovementTags>();

    public bool m_start = false;
    public bool m_reset = false;
    public List<ImprovementTags> m_debugOrder = new List<ImprovementTags>();

    public List<bool> m_buttonsActivated = new List<bool>();
    public int m_numButtons = 10;

    public float m_wait = 1.0f;
    private float m_currentWait = 1.0f;

    void Update()
    {
        if (m_state == State.Waiting)
        {
            if (m_start)
            {
                m_start = false;
                ResetTown();
                BeginSimulation();
            }

            if (m_reset)
            {
                m_reset = false;
                ResetTown();
            }

            if (!m_complete)
            {
                //Add Input
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ButtonPress(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ButtonPress(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ButtonPress(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    ButtonPress(3);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    ButtonPress(4);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    ButtonPress(5);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    ButtonPress(6);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    ButtonPress(7);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    ButtonPress(8);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    ButtonPress(9);
                }
            }
        }
        else if(m_state == State.Simulating)
        {
            UpdateImprovement();

            if(m_updateBuffer.Count == 0)
            {
                m_state = State.Waiting;
                m_complete = TownComplete();
            }
            else
            {
                m_state = State.Pause;
                m_currentWait = m_wait;
            }
        }
        else if(m_state == State.Pause)
        {
            m_currentWait -= Time.deltaTime;
            if(m_currentWait <= 0.0f)
            {
                m_state = State.Simulating;
            }
        }
    }

    private void ButtonPress(int _index)
    {
        if (_index < m_numButtons)
        {
            if (!m_buttonsActivated[_index])
            {
                m_buttonsActivated[_index] = true;
                BeginRound((ImprovementTags)_index);
            }
        }
    }

    public void InputIn(int _id, Improvement _improv)
    {
        switch (_id)
        {
            case 0:
                {
                    m_buttonsActivated[0] = true;
                    m_allImprovements[(int)ImprovementTags.WATER] = _improv;
                    BeginRound(ImprovementTags.WATER);
                    break;
                }
            case 1:
                {
                    m_buttonsActivated[1] = true;
                    m_allImprovements[(int)ImprovementTags.ENERGY] = _improv;
                    BeginRound(ImprovementTags.ENERGY);
                    break;
                }
            case 2:
                {
                    m_buttonsActivated[2] = true;
                    m_allImprovements[(int)ImprovementTags.ACCOMADATION] = _improv;
                    BeginRound(ImprovementTags.ACCOMADATION);
                    break;
                }
            case 3:
                {
                    m_buttonsActivated[3] = true;
                    m_allImprovements[(int)ImprovementTags.RECREATION] = _improv;
                    BeginRound(ImprovementTags.RECREATION);
                    break;
                }
            case 4:
                {
                    m_buttonsActivated[4] = true;
                    m_allImprovements[(int)ImprovementTags.TRANSPORT] = _improv;
                    BeginRound(ImprovementTags.TRANSPORT);
                    break;
                }
            case 5:
                {
                    m_buttonsActivated[5] = true;
                    m_allImprovements[(int)ImprovementTags.EDUCATION] = _improv;
                    BeginRound(ImprovementTags.EDUCATION);
                    break;
                }
            case 6:
                {
                    m_buttonsActivated[6] = true;
                    m_allImprovements[(int)ImprovementTags.FOOD] = _improv;
                    BeginRound(ImprovementTags.FOOD);
                    break;
                }
            case 7:
                {
                    m_buttonsActivated[7] = true;
                    m_allImprovements[(int)ImprovementTags.SERVICES] = _improv;
                    BeginRound(ImprovementTags.SERVICES);
                    break;
                }
            case 8:
                {
                    m_buttonsActivated[8] = true;
                    m_allImprovements[(int)ImprovementTags.COMMERCE] = _improv;
                    BeginRound(ImprovementTags.COMMERCE);
                    break;
                }
            default:
                {                    
                    break;
                }
        }
    }

    private void BeginRound(ImprovementTags _tag)
    {
        m_order.Add(_tag);
        AddImprovement(_tag);

        while (m_changeBuffer.Count != 0)
        {
            BufferHandler();
        }

        m_state = State.Pause;
        m_currentWait = m_wait;
    }

    private void UpdateImprovement()
    {
        if (m_updateBuffer.Count != 0)
        {
            m_allImprovements[(int)m_updateBuffer[0]].Upgrade();
            m_updateBuffer.RemoveAt(0);
        }
    }

    private void UpdateImprovements()
    {
        foreach (ImprovementTags item in m_updateBuffer)
        {
            m_allImprovements[(int)item].Upgrade();
        }

        m_updateBuffer.Clear();
    }

    private bool TownComplete()
    {
        foreach(bool flag in m_buttonsActivated)
        {
            if(!flag)
            {
                return false;
            }
        }
        return true;
    }

    private void BeginSimulation()
    {
        if (!m_complete)
        {
            m_activeImprovements.Clear();

            foreach (ImprovementTags tag in m_debugOrder)
            {
                AddImprovement(tag);

                while (m_changeBuffer.Count != 0)
                {
                    BufferHandler();
                    UpdateImprovements();
                }
            }
            m_complete = true;
        }
    }

    private void AddImprovement(ImprovementTags _tag)
    {
        ChangePackage temp = new ChangePackage(_tag, 1, m_allImprovements[(int)_tag].m_statChanges[0].Population,
            m_allImprovements[(int)_tag].m_statChanges[0].Esteem);
        AddPopulation(temp.Population);
        AddEsteem(temp.Esteem);
        m_changeBuffer.Add(temp);
        BufferHandler();

        m_activeImprovements.Add(m_allImprovements[(int)_tag]);
        m_activeImprovements[m_activeImprovements.Count - 1].Initialise();

        OrderUI.m_instance.AddRender(_tag);
    }

    private void BufferHandler()
    {
        List<ChangePackage> tempBuffer = new List<ChangePackage>();
        foreach (ChangePackage package in m_changeBuffer)
        {
            foreach (Improvement improv in m_activeImprovements)
            {
                ChangePackage temp = improv.Resolve(package);
                if (temp != null)
                {
                    m_updateBuffer.Add(temp.Tag);
                    AddPopulation(temp.Population);
                    AddEsteem(temp.Esteem);
                    tempBuffer.Add(temp);
                }
            }
        }

        m_changeBuffer.Clear();

        if (tempBuffer.Count != 0)
        {
            m_changeBuffer.AddRange(tempBuffer);
        }
    }

    private void AddPopulation(int _newPop)
    {
        int newPop = m_population + _newPop;
        if (newPop <= m_populationLevels[m_populationLevels.Count - 1])
        {
            m_population = newPop;
            if(m_population >= m_populationLevels[m_populationLevel])
            {
                m_populationLevel++;
            }
        }
        else if(m_population != m_populationLevels[m_populationLevels.Count - 1])
        { 
            m_population = m_populationLevels[m_populationLevels.Count - 1];
            m_populationLevel = m_populationLevels.Count;
        }
    }

    private void AddEsteem(int _newCoefficient)
    {
        m_esteemCoefficient += _newCoefficient;
        m_townSelfEsteem = m_esteemCoefficient * m_population;
        if (m_esteemLevel != m_esteemLevels.Count)
        {
            if (m_townSelfEsteem >= m_esteemLevels[m_esteemLevel])
            {
                m_esteemLevel++;
            }
        }
    }
}