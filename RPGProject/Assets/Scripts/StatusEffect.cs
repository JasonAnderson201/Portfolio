using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Data/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    public string effectName;
    public bool effectOnApply;
    public EffectType effectType;
    public Character target;
    private int appliedCountdown;
    public bool stackable;
    [ShowIf("stackable", true, true)] public int maxStacks = 999;

    [Header("Effect Type Variables")]
    [Tooltip("Effects with duration 0 are only removed when target dies or cleanses")] public int duration;
    public int tickDamage;
    public Stat statToModifyAndAmount;

    private bool onApplyTriggered;

    public void OnValidate()
    {
        effectName = name;
    }

    public StatusEffect Clone(Character Target)
    {
        StatusEffect clone = Instantiate(this);
        clone.target = Target;
        clone.appliedCountdown = duration;
        return clone;
    }

    public void OnApplyEffect()
    {
        if (!effectOnApply)
            return;

        TickEffect(out bool _);

    }

    public void TickEffect(out bool canRemove)
    {
        if (effectOnApply && onApplyTriggered)
        {
            canRemove = false;
            return;
        }

        onApplyTriggered = true; ;

        //do actions based on effect type flags
        if(effectType.HasFlag(EffectType.Damage))
            target.TakeDamage(tickDamage, DamageType.Magic);

        if (effectType.HasFlag(EffectType.StatDecrease))
            target.ModifyStat(statToModifyAndAmount.statName, statToModifyAndAmount.statValue, duration);

        //checks if effect has run out
        appliedCountdown--;

        if(duration > 0 && appliedCountdown < 0)
            canRemove = true;

        canRemove = false;
    }

    [Flags]
    public enum EffectType
    {
        Damage = 1,
        StatDecrease = 1 << 1
    }
}

[System.Serializable]
public struct StatusEffectData
{
    public Character owner;
    public Character target;
    public StatusEffect effect;
}
