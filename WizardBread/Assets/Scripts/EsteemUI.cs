using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EsteemUI : MonoBehaviour
{
    public Text m_esteemnText;

    void Update()
    {
        m_esteemnText.text = "Esteem\n" + Town.m_instance.m_esteemCoefficient;
    }
}
