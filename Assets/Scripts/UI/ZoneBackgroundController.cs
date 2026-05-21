using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ZoneBackgroundController : MonoBehaviour
{
    public static ZoneBackgroundController Instance;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float fadeSpeed = 2f;

    [Header("Sprites")]
    [SerializeField] private Sprite forestSprite;
    // [SerializeField] private Sprite desertSprite;
    //[SerializeField] private Sprite snowSprite;
    //[Header("Particles")]
    //[SerializeField] private GameObject rainParticles;
    //[SerializeField] private Sprite snowSprite;
    [SerializeField] private FadeTransition fadeTransition;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeBackgroundController(int zoneId)
    {
        //rainParticles.SetActive(false);
        switch (zoneId)
        {
            case 1:
                //fadeTransition.fadeToHome();

                //backgroundImage.gameObject.SetActive(true);

                backgroundImage.sprite = forestSprite;
                fadeTransitionAlpha();
                fadeTransition.fadeToHome();   

                // rainParticles.SetActive (false);
                break;
            case 2:
                backgroundImage.sprite = forestSprite;
              //  rainParticles.SetActive(false);
                break;
        }
    }
    public void RestoredDefaultBackground()
    {
        backgroundImage.gameObject.SetActive(false);
        fadeToImage();
        
       

    }
    public void fadeToImage()
    {
        StartCoroutine(fadeToImageTransition());
    }
    IEnumerator fadeToImageTransition()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            backgroundImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
    public void fade()
    {
        StartCoroutine(fadeTransitionAlpha());
    }
    IEnumerator fadeTransitionAlpha()
    {
        float alpha = 0;
        while (alpha > 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            backgroundImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
