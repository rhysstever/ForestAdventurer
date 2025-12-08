using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectIcon : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private GameObject characterIconSelectedObj;
    [SerializeField]
    private SpriteRenderer characterSpriteRenderer;
    [SerializeField]
    private Character character;

    void Start()
    {
        characterIconSelectedObj.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        characterIconSelectedObj.SetActive(false);
        CharacterManager.instance.ChooseCharacter(character);
    }

    private void OnMouseEnter() 
    {
        characterIconSelectedObj.SetActive(true);
        CharacterManager.instance.SetCharacterSelectInfo(character);
    }

    private void OnMouseExit() 
    {
        characterIconSelectedObj.SetActive(false);
        CharacterManager.instance.ClearCharacterSelectInfo();
    }
}
