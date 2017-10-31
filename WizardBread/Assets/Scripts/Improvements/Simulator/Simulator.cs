using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Simulator : MonoBehaviour
{
    public enum ScoreFactor
    {
        ImprovementLevel,
        PopulationLevel,
        EsteemLevel,
        TotalEsteem
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
            if (MaxLevel != 0)
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
        public int TotalEsteem = 0;
        public int PopulationLevel = 1;
        public int EsteemLevel = 1;
        public float ImprovementGrade = 0.0f;
        public float Score = 0.0f;

        public float CalculateScore(List<ScoreFactor> _factors, int _maxPopLevel, int _maxEsteemLevel)
        {
            Score = 0.0f;

            foreach (ScoreFactor item in _factors)
            {
                FactorHandler(item, _maxPopLevel, _maxEsteemLevel);
            }

            return Score;
        }

        private void FactorHandler(ScoreFactor _factor, int _maxPopLevel, int _maxEsteemLevel)
        {
            switch (_factor)
            {
                case ScoreFactor.ImprovementLevel:
                    {
                        foreach (ImproveResult item in Results)
                        {
                            ImprovementGrade += item.CalculateScore();
                        }
                        Score += ImprovementGrade;
                        break;
                    }
                case ScoreFactor.PopulationLevel:
                    {
                        Score += ((float)PopulationLevel / (float)_maxPopLevel) * 100.0f;
                        break;
                    }
                case ScoreFactor.EsteemLevel:
                    {
                        Score += ((float)EsteemLevel / (float)_maxEsteemLevel) * 100.0f;
                        break;
                    }
                case ScoreFactor.TotalEsteem:
                    {
                        Score += TotalEsteem;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }
    }

    public bool m_start = false;
    private int m_iteration = 0;

    public List<Town.ImprovementTags> m_order = new List<Town.ImprovementTags>();
    public List<ScoreFactor> m_scoreFactors = new List<ScoreFactor>();

    public List<int> m_populationLevels = new List<int>();
    private int m_population;

    public List<int> m_esteemLevels = new List<int>();
    private int m_esteemCoefficient;

    public int m_totalPermutations = 1;
    public List<DataSet> m_results = new List<DataSet>();

    public List<ImprovementSim> m_allImprovements = new List<ImprovementSim>();

    private List<ImprovementSim> m_activeImprovements = new List<ImprovementSim>();
    private List<ChangePackage> m_changeBuffer = new List<ChangePackage>();

    public bool m_save = false;
    public string m_name = "";
    private string m_filePath;

    private void OnDestroy()
    {
        m_results.Clear();
    }

    private void Update()
    {
        if (m_start)
        {
            m_start = false;
            BeginSimulation();
        }

        if (m_save)
        {
            m_save = false;
#if UNITY_EDITOR
            SaveData();
#endif
        }
    }

    private void BeginSimulation()
    {
        m_results.Clear();
        m_iteration = 0;
        CalculateTotalPermutations();

        while (!NextOrder())
        {
            m_activeImprovements.Clear();
            m_results.Add(new DataSet());

            m_population = m_populationLevels[0];
            m_esteemCoefficient = m_esteemLevels[0];

            foreach (Town.ImprovementTags tag in m_order)
            {
                AddImprovement(tag);
                m_results[m_results.Count - 1].Order.Add((int)tag);

                while (m_changeBuffer.Count != 0)
                {
                    BufferHandler();
                }
            }

            foreach (ImprovementSim item in m_activeImprovements)
            {
                m_results[m_results.Count - 1].Results.Add(new ImproveResult(item.m_tag.ToString(), item.m_level, item.m_maxLevel));
            }

            m_results[m_results.Count - 1].TotalEsteem = m_population * m_esteemCoefficient;
            m_results[m_results.Count - 1].CalculateScore(m_scoreFactors, m_populationLevels.Count, m_esteemLevels.Count);
            m_iteration++;
        }
        CullResults();
        m_activeImprovements.Clear();
        Reorder(m_results[0].Order);
    }

    private void CalculateTotalPermutations()
    {
        int max = m_order.Count;
        m_totalPermutations = 1;

        for (int iter = 0; iter < m_order.Count; iter++)
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
        foreach (int item in _newOrder)
        {
            m_order.Add((Town.ImprovementTags)item);
        }
    }

    private void CullResults()
    {
        if (m_results.Count > 100)
        {
            m_results.Sort((a, b) => a.Score.CompareTo(b.Score));
            m_results.RemoveRange(100, m_results.Count - 100);
            m_results.Reverse();
        }
    }

    private void AddImprovement(Town.ImprovementTags _tag)
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
            foreach (ImprovementSim improv in m_activeImprovements)
            {
                ChangePackage temp = improv.Resolve(package);
                if (temp != null)
                {
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
            if (m_population >= m_populationLevels[m_results[m_results.Count - 1].PopulationLevel])
            {
                m_results[m_results.Count - 1].PopulationLevel++;
            }
        }
        else if (m_population != m_populationLevels[m_populationLevels.Count - 1])
        {
            m_population = m_populationLevels[m_populationLevels.Count - 1];
            m_results[m_results.Count - 1].PopulationLevel = m_populationLevels.Count;
        }
    }

    private void AddEsteem(int _newCoefficient)
    {
        m_esteemCoefficient += _newCoefficient;
        m_results[m_results.Count - 1].TotalEsteem = m_esteemCoefficient * m_population;
        if (m_results[m_results.Count - 1].EsteemLevel != m_esteemLevels.Count)
        {
            if (m_results[m_results.Count - 1].TotalEsteem >= m_esteemLevels[m_results[m_results.Count - 1].EsteemLevel])
            {
                m_results[m_results.Count - 1].EsteemLevel++;
            }
        }
    }

#if UNITY_EDITOR

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

#endif
}

[Serializable]
internal class OrderData
{
    public List<int> Data = new List<int>();

    public OrderData(List<int> _data)
    {
        Data.AddRange(_data);
    }
}