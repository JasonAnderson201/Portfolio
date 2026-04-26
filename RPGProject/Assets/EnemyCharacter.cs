using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCharacter : Character
{

    public override void SetIsActiveCharacter()
    {

        ActionData actionStruct = new ActionData() 
        {
            user = this,
            target = BattleManager.instance.party[Random.Range(0, BattleManager.instance.party.Length)],
            action = actions[Random.Range(0, actions.Length - 1)]
        };
        DoAction(actionStruct);

        base.SetIsActiveCharacter();
    }

    public override void DoAction(ActionData data)
    {
        data.action.DoAction(data);

        base.DoAction(data);
    }
}
