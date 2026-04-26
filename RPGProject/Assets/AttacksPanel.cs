using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
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

        if(actionType == ActionType.Attacks)
        {
            for (int i = 0; i < activePlayer.actions.Length; i++)
            {
                ActionButton newButton = Instantiate(actionPrefab, transform);

                newButton.Setup(activePlayer.actions[i]);

                newButton.onClick += ActionSelect;

                actionButtons.Add(newButton);
            }
        }
        else
        {
            for (int i = 0; i < activePlayer.items.Length; i++)
            {
                ActionButton newButton = Instantiate(actionPrefab, transform);

                newButton.Setup(activePlayer.items[i]);

                newButton.onClick += ItemSelect;

                actionButtons.Add(newButton);
            }
        }


    }

    private void ActionSelect(Action Action)
    {
        switch (Action.targetType)
        {
            case Action.TargetType.Single:
                targetsPanel.SetActive(true);

                foreach(Character target in BattleManager.instance.enemies)
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
                break;
        }

    }

    private void ItemSelect(Action item)
    {
        switch (item.targetType)
        {
            case Action.TargetType.Single:

                break;

            case Action.TargetType.Self:

                break;
        }
    }

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