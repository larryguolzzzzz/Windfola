using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//test card
public class Card_test : CardBasic {


    public Card_test()
    {
        card_Type = Card_type.basic;
        id = 3;
    }
    //player test card on player
    public override void Play(Player p)
    {
        //if the player has red pen, then the function doesn't effect on him
        if(GameManager.GetInstance.FindMe().equip!=null&& GameManager.GetInstance.FindMe().equip.GetType()==typeof(Card_redpen))
        {
            GameManager.GetInstance.players.FindAll(x => x.team != GameManager.GetInstance.FindMe().team).ForEach(x => x.hp--);
        }
        else
            GameManager.GetInstance.SendToAll("exam", GameManager.GetInstance.myid);



        base.Play(p);

    }
}
