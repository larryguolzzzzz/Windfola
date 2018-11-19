using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContral : MonoBehaviour {

    public static UIContral getInstance;
    public GameObject game_pannel;
    public GameObject ask_pannel;
    public GameObject card_prefab;
    public List<Sprite> bloods;
    public List<Sprite> cards;
    public Sprite cardBack;
    public GameObject end_panenl;
    public GameObject center_card; 


    public void RefreshCenter(CardBasic card)
    {
        Destroy(center_card.GetComponent<Card>());
        center_card.AddComponent<Card>().card = card;
        center_card.GetComponent<Card>().useable = false;

    }





    private void Awake()
    {
        getInstance = this;
    }

    public bool contral;


    public void ShowEnd(string text)
    {
        end_panenl.transform.GetChild(0).GetComponent<Text>().text = text;
        end_panenl.SetActive(true);
    }



    void Sy() { GameManager.GetInstance.FindMe().hp--; GameManager.GetInstance.Sync(); }

    void FightBack()
    {
        NetManager.GetInstance.RegistNet("exam", (object ob) =>
        {
            if ((int)ob == GameManager.GetInstance.myid)
                return;
            if (GameManager.GetInstance.players.Find(x => x.id == (int)ob).team == GameManager.GetInstance.FindMe().team)
                return;

            if (!GameManager.GetInstance.FindMe().cards.Exists(x => x.GetType() == typeof(Card_test_r)))
            {
                Invoke("Sy", 1); 
                return;
            }


            ask_pannel.SetActive(true);
            ask_pannel.transform.GetChild(0).GetComponent<Text>().text = "Enemy use the test card, do you want to block it?";
            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                List<CardBasic> cards = GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).cards;
                cards.Remove(cards.Find(x => x.GetType() == typeof(Card_test_r)));
                GameManager.GetInstance.Sync();
            });

            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).hp--;
                GameManager.GetInstance.Sync();
            });

        });


        NetManager.GetInstance.RegistNet("homework", (object ob) =>
        {
            if (GameManager.GetInstance.FindMe().equip != null && GameManager.GetInstance.FindMe().equip.GetType() == typeof(Card_book))
                return;

            if (!GameManager.GetInstance.FindMe().cards.Exists(x => x.GetType() == typeof(Card_homework_r)))
            {
                Invoke("Sy", 1);
                return;
            }

            ask_pannel.SetActive(true);
            ask_pannel.transform.GetChild(0).GetComponent<Text>().text = "Enemy use the assignment card, do you want to block it?";
            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                List<CardBasic> cards = GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).cards;
                cards.Remove(cards.Find(x => x.GetType() == typeof(Card_homework_r)));
                GameManager.GetInstance.Sync();
            });

            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).hp--;
                GameManager.GetInstance.Sync();
            });

        });

        NetManager.GetInstance.RegistNet("people skill", (object ob) =>
        {
            if (!GameManager.GetInstance.FindMe().cards.Exists(x => x.GetType() == typeof(Card_test_r))&& !GameManager.GetInstance.FindMe().cards.Exists(x => x.GetType() == typeof(Card_test_r)))
            {
                Invoke("Sy", 1);
                return;
            }


            ask_pannel.SetActive(true);
            ask_pannel.transform.GetChild(0).GetComponent<Text>().text = "Enemy use the Professor 8 skill, do you want to block it?";
            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

            ask_pannel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                List<CardBasic> cards = GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).cards;
                if(cards.Find(x => x.GetType() == typeof(Card_homework_r))==null)
                    cards.Remove(cards.Find(x => x.GetType() == typeof(Card_test_r)));
                else cards.Remove(cards.Find(x => x.GetType() == typeof(Card_homework_r)));
                GameManager.GetInstance.Sync();
            });

            ask_pannel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
            {
                ask_pannel.SetActive(false);
                GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).hp--;
                GameManager.GetInstance.Sync();
            });

        });


    }



	// Use this for initialization
	void Start () {

        game_pannel = GameMenu.instance.game_pannel;

        NetManager.GetInstance.RegistNet("contral", (object ob) => {
            contral = true;
            game_pannel.transform.Find("contral").Find("end").gameObject.SetActive(true);
            Skill.instance.Refresh();

        });

        FightBack();

        NetManager.GetInstance.RegistNet("put card", (object ob) =>
        {
            RefreshCenter(ob as CardBasic);

        });


    }
	
    public void EndRound()
    {
        contral = false;
        NetManager.GetInstance.SendToServer("end", new object());
        game_pannel.transform.Find("contral").Find("end").gameObject.SetActive(false);
        game_pannel.transform.Find("contral").Find("skill").gameObject.SetActive(false);
        Skill.instance.times = 1;
    }


	// Update is called once per frame
	void Update () {
		
	}



    public void Hide(GameObject ob)
    {
        ob.SetActive(false);

    }


    public delegate void PlayCard(Player p);
    PlayCard callback;
    bool choosing = false;
    public void GoChoose(PlayCard _callback)
    {
        if (!contral)
        {
            choosing = false;return;
        }

        callback = _callback;
        choosing = true;
    }

    public void Choose(Player p)
    {
        if (!choosing||p.dead==true)
            return;

        choosing = false;
        if (callback != null)
            callback(p);
    }


    GameObject CreateOb(GameObject prefab)
    {
        GameObject ob = Instantiate<GameObject>(prefab);
        ob.transform.SetParent(prefab.transform.parent);
        ob.transform.localScale = prefab.transform.localScale;
        ob.SetActive(true);
        return ob;

    }


    public void RefreshCard()
    {
        Transform pa = card_prefab.transform.parent;
        for (int i = 1; i < pa.childCount; i++)
        {
            Destroy(pa.GetChild(i).gameObject);
        }
        GameManager.GetInstance.players.Find(x => x.id == GameManager.GetInstance.myid).cards.ForEach(y =>
           {
               CreateOb(card_prefab).AddComponent<Card>().card = y;
           });

    }


    public void RefreshBlood()
    {
        Transform pa = game_pannel.transform.Find("players");
        for (int i = 0; i < pa.childCount; i++)
        {
            if(pa.GetChild(i).GetComponent<GamePlayer>())
            {
                pa.GetChild(i).GetComponent<GamePlayer>().player = GameManager.GetInstance.players.Find(x => x.id == pa.GetChild(i).GetComponent<GamePlayer>().player.id);
                pa.GetChild(i).GetComponent<GamePlayer>().RefreshBlood();

            }
            else
                pa.GetChild(i).GetComponent<Image>().sprite = cardBack;
        }
    }


}
