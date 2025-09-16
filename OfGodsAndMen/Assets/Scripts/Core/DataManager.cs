using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // Data collections
    private Dictionary<string, GameDataSO> allGameData = new Dictionary<string, GameDataSO>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllGameData()
    {
        // Load all scriptable objects from Resources folder
        GameDataSO[] allData = Resources.LoadAll<GameDataSO>("Data");

        foreach (GameDataSO data in allData)
        {
            if (!string.IsNullOrEmpty(data.id))
            {
                allGameData[data.id] = data;
            }
            else
            {
                Debug.LogWarning($"Found GameDataSO with empty ID: {data.name}");
            }
        }

        Debug.Log($"Loaded {allGameData.Count} game data objects");
    }

    public T GetData<T>(string id) where T : GameDataSO
    {
        if (allGameData.TryGetValue(id, out GameDataSO data))
        {
            if (data is T typedData)
            {
                return typedData;
            }
            else
            {
                Debug.LogWarning($"Data with ID {id} is not of requested type {typeof(T).Name}");
            }
        }
        else
        {
            Debug.LogWarning($"No data found with ID {id}");
        }

        return null;
    }

    public List<T> GetAllDataOfType<T>() where T : GameDataSO
    {
        List<T> result = new List<T>();

        foreach (GameDataSO data in allGameData.Values)
        {
            if (data is T typedData)
            {
                result.Add(typedData);
            }
        }

        return result;
    }
}