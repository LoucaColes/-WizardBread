using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
                return ((float)Level / (float)MaxLevel) * 100.0f;
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
    public int m_totalPermutations = 1;
    public List<ImproveTags> m_order = new List<ImproveTags>();
    public List<DataSet> m_results = new List<DataSet>();

    public List<ImprovementSim> m_improvementTemplates = new List<ImprovementSim>();

    private List<ImprovementSim> m_activeSims = new List<ImprovementSim>();
    private List<ChangePackage> m_changeBuffer = new List<ChangePackage>();

    public bool m_save = false;
    public string m_name = "";
    private string m_filePath;

    void Update ()
    {
		if(m_start)
        {
            m_start = false;
            BeginSimulation();
        }

        if(m_save)
        {
            m_save = false;
            SaveData();
        }
	}

    private void BeginSimulation()
    {
        m_results.Clear();
        m_iteration = 0;
        CalculateTotalPermutations();

        while (!NextOrder())
        {
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
            CullResults();
            m_iteration++;
        }
        m_activeSims.Clear();
        Reorder(m_results[0].Order);
    }

    private void CalculateTotalPermutations()
    {
        int max = m_order.Count;
        m_totalPermutations = 1;

        for(int iter = 0; iter < m_order.Count; iter++)
        {
            m_totalPermutations *= (max - iter);
        }
    }

    private bool NextOrder()
    {
        bool last = true;

        List<int> next = NumberPermutation.LoadData(m_order.Count);
        if (m_iteration < m_totalPermutations)
        {
            last = false;
            Reorder(next);
        }

        return last;
    }

    private void Reorder(List<int> _newOrder)
    {
        m_order.Clear();
        foreach(int item in _newOrder)
        {
            m_order.Add((ImproveTags)item);
        }
    }

    private void CullResults()
    {
        if(m_results.Count > 100)
        {
            m_results.Sort((a, b) => a.Score.CompareTo(b.Score));
            m_results.RemoveRange(100, m_results.Count - 100);
            m_results.Reverse();
        }
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

    private void SaveData()
    {
        if (m_name != "")
        {
            m_filePath = "Assets/Data/Sim/Results/" + m_name + ".txt";

            int pathIter = 0;
            string name = m_name;
            while (File.Exists(m_filePath))
            {
                pathIter++;
                name = m_name + pathIter;
                m_filePath = "Assets/Data/Sim/Results/" + name + ".txt";
            }
            m_name = "";

            File.CreateText(m_filePath).Dispose();

            StreamWriter writer = new StreamWriter(m_filePath, true);

            writer.WriteLine("Name: " + name);
            writer.WriteLine("");
            writer.WriteLine("================================");
            writer.WriteLine("");

            int dataIter = 1;
            foreach (DataSet set in m_results)
            {
                writer.WriteLine("Set: " + dataIter);
                writer.WriteLine("");

                writer.Write("Order: " + set.Order[0]);
                for (int orderIter = 1; orderIter < set.Order.Count; orderIter++)
                {
                    writer.Write(", " + set.Order[orderIter]);
                }
                writer.WriteLine("");
                writer.WriteLine("");

                writer.WriteLine("Score: " + set.Score);
                writer.WriteLine("");

                foreach (ImproveResult result in set.Results)
                {
                    writer.WriteLine("Improvement: " + result.Name);
                    writer.WriteLine("Level: " + result.Level);
                    writer.WriteLine("MaxLevel: " + result.MaxLevel);
                    writer.WriteLine("");
                }
                
                writer.WriteLine("================================");
                writer.WriteLine("");
                dataIter++;
            }
            writer.Close();
            AssetDatabase.ImportAsset(m_filePath);
        }
        else
        {
            Debug.Log("Results file needs name.");
        }
    }
}

[Serializable]
class OrderData
{
    public List<int> Data = new List<int>();

    public OrderData(List<int> _data)
    {
        Data.AddRange(_data);
    }
}
