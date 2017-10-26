using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImprovementSim : MonoBehaviour
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

    public Simulator.ImproveTags m_tag;
    public int m_level = 1;
    public int m_maxLevel = 5;
    public List<ProcessPackage> m_data = new List<ProcessPackage>();
    public List<bool> m_flags = new List<bool>();

    private string m_filePath;
    public bool m_save = false;
    public bool m_load = false;

	void Start ()
    {
        m_save = false;
        LoadData();
    }
	
	void Update ()
    {
		if(m_save)
        {
            m_save = false;
            SaveData();            
        }

        if (m_load)
        {
            m_load = false;
            LoadData();
        }
    }

    private void SaveData()
    {
        m_filePath = "Assets/Data/Sim/Improvements/" + m_tag.ToString() + ".txt";

        if (File.Exists(m_filePath))
        {
            File.Delete(m_filePath);
        }

        File.CreateText(m_filePath).Dispose();

        StreamWriter writer = new StreamWriter(m_filePath, true);
        string Json = JsonUtility.ToJson(new ImproveData(m_maxLevel, m_data));
        writer.WriteLine(Json);
        writer.Close();
        AssetDatabase.ImportAsset(m_filePath);
    }

    private void LoadData()
    {
        m_filePath = "Assets/Data/Sim/Improvements/" + m_tag.ToString() + ".txt";

        if (File.Exists(m_filePath))
        {
            StreamReader reader = new StreamReader(m_filePath);
            ImproveData data = JsonUtility.FromJson<ImproveData>(reader.ReadLine());
            reader.Close();

            m_data.Clear();
            m_data = data.Data;
            m_maxLevel = m_data.Count;

            m_flags.Clear();
            foreach(ProcessPackage pack in m_data)
            {
                m_flags.Add(false);
            }
        }
    }

    public ChangePackage Resolve(ChangePackage _packIn)
    {
        ChangePackage packOut = null;

        for(int iter = 0; iter <= m_data.Count - 1; iter++)
        {
            if(!m_flags[iter] && m_data[iter].Tag == _packIn.Tag)
            {
                m_flags[iter] = m_data[iter].ResolveScenario(m_level, _packIn.Level);

                if(m_flags[iter])
                {
                    Upgrade();
                    packOut = new ChangePackage(m_tag, m_level);
                }
            }
        }

        if(m_level == m_maxLevel)
        {
            for (int iter = 0; iter <= m_flags.Count - 1; iter++)
            {
                m_flags[iter] = true;
            }
        }

        return packOut;
    }

    private void Upgrade()
    {
        m_level++;
    }
}

[Serializable]
class ImproveData
{
    public int MaxLevel = 10;
    public List<ProcessPackage> Data = new List<ProcessPackage>();

    public ImproveData(int _maxLevel, List<ProcessPackage> _data)
    {
        MaxLevel = _maxLevel;
        Data.AddRange(_data);
    }
}
