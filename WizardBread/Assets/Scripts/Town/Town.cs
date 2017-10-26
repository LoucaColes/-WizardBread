using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public static Town m_instance;

    public int m_populationCap;
    public int m_startPopulation;
    private int m_population;

    private int m_townSelfEsteem;

    public int m_initEsteemMultiplier;
    private int m_esteemMultiplier;

    //private List<Improvements> m_improvements;

    public Transform[] m_improvementsPositions;

    // Use this for initialization
    private void Start()
    {
        CreateInstance();
        ResetTown();
    }

    //Functionality
    public void ResetTown()
    {
        m_population = m_startPopulation;
        m_esteemMultiplier = m_initEsteemMultiplier;
        CalculateTownEsteem();
        //ResetImprovementsList();
    }

    /*public void ResetImprovementsList()
    {
        if (m_improvements.Count > 0)
        {
            m_improvements.Clear();
        }
        else
        {
            m_improvements = new List<Improvements>();
        }
    }*/

    public void CalculateTownEsteem()
    {
        m_townSelfEsteem = m_population * m_esteemMultiplier;
    }

    public void CalculateTownEsteem(int _population, int _multipler)
    {
        m_townSelfEsteem = _population * _multipler;
    }

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

    /*public void AddNewImprovement(Improvements _newImprovement)
    {
        m_improvements.Add(_newImprovement);
    }*/

    //Getters
    public int GetPopulation()
    {
        return m_population;
    }

    public int GetTownSelfEsteem()
    {
        return m_townSelfEsteem;
    }

    public int GetEsteemMultiplier()
    {
        return m_esteemMultiplier;
    }

    /*public List<Improvements> GetImprovements()
    {
        return m_improvements;
    }*/

    public Transform GetImprovementPosition(int _index)
    {
        return m_improvementsPositions[_index];
    }

    //Change to use an enum
    //public Transform GetImprovementPosition(int _index)
    //{
    //    return m_improvementsPositions[_index];
    //}

    public Transform[] GetAllPositions()
    {
        return m_improvementsPositions;
    }

    //Setter
    public void SetPopulation(int _newPopulation)
    {
        m_population = _newPopulation;
    }

    public void SetPopulationCap(int _newCap)
    {
        m_populationCap = _newCap;
    }

    public void SetStartPopulation(int _newStartPopulation)
    {
        m_startPopulation = _newStartPopulation;
    }

    public void SetTownSelfEsteem(int _newEsteem)
    {
        m_townSelfEsteem = _newEsteem;
    }

    public void SetInitEsteemMultiplier(int _newMultiplier)
    {
        m_initEsteemMultiplier = _newMultiplier;
    }

    public void SetEsteemMultiplier(int _newMultiplier)
    {
        m_esteemMultiplier = _newMultiplier;
    }
}