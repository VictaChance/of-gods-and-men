using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestObjectiveUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text objectiveText;
    public Image objectiveCompletionIcon;
    
    [Header("Visual Settings")]
    public Sprite completedIcon;
    public Sprite incompletedIcon;
    public Color completedColor = Color.green;
    public Color incompletedColor = Color.white;
    
    private Quest.Objective objective;
    
    // Initialize with objective data
    public void Initialize(Quest.Objective obj)
    {
        objective = obj;
        UpdateObjectiveDisplay();
    }
    
    // Update objective display based on completion status
    public void UpdateObjectiveDisplay()
    {
        if (objectiveText != null)
        {
            // Show objective as strikethrough if completed
            objectiveText.text = objective.isCompleted ? 
                $"<s>{objective.description}</s>" : 
                objective.description;
            
            // Update text color
            objectiveText.color = objective.isCompleted ? completedColor : incompletedColor;
        }
        
        if (objectiveCompletionIcon != null)
        {
            // Update completion icon
            objectiveCompletionIcon.sprite = objective.isCompleted ? completedIcon : incompletedIcon;
            
            // Enable/disable icon based on assignment
            objectiveCompletionIcon.enabled = objective.isCompleted ? completedIcon != null : incompletedIcon != null;
        }
    }
}