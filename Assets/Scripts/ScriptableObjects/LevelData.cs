using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName ="Level/Level Data")]
public class LevelData : ScriptableObject
{

    public int levelId;
    public string levelName;

    [TextArea]
    public string description;
    public int zoneId;

    public string sceneName;
}
