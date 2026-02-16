using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite sprite;
    [Header("Stats")]
    public int damage = 10;
    public int durability = 20;

    [Tooltip("0 = no tiene tiempo de vida")]
    public float lifeTime = 0;
}
