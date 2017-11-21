using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementHardData : MonoBehaviour
{
    public static ImprovementHardData Instance = null;
    public List<Sprite> Icons = new List<Sprite>();

    void Start()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
