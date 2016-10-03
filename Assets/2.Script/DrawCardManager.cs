using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawCardManager : MonoBehaviour {

	//0815 LSJ
	private Card cardData;	//Check Hand Card Data

	public string d_cardName;
	private string d_cardMatName;

	//0809 LSJ
	public string HandCardName{
		get{ return this.d_cardName; }
	}
	public string HandCardMaterial{
		get{ return this.d_cardMatName; }
	}
	//0815 LSJ
	public Card CardData{
		get { return cardData; }
		set{ cardData = value; }
	}
	//~0809 LSJ

	void Awake(){
		this.GetComponent<Image> ().material.mainTexture = Resources.Load ("Handcard") as Texture;
	}

	public void CheckDraw(Transform _curCard){
		//Get Name
		//var _idx = _curCard.name.IndexOf(" "); 
		d_cardName = _curCard.name/*.Substring (0, _idx)*/;

		//Get Material Name
		var _mname = _curCard.GetComponentInParent<Renderer>().material.name.Replace(" (Instance)", "");
		d_cardMatName = _mname;


        //And Then, Change its Material
        //8월 17일 손황호 1줄수정
        this.GetComponent<Image>().material.mainTexture = Resources.Load("UiCharImage/" + d_cardMatName) as Texture;
        this.GetComponent<Image>().SetAllDirty();
        //Check this Changed
        if (this.GetComponent<Image> ().material == null) {
			Debug.Log ("Fuck!!");
		} else {


		}
	}

}
