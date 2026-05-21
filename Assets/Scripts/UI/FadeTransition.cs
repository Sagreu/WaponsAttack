using System.Collections;
using NUnit.Framework.Constraints;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 2f;


    public void fadeToZonas()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeImage no está asignada");
            return;
        }
        StartCoroutine(FadeRuntime());
    }
    IEnumerator FadeRuntime()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public void fadeToHome()
    {
        StartCoroutine(fadeToHomeTransition());
    }

    IEnumerator fadeToHomeTransition()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0,0,0, alpha);
            yield return null;
        }
    }
}
