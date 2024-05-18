using UnityEngine;
using System;

public delegate Character SelectTargetDelegate(bool targetOpposing);

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    public static System.Random random = new System.Random();

    public Action<Character, Character> AttackAct = Attack;
    public Action<Character, Character> HealAct = Heal;
    public Action<Character, Character> WaitAct = Wait;
    public Action OnSlideComplete;
    private void Awake()
    {
        Instance = this;
    }
    private static void Attack(Character user, Character target)
    {
        if (user != null && !user.isPlayable)
            target = selectTarget(true);
        if (user == null || target == null)
            return;
        int damage = user.strength - target.defense;
       /* moveToTarget(user, target,() =>
        {*/
            target.affectHP(-damage); // decrease in hp 
            user.affectFocus(damage); // increase in focus
            EffectsManager.DoFloatingText(target.transform.position, damage.ToString(), Color.red);
            GameObject go = Instantiate(EffectsManager.Instance.attackEffectPrefab, target.transform.position, Quaternion.identity); // animation play
            if (!user.isPlayable) // flip animation if used on player
                go.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Destroy(go, go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // animation destroy
            checkforDeath(target);
            showMessage(user.charName + " attacks " + target.charName + " and deals " + damage + " damage!");
        //});   
    }

    private static void Wait(Character user, Character target) // second parameter is just to match the other ones
    {
        if (user == null)
            return;
        user.affectFocus(15);
        EffectsManager.DoFloatingText(user.transform.position, "15", Color.blue);
        user.isTurn = false;
        showMessage(user.charName + " waits for a turn!");
        BattleUIManager.Instance.SetBattleInfo(user.charName + " waits for a turn!");
    }

    private static void Heal(Character user, Character target)
    {
        if (user != null && !user.isPlayable)
            target = selectTarget(false);
        if (user == null || target == null)
            return;
        int amount = user.strength;
        target.affectHP(amount);
        EffectsManager.DoFloatingText(target.transform.position, amount.ToString(), Color.green);
        user.affectFocus(amount);
        showMessage(user.charName + " heals " + target.charName + " for " + amount + " health!");
    }

    private static Character selectTarget(bool targetOpposing)
    {
        Character character;
        while (true)
        {
            character = BattleManager.characterList[random.Next(BattleManager.characterList.Count)];
            if ((BattleManager.Instance.activeCharacter.isPlayable && targetOpposing) || (!BattleManager.Instance.activeCharacter.isPlayable && !targetOpposing)) // player does action on enemy target OR enemy does action on player target
            {
                if (!character.isPlayable)
                    break;
            }
            else if ((BattleManager.Instance.activeCharacter.isPlayable && !targetOpposing) || (!BattleManager.Instance.activeCharacter.isPlayable && targetOpposing)) // player does action on player target OR enemy does action on enemy target
            {
                if (character.isPlayable)
                    break;
            }
        }
        return character;
    } // selects a random target, used by enemies

    /*private static void moveToTarget(Character user, Character target, Action OnSlideComplete)
    {
        if (user == null || target == null)
            return;
        ActionManager.Instance.OnSlideComplete = OnSlideComplete;
        user.target = target;
        user.state = CharacterState.Moving;
    }*/
    private static void checkforDeath(Character target) // don't forget to add this to the abilities that decrease HP
    {
        if (target.hitPoints <= 0)
        {
            Destroy(target.gameObject);
            if (target.isPlayable)
                BattleManager.Instance.playerCount--;
            else if (!target.isPlayable)
                BattleManager.Instance.enemyCount--;
        }
    }
    private static void showMessage(string text)
    {
        Debug.Log(text);
        BattleUIManager.Instance.SetBattleInfo(text);
    } // shows message on console as well as battle top text
}
