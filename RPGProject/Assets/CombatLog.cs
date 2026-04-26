using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CombatLog : MonoBehaviour
{
    [SerializeField] private CombatEntry combatLogEntryPrefab;
    [SerializeField] private List<CombatEntry> combatEntries;

    private List<Character> characters = new List<Character>();

    private void Awake()
    {
        BattleManager.battleStart += Setup;
    }
    private void OnDestroy()
    {
        BattleManager.battleStart -= Setup;

        foreach (Character Character in characters)
        {
            Character.onCharacterUseAction -= AddEntry;
        }
    }

    private void Setup(List<Character> Characters)
    {
        if(combatEntries.Count > 0)
        {
            for (int i = 0; i < combatEntries.Count; i++)
            {
                Destroy(combatEntries[i].gameObject);
            }
            combatEntries.Clear();
        }


        characters.AddRange(Characters);

        foreach(Character character in Characters)
        {
            character.onCharacterUseAction += AddEntry;
        }
    }

    private void AddEntry(ActionData data)
    {
        CombatEntry newEntry = Instantiate(combatLogEntryPrefab, transform);
        newEntry.Setup(data);

        combatEntries.Add(newEntry);
    }
}
