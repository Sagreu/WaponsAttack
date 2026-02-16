using System.Collections;
using TMPro;
using UnityEngine;

public class WaveUi : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveTitle;
    public TextMeshProUGUI waveLeft;


    public void ShowWaveTitle(int wave, int currentState)
    {
        currentState++;
        waveTitle.text = $"OLEADA {wave}" + " State " + currentState;
        waveTitle.gameObject.SetActive(true);
        //StartCoroutine(ShowTextCoroutine(message, duration));
    }
    public void HideWaveTitle()
    {
        waveTitle.gameObject.SetActive(false);
    }

    public void ShowStatus(string text)
    {
        waveText.text = text;
        waveText.gameObject.SetActive(true);
    }

    public void HideStatus()
    {
        waveText.text = "";
    }

    public void UpdateEnemiesLeft(int count)
    {
        waveLeft.text = $"ENEMIGOS: {count}";
    }
    public void HidenTitle()
    {
        waveTitle.text = "";
    }

    IEnumerator ShowTextCoroutine(string message, float duration)
    {
        waveText.text = message;
        waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        waveText.gameObject.SetActive(false);
    }


}