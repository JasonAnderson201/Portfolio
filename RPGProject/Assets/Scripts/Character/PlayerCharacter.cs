using UnityEngine;

public class PlayerCharacter : Character
{


    public override void DoAction(ActionData data)
    {
        data.action.DoAction(data);
        base.DoAction(data);
    }
}
