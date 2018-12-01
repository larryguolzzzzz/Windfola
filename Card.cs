using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour {

    public bool useable=true;
    public CardBasic card;
	// Use this for initialization
	void Start () {
        //user panel
        GetComponent<Button>().onClick.AddListener(Choose);
        transform.GetChild(0).GetComponent<Text>().text = "";//card.name;
        transform.GetChild(1).GetComponent<Text>().text = "";//card.info;
        GetComponent<Image>().sprite = UIContral.getInstance.cards[card.id];
        GetComponent<CardShow>().id = card.id;
        //This is the initial setup for user panel. It moves to Zoom in function => Cardshow file
        //switch (card.card_Type)
        //{
        //    case Card_type.basic:
        //        GetComponent<Image>().sprite = UIContral.getInstance.card_backs[1];
        //        break;
        //    case Card_type.magic:
        //        GetComponent<Image>().sprite = UIContral.getInstance.card_backs[4];
        //        break;
        //    case Card_type.equip:
        //        GetComponent<Image>().sprite = UIContral.getInstance.card_backs[3];
        //        break;
        //    case Card_type.role:
        //        GetComponent<Image>().sprite = UIContral.getInstance.card_backs[2];
        //        break;
        //    case Card_type.back:
        //        GetComponent<Image>().sprite = UIContral.getInstance.cardBack;
        //        break;
        //    default:
        //        break;
        //}


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Choose()
    {
        if(useable)
        UIContral.getInstance.GoChoose(card.Play);

    }


}
