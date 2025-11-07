using UnityEngine;

public readonly struct Character
{
    public Character(string name, int mainHandCardCount, int offHandCardCount, int allyCardCount, int spiritCardCount, int spellCardCount, int drinkCardCount, Sprite sprite) {
        Name = name;
        MainHandCardCount = mainHandCardCount;
        OffHandCardCount = offHandCardCount;
        AllyCardCount = allyCardCount;
        SpiritCardCount = spiritCardCount;
        SpellCardCount = spellCardCount;
        DrinkCardCount = drinkCardCount;
        Sprite = sprite;
    }

    public Character(string name, int mainHandCardCount, int offHandCardCount, int allyCardCount, int spiritCardCount, int spellCardCount, int drinkCardCount)
        : this(name, mainHandCardCount, offHandCardCount, allyCardCount, spiritCardCount, spellCardCount, drinkCardCount, null) { }

    public string Name { get; }
    public int MainHandCardCount { get; }
    public int OffHandCardCount { get; }
    public int AllyCardCount { get; }
    public int SpiritCardCount { get; }
    public int SpellCardCount { get; }
    public int DrinkCardCount { get; }
    public Sprite Sprite { get; }
}
