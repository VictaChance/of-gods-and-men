using System;
using System.Collections.Generic;
using UnityEngine;

// SettlementSystem manages settlements and their improvement
public class SettlementSystem : MonoBehaviour
{
    // Serializable settlement data
    [Serializable]
    public class Settlement
    {
        public string settlementName;
        public int level = 0;
        public int maxLevel = 5;
        public List<string> improvements = new List<string>();
        public string description;
    }
    
    // Settlements list
    public List<Settlement> settlements = new List<Settlement>();
    
    // Events
    public event Action<Settlement> OnSettlementImproved;
    
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
    
    // Improve a settlement
    public void ImproveSettlement(string settlementName)
    {
        Settlement settlement = settlements.Find(s => s.settlementName == settlementName);
        if (settlement != null && settlement.level < settlement.maxLevel)
        {
            settlement.level++;
            gameManager?.ImproveSettlement(settlementName);
            OnSettlementImproved?.Invoke(settlement);
        }
    }
}