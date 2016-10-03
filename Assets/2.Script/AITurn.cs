using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class AITurn : MonoBehaviour {

	CardEx[] cpudeck;
	int deckIndex;


	List<Transform> playerField;
	List<Transform> pcField;

	public GameMgr GM;

	Vector3 oldVector;
	Vector3 newVector;
	System.Random ran;
	int ranInt;

	int flowFlag = 0;
	int movingFlag = 0;
	int moveCount = 0;
	int cardOrPlayerFlag = 0;

	public GameObject field_Card;


	// Use this for initialization
	void Start ()
    {
        playerField = new List<Transform> ();
		pcField = new List<Transform> ();
		GM = GameObject.Find("GameManager").GetComponent("GameMgr") as GameMgr;

		DBCardHolder db = DBCardHolder.Instance;
        List<CardEx> deck = new List<CardEx>();
        int[] id = CPUDeck.GetDeck();

        for(int i = 0; i < id.Length; i++)
        {
            deck.Add(db.GetItem((byte)(id[i] - 1)));
        }

        cpudeck = deck.ToArray();
		deckIndex = 0;

        for(int i = 0; i < cpudeck.Length; i++)
        {
            Debug.Log(cpudeck[i].id);
        }
    }
	
	// Update is called once per frame
	void Update () {
		AttackUpdate ();
	}

	public AITurn() {
		
	}

	public IEnumerator Attack() {
		Transform[] tempTransform = GameObject.Find ("FieldManager").GetComponentsInChildren<Transform> ();
		//Debug.Log (temp [0].name);
		if (tempTransform.GetLength (0) > 1) {
			pcField.Clear ();
			playerField.Clear ();
			for (int i = 1; i < tempTransform.GetLength (0); i++) {
				if (tempTransform [i].tag == "Enemy_Field_Card") {
					pcField.Add (tempTransform [i]);
					Debug.Log ("pc " + i);
				} else if (tempTransform [i].tag == "Player_Field_Card") {
					playerField.Add (tempTransform [i]);
					Debug.Log ("player " + i);
				}
			}
        }

		//Debug.Log ("pc "+playerField [0].gameObject.GetComponent<Field_CardCtrl> ().SetCardData.attackpoint);

		if (pcField.Count > 0) {
			if (playerField.Count > 0) {
				//상대 카드 때리기

				//때릴 수 있는 카드의 공격력의 합이 상대 플레이어 체력을 넘으면 플레이어를 때림
				int totalShock = 0;
				for (int i = 0; i < pcField.Count; i++) {
					totalShock = totalShock + 
						pcField [i].gameObject.GetComponent<Field_CardCtrl> ().SetCardData.attackpoint;
				}
				if (totalShock >= GM.player1.HP) {
					flowFlag = pcField.Count;
					movingFlag = 1;
					cardOrPlayerFlag = 1;
					oldVector = pcField [flowFlag - 1].position;
					newVector = GameObject.Find ("Cube").transform.position;
				} else {
					flowFlag = pcField.Count;
					movingFlag = 1;
					cardOrPlayerFlag = 2;
					oldVector = pcField [flowFlag - 1].position;
					ran = new System.Random();
					ranInt = ran.Next (playerField.Count);
					newVector = playerField [ranInt].position;
				}

				//왼쪽부터 차례로 때린다
				//때릴 놈 공격력과 맞을 놈 체력이 같은것 우선으로
				//조건에 맞는 상황이 복수 이면 저항이 낮은 놈을 때린다
			} else {
				//플레이어 때리기
				flowFlag = pcField.Count;
				movingFlag = 1;
				cardOrPlayerFlag = 1;
				oldVector = pcField [flowFlag - 1].position;
				newVector = GameObject.Find ("Cube").transform.position;
			}
		}

		Debug.Log (cpudeck [0].name);
        yield return null;
    }

	public void AttackUpdate() {
		if (flowFlag > 0) {
			if (cardOrPlayerFlag == 1) {

				Debug.Log ("flowFlag : " + flowFlag);
				if (movingFlag == 1) {
					pcField [flowFlag - 1].Translate (
						new Vector3 ((oldVector.x - newVector.x) / 10,
							0,
							(newVector.z - oldVector.z) / 10
						)
					);
					Debug.Log ("moveCount : " + moveCount);
					moveCount++;
					if (moveCount >= 10)
						movingFlag = 2;
				}
				if (movingFlag == 2) {
					pcField [flowFlag - 1].Translate (
						new Vector3 ((newVector.x - oldVector.x) / 10,
							0,
							(oldVector.z - newVector.z) / 10
						)
					);
					moveCount--;
					if (moveCount <= 0) {
						GM.player1.HP -= pcField [flowFlag - 1].GetComponent<CubeScript>().shock;
						GM.Player1HPText.text =  System.Convert.ToString(GM.player1.HP);

						if (flowFlag > 0) {
							flowFlag--;

							if (flowFlag > 0) {
								movingFlag = 1;
							} else {
								movingFlag = 0;
								cardOrPlayerFlag = 0;
								GameMgr.myTurn = true;
								GameObject.Find ("GameManager").GetComponent<GameMgr> ().turnEnd.myturn = true;
							}
							oldVector = pcField [flowFlag - 1].position;
							newVector = GameObject.Find ("Cube").transform.position;
						} 
					}
				}
			} else if (cardOrPlayerFlag == 2) {				
				Debug.Log ("flowFlag : " + flowFlag);
				if (movingFlag == 1) {
					pcField [flowFlag - 1].Translate (
						new Vector3 ((oldVector.x - newVector.x) / 10,
							0,
							(newVector.z - oldVector.z) / 10
						)
					);
					Debug.Log ("moveCount : " + moveCount);
					moveCount++;
					if (moveCount >= 10)
						movingFlag = 2;
				}
				if (movingFlag == 2) {
					pcField [flowFlag - 1].Translate (
						new Vector3 ((newVector.x - oldVector.x) / 10,
							0,
							(oldVector.z - newVector.z) / 10
						)
					);
					moveCount--;
					if (moveCount <= 0) {			
						Sorting tempSortScript;
						tempSortScript = GameObject.Find ("FieldManager").GetComponent ("Sorting") as Sorting;
						playerField[ranInt].GetComponent<CubeScript>().stamina -=
							pcField [flowFlag - 1].GetComponent<CubeScript> ().shock;
						pcField [flowFlag - 1].GetComponent<CubeScript> ().stamina -=
							playerField [ranInt].GetComponent<CubeScript> ().resistance;

						if (playerField[ranInt].GetComponent<CubeScript>().stamina <= 0) {
							tempSortScript.removeObj (playerField[ranInt].gameObject);
							Destroy (playerField[ranInt].gameObject);
							playerField.RemoveAt (ranInt);
						}
						if (pcField [flowFlag - 1].GetComponent<CubeScript>().stamina <= 0) {
							tempSortScript.removeObj (pcField [flowFlag - 1].gameObject);
							Destroy (pcField [flowFlag - 1].gameObject);
							pcField.RemoveAt (flowFlag - 1);
						}

						if (flowFlag > 0) {
							flowFlag--;

							if (flowFlag > 0) {
								movingFlag = 1;
							} else {
								movingFlag = 0;
								cardOrPlayerFlag = 0;
								GameMgr.myTurn = true;
								GameObject.Find ("GameManager").GetComponent<GameMgr> ().turnEnd.myturn = true;
							}
							oldVector = pcField [flowFlag - 1].position;
							ran = new System.Random();
							ranInt = ran.Next (playerField.Count);
							newVector = playerField [ranInt].position;
						} 
					}
				}
			}
		}
	}

	public void Throw() {
		System.Random ran = new System.Random();
		int num = ran.Next(140) -70;

		var temp = Instantiate(field_Card, new Vector3(num, 0, 10), new Quaternion(0f, 0f, 0f, 0f)) as GameObject;
		Transform[] childrentemp = temp.GetComponentsInChildren<Transform>();

		temp.tag = "Enemy_Field_Card";
		temp.transform.parent = GameObject.Find("FieldManager").transform;
		childrentemp[2].GetComponent<Renderer>().material = Resources.Load("CharMaterials/" + cpudeck [deckIndex].name) as Material;

        childrentemp[2].tag = "Enemy_Field_Card_S";
        temp.GetComponent<CubeScript> ().shock = cpudeck [deckIndex].attackpoint;
		temp.GetComponent<CubeScript> ().resistance = cpudeck [deckIndex].counterpoint;
		temp.GetComponent<CubeScript> ().stamina = cpudeck [deckIndex].healthpoint;

		temp.GetComponent<Field_CardCtrl>().SetCardData = new Card (cpudeck [deckIndex].id,
			cpudeck [deckIndex].name, 
			cpudeck [deckIndex].cost,
			cpudeck [deckIndex].attackpoint,
			cpudeck [deckIndex].counterpoint,
			cpudeck [deckIndex].healthpoint, 
			cpudeck [deckIndex].card_type);

		GM.player2.Cost += cpudeck [deckIndex].cost;

		deckIndex++;
	}
}
