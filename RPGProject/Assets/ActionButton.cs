using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public event System.Action<Action> onClick;

    private Action action;

    [SerializeField] private TextMeshProUGUI text;

    public void Setup(Action Action)
    {
        action = Action;

        text.text = action.actionName;
    }

    public void OnClick()
    {
        onClick?.Invoke(action);
    }
}
