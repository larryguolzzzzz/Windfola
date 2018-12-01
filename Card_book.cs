using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Book card
[System.Serializable]
public class Card_book : CardBasic {

    public Card_book()
    {
        card_Type = Card_type.equip;
        showName = "Study Guide";
        id = 5;
    }
    //equip book
    public override void Play(Player p)
    {
        if (p.id != GameManager.GetInstance.myid)
            return;
        GameManager.GetInstance.FindMe().equip = this;
        base.Play(p);
    }


    public override void OnDraw()
    {
    }

}
