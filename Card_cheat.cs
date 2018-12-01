using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//cheating card
public class Card_cheat : CardBasic {

    public Card_cheat()
    {
        card_Type = Card_type.magic;
        id = 7;
    }

    public override void Play(Player p)
    {
        
        base.Play(p);
    }

    //each team member take 1 point off
    public override void OnDraw()
    {
        if (used)
            return;
        used = true;

        GameManager.GetInstance.players.FindAll(x => x.team == GameManager.GetInstance.FindMe().team).ForEach(x => x.hp--);
        //

        base.OnDraw();
    }

}
