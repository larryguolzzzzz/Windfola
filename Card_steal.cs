using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//steal card
public class Card_steal : CardBasic {

    public Card_steal()
    {
        card_Type = Card_type.magic;
        id = 8;
    }
    //steal a random card from player
    public override void Play(Player p)
    {
        if (p.id == GameManager.GetInstance.myid)
            return;
        //find the user
        if(GameManager.GetInstance.players.Find(x => x.id == p.id).equip!=null&&Random.value>0.6f)
        {
            GameManager.GetInstance.FindMe().cards.Add(GameManager.GetInstance.players.Find(x => x.id == p.id).equip);
            GameManager.GetInstance.players.Find(x => x.id == p.id).equip = null;
        }
        else
        {
            if(GameManager.GetInstance.players.Find(x => x.id == p.id).cards.Count>0)
            {
                CardBasic b = GameManager.GetInstance.players.Find(x => x.id == p.id).cards[Random.Range(0, GameManager.GetInstance.players.Find(x => x.id == p.id).cards.Count)];
                GameManager.GetInstance.FindMe().cards.Add(b);
                GameManager.GetInstance.players.Find(x => x.id == p.id).cards.Remove(b);

            }


        }
        
        base.Play(p);
    }


    public override void OnDraw()
    {
    }

}
