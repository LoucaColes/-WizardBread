using System;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public enum ImproveTags
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

    [Serializable]
    public class ImproveResult
    {
        public string Name = "";
        public int Level = 1;
        public int MaxLevel = 0;

        public ImproveResult(string _name, int _level, int _maxLevel)
        {
            Name = _name;
            Level = _level;
            MaxLevel = _maxLevel;
        }

        public float CalculateScore()
        {
            if(MaxLevel != 0)
            {
                return (Level / MaxLevel) * 100.0f;
            }
            Debug.LogWarning(Name + " Result's MaxLevel is not set.");
            return 0.0f;
        }
    }

    [Serializable]
    public class DataSet
    {
        public List<int> Order = new List<int>();
        public List<ImproveResult> Results = new List<ImproveResult>();
        public float Score = 0.0f;

        public float CalculateScore()
        {
            Score = 0.0f;
            foreach (ImproveResult item in Results)
            {
                Score += item.CalculateScore();
            }
            return Score;
        }
    }

    public bool m_start = false;
    public int m_iteration = 0;
    public List<ImproveTags> m_order = new List<ImproveTags>();
    public List<DataSet> m_results = new List<DataSet>();

    public List<ImprovementSim> m_improvementTemplates = new List<ImprovementSim>();

    private List<ImprovementSim> m_activeSims = new List<ImprovementSim>();
    private List<ChangePackage> m_changeBuffer = new List<ChangePackage>();

	void Update ()
    {
		if(m_start)
        {
            m_start = false;
            BeginSimulation();
        }
	}

    private void BeginSimulation()
    {
        FirstOrder();
        m_results.Clear();

       // do
        //{
            m_iteration++;
            m_activeSims.Clear();
            m_results.Add(new DataSet());

            foreach (ImproveTags tag in m_order)
            {
                AddImprovement(tag);
                m_results[m_results.Count - 1].Order.Add((int)tag);

                while(m_changeBuffer.Count != 0)
                {
                    BufferHandler();
                }
            }

            foreach(ImprovementSim item in m_activeSims)
            {
                m_results[m_results.Count - 1].Results.Add(new ImproveResult(item.m_tag.ToString(), item.m_level, item.m_maxLevel));
            }

            m_results[m_results.Count - 1].CalculateScore();
        //} while (!NextOrder());
    }

    private void FirstOrder()
    {
        m_order.Clear();
        for(int iter = 0; iter <= (int)ImproveTags.Count - 1; iter++)
        {
            m_order.Add((ImproveTags)iter);
        }
    }

    private bool NextOrder()
    {
        bool last = true;


        return last;
    }

    private void AddImprovement(ImproveTags _tag)
    {
        m_changeBuffer.Add(new ChangePackage(_tag, 1));
        BufferHandler();

        m_activeSims.Add(m_improvementTemplates[(int)_tag]);
    }

    private void BufferHandler()
    {
        List<ChangePackage> tempBuffer = new List<ChangePackage>();
        foreach (ChangePackage package in m_changeBuffer)
        {
            foreach (ImprovementSim improv in m_activeSims)
            {
                ChangePackage temp = improv.Resolve(package);
                if(temp != null)
                {
                    tempBuffer.Add(temp);
                }
            }
        }

        m_changeBuffer.Clear();

        if(tempBuffer.Count != 0)
        {
            m_changeBuffer.AddRange(tempBuffer);
        }
    }
}
