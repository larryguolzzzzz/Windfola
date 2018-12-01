using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//trap card
public class Card_trap : CardBasic {

    public Card_trap()
    {
        card_Type = Card_type.magic;
        id = 9;
    }
    //for any player who have the draw this card, deal 3 damage to them once they draw it
    public override void Play(Player p)
    {
        if (p.id == GameManager.GetInstance.myid)
            return;
        GameManager.GetInstance.players.Find(x => x.id == p.id).hp -= 3;
        base.Play(p);
        UIContral.getInstance.EndRound();
    }


    public override void OnDraw()
    {
    }

}
