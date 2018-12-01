using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : MonoBehaviour
{
    //the functions that skill should have
    public static Skill instance;
    public GameObject use_skill;
    public int id;
    public int times = 1;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NetManager.GetInstance.RegistNet("set skill", (object ob) => id = (int)ob);
        use_skill.GetComponent<Button>().onClick.AddListener(Choose);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F8))
        {
            GameManager.GetInstance.FindMe().hp -= 30;
            GameManager.GetInstance.Sync();
        }
    }

    public void Refresh()
    {
        use_skill.SetActive(times > 0);
    }




    public void Choose()
    {
        if (id == 0 || id == 2 || id == 4)
            return;


        UIContral.getInstance.GoChoose(Play);

    }
    //player take turns to choose professors
    public void Play(Player p)
    {
        switch (id)
        {
            case 1:
                GameManager.GetInstance.FindMe().hp-=3;
                GameManager.GetInstance.players.FindAll(x => x.team == GameManager.GetInstance.FindMe().team).ForEach(x => x.hp+=2);
                break;
            case 3:
                if (p.id == GameManager.GetInstance.myid)
                    return;
                GameManager.GetInstance.SendToPlayer(p, "homework", new object());
                break;
            case 5:
                if (p.team != GameManager.GetInstance.FindMe().team)
                    return;
                GameManager.GetInstance.SendToPlayer(p, "set skill", 2);
                break;
            case 6:
                GameManager.GetInstance.SendToPlayer(p, "set skill", 0);
                UIContral.getInstance.EndRound();
                break;
            case 7:
                GameManager.GetInstance.players.Find(x => x.id==p.id).hp--;
                break;
            case 8:
                GameManager.GetInstance.SendToAll( "people skill", new object());
                break;


        }

        AfterPlay();
    }
    //sync to every player
    public void AfterPlay()
    {
        times--;
        if (times < 1)
            use_skill.SetActive(false);
        GameManager.GetInstance.Sync();
    }



}
