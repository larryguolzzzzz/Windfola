using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//counter assignment
public class Card_homework_r : CardBasic {

    //this card can counter assignment
    public Card_homework_r()
    {
        card_Type = Card_type.basic;
        id = 1;
    }

    public override void Play(Player p)
    {
            return;


        base.Play(p);

    }
}
