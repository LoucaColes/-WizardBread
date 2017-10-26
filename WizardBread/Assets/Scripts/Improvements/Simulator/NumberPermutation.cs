using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NumberPermutation : MonoBehaviour
{
    public bool m_start = false;
    public List<int> m_seed = new List<int>();
    public static List<int> m_internalSeed = new List<int>();
    public static List<List<int>> m_result = new List<List<int>>();

    private static string m_filePath;
    public static int m_currentLine = 0;
    public static StreamReader m_reader = null;

    void Update()
    {
        if (m_start)
        {
            m_start = false;
            GenerateAllPermutations(m_seed);
            SaveData();
            m_result.Clear();
        }
    }

    public static List<List<int>> GenerateAllPermutations(List<int> _seed)
    {
        m_internalSeed.Clear();
        m_internalSeed.AddRange(_seed);
        if (m_internalSeed.Count > 2)
        {
            for (int iter = 0; iter < m_internalSeed.Count; iter++)
            {
                GenerateRow(iter);
            }
        }
        else if (m_internalSeed.Count == 2)
        {
            m_result.AddRange(GetPair(m_internalSeed[0], m_internalSeed[1]));
        }
        else if (m_internalSeed.Count == 1)
        {
            m_result.Add(m_internalSeed);
        }
        else
        {
            Debug.LogWarning("Seed is empty, no numbers to permutate");
        }
        return m_result;
   }

    private static void GenerateRow(int _row)
    {
        List<int> nextOut = new List<int>();
        nextOut.AddRange(m_internalSeed);
        nextOut.Remove(m_internalSeed[_row]);
        nextOut.Insert(0, m_internalSeed[_row]);
        m_result.AddRange(GetPermutation(nextOut));
    }

    private static List<List<int>> GetPermutation(List<int> _permIn)
    {
        List<List<int>> permOut = new List<List<int>>();
        List<List<int>> permBack = new List<List<int>>();

        if (_permIn.Count - 1 == 2)
        {
            permBack.AddRange(GetPair(_permIn[1], _permIn[2]));
        }
        else
        {
            List<int> nextSeed = new List<int>();
            nextSeed.AddRange(_permIn);
            nextSeed.RemoveAt(0);

            for (int swapIter = 0; swapIter < nextSeed.Count; swapIter++)
            {
                List<int> nextPerm = new List<int>();
                nextPerm.AddRange(nextSeed);
                nextPerm.RemoveAt(swapIter);
                nextPerm.Insert(0, nextSeed[swapIter]);

                for (int permIter = 0; permIter < nextPerm.Count; permIter++)
                {
                    List<int> nextOut = new List<int>();
                    nextOut.AddRange(nextPerm);
                    nextOut.Remove(nextPerm[permIter]);
                    nextOut.Insert(0, nextPerm[permIter]);
                    nextPerm.Clear();
                    permBack.AddRange(GetPermutation(nextOut));
                    nextOut.Clear();
                }
            }
        }

        foreach (List<int> item in permBack)
        {
            List<int> result = new List<int>();
            result.Add(_permIn[0]);
            result.AddRange(item);
            permOut.Add(result);
        }
        permBack.Clear();

        return permOut;
    }

    private static List<List<int>> GetPair(int _a, int _b)
    {
        List<List<int>> permOut = new List<List<int>>();

        List<int> a = new List<int>();
        a.Add(_a);
        a.Add(_b);
        permOut.Add(a);
        List<int> b = new List<int>();
        b.Add(_b);
        b.Add(_a);
        permOut.Add(b);

        return permOut;
    }

    private void SaveData()
    {
        m_filePath = "Assets/Data/Sim/Permutations/Size-" + m_internalSeed.Count + ".txt";

        if (File.Exists(m_filePath))
        {
            File.Delete(m_filePath);
        }

        File.CreateText(m_filePath).Dispose();

        StreamWriter writer = new StreamWriter(m_filePath, true);
        writer.WriteLine("Permutations: " + m_result.Count);
        foreach (List<int> item in m_result)
        {
            string Json = JsonUtility.ToJson(new PermData(item));
            writer.WriteLine(Json);
        }
        writer.Close();
        AssetDatabase.ImportAsset(m_filePath);
    }

    public static List<int> LoadData(int _length)
    {
        PermData data = null;
        m_filePath = "Assets/Data/Sim/Permutations/Size-" + _length + ".txt";

        if (File.Exists(m_filePath))
        {
            if (m_currentLine == 0)
            {
                m_reader = new StreamReader(m_filePath);
                m_reader.ReadLine();
                m_currentLine++;
            }

            string textIn = m_reader.ReadLine();
            data = JsonUtility.FromJson<PermData>(textIn);
            m_currentLine++;

            if (m_reader.EndOfStream)
            {
                m_reader.Close();
                m_currentLine = 0;
            }

        }
        return data.Data;
    }
}

[Serializable]
class PermData
{
    public List<int> Data;

    public PermData(List<int> _data)
    {
        Data = _data;
    }
}
