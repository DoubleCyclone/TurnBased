using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUD : MonoBehaviour
{

    public Text nameText;
    public Text hpText;
    public Text focusText;
    public Slider hpSlider;
    public Slider focusSlider;

    public void SetHUD(Character character)
    {
        if (character == null)
            return;
        nameText.text = character.charName;
        SetHP(character);
        SetFocus(character);
    }

    public void SetHP(Character character)
    {
        if (character == null)
            return;
        hpText.text = character.hitPoints + " / " + character.maxHitPoints;
        hpSlider.maxValue = character.maxHitPoints;
        hpSlider.value = character.hitPoints;
    }

    public void SetFocus(Character character)
    {
        if (character == null)
            return;
        focusText.text = character.focus + " / " + character.maxFocus;
        focusSlider.maxValue = character.maxFocus;
        focusSlider.value = character.focus;
    }

}
