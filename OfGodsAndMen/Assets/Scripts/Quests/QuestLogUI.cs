using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questLogPanel;
    public Transform completedQuestsContainer;
    public GameObject questEntryPrefab;
    
    [Header("Navigation")]
    public Button closeButton;
    
    private QuestManager questManager;
    private List<GameObject> questEntryObjects = new List<GameObject>();
    
    private void Start()
    {
        // Get QuestManager instance
        questManager = QuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found in scene");
            return;
        }
        
        // Setup close button listener
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideQuestLog);
        }
        
        // Hide panel by default
        if (questLogPanel != null)
        {
            questLogPanel.SetActive(false);
        }
    }
    
    // Show quest log with completed quests
    public void ShowQuestLog()
    {
        if (questLogPanel == null || completedQuestsContainer == null || questEntryPrefab == null)
        {
            Debug.LogError("QuestLogUI components not properly assigned");
            return;
        }
        
        // Clear existing quest entries
        foreach (GameObject entry in questEntryObjects)
        {
            Destroy(entry);
        }
        questEntryObjects.Clear();
        
        // Get completed quests
        List<Quest> completedQuests = questManager.GetCompletedQuests();
        
        // Create UI entries for each completed quest
        foreach (Quest quest in completedQuests)
        {
            GameObject questEntryObj = Instantiate(questEntryPrefab, completedQuestsContainer);
            Text questEntryText = questEntryObj.GetComponent<Text>();
            
            if (questEntryText != null)
            {
                questEntryText.text = quest.questName;
            }
            
            questEntryObjects.Add(questEntryObj);
        }
        
        // Show panel
        questLogPanel.SetActive(true);
    }
    
    // Hide quest log
    public void HideQuestLog()
    {
        if (questLogPanel != null)
        {
            questLogPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        // Remove close button listener
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HideQuestLog);
        }
    }
}