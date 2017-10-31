using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Canvas m_gameOverCanvas;
    public Text m_esteemText;

    public float m_time;
    private float m_timer;

    private bool m_played;

    // Use this for initialization
    private void Start()
    {
        m_played = false;
        m_esteemText.text = "";
        m_timer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Town.m_instance.m_complete)
        {
            if (!m_played)
            {
                if (Town.m_instance.m_townSelfEsteem >= 750)
                {
                    AudioManager.m_instance.PlaySFX("Win");
                }
                else
                {
                    AudioManager.m_instance.PlaySFX("GameOver");
                }
                m_played = true;
            }
            m_gameOverCanvas.enabled = true;
            m_esteemText.text = "Final Esteem: " + Town.m_instance.m_townSelfEsteem;
            m_timer += Time.deltaTime;
            if (m_timer >= m_time)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            m_gameOverCanvas.enabled = false;
        }
    }
}