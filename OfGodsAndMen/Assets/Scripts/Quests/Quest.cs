using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quest
{
    [Header("Quest Information")]
    public string questID;
    public string questName;
    public string description;
    public bool isMainQuest = false;
    
    [Header("Quest Progression")]
    public QuestStatus status = QuestStatus.NotStarted;
    public List<Objective> objectives = new List<Objective>();
    
    [Header("Rewards")]
    public int goldReward = 0;
    public int experienceReward = 0;
    public List<string> itemRewards = new List<string>();
    
    [Header("Prerequisites")]
    public List<string> prerequisiteQuests = new List<string>();
    
    // Events
    public event Action<Quest> OnQuestStatusChanged;
    public event Action<Quest, Objective> OnObjectiveCompleted;
    
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }
    
    [Serializable]
    public class Objective
    {
        public string objectiveID;
        public string description;
        public ObjectiveType type;
        public string targetID; // ID of target (NPC, item, location, etc.)
        public int requiredAmount = 1;
        public int currentAmount = 0;
        public bool isCompleted = false;
        
        public enum ObjectiveType
        {
            CollectItem,
            KillEnemy,
            TalkToNPC,
            ReachLocation,
            Custom
        }
    }
    
    // Check if all objectives are completed
    public bool AreAllObjectivesCompleted()
    {
        foreach (var objective in objectives)
        {
            if (!objective.isCompleted)
            {
                return false;
            }
        }
        return true;
    }
    
    // Update quest status
    public void UpdateStatus(QuestStatus newStatus)
    {
        status = newStatus;
        OnQuestStatusChanged?.Invoke(this);
    }
    
    // Complete an objective
    public void CompleteObjective(string objectiveID)
    {
        Objective objective = objectives.Find(o => o.objectiveID == objectiveID);
        if (objective != null && !objective.isCompleted)
        {
            objective.isCompleted = true;
            objective.currentAmount = objective.requiredAmount;
            OnObjectiveCompleted?.Invoke(this, objective);
            
            // Check if all objectives are completed
            if (AreAllObjectivesCompleted())
            {
                UpdateStatus(QuestStatus.Completed);
            }
        }
    }
    
    // Progress an objective
    public void ProgressObjective(string objectiveID, int amount = 1)
    {
        Objective objective = objectives.Find(o => o.objectiveID == objectiveID);
        if (objective != null && !objective.isCompleted)
        {
            objective.currentAmount += amount;
            if (objective.currentAmount >= objective.requiredAmount)
            {
                CompleteObjective(objectiveID);
            }
        }
    }
}