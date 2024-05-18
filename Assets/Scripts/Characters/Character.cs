using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    [SerializeField] public string charName;
    [SerializeField] public bool isPlayable;
    [ReadOnly] public bool isTurn;

    [SerializeField] public int maxHitPoints; // max HP
    [SerializeField] public int maxFocus; // max Focus, ability resource
    [ReadOnly] public int hitPoints; // HP
    [ReadOnly] public int focus; // Focus
    [SerializeField] public int speed; // determines who goes first
    [SerializeField] public int strength;
    [SerializeField] public int defense;

    public List<Action<Character, Character>> abilityList = new List<Action<Character, Character>>();

    [ReadOnly] public CharacterHUD charHUD;
    private void Awake()
    {
        hitPoints = maxHitPoints;
    }
    public void affectHP(int amount)
    {
        hitPoints += amount;
        if (amount > 0) // HEAL
        {
            if (hitPoints > maxHitPoints)
                hitPoints = maxHitPoints;
        }
        else if (amount < 0) // DAMAGE
        {
            if (hitPoints < 0)
                hitPoints = 0;
        }
        charHUD.SetHP(this);
    }
    public void affectFocus(int amount)
    {
        focus += amount;
        if (amount > 0)//INCREASE
        {
            if (focus > maxFocus)
                focus = maxFocus;
        }
        else if (amount < 0)//DECREASE
        {
            if (focus < 0)
                focus = 0;
        }
        charHUD.SetFocus(this);
    }
    public void affectHpAndFocus(int hp, int foc)
    {
        affectHP(hp);
        affectFocus(foc);
    }
    public void SetHUD()
    {
        charHUD.SetHUD(this);
    }
}
