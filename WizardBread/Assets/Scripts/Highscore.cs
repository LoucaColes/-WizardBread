using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Text Position;
    public Text Score;
    public List<Image> Icons = new List<Image>();

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
}
