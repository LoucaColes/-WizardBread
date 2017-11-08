using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUISwap : MonoBehaviour
{
    public Town.ImprovementTags m_tag;
    public List<Sprite> m_levels = new List<Sprite>();
    private int m_level = 0;
    private SpriteRenderer m_render;
    private bool m_finished = false;

	void Start ()
    {
        m_render = GetComponent<SpriteRenderer>();
        m_render.sprite = m_levels[m_level];
        m_level++;
	}
	
	void Update ()
    {
        if (!m_finished)
        {
            if (Town.m_instance.m_allImprovements[(int)m_tag].m_level > m_level)
            {
                m_render.sprite = m_levels[m_level];
                m_level++;

                m_finished = Town.m_instance.m_allImprovements[(int)m_tag].CheckComplete();
            }
        }
	}
}
