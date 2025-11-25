using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectIcon : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private GameObject characterIconSelectedObj;
    [SerializeField]
    private SpriteRenderer characterSprite;
    [SerializeField]
    private Character character;

    void Start()
    {
        characterSprite.sprite = CharacterManager.instance.GetCharacterIconSprite(character);
        characterIconSelectedObj.SetActive(false);
    }

    private void OnMouseUpAsButton() {
        CharacterManager.instance.ChooseCharacter(character);
    }

    private void OnMouseEnter() {
        characterIconSelectedObj.SetActive(true);
        CharacterManager.instance.SetCharacterSelectInfo(character);
    }

    private void OnMouseExit() {
        characterIconSelectedObj.SetActive(false);
        CharacterManager.instance.ClearCharacterSelectInfo();
    }
}
