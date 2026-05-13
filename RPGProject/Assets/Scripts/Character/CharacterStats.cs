using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CharacterStats"), System.Serializable]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public Stat[] stats; 

    //default stats:
    //Strength, = physical damage
    //Dexterity,
    //Vitality, = health
    //Int, = magic damage
    //WillPower, action point regen per turn

    public Sprite characterPortrait;

    public void OnValidate()
    {
        characterName = name;
    }
}

[System.Serializable]
public struct Stat
{
    public string statName;
    public int statValue;
}