using System;
using System.Linq;
using UnityEngine;

public enum Character
{
    Badger,
    Beaver,
    Fox,
    Opossum,
    Otter,
    Skunk
}

public class CharacterManager : MonoBehaviour
{
    // Singleton
    public static CharacterManager instance = null;

    // Set in inspector
    [SerializeField]
    private Transform characterSelectIconParent;
    [SerializeField]
    private Sprite badgerSpriteIcon, beaverSpriteIcon, foxSpriteIcon, opossumSpriteIcon, otterSpriteIcon, skunkSpriteIcon;

    // Set at Start
    private Character chosenCharacter;

    public Character ChosenCharacter { get { return chosenCharacter; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        HideCharacterSelectIcons();
    }

    public void ShowCharacterSelectIcons() {
        characterSelectIconParent.gameObject.SetActive(true);
    }

    public void HideCharacterSelectIcons() {
        characterSelectIconParent.gameObject.SetActive(false);
    }

    public void ChooseCharacter(Character character) {
        if(!string.IsNullOrEmpty(character.ToString())) {
            chosenCharacter = character;
        } else {
            Character firstCharacter = Enum.GetValues(typeof(Character)).Cast<Character>().ToList().First();
            Debug.LogFormat("Error! No character found with name: {0}. Defaulting to {1}", name, firstCharacter.ToString());
            chosenCharacter = firstCharacter;
        }
    }

    public int GetSlotCardCountOfChosenCharacter(Slot slotType) {
        return slotType switch {
            Slot.MainHand => GetCharacterDeckStructure(chosenCharacter)[0],
            Slot.OffHand => GetCharacterDeckStructure(chosenCharacter)[1],
            Slot.Ally => GetCharacterDeckStructure(chosenCharacter)[2],
            Slot.Spirit => GetCharacterDeckStructure(chosenCharacter)[3],
            Slot.Spell => GetCharacterDeckStructure(chosenCharacter)[4],
            Slot.Drink => GetCharacterDeckStructure(chosenCharacter)[5],
            _ => 0,
        };
    }

    public int[] GetCharacterDeckStructure(Character character) {
        return character switch {
            Character.Badger => new int[] { 4, 4, 3, 3, 2, 2 },
            Character.Beaver => new int[] { 3, 4, 3, 2, 2, 4 },
            Character.Fox => new int[] { 3, 2, 2, 4, 4, 3 },
            Character.Opossum => new int[] { 3, 2, 4, 3, 4, 2 },
            Character.Otter => new int[] { 4, 2, 2, 4, 3, 3 },
            Character.Skunk => new int[] { 2, 3, 2, 4, 3, 4 },
            _ => null,
        };
    }

    public Sprite GetCharacterSprite(Character character) {
        return character switch {
            Character.Badger => badgerSpriteIcon,
            Character.Beaver => beaverSpriteIcon,
            Character.Fox => foxSpriteIcon,
            Character.Opossum => opossumSpriteIcon,
            Character.Otter => otterSpriteIcon,
            Character.Skunk => skunkSpriteIcon,
            _ => null,
        };
    }
}
