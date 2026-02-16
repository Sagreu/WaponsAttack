using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    [SerializeField] private GameObject panelBott;
    [SerializeField] private Slider slider;
    private void Start()
    {
        panelBott.SetActive(false);
    }
    private void Awake()
    {
        Instance = this;
    }

    public void MostrarBossUI(int vidaMax, int vidaMin)
    {
        panelBott.SetActive(true);
        slider.value = vidaMax;
    }
    public void ActualizarVida(int vidaActual)
    {
        slider.value = vidaActual;
    }
    public void OcultarPanel()
    {
        panelBott.SetActive(false);
    }
}
