using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    [SerializeField] private GameObject actionsMenu;
    [SerializeField] private TextMeshProUGUI activeCharacterName;
    [SerializeField] private GameObject initiativeMenu; //init menu

    [SerializeField] private List<CharacterProfileDisplay> displays = new List<CharacterProfileDisplay>();

    [SerializeField] private CharacterProfileDisplay displayerPrefab;

    private void Awake()
    {
        BattleManager.battleStart += StartBattle;
    }

    private void StartBattle(List<Character> characters)
    {
        UpdateCards();
    }

    public void UpdateCards()
    {
        for(int i = 0; i < displays.Count; i++)
        {
            Destroy(displays[i].gameObject);
        }

        displays.Clear();

        for (int i = 0; i < BattleManager.instance.GetInitiative().Count; i++)
        {
            CharacterProfileDisplay newCard = Instantiate(displayerPrefab);
            newCard.transform.parent = initiativeMenu.transform;
            newCard.Setup(BattleManager.instance.GetInitiative()[i].stats);
            displays.Add(newCard);
        }

        if (BattleManager.instance.activeCharacter as PlayerCharacter)
        {
            activeCharacterName.text = BattleManager.instance.activeCharacter.stats.characterName;
            actionsMenu.SetActive(true);
        }
        else
        {
            actionsMenu.SetActive(false);
        }

    }
}
