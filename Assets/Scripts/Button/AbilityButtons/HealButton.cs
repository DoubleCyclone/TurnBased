using UnityEngine;

class HealButton : BaseButton
{
    private void Awake()
    {
        target = Target.Ally;
    }
    public override void Ability(Character user, Character target)
    {
        ActionManager.Instance.HealAct.Invoke(user,target);
    }
}

