using System;
using System.Collections.Generic;
using UnityEngine;

// SkillTreeSystem manages player skills and their progression
public class SkillTreeSystem : MonoBehaviour
{
    // Serializable skill data
    [Serializable]
    public class Skill
    {
        public string skillName;
        public int level = 0;
        public int maxLevel = 10;
        public List<string> prerequisites = new List<string>();
        public bool isUnlocked = false;
        public string description;
    }
    
    // Skills list
    public List<Skill> skills = new List<Skill>();
    
    // Events
    public event Action<Skill> OnSkillUnlocked;
    public event Action<Skill> OnSkillLevelUp;
    
    private GameManager gameManager;
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
        
        // Initialize skills
        foreach (var skill in skills)
        {
            // Check if skill has no prerequisites, if so unlock it
            if (skill.prerequisites.Count == 0)
            {
                skill.isUnlocked = true;
                OnSkillUnlocked?.Invoke(skill);
            }
        }
    }
    
    // Unlock a skill
    public void UnlockSkill(string skillName)
    {
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null && !skill.isUnlocked)
        {
            // Check if all prerequisites are met
            bool allPrerequisitesMet = true;
            foreach (string prerequisite in skill.prerequisites)
            {
                Skill prerequisiteSkill = skills.Find(s => s.skillName == prerequisite);
                if (prerequisiteSkill == null || !prerequisiteSkill.isUnlocked)
                {
                    allPrerequisitesMet = false;
                    break;
                }
            }
            
            // If all prerequisites are met, unlock the skill
            if (allPrerequisitesMet)
            {
                skill.isUnlocked = true;
                OnSkillUnlocked?.Invoke(skill);
            }
        }
    }
    
    // Level up a skill
    public void LevelUpSkill(string skillName)
    {
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null && skill.isUnlocked && skill.level < skill.maxLevel)
        {
            skill.level++;
            gameManager?.IncreaseSkill(skillName);
            OnSkillLevelUp?.Invoke(skill);
        }
    }
}