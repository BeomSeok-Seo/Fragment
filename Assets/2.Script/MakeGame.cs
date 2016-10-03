/* 파일명      : MakeGame.cs
   작성자      : 손황호 (feat.이승진)
   목적        : 게임내 실질적인 덱 오브젝트 생성및 덱 데이터값 보유 덱드로우 레이캐스트 사용
   최종 수정 날 : 7월 29일
  */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class MakeGame : MonoBehaviour {

	public List<GameObject> ui_card;
	public Transform m_curCard; //0806LSJ

    private int deckCount;
    Sprite back_image;
    CardSuffle cardmaneger;
    Transform DECK;

	Vector3[] ui_cardPos;	//old position
	Quaternion[] ui_cardRot;	//old rotation

	DrawCardManager[] dcManager = new DrawCardManager[4]; //0806 LSJ

    void Start () {
		//0806 LSJ
		for (int i = 0; i < 4; i++) {
			dcManager [i] = GameObject.Find ("Card" + (i + 1)).GetComponent<DrawCardManager> ();
		}

        DBCardHolder db = DBCardHolder.Instance;
        List<CardEx> deck = new List<CardEx>();

        DeckLoader loader = new DeckLoader();
        string path = loader.GetPath("deck.dat");

        using (StreamReader sr = new StreamReader(path, Encoding.UTF32, false))
        {
            while (sr.Peek() > -1)
            {
                int id = byte.Parse(sr.ReadLine()) - 1;
                deck.Add(db.GetItem((byte)id));
            }
        }

        deckCount = deck.Count - 1;

        cardmaneger = new CardSuffle ();

        this.cardmaneger.cards = new List<Card>();

        for (int i = 0; i < deck.Count; i++)
        {
            this.cardmaneger.cards.Add(new Card(deck[i].id, deck[i].name,deck[i].cost,deck[i].attackpoint,deck[i].counterpoint,deck[i].healthpoint,deck[i].card_type));
            
        }

        this.cardmaneger.shuffle();
        for (int num = 0; num < deck.Count; num++)
            CreateCard(num);

		foreach (Transform t in GameObject.Find("MyHand").transform) {
			ui_card.Add (t.gameObject);
		}

    }

    // Update is called once per frame
    public void CallCard(Transform t)
    {

        //if (Input.GetMouseButtonDown (0)) {
        RaycastHit hit;
        //Ray ray;

        var up = transform.TransformDirection(Vector3.down);
        if (CardCtrl.chkOn < 4)
        {
            if (Physics.Raycast(t.position, up, out hit, 20f))
            {
                if (hit.collider.gameObject.tag == "BACK")
                {
					//0806 LSJ
					m_curCard = hit.collider.transform.parent;
                    //7월 14일 끝
                    for (int i = 0; i < 4; i++)
                    {
						//0815 PJS
						float temp = ui_card[i].GetComponent<CanvasRenderer>().GetAlpha();
						if (temp == 0)
						{
							ui_card [i].GetComponent<CanvasRenderer> ().SetAlpha (1.0f);
							dcManager [i].CardData = this.cardmaneger.cards [deckCount]; //0815 LSJ
							dcManager [i].CheckDraw (m_curCard);
                            deckCount--;

							// **Audio Clip** : Draw Card Sound
							this.GetComponent<AudioSource>().Play();
							break;
						}
                        if (!ui_card[i].activeSelf)
                        {
                            ui_card[i].SetActive(true);
                            dcManager [i].CardData = this.cardmaneger.cards [deckCount]; //0815 LSJ
							dcManager [i].CheckDraw (m_curCard);
                            deckCount--;
                            
							// **Audio Clip** : Draw Card Sound
							this.GetComponent<AudioSource>().Play();
                            break;
                        }

                    }

					if (m_curCard != null) {
						StartCoroutine (cardmaneger.GetCard (m_curCard));
						CardCtrl.chkOn++;
					}

                }
            }
        }
    }

    //나중 마테리얼 변경참고
    void ChangeMaterial (GameObject tempObj)
    {
        //tempObj.GetComponent<MeshRenderer>().material = tempMat;
    }

  

    void CreateCard(int num)
    {
        
        string Snum = num.ToString();
        if (num < 10)
            Snum = '0' + Snum;
        // OCard;
        if (this.cardmaneger.cards[num].card_type==CARD_TYPE.고체)
        {
            GameObject OCard = Resources.Load("CARD_earth_spirit") as GameObject;
            OCard.GetComponent<MeshRenderer>().material = Resources.Load("UiCharMaterials/" + this.cardmaneger.cards[num].name) as Material;
            OCard.name = this.cardmaneger.cards[num].name +" 비용 " + this.cardmaneger.cards[num].cost+" 충격 " + this.cardmaneger.cards[num].attackpoint+" 저항 " + this.cardmaneger.cards[num].counterpoint +" 기력 "+ this.cardmaneger.cards[num].healthpoint+" 타입 " + this.cardmaneger.cards[num].card_type;
            Instantiate(OCard, new Vector3(68f, (0.002f * (num+1))+1f, -10f), Quaternion.identity);
        }
        else if(this.cardmaneger.cards[num].card_type == CARD_TYPE.기체)
        {
            /*
            GameObject OCard = Resources.Load("CARD_ice_spirit") as GameObject;
            OCard.name = this.cardmaneger.cards[num].name + " 비용 " + this.cardmaneger.cards[num].cost + " 충격 " + this.cardmaneger.cards[num].attackpoint + " 저항 " + this.cardmaneger.cards[num].counterpoint + " 기력 " + this.cardmaneger.cards[num].healthpoint + " 타입 " + this.cardmaneger.cards[num].card_type;
            Instantiate(OCard, new Vector3(68f, (0.002f * (num + 1))+1f, -10f), Quaternion.identity);
            */
            
        }
        else if(this.cardmaneger.cards[num].card_type == CARD_TYPE.액체)
        {
            /*
            GameObject OCard = Resources.Load("CARD_wind_spirit") as GameObject;
            OCard.name = this.cardmaneger.cards[num].name + " 비용 " + this.cardmaneger.cards[num].cost + " 충격 " + this.cardmaneger.cards[num].attackpoint + " 저항 " + this.cardmaneger.cards[num].counterpoint + " 기력 " + this.cardmaneger.cards[num].healthpoint + " 타입 " + this.cardmaneger.cards[num].card_type;
            Instantiate(OCard, new Vector3(68f, (0.002f * (num + 1))+1f, -10f), Quaternion.identity);
            */
        }




        //나중 마테리얼 변경참고
        /*GameObject OCard_temp = Resources.Load("CARD_earth_spirit") as GameObject;
        Instantiate(OCard_temp, new Vector3(11f, (0.002f * num), -2f), Quaternion.identity);
        OCard_temp.GetComponent<MeshRenderer>().material = null;*/
        
    }

	IEnumerator DrawTimer(){
		yield return new WaitForEndOfFrame ();
	}
}
