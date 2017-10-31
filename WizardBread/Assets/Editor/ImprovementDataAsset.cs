using UnityEditor;

public class ImprovementDataAsset
{
    [MenuItem("Assets/Create/Improvement Data")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ImprovementData>();
    }
}
