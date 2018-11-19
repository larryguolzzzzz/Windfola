using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : MonoBehaviour {

    public Player player;

    

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { UIContral.getInstance.Choose(player); });
	}
	
    public void RefreshBlood()
    {
        transform.GetChild(0).GetComponent<Text>().text = "Id:"+player.id.ToString()+" Team:"+player.team.ToString()+" CardCount:"+player.cards.Count;

        if (player.hp < 1)
            GetComponent<Image>().sprite = UIContral.getInstance.cardBack;
        else GetComponent<Image>().sprite = UIContral.getInstance.bloods[player.hp-1];

        transform.GetChild(1).gameObject.SetActive(player.equip != null);
        if (player.equip != null)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = player.equip.showName;
            transform.GetChild(1).GetComponent<CardShow>().id = player.equip.id;
        }
            

        if (player.hp < 1)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
            


            transform.GetChild(2).GetComponent<Image>().sprite = UIContral.getInstance.cards[player.skill+10-1];
            transform.GetChild(2).GetComponent<CardShow>().id = player.skill + 10 - 1;


            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "";//"Professor" + player.skill.ToString();
            string text = "";
            switch (player.skill)
            {
                case 0:
                    text = " ";
                    break;
                case 1:
                    text = " ";
                    break;
                case 2:
                    text = " ";
                    break;
                case 3:
                    text = " ";
                    break;
                case 4:
                    text = " ";
                    break;
                case 5:
                    text = " ";
                    break;
                case 6:
                    text = " ";
                    break;
                case 7:
                    text = " ";
                    break;
                default:
                    break;
            }
            transform.GetChild(2).GetChild(1).GetComponent<Text>().text = text;

        }


    }



	// Update is called once per frame
	void Update () {
		
	}
}
