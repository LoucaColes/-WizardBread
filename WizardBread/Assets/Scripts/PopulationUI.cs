using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationUI : MonoBehaviour
{
    public Text m_populationText;

    void Update ()
    {
        m_populationText.text = "Population\n" + Town.m_instance.m_population;
    }
}
