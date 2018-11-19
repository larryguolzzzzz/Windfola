using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    public static GameMenu instance;
    NetManager net;
    public Text ip_lable;
    public Text info;
    public GameObject play_button;
    public GameObject second_pannel;
    public GameObject game_pannel;
    public GameObject skill_panenl;

    public Image card_show;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }


    void Start () {
        net = NetManager.GetInstance;
        ip_lable.text = "YOUR IP：" + net.GetIP();
        InitSkillCards();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Exit()
    { Application.Quit(); }


    public void GoConnect()
    {

        play_button.SetActive(NetManager.host);
        second_pannel.SetActive(true);

    }

    public void PlayGame()
    {
        game_pannel.SetActive(true);
        Transform players = game_pannel.transform.Find("players");


        bool find = false;
        int ii = 0;
        for (int i = 0; i < GameManager.GetInstance.players.Count; i++)
        {
            if (ii == GameManager.GetInstance.players.Count)
                break;

            if(GameManager.GetInstance.players[i].id== GameManager.GetInstance.myid)
            {
                find = true;
                GamePlayer gp=players.GetChild(ii++).gameObject.AddComponent<GamePlayer>();
                gp.player = GameManager.GetInstance.players[i];
            }else if (find)
            {
                GamePlayer gp = players.GetChild(ii++).gameObject.AddComponent<GamePlayer>();
                gp.player = GameManager.GetInstance.players[i];


            }
        }
        for (int i = 0; i < GameManager.GetInstance.players.Count; i++)
        {
            if (ii == GameManager.GetInstance.players.Count)
                break;
            if (GameManager.GetInstance.players[i].id == GameManager.GetInstance.myid)
            {
                find = true;
                GamePlayer gp = players.GetChild(ii++).gameObject.AddComponent<GamePlayer>();
                gp.player = GameManager.GetInstance.players[i];
            }
            else if (find)
            {
                GamePlayer gp = players.GetChild(ii++).gameObject.AddComponent<GamePlayer>();
                gp.player = GameManager.GetInstance.players[i];


            }
        }




    }



    public Dropdown dropdown;
    public void ChangeSkill(int i)
    {
        net.SendToServer("skill change",new object[]{GameManager.GetInstance.myid,i});
        skill_panenl.SetActive(false);
    }

    public Transform skillParent;
    public void InitSkillCards()
    {
        for (int i = 0; i < skillParent.childCount; i++)
        {
            skillParent.GetChild(i).GetComponent<Image>().sprite= UIContral.getInstance.cards[i + 10];
        }


    }



}
