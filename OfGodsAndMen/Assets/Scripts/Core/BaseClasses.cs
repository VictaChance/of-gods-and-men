using UnityEngine;

// Base class for all game data scriptable objects
public abstract class GameDataSO : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea(3, 10)]
    public string description;
}
