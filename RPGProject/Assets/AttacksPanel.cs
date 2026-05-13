using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static AttacksPanel;

public class AttacksPanel : MonoBehaviour
{
    [SerializeField] private GameObject targetsPanel;
    private PlayerCharacter activePlayer;

    [SerializeField] private ActionButton actionPrefab; // also do targets prefab

    [SerializeField] private List<ActionButton> actionButtons = new List<ActionButton>();
    [SerializeField] private List<Button> targetsButtons = new List<Button>();

    private bool active;

    private ActionType curType;

    [System.Serializable]
    public enum ActionType
    {
        None,
        Attacks,
        Items,
        Run
    }

    public void SetUp(string type)
    {
        ActionType actionType = (ActionType)Enum.Parse(typeof(ActionType),type);

        if (actionType == curType)
            return;

        ClosePanel();

        curType = actionType;
 
        if (actionType == ActionType.Run)
            return;


        active = true;

        gameObject.SetActive(true);

        activePlayer = BattleManager.instance.activeCharacter as PlayerCharacter;

        List<Action> actionsToPass = new List<Action>();

        //instantiate buttons based on different lists
        if(actionType == ActionType.Attacks)
        {
            actionsToPass.AddRange(activePlayer.actions);
            InstantiateActionButtons(actionsToPass);
        }
        else
        {
            actionsToPass.AddRange(activePlayer.items);
            InstantiateActionButtons(actionsToPass);
        }
    }

    private void InstantiateActionButtons(List<Action> actionList)
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            ActionButton newButton = Instantiate(actionPrefab, transform);

            newButton.Setup(actionList[i]);

            newButton.onClick += ActionSelect;

            actionButtons.Add(newButton);
        }
    }

    private void ActionSelect(Action Action)
    {
        if (BattleManager.instance.activeCharacter.actionPoints < Action.pointCost)
            return;

        switch (Action.targetType)
        {
            case Action.TargetType.Single:
                targetsPanel.SetActive(true);
                InstantiateButtons(BattleManager.instance.enemies, Action);

            break;

            case Action.TargetType.Friendly:
                targetsPanel.SetActive(true);
                InstantiateButtons(BattleManager.instance.party, Action);
                break;
        }

    }

    public void InstantiateButtons(Character[] targets, Action Action)
    {
        foreach (Character target in targets)
        {
            if (target.currentHealth >= 0)
            {
                Button newButton = Instantiate(actionPrefab, targetsPanel.transform).GetComponent<Button>();
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = target.stats.characterName;
                targetsButtons.Add(newButton);

                newButton.onClick.AddListener(() =>
                {
                    ActionData actionStruct = new ActionData()
                    {
                        user = BattleManager.instance.activeCharacter,
                        target = target,
                        action = Action
                    };
                    BattleManager.instance.activeCharacter.DoAction(actionStruct);
                    ClosePanel();
                });
            }
        }
    }

    //turn off all un-needed objects and unsubscribe from events
    private void ClosePanel()
    {
        targetsPanel.SetActive(false);
        this.gameObject.SetActive(false);

        for(int i = 0; i < targetsButtons.Count; i++)
        {
            Destroy(targetsButtons[i].gameObject);
        }

        for(int i = 0; i < actionButtons.Count; i++)
        {
            actionButtons[i].onClick -= ActionSelect;
            Destroy(actionButtons[i].gameObject);
        }

        actionButtons.Clear();
        targetsButtons.Clear();

        curType = ActionType.None;
    }
}