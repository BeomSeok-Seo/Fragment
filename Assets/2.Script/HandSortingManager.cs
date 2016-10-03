/* 파일명      : HandSortingManager.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;

public class HandSortingManager : MonoBehaviour {

	public GameObject[] cards;
	public int cardCnt;
	public float cardRds;

	// Use this for initialization
	void Start () {
		CountCards ();
	}
	
	// Update is called once per frame
	void Update () {
		CountCards ();
		SortingCards ();
	}

	void CountCards(){
		foreach (GameObject cc in cards) {
			if (cc.activeSelf) {
				cardCnt++;
			}
		}
		cardRds = 25.6f / (float)cardCnt;
	}

	void SortingCards(){
		switch (cardCnt) {
		case 4:
			//Rotation
			for (int i = 2; i >= -2; i--) {
				cards [i + 1].transform.Rotate (new Vector3 (0f, 0f, cardRds * (float)i));
				if (i == 0) {
					continue;
				}
				if (i == -2)
					cards [i + 3].transform.Rotate (new Vector3 (0f, 0f, cardRds * (float)i));
			}
			//Position
			for (int j = 1; j <= cardCnt; j++) {
				//cards [j - 1].transform.position += new Vector3 (4.0f+(j*60),);
			}
			break;

		case 3:
			//Rotation
			for (int i = 1; i >= -1; i--) {
				cards [i].transform.Rotate (new Vector3(0f, 0f, cardRds*(float)i));
			}
			//Position
			for (int j = 1; j <= cardCnt; j++) {
				//cards [j - 1].transform.position += new Vector3 (34.0f+(j*60));
			}
			break;

		case 2:
			//for()
			break;

		case 1:
			cards[0].transform.Rotate(new Vector3(0f,0f,0f));
			//cards[0].transform.position += new Vector3(154.0f);
			break;

		default:
			break;
		}
	}
}
