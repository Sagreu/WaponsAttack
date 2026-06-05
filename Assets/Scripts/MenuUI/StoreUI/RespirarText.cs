using System.Collections;
using UnityEngine;

public class RespirarText : MonoBehaviour
{
    [SerializeField] private RectTransform tapToContinue;

    private Coroutine pulseRoutine;

    private void OnEnable()
    {
        pulseRoutine = StartCoroutine(PulseText());
    }

    private void OnDisable()
    {
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        tapToContinue.localScale = Vector3.one;
    }

    IEnumerator PulseText()
    {
        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * 1.1f;

        while (true)
        {
            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime * 1.5f;
                tapToContinue.localScale =
                    Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            t = 0;

            while (t < 1)
            {
                t += Time.deltaTime * 1.5f;
                tapToContinue.localScale =
                    Vector3.Lerp(targetScale, startScale, t);
                yield return null;
            }
        }
    }
}