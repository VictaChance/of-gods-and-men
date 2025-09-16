using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestListEntryUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questNameText;
    public Text questStatusText;
    public Button questDetailsButton;
    
    private Quest quest;
    private QuestUI questUI;
    
    private void Start()
    {
        // Find QuestUI component
        questUI = FindObjectOfType<QuestUI>();
        
        if (questUI == null)
        {
            Debug.LogError("QuestUI not found in scene");
        }
        
        // Setup button listener
        if (questDetailsButton != null)
        {
            questDetailsButton.onClick.AddListener(ShowQuestDetails);
        }
    }
    
    // Initialize with quest data
    public void Initialize(Quest q)
    {
        quest = q;
        UpdateQuestListEntry();
    }
    
    // Update quest list entry display
    public void UpdateQuestListEntry()
    {
        if (questNameText != null)
        {
            questNameText.text = quest.questName;
        }
        
        if (questStatusText != null)
        {
            switch (quest.status)
            {
                case Quest.QuestStatus.NotStarted:
                    questStatusText.text = "Not Started";
                    questStatusText.color = Color.gray;
                    break;
                case Quest.QuestStatus.InProgress:
                    questStatusText.text = "In Progress";
                    questStatusText.color = Color.yellow;
                    break;
                case Quest.QuestStatus.Completed:
                    questStatusText.text = "Completed";
                    questStatusText.color = Color.green;
                    break;
                case Quest.QuestStatus.Failed:
                    questStatusText.text = "Failed";
                    questStatusText.color = Color.red;
                    break;
            }
        }
    }
    
    // Show quest details
    private void ShowQuestDetails()
    {
        if (questUI != null && quest != null)
        {
            questUI.ShowQuestDetails(quest);
        }
    }
    
    private void OnDestroy()
    {
        // Remove button listener
        if (questDetailsButton != null)
        {
            questDetailsButton.onClick.RemoveListener(ShowQuestDetails);
        }
    }
}