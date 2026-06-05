using UnityEngine;


[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private int id;
    public int ID => id;

    public string weaponName;
    public Sprite sprite;
    public Sprite iconRare;

    [Header("Stats")]
    public int damage = 10;

    public bool unlocked;

    [TextArea]
    public string description;

    [TextArea]
    public string history;

    public WaeponRaririty rarity;
    public bool obtenibleGacha;
    public Sprite elementIcon;
    public int elementalDagame;
    public int velocityAtack;
    public Sprite UI;
    public string elemental;
    public Sprite raresaImg;

}

public enum WaeponRaririty
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}