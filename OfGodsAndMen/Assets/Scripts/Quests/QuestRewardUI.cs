using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject rewardPanel;
    public Text questNameText;
    public Text goldRewardText;
    public Text experienceRewardText;
    public Transform itemRewardsContainer;
    public GameObject itemRewardPrefab;
    
    [Header("Navigation")]
    public Button acceptButton;
    public Button closeButton;
    
    private Quest currentQuest;
    private List<GameObject> itemRewardObjects = new List<GameObject>();
    private GameManager gameManager;
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        
        // Setup button listeners
        if (acceptButton != null)
        {
            acceptButton.onClick.AddListener(AcceptRewards);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideRewards);
        }
        
        // Hide panel by default
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(false);
        }
    }
    
    // Show quest rewards
    public void ShowRewards(Quest quest)
    {
        if (rewardPanel == null || questNameText == null || goldRewardText == null || 
            experienceRewardText == null || itemRewardsContainer == null)
        {
            Debug.LogError("QuestRewardUI components not properly assigned");
            return;
        }
        
        currentQuest = quest;
        
        // Clear existing item rewards
        foreach (GameObject item in itemRewardObjects)
        {
            Destroy(item);
        }
        itemRewardObjects.Clear();
        
        // Set quest name
        questNameText.text = quest.questName;
        
        // Set reward texts
        goldRewardText.text = $"Gold: {quest.goldReward}";
        experienceRewardText.text = $"Experience: {quest.experienceReward}";
        
        // Create item reward entries
        foreach (string itemReward in quest.itemRewards)
        {
            if (itemRewardPrefab != null)
            {
                GameObject itemRewardObj = Instantiate(itemRewardPrefab, itemRewardsContainer);
                Text itemRewardText = itemRewardObj.GetComponent<Text>();
                
                if (itemRewardText != null)
                {
                    itemRewardText.text = itemReward;
                }
                
                itemRewardObjects.Add(itemRewardObj);
            }
        }
        
        // Show panel
        rewardPanel.SetActive(true);
    }
    
    // Accept quest rewards
    public void AcceptRewards()
    {
        if (currentQuest != null && gameManager != null)
        {
            // Add gold reward
            gameManager.AddGold(currentQuest.goldReward);
            
            // Add experience reward (if implemented)
            // gameManager.AddExperience(currentQuest.experienceReward);
            
            // Add item rewards (if implemented)
            // foreach (string itemReward in currentQuest.itemRewards)
            // {
            //     gameManager.AddItem(itemReward);
            // }
            
            // Hide panel
            HideRewards();
        }
    }
    
    // Hide reward panel
    public void HideRewards()
    {
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(false);
        }
        
        currentQuest = null;
    }
    
    private void OnDestroy()
    {
        // Remove button listeners
        if (acceptButton != null)
        {
            acceptButton.onClick.RemoveListener(AcceptRewards);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HideRewards);
        }
    }
}