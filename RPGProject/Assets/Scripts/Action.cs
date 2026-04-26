using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Data/Actions")]
public abstract class Action : ScriptableObject
{
    public string actionName;
    public int damage;
    public TargetType targetType;
    public DamageType damageType;
    public string modifierStat;

    public abstract void DoAction(ActionData data);


    public enum TargetType
    {
        Single,
        AOE,
        Friendly,
        Self
    }


}

public struct ActionData
{
    public Character user;
    public Character target;
    public Action action;
}

public enum DamageType
{
    Physical,
    Magic,
}