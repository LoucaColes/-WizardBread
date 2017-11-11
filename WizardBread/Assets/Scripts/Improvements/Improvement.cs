using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Improvement : MonoBehaviour
{
    public enum ImprovementProcess
    {
        None,
        Equals,
        NotEquals,
        Greater,
        GreaterOrEquals,
        Less,
        LessOrEquals
    }

    [Serializable]
    public class StatChange
    {
        public int Population;
        public int Esteem;

        public StatChange(int _population, int _esteem)
        {
            Population = _population;
            Esteem = _esteem;
        }
    }

    public Town.ImprovementTags m_tag;
    public int m_level = 1;
    public int m_maxLevel = 5;
    public List<Sprite> m_visualLevels;
    public List<ProcessPackage> m_data = new List<ProcessPackage>();
    public List<StatChange> m_statChanges = new List<StatChange>();
    public List<bool> m_flags = new List<bool>();
    public ImprovementData m_externalData;

    private string m_filePath;
    public bool m_save = false;
    public bool m_load = false;

    private SpriteRenderer m_renderer;

    private bool m_buildPlayed = false;

    private Animator m_smokeAnimator;

    private void Start()
    {
        LoadData();

        m_smokeAnimator = GetComponentInChildren<Animator>();

        int count = m_statChanges.Count - 1;
        while (m_statChanges.Count < m_maxLevel)
        {
            if (m_statChanges.Count != 0)
            {
                m_statChanges.Add(new StatChange(m_statChanges[count].Population, m_statChanges[count].Esteem));
            }
            else
            {
                count = 0;
                m_statChanges.Add(new StatChange(1, 1));
            }
        }

        while (m_visualLevels.Count < m_maxLevel)
        {
            m_visualLevels.Add(new Sprite());
        }

        m_renderer = GetComponent<SpriteRenderer>();
    }

    public void Clear()
    {
        m_level = 1;
        m_flags.Clear();
        foreach (ProcessPackage pack in m_data)
        {
            m_flags.Add(false);
        }
    }

    public void Initialise()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if (!m_buildPlayed)
        {
            m_buildPlayed = true;
            AudioManager.AudioManager.m_instance.PlaySFX("Build", Vector3.zero);
        }
    }

    private void Update()
    {
        if (m_save)
        {
            m_save = false;
#if UNITY_EDITOR
            SaveData();
#endif
        }

        if (m_load)
        {
            m_load = false;
            LoadData();
        }
    }

#if UNITY_EDITOR

    private void SaveData()
    {
        //m_filePath = "Assets/Data/Sim/Improvements/" + m_tag.ToString() + ".txt";

        //if (File.Exists(m_filePath))
        //{
        //    File.Delete(m_filePath);
        //}

        //File.CreateText(m_filePath).Dispose();

        //StreamWriter writer = new StreamWriter(m_filePath, true);
        //string Json = JsonUtility.ToJson(new ImproveData(m_data));
        //writer.WriteLine(Json);
        //writer.Close();
        //AssetDatabase.ImportAsset(m_filePath);
        m_externalData.Data.Clear();
        m_externalData.Data.AddRange(m_data);
    }

#endif

    private void LoadData()
    {
        //m_filePath = "Assets/Data/Sim/Improvements/" + m_tag.ToString() + ".txt";

        //if (File.Exists(m_filePath))
        //{
        //    StreamReader reader = new StreamReader(m_filePath);
        //    ImproveData data = JsonUtility.FromJson<ImproveData>(reader.ReadLine());
        //    reader.Close();

        //    m_data.Clear();
        //    m_data = data.Data;
        //    if (m_data.Count + 1 < m_maxLevel)
        //    {
        //        m_maxLevel = m_data.Count + 1;
        //    }

        //    m_flags.Clear();
        //    foreach (ProcessPackage pack in m_data)
        //    {
        //        m_flags.Add(false);
        //    }
        //}

        m_data.Clear();
        m_data.AddRange(m_externalData.Data);
        if (m_data.Count + 1 < m_maxLevel)
        {
            m_maxLevel = m_data.Count + 1;
        }

        m_flags.Clear();
        foreach (ProcessPackage pack in m_data)
        {
            m_flags.Add(false);
        }
    }

    public ChangePackage Resolve(ChangePackage _packIn)
    {
        ChangePackage packOut = null;

        for (int iter = 0; iter <= m_data.Count - 1; iter++)
        {
            if (!m_flags[iter] && m_data[iter].Tag == _packIn.Tag)
            {
                m_flags[iter] = m_data[iter].ResolveScenario(m_level, _packIn.Level);

                if (m_flags[iter])
                {
                    packOut = new ChangePackage(m_tag, m_level, m_statChanges[m_level - 1].Population, m_statChanges[m_level - 1].Esteem);
                }
            }
        }

        if (m_level == m_maxLevel)
        {
            for (int iter = 0; iter <= m_flags.Count - 1; iter++)
            {
                m_flags[iter] = true;
            }
        }

        return packOut;
    }

    public void Upgrade()
    {
        if (m_level != m_maxLevel)
        {
            if (m_visualLevels[m_level])
            {
                m_smokeAnimator.SetBool("Play", true);
                m_renderer.sprite = m_visualLevels[m_level];
            }

            AudioManager.AudioManager.m_instance.PlaySFX("Ding", Vector3.zero);

            m_level++;
        }
    }

    public bool CheckComplete()
    {
        if (m_level == m_maxLevel)
        {
            return true;
        }
        return false;
   }
}

[Serializable]
internal class ImproveData
{
    public List<ProcessPackage> Data = new List<ProcessPackage>();

    public ImproveData(List<ProcessPackage> _data)
    {
        Data.AddRange(_data);
    }
}