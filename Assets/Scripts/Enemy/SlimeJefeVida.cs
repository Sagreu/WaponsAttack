using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlimeJefeVida : MonoBehaviour
{
    [SerializeField] private int vidaJefe = 100;
    [SerializeField] private int vidaActualJefe;
    bool jefeMuerto = false;
    public event Action OnDead;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BossUIManager.Instance.MostrarBossUI(vidaJefe, vidaActualJefe);
    }
    private void Awake()
    {
        vidaActualJefe = vidaJefe;
    }

    // Update is called once per frame


    public void resivirDano(int dano)
    {
        if (jefeMuerto) return;
        vidaActualJefe -= dano;

        vidaActualJefe = Math.Clamp(vidaActualJefe, 0, vidaJefe);
        BossUIManager.Instance.ActualizarVida(vidaActualJefe);


        print("Vida Total " + vidaJefe + " Dano resivido " + dano + " vida actual " + vidaActualJefe);

        if (vidaActualJefe <= 0)
        {
            EstasMuerto();
        }
    }

    public void EstasMuerto()
    {
        jefeMuerto = true;
        print("El jefe a muerto");
        BossUIManager.Instance.OcultarPanel();
        OnDead?.Invoke();

        Destroy(gameObject);

    }

}
