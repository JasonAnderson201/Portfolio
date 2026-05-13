using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item")]
public class Item : Action
{
    public UseType useType;
    [ShowEnum("useType", 0, true)] public int healthModValue;

    [ShowEnum("useType", 1, true)] public string statToModify;
    [ShowEnum("useType", 1, true)] public int statModValue;
    [ShowEnum("useType", 1, true)] public int statModDuration;

    [ShowEnum("useType", 2, true)] public StatusEffect statusEffect;
    [ShowEnum("useType", 2, true)] public int statusEffectDuration;
    public enum UseType
    {
        HealthModify,
        StatModify,
        StatusEffect
    }

    public override void DoAction(ActionData data)
    {
        switch (targetType)
        {
            case TargetType.Friendly:
            case TargetType.Single:

                if (useType == UseType.HealthModify)
                {
                    data.target.TakeDamage(healthModValue, DamageType.Magic);
                }
                else if( useType == UseType.StatModify)
                    data.target.ModifyStat(statToModify, statModValue, statModDuration);
                else
                {
                    StatusEffectData statusData = new StatusEffectData()
                    {
                        effect = statusEffect,
                        user = data.user,
                        target = data.target,
                    };

                    data.target.ApplyStatusEffect(statusData);
                }


                    break;
        }

        data.user.items.Remove(this);
    }
}
