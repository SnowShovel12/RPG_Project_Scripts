using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    public IEnumerator FadeOut(float fadeDuration)
    {
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
    }

    public IEnumerator FadeIn(float fadeDuration)
    {
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float from, float to, float fadeDuration)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
