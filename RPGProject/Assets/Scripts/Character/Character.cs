using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IComparable<Character>
{
    public event Action<Character> onCharacterPass;
    public event Action<ActionData> onCharacterUseAction;
    public event Action<int> onTakeDamage;

    [Header("Local refs")]
    public CharacterStats stats;
    public List<StatusEffectData> activeEffects = new List<StatusEffectData>();

    public string CharacterName { get { return stats.characterName; } }

    //preset stats
    public Stat Strength { get { return FindStat("Strength"); } }
    public Stat Defence { get { return FindStat("Defence"); } }
    public Stat Dexterity { get { return FindStat("Dexterity"); } }


    public int currentHealth;
    public int actionPoints;
    public List<Action> actions = new List<Action>();
    public List<Item> items = new List<Item>();

    private void Start()
    {
        stats = Instantiate(stats);

        currentHealth = stats.maxHealth;
    }

    public int CompareTo(Character other)
    {
        return other.Dexterity.statValue.CompareTo(Dexterity.statValue);
    }

    public void TakeDamage(int damage, DamageType dmgType)
    {
        int calculatedDamage = damage;

        //check if healing
        if(calculatedDamage > 0)
        calculatedDamage -= dmgType == DamageType.Physical ? Defence.statValue : 0;

        currentHealth = Mathf.Clamp(currentHealth - calculatedDamage, 0, stats.maxHealth);

        if (currentHealth <= 0)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        onTakeDamage?.Invoke(currentHealth);
    }

    public void ModifyStat(string statToModify, int modifyAmount, int duration = 0)
    {
        FindStat(statToModify).statValue += modifyAmount;
    }

    public ref Stat FindStat(string StatName)
    {
        for(int i = 0; i < stats.stats.Length; i++)
        {
            if (stats.stats[i].statName == StatName)
                    return ref stats.stats[i];
        }

        return ref stats.stats[0];
    }

    public virtual void SetIsActiveCharacter()
    {
        foreach(StatusEffectData effect in activeEffects)
        {
            effect.effect.TickEffect(out bool canRemove);
            if(canRemove)
                activeEffects.Remove(effect);
        }

        actionPoints += FindStat("WillPower").statValue;
        actionPoints = Mathf.Clamp(actionPoints, 0, 8);
    }

    public virtual void DoAction(ActionData data)
    {
        actionPoints -= data.action.pointCost;
        onCharacterUseAction?.Invoke(data);
    }

    public void ApplyStatusEffect(StatusEffectData data)
    {
        //check if effect is already applied and applied less than max amount
        if (activeEffects.Contains(data))
        {
            if (!data.effect.stackable)
                return;

            int appliedCount = 0;
            for(int i = 0; i < activeEffects.Count; i++)
            {
                if (activeEffects[i].effect.effectName == data.effect.effectName)
                {
                    appliedCount++;

                    if (appliedCount > data.effect.maxStacks)
                        return;
                }
            }
        }

        data.effect = data.effect.Clone(data.target);

        activeEffects.Add(data);

        data.effect.OnApplyEffect();
    }

    public void PassTurn()
    {
        onCharacterPass?.Invoke(this);
    }
}
