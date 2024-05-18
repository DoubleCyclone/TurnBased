using UnityEngine;

class WaitButton : BaseButton
{
    private void Awake()
    {
        target = Target.Self;
    }
    public override void Ability(Character user, Character target)
    {
        ActionManager.Instance.WaitAct.Invoke(user,target);
    }
}

