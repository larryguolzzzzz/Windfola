using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager GetInstance;
    public int myid;
    public NetManager net;

    public Player nowPlayer;

    public List<Player> players = new List<Player>();
    public List<CardBasic> cards = new List<CardBasic>();


    public Player FindMe()
    {
        return players.Find(x => x.id == myid);
    }

    public void InitCards()
    {
        List<CardBasic> cardss=new List<CardBasic>();
        for (int i = 0; i < 10; i++)
        {
            cardss.Add(new Card_test());
            cardss.Add(new Card_test_r());
            cardss.Add(new Card_homework());
            cardss.Add(new Card_homework_r());
        }

        for (int i = 0; i < 5; i++)
        {
            cardss.Add(new Card_trap());
            cardss.Add(new Card_steal());
            cardss.Add(new Card_destroy());
            cardss.Add(new Card_cheat());
            cardss.Add(new Card_redpen());
            cardss.Add(new Card_book());
        }

        cardss = RandomSort(cardss);
        cards.AddRange(cardss);

    }
    private List<T> RandomSort<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }



    private void Awake()
    {
        GetInstance = this;
    }

    // Use this for initialization
    void Start () {
        net = NetManager.GetInstance;
        net.RegistNet("your netId", (object id) => { myid = (int)id;GameMenu.instance.info.text = "Your id:" + id.ToString(); });
        net.onConnectToServer_Stable += delegate (int id) { GameMenu.instance.GoConnect(); };



        net.RegistNet("send to all", (object id) => {
            object[] obs = id as object[];
            SendToAll(obs[0].ToString(), obs[1]);

        });

        net.RegistNet("send to player", (object id) =>
        {
            object[] obs = id as object[];
            SendToPlayer(obs[0] as Player, obs[1].ToString(), obs[2]);

        });



        net.onClientConnect += delegate (NetworkConnection conn) {
            Player p = new Player();
            p.id = conn.connectionId;
            players.Add(p);
            net.SendToNetId(p.id, "your netId", p.id);
        };

        net.RegistNet("start", (object ob) => {
            if (!NetManager.host) { 
            players = ob as List<Player>;
            }
            GameMenu.instance.PlayGame();


            });

        net.RegistNet("end", (object ob) =>
        {
            List<Player> ps = players.FindAll(x => !x.dead);
            
            for (int i = 0; i < ps.Count; i++)
            {
                if(ps[i].id==nowPlayer.id)
                { 
                    if(i== ps.Count-1)
                    { net.SendToNetId(ps[0].id, "contral", new object());
                        nowPlayer = players[0];
                    }
                    else
                    { net.SendToNetId(ps[i + 1].id, "contral", new object()); nowPlayer = ps[i+1]; }
                    break;
                }
            }

            if(players.Find(x=>x.id== nowPlayer.id).dead==true)
            {
                net.SendToNetId(ps[0].id, "contral", new object());
                nowPlayer = ps[0];
            }

            nowPlayer.cards.AddRange(GetCards(2));
            Sync();


        });

        


        net.RegistNet("sync",Sync_get);


        net.RegistNet("skill change", (object ob) =>
        {
            object[] obs = ob as object[];
            players.Find(x => x.id == (int)obs[0]).skill = (int)obs[1] ;

        });


    }
	
    public void SendToAll(string type,object ob)
    {
        if (NetManager.host)
            net.SendToAllPears(type, ob);
        else
            net.SendToServer("send to all", new object[2] { type, ob });
    }

    public void SendToPlayer(Player p, string type, object ob)
    {
        if (NetManager.host)
            net.SendToNetId(p.id,type, ob);
        else
            net.SendToServer("send to player", new object[3] { p,type, ob });

    }


    public List<CardBasic> GetCards(int num)
    {
        if (cards.Count < num) 
        InitCards();

        List<CardBasic> ls = cards.GetRange(0, num);
        cards.RemoveRange(0, num);
        return ls;


    }


    public void GameStart()
    {
        InitCards();
        players.ForEach(x =>
        {
            x.cards.AddRange(GetCards(5));
        });

        players.GetRange(0, players.Count / 2).ForEach(x => x.team = 2);


        net.SendToAllPears("start", players);
        nowPlayer = players[0];
        nowPlayer.cards.AddRange(GetCards(1));
        Sync();
        net.SendToNetId(players[0].id, "contral", new object());
        
    }

    public void Sync()
    {
        SendToAll("sync", players);

    }

    public void Sync_get(object ob)
    {
        players = ob as List<Player>;
        players.FindAll(x => x.hp > 12).ForEach(x => x.hp = 12);

        UIContral.getInstance.RefreshCard();
        UIContral.getInstance.RefreshBlood();
        players.FindAll(x => x.hp <= 0).ForEach(x => x.dead = true);
        Skill.instance.id = FindMe().skill;
        CheckEnd();

        if (FindMe().dead && UIContral.getInstance.contral)
            UIContral.getInstance.EndRound();

        FindMe().cards.ForEach(x => x.OnDraw());


    }




    void CheckEnd()
    {
        List<Player>ls= players.FindAll(x => !x.dead);
        if (!ls.Exists(x => x.team == 1))
            UIContral.getInstance.ShowEnd("Team 2 win!");
        else if (!ls.Exists(x => x.team == 2))
            UIContral.getInstance.ShowEnd("Team 1 win!");

    }


	// Update is called once per frame
	void Update () {
		
	}
}
