using System;
using System.Collections.Generic;
using UnityEngine;

// QuestSystem manages quests and their completion
public class QuestSystem : MonoBehaviour
{
    // Quest data structure
    [Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public bool isCompleted = false;
        public List<string> objectives = new List<string>();
        public List<bool> objectiveCompletionStatus = new List<bool>();
    }
    
    // Quests list
    public List<Quest> quests = new List<Quest>();
    
    // Events
    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest, int> OnObjectiveCompleted;
    
    private GameManager gameManager;
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        
        // Initialize objective completion status for each quest
        foreach (var quest in quests)
        {
            // Ensure the objective completion status list matches the objectives list
            while (quest.objectiveCompletionStatus.Count < quest.objectives.Count)
            {
                quest.objectiveCompletionStatus.Add(false);
            }
        }
    }
    
    // Start a quest
    public void StartQuest(string questName)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null && !quest.isCompleted)
        {
            OnQuestStarted?.Invoke(quest);
        }
    }
    
    // Complete a quest
    public void CompleteQuest(string questName)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            gameManager?.CompleteQuest(questName);
            OnQuestCompleted?.Invoke(quest);
        }
    }
    
    // Complete an objective
    public void CompleteObjective(string questName, int objectiveIndex)
    {
        Quest quest = quests.Find(q => q.questName == questName);
        if (quest != null && !quest.isCompleted && objectiveIndex < quest.objectiveCompletionStatus.Count)
        {
            quest.objectiveCompletionStatus[objectiveIndex] = true;
            OnObjectiveCompleted?.Invoke(quest, objectiveIndex);
            
            // Check if all objectives are completed
            bool allObjectivesCompleted = true;
            foreach (bool status in quest.objectiveCompletionStatus)
            {
                if (!status)
                {
                    allObjectivesCompleted = false;
                    break;
                }
            }
            
            // If all objectives are completed, complete the quest
            if (allObjectivesCompleted)
            {
                CompleteQuest(questName);
            }
        }
    }
}