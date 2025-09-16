using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI tipText;

    [TextArea]
    public string[] loadingTips;

    private void Start()
    {
        // Set initial progress
        UpdateProgress(0);

        // Show random tip
        if (loadingTips != null && loadingTips.Length > 0 && tipText != null)
        {
            tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }
    }

    public void UpdateProgress(float progress)
    {
        // Update progress bar
        if (progressBar != null)
        {
            progressBar.value = progress;
        }

        // Update progress text
        if (progressText != null)
        {
            progressText.text = $"Loading: {Mathf.Round(progress * 100)}%";
        }
    }
}