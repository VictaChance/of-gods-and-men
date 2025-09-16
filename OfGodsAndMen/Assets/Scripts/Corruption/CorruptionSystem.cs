using UnityEngine;

public class CorruptionSystem : MonoBehaviour
{
    public static CorruptionSystem Instance { get; private set; }

    public int corruptionLevel = 0;
    public int maxCorruptionLevel = 100;

    public int slightlyCorruptedThreshold = 25;
    public int corruptedThreshold = 50;
    public int fullyCorruptedThreshold = 75;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseCorruption(int amount)
    {
        corruptionLevel += amount;
        corruptionLevel = Mathf.Clamp(corruptionLevel, 0, maxCorruptionLevel);
    }

    public void DecreaseCorruption(int amount)
    {
        corruptionLevel -= amount;
        corruptionLevel = Mathf.Clamp(corruptionLevel, 0, maxCorruptionLevel);
    }

    public string GetCorruptionStatus()
    {
        if (corruptionLevel >= fullyCorruptedThreshold)
            return "Fully Corrupted";
        else if (corruptionLevel >= corruptedThreshold)
            return "Corrupted";
        else if (corruptionLevel >= slightlyCorruptedThreshold)
            return "Slightly Corrupted";
        else
            return "Pure";
    }
}
