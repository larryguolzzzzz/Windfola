using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{



    public int id;
	// Use this for initialization
	void Start () {
        
	}

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GameMenu.instance.card_show.sprite = UIContral.getInstance.cards[id];
        GameMenu.instance.card_show.DOKill();
        GameMenu.instance.card_show.DOFade(1, 0.3f);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        GameMenu.instance.card_show.DOKill();
        GameMenu.instance.card_show.DOFade(0, 0.3f);

        //Debug.Log("Cursor Exiting " + name + " GameObject");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
