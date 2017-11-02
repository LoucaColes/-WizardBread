using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipUI : MonoBehaviour
{

    public string m_string;
    private Text m_toolTipText;
    public float m_fadeTime = 5.0f;

    private void Start()
    {
        m_toolTipText = GameObject.Find("ToolTipText").GetComponent<Text>();
        m_toolTipText.color = Color.clear;
    }

    private void OnMouseOver()
    {
        m_toolTipText.color = Color.Lerp(m_toolTipText.color, Color.black, m_fadeTime * Time.deltaTime);
        m_toolTipText.text = m_string;
    }
}

