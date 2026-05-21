using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
[CreateAssetMenu(fileName ="GameDataBase", menuName ="DataBase/GameDataBase")]
public class GameDataBase : ScriptableObject
{
    public List<CharacterData> characters;
    public List<WeaponData> weapons;
    public List<LevelData> levels;
}
