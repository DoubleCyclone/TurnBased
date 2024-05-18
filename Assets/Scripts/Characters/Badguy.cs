using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badguy : Character
{
    private void Start()
    {
        abilityList.Add(ActionManager.Instance.AttackAct);
        abilityList.Add(ActionManager.Instance.HealAct);
        abilityList.Add(ActionManager.Instance.WaitAct);
    }
}
