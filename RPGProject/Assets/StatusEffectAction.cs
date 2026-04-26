using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectAction", menuName = "Data/Actions/StatusEffectAction")]
public class StatusEffectAction : Action
{
    public StatusEffect statusEffect;

    public override void DoAction(ActionData data)
    {
        StatusEffectData newData = new StatusEffectData()
        {
            owner = data.user,
            target = data.target,
            effect = statusEffect
        };

        data.target.ApplyStatusEffect(newData);
    }
}
