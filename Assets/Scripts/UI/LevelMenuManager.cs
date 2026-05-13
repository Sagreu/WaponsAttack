using UnityEngine;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenuPrefab;
    [SerializeField] private Transform canvasParent;
    [SerializeField] private GameObject currentMenu;

    [SerializeField] private Button BtnTMP;


    public void OpenZone(int zoneId, ZonaPanel zonaPanel)
    {
        if (currentMenu != null)
            Destroy(currentMenu);

        currentMenu = Instantiate(levelMenuPrefab, canvasParent);

        LevelMenu menu = currentMenu.GetComponent<LevelMenu>();
        menu.SetZonaPanel(zonaPanel);
        menu.GenerateLevels(zoneId);
    }


}
