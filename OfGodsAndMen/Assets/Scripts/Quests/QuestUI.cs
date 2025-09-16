using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questPanel;
    public Text questTitleText;
    public Text questDescriptionText;
    public Transform objectiveListContainer;
    public GameObject objectivePrefab;
    
    [Header("Quest List UI")]
    public GameObject questListPanel;
    public Transform questListContainer;
    public GameObject questListButtonPrefab;
    
    private QuestManager questManager;
    private List<GameObject> objectiveUIObjects = new List<GameObject>();
    private List<GameObject> questListUIObjects = new List<GameObject>();
    
    private void Start()
    {
        // Get QuestManager instance
        questManager = QuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found in scene");
        }
        
        // Hide panels by default
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
        
        if (questListPanel != null)
        {
            questListPanel.SetActive(false);
        }
    }
    
    // Show quest details
    public void ShowQuestDetails(Quest quest)
    {
        if (questPanel == null || questTitleText == null || questDescriptionText == null || objectiveListContainer == null)
        {
            Debug.LogError("QuestUI components not properly assigned");
            return;
        }
        
        // Clear existing objectives
        foreach (GameObject obj in objectiveUIObjects)
        {
            Destroy(obj);
        }
        objectiveUIObjects.Clear();
        
        // Set quest information
        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.description;
        
        // Create objective UI elements
        foreach (Quest.Objective objective in quest.objectives)
        {
            if (objectivePrefab != null)
            {
                GameObject objectiveObj = Instantiate(objectivePrefab, objectiveListContainer);
                Text objectiveText = objectiveObj.GetComponent<Text>();
                if (objectiveText != null)
                {
                    objectiveText.text = objective.isCompleted ? 
                        $"<s>{objective.description}</s>" : 
                        $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
                }
                objectiveUIObjects.Add(objectiveObj);
            }
        }
        
        // Show panel
        questPanel.SetActive(true);
    }
    
    // Hide quest details
    public void HideQuestDetails()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
    
    // Show quest list
    public void ShowQuestList()
    {
        if (questListPanel == null || questListContainer == null || questListButtonPrefab == null)
        {
            Debug.LogError("QuestUI components not properly assigned for quest list");
            return;
        }
        
        // Clear existing quest list
        foreach (GameObject obj in questListUIObjects)
        {
            Destroy(obj);
        }
        questListUIObjects.Clear();
        
        // Create quest list UI elements
        List<Quest> activeQuests = questManager.GetActiveQuests();
        foreach (Quest quest in activeQuests)
        {
            GameObject questButtonObj = Instantiate(questListButtonPrefab, questListContainer);
            Button questButton = questButtonObj.GetComponent<Button>();
            Text questButtonText = questButtonObj.GetComponentInChildren<Text>();
            
            if (questButtonText != null)
            {
                questButtonText.text = quest.questName;
            }
            
            // Add listener to show quest details when clicked
            if (questButton != null)
            {
                questButton.onClick.AddListener(() => ShowQuestDetails(quest));
            }
            
            questListUIObjects.Add(questButtonObj);
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
}