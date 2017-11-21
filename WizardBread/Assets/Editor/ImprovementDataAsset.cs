using UnityEditor;

public class ImprovementDataAsset
{
    [MenuItem("Assets/Create/Improvement Data")]
    public static void CreateImprovement()
    {
        ScriptableObjectUtility.CreateAsset<ImprovementData>();
    }

    [MenuItem("Assets/Create/Highscore Data")]
    public static void CreateHighscore()
    {
        ScriptableObjectUtility.CreateAsset<HighscoreData>();
    }
}
