using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestRewardHandler : MonoBehaviour
{
    [Header("Reward Settings")]
    public List<Reward> rewards = new List<Reward>();
    
    private GameManager gameManager;
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
    }
    
    // Award rewards to player
    public void AwardRewards(Quest quest)
    {
        if (gameManager == null)
        {
            Debug.LogError("Cannot award rewards: GameManager not found");
            return;
        }
        
        // Award gold
        if (quest.goldReward > 0)
        {
            gameManager.AddGold(quest.goldReward);
            Debug.Log($"Awarded {quest.goldReward} gold for completing quest: {quest.questName}");
        }
        
        // Award experience
        if (quest.experienceReward > 0)
        {
            // Assuming there's an AddExperience method in GameManager
            // gameManager.AddExperience(quest.experienceReward);
            Debug.Log($"Awarded {quest.experienceReward} experience for completing quest: {quest.questName}");
        }
        
        // Award items
        foreach (string itemReward in quest.itemRewards)
        {
            // Assuming there's an AddItem method in GameManager
            // gameManager.AddItem(itemReward);
            Debug.Log($"Awarded item {itemReward} for completing quest: {quest.questName}");
        }
    }
    
    // Serializable reward data
    [Serializable]
    public class Reward
    {
        public string rewardID;
        public RewardType type;
        public string targetID; // ID of target (item, skill, etc.)
        public int amount = 1;
        public string description;
    }
    
    public enum RewardType
    {
        Gold,
        Experience,
        Item,
        SkillPoint,
        SettlementImprovement
    }
}