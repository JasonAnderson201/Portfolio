using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileDisplay : MonoBehaviour
{
    private CharacterStats character;

    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private Image characterPortrait;


    public void Setup(CharacterStats Character)
    {
        character = Character;

        charName.text = character.characterName;
        characterPortrait.sprite = character.characterPortrait;
    }
}
