using UnityEngine;

// TestSceneController provides test controls for verification
public class TestSceneController : MonoBehaviour
{
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
    
    private void Update()
    {
        // Test controls
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Increase level
            gameManager?.IncreaseLevel();
            Debug.Log($"Player level increased to {gameManager?.playerLevel}");
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Add gold
            gameManager?.AddGold(10);
            Debug.Log($"Added 10 gold. Total gold: {gameManager?.gold}");
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Increase corruption
            gameManager?.IncreaseCorruption();
            Debug.Log($"Corruption increased to {gameManager?.corruption}");
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Tame beast
            gameManager?.TameBeast("Wolf");
            Debug.Log($"Beast tamed: {gameManager?.tamedBeast}");
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Next scene
            LoadNextScene();
        }
    }
    
    private void LoadNextScene()
    {
        // Load next scene in sequence
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // If we're at the end, loop back to the first game scene
        if (nextSceneIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1; // Assuming scene 0 is MainMenu
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
}