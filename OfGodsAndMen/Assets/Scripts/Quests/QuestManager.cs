using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Singleton instance
    public static QuestManager Instance { get; private set; }
    
    // Quest list
    public List<Quest> quests = new List<Quest>();
    
    // Reward handler reference
    public QuestRewardHandler rewardHandler;
    
    // Events
    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest> OnQuestFailed;
    
    private GameManager gameManager;
    
    private void Awake()
    {
        // Ensure only one instance exists
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
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        
        // Get reward handler component
        rewardHandler = FindObjectOfType<QuestRewardHandler>();
        
        if (rewardHandler == null)
        {
            Debug.LogError("QuestRewardHandler not found in scene");
        }
    }
    
    // Start a quest
    public void StartQuest(string questID)
    {
        Quest quest = quests.Find(q => q.questID == questID);
        if (quest != null && quest.status == Quest.QuestStatus.NotStarted)
        {
            // Check if prerequisites are met
            if (ArePrerequisitesMet(quest))
            {
                quest.UpdateStatus(Quest.QuestStatus.InProgress);
                OnQuestStarted?.Invoke(quest);
            }
            else
            {
                Debug.LogWarning($"Cannot start quest {quest.questName} because prerequisites are not met.");
            }
        }
    }
    
    // Complete a quest
    public void CompleteQuest(string questID)
    {
        Quest quest = quests.Find(q => q.questID == questID);
        if (quest != null && quest.status == Quest.QuestStatus.InProgress)
        {
            quest.UpdateStatus(Quest.QuestStatus.Completed);
            
            // Award rewards through reward handler
            if (rewardHandler != null)
            {
                rewardHandler.AwardRewards(quest);
            }
            else
            {
                // Fallback to direct reward awarding
                gameManager?.AddGold(quest.goldReward);
            }
            
            // Add completed quest to GameManager
            gameManager?.CompleteQuest(quest.questName);
            
            OnQuestCompleted?.Invoke(quest);
        }
    }
    
    // Fail a quest
    public void FailQuest(string questID)
    {
        Quest quest = quests.Find(q => q.questID == questID);
        if (quest != null && quest.status == Quest.QuestStatus.InProgress)
        {
            quest.UpdateStatus(Quest.QuestStatus.Failed);
            OnQuestFailed?.Invoke(quest);
        }
    }
    
    // Check if prerequisites are met
    private bool ArePrerequisitesMet(Quest quest)
    {
        foreach (string prerequisiteID in quest.prerequisiteQuests)
        {
            Quest prerequisiteQuest = quests.Find(q => q.questID == prerequisiteID);
            if (prerequisiteQuest == null || prerequisiteQuest.status != Quest.QuestStatus.Completed)
            {
                return false;
            }
        }
        return true;
    }
    
    // Get quest by ID
    public Quest GetQuest(string questID)
    {
        return quests.Find(q => q.questID == questID);
    }
    
    // Get all active quests
    public List<Quest> GetActiveQuests()
    {
        List<Quest> activeQuests = new List<Quest>();
        foreach (Quest quest in quests)
        {
            if (quest.status == Quest.QuestStatus.InProgress)
            {
                activeQuests.Add(quest);
            }
        }
        return activeQuests;
    }
    
    // Get all completed quests
    public List<Quest> GetCompletedQuests()
    {
        List<Quest> completedQuests = new List<Quest>();
        foreach (Quest quest in quests)
        {
            if (quest.status == Quest.QuestStatus.Completed)
            {
                completedQuests.Add(quest);
            }
        }
        return completedQuests;
    }
}