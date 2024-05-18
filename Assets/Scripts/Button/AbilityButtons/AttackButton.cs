using UnityEngine;

class AttackButton : BaseButton
{
    private void Awake()
    {
        target = Target.Enemy;
    }
    public override void Ability(Character user, Character target)
    {
        ActionManager.Instance.AttackAct.Invoke(user,target);
    }
}

