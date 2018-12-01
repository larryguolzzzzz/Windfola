using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//counter test
public class Card_test_r : CardBasic {
    //professor number 2 has the this function
    public Card_test_r()
    {
        card_Type = Card_type.basic;
        id = 2;
    }

    //ask player if they want to block this
    public override void Play(Player p)
    {
            return;


        base.Play(p);

    }
}
