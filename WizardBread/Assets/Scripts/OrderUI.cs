using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    public static OrderUI m_instance = null;
    public List<Sprite> m_sprites = new List<Sprite>();
    public List<SpriteRenderer> m_orderRender = new List<SpriteRenderer>();
    private int m_level = 0;

	void Start ()
    {
        if(m_instance)
        {
            Destroy(this);
        }
        else
        {
            m_instance = this;
        }

        Clear();
	}

    public void Clear()
    {
        m_level = 0;

        foreach (SpriteRenderer ren in m_orderRender)
        {
            ren.sprite = null;
        }
    }
	
	public void AddRender(Town.ImprovementTags _tag)
    {
        m_orderRender[m_level].sprite = m_sprites[(int)_tag];
        m_level++;
    }
}
