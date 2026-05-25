using UnityEngine;
[CreateAssetMenu(fileName ="CharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public int characterId;
    public string characterName;
    [TextArea]
    public string lore;
    public string gender;
    public string role;

    public Sprite icon;
    public Sprite portrait;
    public Sprite tienda;
    public Sprite sombraTienda;

    public bool unlocked;

    public int priceGold;
    public bool purchased;
    public GameObject prefab;

}
