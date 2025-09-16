using System;
using System.Collections.Generic;
using UnityEngine;

// BeastTamingSystem manages beast taming mechanics
public class BeastTamingSystem : MonoBehaviour
{
    // Serializable beast data
    [Serializable]
    public class Beast
    {
        public string beastName;
        public int level = 1;
        public int maxLevel = 10;
        public bool isTamed = false;
        public List<string> abilities = new List<string>();
        public string description;
    }
    
    // Beasts list
    public List<Beast> beasts = new List<Beast>();
    
    // Events
    public event Action<Beast> OnBeastTamed;
    public event Action<Beast> OnBeastUntamed;
    public event Action<Beast> OnBeastLeveledUp;
    
    private GameManager gameManager;
    
    private void Start()
    {
        // Get GameManager instance
        gameManager = GameManager.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene");
        }
    }
    
    // Tame a beast
    public void TameBeast(string beastName)
    {
        Beast beast = beasts.Find(b => b.beastName == beastName);
        if (beast != null && !beast.isTamed)
        {
            beast.isTamed = true;
            gameManager?.TameBeast(beastName);
            OnBeastTamed?.Invoke(beast);
        }
    }
    
    // Untame a beast
    public void UntameBeast(string beastName)
    {
        Beast beast = beasts.Find(b => b.beastName == beastName);
        if (beast != null && beast.isTamed)
        {
            beast.isTamed = false;
            gameManager?.UntameBeast();
            OnBeastUntamed?.Invoke(beast);
        }
    }
    
    // Level up a tamed beast
    public void LevelUpTamedBeast(string beastName)
    {
        Beast beast = beasts.Find(b => b.beastName == beastName);
        if (beast != null && beast.isTamed && beast.level < beast.maxLevel)
        {
            beast.level++;
            OnBeastLeveledUp?.Invoke(beast);
        }
    }
}