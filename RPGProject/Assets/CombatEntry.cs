using TMPro;
using UnityEngine;

public class CombatEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Setup(ActionData data)
    {
        string message = $"{data.user.stats.characterName} has cast {data.action.actionName} on {data.target.stats.characterName} ";

        AttackAction attack = data.action as AttackAction;
        if (attack != null)
        {
            message += $"and dealt {attack.damage} damage";
        }

        text.text = message;
    }
}
