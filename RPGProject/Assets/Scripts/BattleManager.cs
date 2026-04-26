using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BattleManager : MonoBehaviour
{
    public static event Action<List<Character>> battleStart;

    public BattleMenu battleMenu;

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
        activeCharacter.onCharacterUseAction += OnUseAction;
        battleStart?.Invoke(characters);

        activeCharacter.SetIsActiveCharacter();
    }

    private void OnUseAction(ActionData data)
    {
        if (initiative.Contains(data.user))
        {
            data.user.onCharacterUseAction -= OnUseAction;
            initiative.Remove(data.user);
            initiative.Add(data.user);
        }

        activeCharacter = initiative[0];
        activeCharacter.onCharacterUseAction += OnUseAction;
        activeCharacter.SetIsActiveCharacter();


        battleMenu.UpdateCards();
    }

    public List<Character> GetInitiative() {  return initiative; }
}
