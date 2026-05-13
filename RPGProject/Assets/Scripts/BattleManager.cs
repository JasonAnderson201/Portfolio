using System;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    public static event Action<List<Character>> battleStart;

    public BattleMenu battleMenu;
    public ActionPointUI actionUI;

    public Character[] enemies;
    public PlayerCharacter[] party;

    [SerializeField] private List<Character> initiative = new List<Character>();
    public Character activeCharacter;

    public static BattleManager instance;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private bool battleStarted;
    private void Update()
    {
        if (battleStarted)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            battleStarted = true;
            List<Character> characters = new List<Character>();
            characters.AddRange(enemies);

            BattleStart(characters);
        }
    }

    public void BattleStart(List<Character> enemies)
    {
        List<Character> characters = new List<Character>();

        characters.AddRange(enemies);
        characters.AddRange(party);
        
        characters.Sort();

        initiative = characters;
        activeCharacter = initiative[0];
        activeCharacter.onCharacterPass += OnPassTurn;
        battleStart?.Invoke(characters);

        activeCharacter.SetIsActiveCharacter();

        if(activeCharacter as PlayerCharacter)
        {
            actionUI.UpdateUI();
        }
    }

    private void OnPassTurn(Character character)
    {
        if (initiative.Contains(character))
        {
            character.onCharacterPass -= OnPassTurn;
            initiative.Remove(character);
            initiative.Add(character);
        }

        for(int i = 0; i < initiative.Count; i++)
        {
            if (initiative[i].currentHealth <= 0)
                initiative.Remove(initiative[i]);
        }

        activeCharacter = initiative[0];
        activeCharacter.onCharacterPass += OnPassTurn;
        activeCharacter.SetIsActiveCharacter();


        battleMenu.UpdateCards();

        if (activeCharacter as PlayerCharacter)
        {
            actionUI.UpdateUI();
        }
    }

    public List<Character> GetInitiative() {  return initiative; }
}
