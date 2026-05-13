using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileDisplay : MonoBehaviour
{
    private Character character;

    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Slider healthSlider;


    public void Setup(Character Character)
    {
        character = Character;

        charName.text = character.CharacterName;
        characterPortrait.sprite = character.stats.characterPortrait;

        healthSlider.maxValue = character.stats.maxHealth;
        healthSlider.value = character.currentHealth;
        character.onTakeDamage += UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
