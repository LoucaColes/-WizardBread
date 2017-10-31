using System.Collections.Generic;
using UnityEngine;

public class ImprovementData : ScriptableObject
{
    public List<ProcessPackage> Data = new List<ProcessPackage>();

    public ImprovementData(List<ProcessPackage> _data)
    {
        Data.AddRange(_data);
    }
}
