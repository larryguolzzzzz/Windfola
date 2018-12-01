using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//assignment card
public class Card_homework : CardBasic {

    public Card_homework()
    {
        card_Type = Card_type.basic;
        id = 0;
    }


    //let the id decide if they are going to block
    public override void Play(Player p)
    {
        if (p.id == GameManager.GetInstance.myid)
            return;
        GameManager.GetInstance.SendToPlayer(p, "homework", new object());



        base.Play(p);

    }
}
