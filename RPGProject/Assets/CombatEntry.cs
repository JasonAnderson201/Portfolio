using TMPro;
using UnityEngine;

public class CombatEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Setup(ActionData data)
    {
        string message = "";

        Item item = data.action as Item;
        if (item != null)
        {
            message += $"{data.user.CharacterName} used {data.action.actionName} on {data.target.CharacterName}";

            text.text = message;

            return;
        }

        message += $"{data.user.CharacterName} has cast {data.action.actionName} on {data.target.CharacterName} ";

        AttackAction attack = data.action as AttackAction;
        if (attack != null)
        {
            message += $"and dealt {attack.damage} damage";
        }

        text.text = message;
    }
}
