using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private Image fadeImage;

    public void Initialize(Color fadeColor)
    {
        // Create image that covers the screen
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(transform, false);

        // Add and configure image component
        fadeImage = imageObject.AddComponent<Image>();
        fadeImage.color = fadeColor;
        fadeImage.raycastTarget = false;

        // Set to cover the entire screen
        RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        // Start fully transparent
        Color startColor = fadeColor;
        startColor.a = 0;
        fadeImage.color = startColor;
    }

    public System.Collections.IEnumerator FadeOut(float duration)
    {
        float startTime = Time.time;
        Color startColor = fadeImage.color;
        Color targetColor = startColor;
        targetColor.a = 1;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadeImage.color = targetColor;
    }

    public System.Collections.IEnumerator FadeIn(float duration)
    {
        float startTime = Time.time;
        Color startColor = fadeImage.color;
        Color targetColor = startColor;
        targetColor.a = 0;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadeImage.color = targetColor;
    }
}
