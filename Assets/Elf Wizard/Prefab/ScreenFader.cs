using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image screenFadeImage;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            screenFadeImage.color = new Color(0f, 0f, 0f, alpha);

            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // Ensure the screen is completely black at the end of the fade.
        screenFadeImage.color = Color.black;
    }

    public IEnumerator FadeIn()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            screenFadeImage.color = new Color(0f, 0f, 0f, alpha);

            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // Ensure the screen is completely transparent at the end of the fade.
        screenFadeImage.color = Color.clear;
    }
}
