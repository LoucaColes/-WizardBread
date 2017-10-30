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

    public static Town m_instance;

    public bool m_complete = false;

    public int m_townSelfEsteem;

    public int m_population;
    private int m_populationLevel = 1;
    public List<int> m_populationLevels = new List<int>();

    public int m_esteemCoefficient;
    private int m_esteemLevel = 1;
    public List<int> m_esteemLevels = new List<int>();

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
    private List<ImprovementTags> m_updateBuffer = new List<ImprovementTags>();

    public bool m_start = false;
    public bool m_reset = false;
    public List<ImprovementTags> m_debugOrder = new List<ImprovementTags>();

    public List<bool> m_buttonsActivated = new List<bool>();
    public int m_numButtons = 10;

    private bool m_upgrading = false;

    void Update()
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

        if(!m_complete && !m_upgrading)
        {
            //Add Input
            if(!m_buttonsActivated[0] && Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_buttonsActivated[0] = true;
                BeginRound(ImprovementTags.FOOD);
            }
            else if (!m_buttonsActivated[1] && Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_buttonsActivated[1] = true;
                BeginRound(ImprovementTags.ACCOMADATION);
            }
            else if (!m_buttonsActivated[2] && Input.GetKeyDown(KeyCode.Alpha3))
            {
                m_buttonsActivated[2] = true;
                BeginRound(ImprovementTags.WATER);
            }
            else if (!m_buttonsActivated[3] && Input.GetKeyDown(KeyCode.Alpha4))
            {
                m_buttonsActivated[3] = true;
                BeginRound(ImprovementTags.ENERGY);
            }
            else if (!m_buttonsActivated[4] && Input.GetKeyDown(KeyCode.Alpha5))
            {
                m_buttonsActivated[4] = true;
                BeginRound(ImprovementTags.TRANSPORT);
            }
            else if (!m_buttonsActivated[5] && Input.GetKeyDown(KeyCode.Alpha6))
            {
                m_buttonsActivated[5] = true;
                BeginRound(ImprovementTags.EDUCATION);
            }
            else if (!m_buttonsActivated[6] && Input.GetKeyDown(KeyCode.Alpha7))
            {
                m_buttonsActivated[6] = true;
                BeginRound(ImprovementTags.COMMERCE);
            }
            else if (!m_buttonsActivated[7] && Input.GetKeyDown(KeyCode.Alpha8))
            {
                m_buttonsActivated[7] = true;
                BeginRound(ImprovementTags.SERVICES);
            }
            else if (!m_buttonsActivated[8] && Input.GetKeyDown(KeyCode.Alpha9))
            {
                m_buttonsActivated[8] = true;
                BeginRound(ImprovementTags.RECREATION);
            }
        }
    }

    public void InputIn(int _id)
    {
        switch (_id)
        {
            case 0:
                {
                    m_buttonsActivated[0] = true;
                    BeginRound(ImprovementTags.WATER);
                    break;
                }
            case 1:
                {
                    m_buttonsActivated[1] = true;
                    BeginRound(ImprovementTags.ENERGY);
                    break;
                }
            case 2:
                {
                    m_buttonsActivated[2] = true;
                    BeginRound(ImprovementTags.ACCOMADATION);
                    break;
                }
            case 3:
                {
                    m_buttonsActivated[3] = true;
                    BeginRound(ImprovementTags.RECREATION);
                    break;
                }
            case 4:
                {
                    m_buttonsActivated[4] = true;
                    BeginRound(ImprovementTags.TRANSPORT);
                    break;
                }
            case 5:
                {
                    m_buttonsActivated[5] = true;
                    BeginRound(ImprovementTags.EDUCATION);
                    break;
                }
            case 6:
                {
                    m_buttonsActivated[6] = true;
                    BeginRound(ImprovementTags.FOOD);
                    break;
                }
            case 7:
                {
                    m_buttonsActivated[7] = true;
                    BeginRound(ImprovementTags.SERVICES);
                    break;
                }
            case 8:
                {
                    m_buttonsActivated[8] = true;
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
            UpdateImprovements();
        }

        m_complete = TownComplete();
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