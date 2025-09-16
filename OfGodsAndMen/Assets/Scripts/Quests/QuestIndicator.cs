using System;
using UnityEngine;

public class QuestIndicator : MonoBehaviour
{
    [Header("Indicator Settings")]
    public string questID;
    public string objectiveID;
    public IndicatorType type = IndicatorType.QuestGiver;
    public bool isInteractable = true;
    
    [Header("Visual Elements")]
    public GameObject indicatorObject;
    public Material questAvailableMaterial;
    public Material questInProgressMaterial;
    public Material questCompletedMaterial;
    
    private QuestManager questManager;
    private Renderer indicatorRenderer;
    
    public enum IndicatorType
    {
        QuestGiver,
        ObjectiveLocation,
        RewardLocation
    }
    
    private void Start()
    {
        // Get QuestManager instance
        questManager = QuestManager.Instance;
        
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found in scene");
            return;
        }
        
        // Get renderer component
        indicatorRenderer = indicatorObject?.GetComponent<Renderer>();
        
        if (indicatorRenderer == null)
        {
            Debug.LogError("Indicator object or renderer component not assigned");
            return;
        }
        
        // Set initial material
        UpdateIndicatorMaterial();
    }
    
    private void Update()
    {
        // Update indicator material every frame
        UpdateIndicatorMaterial();
    }
    
    // Update indicator material based on quest status
    private void UpdateIndicatorMaterial()
    {
        if (indicatorRenderer == null || questManager == null) return;
        
        Quest quest = questManager.GetQuest(questID);
        
        if (quest == null)
        {
            // Quest not available
            if (questAvailableMaterial != null)
            {
                indicatorRenderer.material = questAvailableMaterial;
            }
            return;
        }
        
        switch (quest.status)
        {
            case Quest.QuestStatus.NotStarted:
                if (questAvailableMaterial != null)
                {
                    indicatorRenderer.material = questAvailableMaterial;
                }
                break;
            case Quest.QuestStatus.InProgress:
                // Check if this objective is completed
                Quest.Objective objective = quest.objectives.Find(o => o.objectiveID == objectiveID);
                if (objective != null && objective.isCompleted)
                {
                    if (questCompletedMaterial != null)
                    {
                        indicatorRenderer.material = questCompletedMaterial;
                    }
                }
                else
                {
                    if (questInProgressMaterial != null)
                    {
                        indicatorRenderer.material = questInProgressMaterial;
                    }
                }
                break;
            case Quest.QuestStatus.Completed:
                if (questCompletedMaterial != null)
                {
                    indicatorRenderer.material = questCompletedMaterial;
                }
                break;
            case Quest.QuestStatus.Failed:
                // You could add a failed material here if needed
                if (questAvailableMaterial != null)
                {
                    indicatorRenderer.material = questAvailableMaterial;
                }
                break;
        }
    }
    
    // Handle player interaction with indicator
    private void OnTriggerEnter(Collider other)
    {
        if (isInteractable && other.CompareTag("Player"))
        {
            // Handle interaction based on indicator type
            switch (type)
            {
                case IndicatorType.QuestGiver:
                    // Find QuestGiver component and give quest
                    QuestGiver questGiver = GetComponent<QuestGiver>();
                    if (questGiver != null && questGiver.CanGiveQuest())
                    {
                        questGiver.GiveQuest();
                    }
                    break;
                case IndicatorType.ObjectiveLocation:
                    // Complete objective
                    Quest quest = questManager.GetQuest(questID);
                    if (quest != null && quest.status == Quest.QuestStatus.InProgress)
                    {
                        quest.ProgressObjective(objectiveID);
                    }
                    break;
                case IndicatorType.RewardLocation:
                    // Complete quest and award rewards
                    quest = questManager.GetQuest(questID);
                    if (quest != null && quest.status == Quest.QuestStatus.InProgress && quest.AreAllObjectivesCompleted())
                    {
                        questManager.CompleteQuest(questID);
                    }
                    break;
            }
        }
    }
}