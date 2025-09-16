using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager handles global game state, player stats, corruption system, and beast taming
public class GameManager : MonoBehaviour
{
    // DO NOT CHANGE - This makes it accessible everywhere
    public static GameManager Instance { get; private set; }

    // Player Stats - You can change these numbers
    public float playerHealth = 100f;
    public float playerMana = 100f;
    public int playerGold = 0;
    public int playerLevel { get; private set; } = 1;
    public int gold { get; private set; } = 0;
    public int corruption { get; private set; } = 0;

    // Corruption System
    public float corruptionLevel = 0f;
    public float corruptionResistance = 50f;

    // Player Attributes
    public int strength = 10;
    public int dexterity = 10;
    public int intelligence = 10;
    public int constitution = 10;

    // Level up function
    public void LevelUp()
    {
        playerLevel++;
        strength++;
        dexterity++;
        intelligence++;
        constitution++;
        Debug.Log("Player leveled up to level " + playerLevel);
    }

    // Beast taming system
    public bool isBeastTamed { get; private set; } = false;
    public string tamedBeast { get; private set; } = "";
    
    // Quest system
    public List<string> completedQuests { get; private set; } = new List<string>();
    
    // Skills system
    public Dictionary<string, int> skills { get; private set; } = new Dictionary<string, int>();
    
    // Settlement system
    public Dictionary<string, int> settlements { get; private set; } = new Dictionary<string, int>();
    
    // Game events
    public event Action<int> OnLevelChanged;
    public event Action<int> OnGoldChanged;
    public event Action<int> OnCorruptionChanged;
    public event Action<string> OnBeastTamed;
    public event Action<string> OnQuestCompleted;
    public event Action<string, int> OnSkillChanged;
    public event Action<string, int> OnSettlementChanged;
    
    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize skills
            skills["Combat"] = 0;
            skills["Magic"] = 0;
            skills["Stealth"] = 0;
            skills["Crafting"] = 0;
            
            // Initialize settlements
            settlements["Village"] = 0;
            settlements["Town"] = 0;
            settlements["City"] = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Player stat methods
    public void IncreaseLevel()
    {
        playerLevel++;
        OnLevelChanged?.Invoke(playerLevel);
    }
    
    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }
    
    public void IncreaseCorruption()
    {
        corruption++;
        OnCorruptionChanged?.Invoke(corruption);
    }
    
    // Beast taming methods
    public void TameBeast(string beastName)
    {
        isBeastTamed = true;
        tamedBeast = beastName;
        OnBeastTamed?.Invoke(beastName);
    }
    
    public void UntameBeast()
    {
        isBeastTamed = false;
        tamedBeast = "";
    }
    
    // Quest methods
    public void CompleteQuest(string questName)
    {
        if (!completedQuests.Contains(questName))
        {
            completedQuests.Add(questName);
            OnQuestCompleted?.Invoke(questName);
        }
    }
    
    // Skill methods
    public void IncreaseSkill(string skillName)
    {
        if (skills.ContainsKey(skillName))
        {
            skills[skillName]++;
            OnSkillChanged?.Invoke(skillName, skills[skillName]);
        }
    }
    
    // Settlement methods
    public void ImproveSettlement(string settlementName)
    {
        if (settlements.ContainsKey(settlementName))
        {
            settlements[settlementName]++;
            OnSettlementChanged?.Invoke(settlementName, settlements[settlementName]);
        }
    }
}