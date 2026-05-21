using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
 public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite sprite;
    public Sprite marco;
    [Header("Stats")]
    public int damage = 10;
    public int durability = 20;

    [Tooltip("0 = no tiene tiempo de vida")]
    public float lifeTime = 0;
    
    //aditional data
    public bool unlocked;
    public int id;
    [TextArea]
    public string description;
    [TextArea]
    public string history;
    //public string rareza;
    public WaeponRaririty rarity;
    public bool obtenibleGacha;

}


public enum WaeponRaririty
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}
