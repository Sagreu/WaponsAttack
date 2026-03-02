using UnityEngine;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenuPrefab;
    [SerializeField]private Transform canvasParent;
    [SerializeField] private GameObject currentMenu;

    public void OpenZone(int zoneId)
    {
        if(currentMenu != null)
            Destroy(currentMenu);

        currentMenu = Instantiate(levelMenuPrefab, canvasParent);

        LevelMenu menu = currentMenu.GetComponent<LevelMenu>();
        menu.GenerateLevels(zoneId);
    }
}
