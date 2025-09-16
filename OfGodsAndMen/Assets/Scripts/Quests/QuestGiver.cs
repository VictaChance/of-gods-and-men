using System;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [Header("Quest Giver Settings")]
    public string questGiverID;
    public string questGiverName;
    public Quest questToGive;
    
    private QuestManager questManager;
    
    private void Start()
    {
        // Get QuestManager instance
        questManager = QuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found in scene");
        }
    }
    
    // Give quest to player
    public void GiveQuest()
    {
        if (questManager != null && questToGive != null)
        {
            // Add quest to QuestManager if not already present
            Quest existingQuest = questManager.GetQuest(questToGive.questID);
            if (existingQuest == null)
            {
                questManager.quests.Add(questToGive);
            }
            
            // Start the quest
            questManager.StartQuest(questToGive.questID);
        }
    }
    
    // Check if player can receive quest
    public bool CanGiveQuest()
    {
        if (questManager != null && questToGive != null)
        {
            Quest quest = questManager.GetQuest(questToGive.questID);
            return quest == null || quest.status == Quest.QuestStatus.NotStarted;
        }
        return false;
    }
}