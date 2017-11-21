using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Canvas m_gameOverCanvas;
    public Text m_esteemText;
    public Text m_messageText;
    public List<Highscore> m_highscoreUI = new List<Highscore>();
    private HighscoreData m_internalData;
    public HighscoreData m_externalData;
    public List<Sprite> m_sprites = new List<Sprite>();

    public float m_time;
    private float m_timer;

    private bool m_played;

    public int m_targetScore;

    // Use this for initialization
    private void Start()
    {
        m_played = false;
        m_esteemText.text = "";
        m_messageText.text = "";
        m_timer = 0;

        m_internalData = ScriptableObject.CreateInstance<HighscoreData>();
        m_internalData.Initialise();
        LoadData();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Town.m_instance.m_complete)
        {
            if (!m_played)
            {
                m_gameOverCanvas.enabled = true;
                m_esteemText.text = "Final Esteem: " + Town.m_instance.m_townSelfEsteem; 

                if (Town.m_instance.m_townSelfEsteem >= m_targetScore)
                {
                    AudioManager.AudioManager.m_instance.PlaySFX("Win", Vector3.zero);
                    m_messageText.text = "Well Done! Goal Achieved";
                }
                else
                {
                    AudioManager.AudioManager.m_instance.PlaySFX("GameOver", Vector3.zero);
                    m_messageText.text = "Unlucky! Try Again!";
                }

                LoadData();
                m_internalData.AddHighscore(Town.m_instance.m_townSelfEsteem, Town.m_instance.m_order);
                SaveData();

                for (int iter = 0; iter <= m_highscoreUI.Count - 1; iter++)
                {
                    if (iter < m_internalData.m_data.Count)
                    {
                        m_highscoreUI[iter].SetPosition(m_internalData.m_data[iter].Position);
                        m_highscoreUI[iter].SetScore(m_internalData.m_data[iter].Score);

                        for (int iconIter = 0; iconIter <= m_sprites.Count - 1; iconIter++)
                        {
                            m_highscoreUI[iter].SetIcons(m_internalData.m_data[iter].Order);
                        }
                    }
                }

                m_played = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            m_gameOverCanvas.enabled = false;
        }
    }

    private void SaveData()
    {
        m_externalData.m_data.Clear();
        m_externalData.m_maxHighscores = m_internalData.m_maxHighscores;
        m_externalData.m_data.AddRange(m_internalData.m_data);
    }

    private void LoadData()
    {
        m_internalData.m_data.Clear();
        m_internalData.m_maxHighscores = m_externalData.m_maxHighscores;
        m_internalData.m_data.AddRange(m_externalData.m_data);
    }
}