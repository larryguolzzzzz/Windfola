using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player  {
    public int id;
    public int team = 1;
    public List<CardBasic> cards=new List<CardBasic>();
    public CardBasic equip;
    public int hp=12;
    public bool dead = false;
    public int skill=1;

}
