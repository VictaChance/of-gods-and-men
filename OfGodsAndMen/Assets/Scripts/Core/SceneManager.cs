using UnityEngine;
using UnityEngine.SceneManagement;

// SceneManager handles scene navigation between main menu, game scenes, settings, credits
public class SceneManager : MonoBehaviour
{
    // Scene names
    private const string MainMenuScene = "MainMenu";
    private const string GameScene1 = "GameScene1";
    private const string GameScene2 = "GameScene2";
    private const string GameScene3 = "GameScene3";
    private const string SettingsScene = "Settings";
    private const string CreditsScene = "Credits";
    
    // Load main menu
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuScene);
    }
    
    // Load game scenes
    public void LoadGameScene1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScene1);
    }
    
    public void LoadGameScene2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScene2);
    }
    
    public void LoadGameScene3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScene3);
    }
    
    // Load settings
    public void LoadSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SettingsScene);
    }
    
    // Load credits
    public void LoadCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CreditsScene);
    }
    
    // Quit application
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}