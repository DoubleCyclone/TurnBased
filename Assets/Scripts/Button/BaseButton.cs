using UnityEngine;
using System;

abstract class BaseButton : MonoBehaviour
{
    public Target target = Target.Nothing;
    public abstract void Ability(Character user, Character target);
    public enum Target
    {
        Nothing,
        Self,
        Ally,
        Enemy
    }
}

