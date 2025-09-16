using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrackerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questTrackerPanel;
    public Transform activeQuestsContainer;
    public GameObject questTrackerEntryPrefab;
    
    private QuestManager questManager;
    private List<GameObject> questTrackerEntryObjects = new List<GameObject>();
    
    private void Start()
    {
        // Get QuestManager instance
        questManager = QuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found in scene");
            return;
        }
        
        // Hide panel by default
        if (questTrackerPanel != null)
        {
            questTrackerPanel.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Update quest tracker every frame
        UpdateQuestTracker();
    }
    
    // Update quest tracker display
    private void UpdateQuestTracker()
    {
        if (questTrackerPanel == null || activeQuestsContainer == null || questTrackerEntryPrefab == null)
        {
            return;
        }
        
        // Clear existing quest tracker entries
        foreach (GameObject entry in questTrackerEntryObjects)
        {
            Destroy(entry);
        }
        questTrackerEntryObjects.Clear();
        
        // Get active quests
        List<Quest> activeQuests = questManager.GetActiveQuests();
        
        // Create UI entries for each active quest
        foreach (Quest quest in activeQuests)
        {
            GameObject questTrackerEntryObj = Instantiate(questTrackerEntryPrefab, activeQuestsContainer);
            Text questTrackerEntryText = questTrackerEntryObj.GetComponent<Text>();
            
            if (questTrackerEntryText != null)
            {
                // Display quest name and completion status
                int completedObjectives = 0;
                foreach (Quest.Objective objective in quest.objectives)
                {
                    if (objective.isCompleted)
                    {
                        completedObjectives++;
                    }
                }
                
                questTrackerEntryText.text = $"{quest.questName} ({completedObjectives}/{quest.objectives.Count})";
            }
            
            questTrackerEntryObjects.Add(questTrackerEntryObj);
        }
        
        // Show/hide panel based on whether there are active quests
        questTrackerPanel.SetActive(activeQuests.Count > 0);
    }
}