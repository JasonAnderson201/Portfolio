using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    private List<Action> historyList = new List<Action>();
    [SerializeField] private int decisionWeightReduction = 10;
    [SerializeField] private int randomWeightRange = 100;

    public enum EnemyBehaviourType
    {
        Random,
        Weighted,
    }

    public enum EnemyTargetingType
    {
        Random,
        FocusWeakest,
    }

    [SerializeField] private EnemyBehaviourType enemyBehaviour;
    [SerializeField] private EnemyTargetingType enemyTargetingType;

    public override void SetIsActiveCharacter()
    {

        ActionData actionStruct = new ActionData() 
        {
            user = this,
            target = GetTarget(),
            action = GetAction()
        };

        DoAction(actionStruct);

        base.SetIsActiveCharacter();
    }

    public override void DoAction(ActionData data)
    {
        data.action.DoAction(data);

        base.DoAction(data);
    }

    public Action GetAction()
    {
        switch (enemyBehaviour)
        {
            case EnemyBehaviourType.Random:
                return actions[Random.Range(0, actions.Length)];

            case EnemyBehaviourType.Weighted:
                Action returnAction = actions[0];

                int hightestWeight = 999;

                for(int i = 0; i < actions.Length; i++)
                {
                    int curWeight = 0;
                    int historyCount = 0;
                    for (int j = 0; j < historyList.Count; j++)
                    {
                        if (historyList[i].actionName == actions[i].actionName)
                            historyCount++;
                    }

                    curWeight += historyCount * decisionWeightReduction;

                    curWeight += Random.Range(-decisionWeightReduction, decisionWeightReduction);

                    if (curWeight > hightestWeight)
                        returnAction = actions[i];
                }
                return returnAction;
        }

        return actions[Random.Range(0, actions.Length)];
    }

    public Character GetTarget()
    {
        switch (enemyTargetingType)
        {
            case EnemyTargetingType.Random:
                return BattleManager.instance.party[Random.Range(0, BattleManager.instance.party.Length)];

            case EnemyTargetingType.FocusWeakest:
                int lowestHealth = 999999;
                Character curTarget = BattleManager.instance.party[0];
                for (int i = 0; i < BattleManager.instance.party.Length; i++)
                {
                    if (BattleManager.instance.party[i].currentHealth < lowestHealth)
                        curTarget = BattleManager.instance.party[i];
                }

                return curTarget;   
        }

        return BattleManager.instance.party[Random.Range(0, BattleManager.instance.party.Length)];
    }
}

