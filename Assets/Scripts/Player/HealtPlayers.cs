using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealtPlayers : MonoBehaviour
{
    [Header("Datos de vida")]
    [SerializeField] private int vidaTotal = 100;
    [SerializeField] private int vidaActual;
    bool estaMuerto = false;

    [SerializeField] private Slider barraVida;
    private void Start()
    {
        barraVida = GameObject.Find("PanelGeneral/TopPanel/BarraVida").GetComponent<Slider>();
    }
    void Awake()

    {
        vidaActual = vidaTotal;
    }
    private void Update()
    {
        barraVida.value = vidaActual;
    }

    public void ResivirDano(int dano)
    {
        if (estaMuerto) return;

        vidaActual -= dano;
        vidaActual = Math.Clamp(vidaActual, 0, vidaTotal);

        print("Vida Total " + vidaTotal + " Dano resivido " + dano + " vida actual " + vidaActual);

        

        if (vidaActual <= 0)
        {
           StartCoroutine(EstasMuerto());
        }
    }

    private IEnumerator EstasMuerto()
    {
        estaMuerto = true;
        print("El player a muerto");

        yield return null;
        Time.timeScale = 0f;
        GameObject.Destroy(gameObject);
       
    }
}
 