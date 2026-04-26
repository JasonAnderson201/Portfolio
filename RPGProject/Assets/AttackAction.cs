using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Data/Actions/AttackAction")]
public class AttackAction : Action
{
    public int damage;
    public DamageType damageType;
    public string modifierStat;

    public Optional<StatusEffect> statusEffect;

    public override void DoAction(ActionData data)
    {
        int calculatedDamage = damage + (modifierStat != "" ?  + data.user.FindStat(modifierStat).statValue : 0);
        data.target.TakeDamage(calculatedDamage, damageType);

        if (statusEffect.Enabled)
        {
            StatusEffectData effectData = new StatusEffectData()
            {
                owner = data.user,
                target = data.target,
                effect = statusEffect
            };

            data.target.ApplyStatusEffect(effectData);
        }
    }
}
