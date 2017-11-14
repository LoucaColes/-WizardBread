using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Canvas m_gameOverCanvas;
    public Text m_esteemText;
    public Text m_messageText;
    public List<Highscore> m_highscores = new List<Highscore>();
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
    }

    // Update is called once per frame
    private void Update()
    {
        if (Town.m_instance.m_complete)
        {
            if (!m_played)
            {
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

                for(int iter = 0; iter <= m_highscores.Count - 1; iter++)
                {
                    m_highscores[iter].Position.text = Highscore.GetPosition(iter);
                    m_highscores[iter].Score.text = "" + 100 + iter;

                    for (int iconIter = 0; iconIter <= m_sprites.Count - 1; iconIter++)
                    {
                        m_highscores[iter].Icons[iconIter].sprite = m_sprites[iconIter];
                    }
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
            for (int iter = 0; iter <= m_highscores.Count - 1; iter++)
            {
                m_highscores[iter].Position.text = "---";
                m_highscores[iter].Score.text = "---";

                for (int iconIter = 0; iconIter <= m_sprites.Count - 1; iconIter++)
                {
                    m_highscores[iter].Icons[iconIter].sprite = null;
                }
            }

            m_gameOverCanvas.enabled = false;
        }
    }
}