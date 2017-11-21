using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public int Position;
    public Text PositionText;
    public int Score;
    public Text ScoreText;
    public List<Town.ImprovementTags> Icons = new List<Town.ImprovementTags>();
    public List<Image> IconsImages = new List<Image>();

    public void SetScore(int _score)
    {
        Score = _score;
        ScoreText.text = _score.ToString();
    }

    public void SetPosition(int _position)
    {
        Position = _position;
        PositionText.text = GetPosition(_position);
    }

    public static string GetPosition(int _pos)
    {
        string position = _pos.ToString();
        switch (_pos)
        {
            case 1:
                {
                    position += "st";
                    break;
                }
            case 2:
                {
                    position += "nd";
                    break;
                }
            case 3:
                {
                    position += "rd";
                    break;
                }
            default:
                {
                    position += "th";
                    break;
                }
        }
        return position;
    }

    public void SetIcons(List<Town.ImprovementTags> _icons)
    {
        Icons = _icons;

        for(int iter = 0; iter < IconsImages.Count; iter++)
        {
            IconsImages[iter].sprite = null;
            if (Icons != null)
            {
                if (iter < Icons.Count)
                {
                    IconsImages[iter].sprite = ImprovementHardData.Instance.Icons[(int)Icons[iter]];
                }
            }
        }
    }
}
