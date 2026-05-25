using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
[CreateAssetMenu(menuName = "Gacha/Weapon Banner")]
public class WeaponDataBanner : ScriptableObject
{
    public int Id;
    [Header("Informacion Banner")]
    public string bannerName;

    [TextArea]
    public string description;
    [Header("Nombre de la serie del banner 'Forjadas con las partes de un dragon carmesi'")]
    [TextArea]
    public string serie;
    public Sprite bannerImage;

    [Header("WeaponsBy Rarity")]
    public List<WeaponData> legendaryWeapons;
    public List<WeaponData> mythicWeapons;
}
