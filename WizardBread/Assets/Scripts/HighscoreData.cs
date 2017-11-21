using System;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreData : ScriptableObject
{
    [Serializable]
    public class ScoreData
    {
        public int Position;
        public int Score;
        public List<Town.ImprovementTags> Order = new List<Town.ImprovementTags>();

        public ScoreData(Highscore _in)
        {
            Score = _in.Score;
            Order.AddRange(_in.Icons);
        }

        public ScoreData(int _score, List<Town.ImprovementTags> _order)
        {
            Score = _score;
            Order.AddRange(_order);
        }
    }

    public int m_maxHighscores;
    public List<ScoreData> m_data;

    public void Initialise()
    {
        m_data = new List<ScoreData>();
    }

    public void AddHighscore(Highscore _in)
    {
        m_data.Add(new ScoreData(_in));
        m_data.Sort((b, a) => a.Score.CompareTo(b.Score));
        if (m_data.Count > m_maxHighscores)
        {
            m_data.RemoveAt(m_data.Count - 1);
        }

        for (int iter = 0; iter < m_data.Count; iter++)
        {
            m_data[iter].Position = iter + 1;
        }
    }

    public void AddHighscore(int _score, List<Town.ImprovementTags> _order)
    {
        m_data.Add(new ScoreData(_score, _order));
        m_data.Sort((b, a) => a.Score.CompareTo(b.Score));
        if (m_data.Count > m_maxHighscores)
        {
            m_data.RemoveAt(m_data.Count - 1);
        }

        for(int iter = 0; iter < m_data.Count; iter++)
        {
            m_data[iter].Position = iter + 1;
        }
    }
}
