using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Card_type
{
    basic,
    magic,
    equip,
    role,
    back
}


[Serializable]
public class CardBasic
{
    //card basic setup: should contain if used, card type, user id, etc
    public bool used = false;
    public Card_type card_Type = Card_type.equip;
    public int id = 0;
    public string showName = "";
    public virtual void Play(Player p)
    {
        UnityEngine.Debug.Log("use card:" + this);

        RemoveMe();
    }

    public void RemoveMe()
    {
        GameManager.GetInstance.SendToAll("put card", this);
        GameManager.GetInstance.FindMe().cards.Remove(this);
        GameManager.GetInstance.Sync();
    }



    public virtual void OnDraw()
    {
        if(used)
        GameManager.GetInstance.Sync();
    }



}
