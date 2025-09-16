using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questListPanel;
    public Transform questListContainer;
    public GameObject questListEntryPrefab;
    
    [Header("Navigation")]
    public Button closeButton;
    
    private QuestManager questManager;
    private List<GameObject> questListEntryObjects = new List<GameObject>();
    
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
            closeButton.onClick.AddListener(HideQuestList);
        }
        
        // Hide panel by default
        if (questListPanel != null)
        {
            questListPanel.SetActive(false);
        }
    }
    
    // Show quest list with all quests
    public void ShowQuestList()
    {
        if (questListPanel == null || questListContainer == null || questListEntryPrefab == null)
        {
            Debug.LogError("QuestListUI components not properly assigned");
            return;
        }
        
        // Clear existing quest list entries
        foreach (GameObject entry in questListEntryObjects)
        {
            Destroy(entry);
        }
        questListEntryObjects.Clear();
        
        // Create UI entries for each quest
        foreach (Quest quest in questManager.quests)
        {
            GameObject questListEntryObj = Instantiate(questListEntryPrefab, questListContainer);
            QuestListEntryUI questListEntryUI = questListEntryObj.GetComponent<QuestListEntryUI>();
            
            if (questListEntryUI != null)
            {
                questListEntryUI.Initialize(quest);
            }
            
            questListEntryObjects.Add(questListEntryObj);
        }
        
        // Show panel
        questListPanel.SetActive(true);
    }
    
    // Hide quest list
    public void HideQuestList()
    {
        if (questListPanel != null)
        {
            questListPanel.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        // Remove close button listener
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HideQuestList);
        }
    }
}