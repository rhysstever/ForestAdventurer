using UnityEngine;

public class Character
{
    private string name;
    private int mainHandCardCount;
    private int offHandCardCount;
    private int allyCardCount;
    private int spiritCardCount;
    private int spellCardCount;
    private int drinkCardCount;
    private Sprite sprite;

    public string Name {  get { return name; } }
    public int MainHandCardCount { get { return mainHandCardCount; } }
    public int OffHandCardCount { get { return offHandCardCount; } }
    public int AllyCardCount { get { return allyCardCount; } }
    public int SpiritCardCount { get { return spiritCardCount; } }
    public int SpellCardCount { get { return spellCardCount; } }
    public int DrinkCardCount { get { return drinkCardCount; } }
    public Sprite Sprite { get { return sprite; } }

    public Character(string name, int mainHandCardCount, int offHandCardCount, int allyCardCount, int spiritCardCount, int spellCardCount, int drinkCardCount, Sprite sprite) {
        this.name = name;
        this.mainHandCardCount = mainHandCardCount;
        this.offHandCardCount = offHandCardCount;
        this.allyCardCount = allyCardCount;
        this.spiritCardCount = spiritCardCount;
        this.spellCardCount = spellCardCount;
        this.drinkCardCount = drinkCardCount;
        this.sprite = sprite;
    }

    public Character(string name, int mainHandCardCount, int offHandCardCount, int allyCardCount, int spiritCardCount, int spellCardCount, int drinkCardCount)
        : this(name, mainHandCardCount, offHandCardCount, allyCardCount, spiritCardCount, spellCardCount, drinkCardCount, null) { }
}
