using UnityEngine;
using UnityEngine.UI;

// MainMenuController implements main menu UI with navigation options
public class MainMenuController : MonoBehaviour
{
    // UI elements
    public Button startButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;
    
    // Scene manager
    private SceneManager sceneManager;
    
    private void Start()
    {
        // Get SceneManager instance
        sceneManager = FindObjectOfType<SceneManager>();
        
        if (sceneManager == null)
        {
            Debug.LogError("SceneManager not found in scene");
        }
        
        // Setup button listeners
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
        
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OpenCredits);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }
    
    private void StartGame()
    {
        // Load first game scene
        sceneManager?.LoadGameScene1();
    }
    
    private void OpenSettings()
    {
        // Load settings scene
        sceneManager?.LoadSettings();
    }
    
    private void OpenCredits()
    {
        // Load credits scene
        sceneManager?.LoadCredits();
    }
    
    private void QuitGame()
    {
        // Quit application
        sceneManager?.QuitGame();
    }
    
    private void OnDestroy()
    {
        // Remove button listeners
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartGame);
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveListener(OpenSettings);
        }
        
        if (creditsButton != null)
        {
            creditsButton.onClick.RemoveListener(OpenCredits);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.RemoveListener(QuitGame);
        }
    }
}