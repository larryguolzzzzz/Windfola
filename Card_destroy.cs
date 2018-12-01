using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//destory card
public class Card_destroy : CardBasic {

    public Card_destroy()
    {
        card_Type = Card_type.magic;
        id = 6;
    }
    //destory an equipment from what they equip
    public override void Play(Player p)
    {
        if (p.id == GameManager.GetInstance.myid)
            return;
        GameManager.GetInstance.players.Find(x => x.id == p.id).equip = null;
        base.Play(p);
    }


    public override void OnDraw()
    {
    }

}
