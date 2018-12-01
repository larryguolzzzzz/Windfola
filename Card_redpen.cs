using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//red pen card
public class Card_redpen : CardBasic {

    public Card_redpen()
    {
        card_Type = Card_type.equip;
        showName = "Red Pen";
        id = 4;
    }
    //equip the red pen
    public override void Play(Player p)
    {
        if (p.id != GameManager.GetInstance.myid)
            return;
        //GameManager.GetInstance.players.Find(x => x.id == p.id).hp -= 3;
        GameManager.GetInstance.FindMe().equip = this;
        base.Play(p);
    }


    public override void OnDraw()
    {
    }

}
