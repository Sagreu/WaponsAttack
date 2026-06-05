using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SummonAnimationController : MonoBehaviour
{
    public GameObject summonAnimationSingle;
    public GameObject summonAnimationTen;
    public GameObject imagePanel;
    public Image fadeImage;

    public ParticleSystem singleEffect;
    public ParticleSystem[] tenEffects;

    public float fadeDuration = 1f;
    public float delayBetweenTenEffects = 0.15f;
    public float finalDelay = 5f;

    public IEnumerator PlaySingle(WaeponRaririty rarity)
    {
        Debug.Log("1. Inicia PlaySingle");

        imagePanel.SetActive(true);
        summonAnimationSingle.SetActive(true);
        summonAnimationTen.SetActive(false);
        fadeImage.gameObject.SetActive(true);

        Debug.Log("2. Antes del fade entrada");

        yield return StartCoroutine(FadeColor(
            new Color32(0, 0, 0, 255),
            new Color32(255, 255, 255, 255)
        ));

        Debug.Log("3. Terminó fade entrada");

        SetEffectColor(singleEffect, rarity);
        singleEffect.gameObject.SetActive(true);
        singleEffect.Play(true);

        Debug.Log("4. Partículas activadas");

        yield return new WaitForSeconds(finalDelay);

        Debug.Log("5. Antes del fade salida");

        yield return StartCoroutine(FadeColor(
            new Color32(255, 255, 255, 255),
            new Color32(0, 0, 0, 255)
        ));

        Debug.Log("6. Terminó fade salida");

        singleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        singleEffect.gameObject.SetActive(false);
        summonAnimationSingle.SetActive(false);
        summonAnimationTen.SetActive(false);
        imagePanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);

        Debug.Log("7. Termina PlaySingle");
    }

    public IEnumerator PlayTen(WaeponRaririty[] rarities)
    {
        imagePanel.SetActive(true);
        summonAnimationTen.SetActive(true);
        summonAnimationSingle.SetActive(false);

        foreach (ParticleSystem effect in tenEffects)
        {
            effect.gameObject.SetActive(false);
        }

        yield return StartCoroutine(FadeColor(
            new Color32(0, 0, 0, 255),
            new Color32(255, 255, 255, 255)
        ));

        for (int i = 0; i < tenEffects.Length; i++)
        {
            SetEffectColor(tenEffects[i], rarities[i]);
            tenEffects[i].gameObject.SetActive(true);
            tenEffects[i].Play(true);

            yield return new WaitForSeconds(delayBetweenTenEffects);
        }

        yield return new WaitForSeconds(finalDelay);
        
        foreach (ParticleSystem effect in tenEffects)
        {
            effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            effect.gameObject.SetActive(false);
        }
        yield return StartCoroutine(FadeColor(
            new Color32(255, 255, 255, 255),
            new Color32(0, 0, 0, 255)
        ));

        
        imagePanel.SetActive(false);
        summonAnimationTen.SetActive(false);
    }

    IEnumerator FadeColor(Color from, Color to)
    {
        if (fadeDuration <= 0)
        {
            fadeImage.color = to;
            yield break;
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = Color.Lerp(from, to, t);
            yield return null;
        }

        fadeImage.color = to;
    }

    Color GetRarityColor(WaeponRaririty rarity)
    {
        switch (rarity)
        {
            case WaeponRaririty.Common:
                return Color.gray;

            case WaeponRaririty.Rare:
                return Color.blue;

            case WaeponRaririty.Epic:
                return new Color(0.6f, 0f, 1f);

            case WaeponRaririty.Legendary:
                return new Color(1f, 0.5f, 0f);

            case WaeponRaririty.Mythic:
                return Color.red;

            default:
                return Color.white;
        }
    }

    void SetEffectColor(ParticleSystem effect, WaeponRaririty rarity)
    {
        Color color = GetRarityColor(rarity);

        ParticleSystem[] systems = effect.GetComponentsInChildren<ParticleSystem>(true);

        foreach (ParticleSystem ps in systems)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }
}