//작성자 : 서범석
//목적 : 필드카드 이벤트 처리
//최근 수정일 : 2016-07-28
//수정 내용 
// - 2016-07-14 : 스크립트 병합
// - 2016-07-28 : 공격 애니메이션 제작
using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {
	

	public int flag = 0; // 공격용 플래그
	private int movingFlag = 0;  // 움직임 애니메이션용 플래그
	private int moveCount = 0; //움직임 애니메이션 흐름용 카운트

	public int shock = 4;
	public int resistance = 2;
	public int stamina = 5;

	GameObject attackObj;
	GameObject deffenseObj;

	CubeScript attack;
	CubeScript deffense;

	private FieldScript fields;
	private LineScript lines;

	private Vector3 oldVector;
	private Vector3 newVector;

	// Use this for initialization
	void Start () {
		fields = GameObject.Find ("FieldManager").GetComponent ("FieldScript") as FieldScript;
		lines = GameObject.Find ("Line").GetComponent("LineScript") as LineScript;
	}
	
	// Update is called once per frame
	void Update () {
		//움직이는 애니메이션
		if (movingFlag == 1) {
			this.gameObject.transform.Translate (
				new Vector3 ((oldVector.x - newVector.x) / 10,
					0,
					(oldVector.z - newVector.z) / 10
				)
			);
			moveCount++;
			if (moveCount >= 10)
				movingFlag = 2;
		}
		if (movingFlag == 2) {
			this.gameObject.transform.Translate (
				new Vector3 ((newVector.x - oldVector.x) / 10,
					0,
					(newVector.z - oldVector.z) / 10
				)
			);
			moveCount--;
			if (moveCount <= 0) {
				movingFlag = 0;
				CardToCard ();
			}
		}
	}

	public void FieldMouseDown(GameObject gameObject) {
		flag = 1;
		attack = gameObject.GetComponent<CubeScript>();
		attackObj = gameObject;
		//Debug.Log (hit.transform.gameObject.name);
		//fields.addBattleCard ();

		oldVector = gameObject.transform.position;
		lines.setStartPosition (oldVector + new Vector3 (0, 10, 0));
	}

	public void FieldMouseUp(GameObject gameObject) {
		if (flag == 1) {
			if (gameObject.transform.gameObject.tag == "Field_Card") {

				deffense = gameObject.GetComponent<CubeScript> ();
				deffenseObj = gameObject;
				//Debug.Log ("bb");

				newVector = gameObject.transform.position;

				if (deffense.flag == 0) {
					//CardToCard ();
					//Debug.Log ("cc");
					movingFlag = 1;
				}
			} else if (gameObject.transform.gameObject.tag == "Player") {
				CardToPlayer ();
			}
			
		}

		lines.setStartPosition (new Vector3 (0, 10, 0));
		lines.setLastPosition (new Vector3 (0, 10, 0));
		flag = 0;
	}

	public void FieldUpdate(GameObject gameObject) {
		if (flag == 1) {
			//라인 그리기
			lines.setLastPosition (oldVector);
			if(gameObject.transform.gameObject.tag == "Field_Card" || gameObject.transform.gameObject.tag == "Player")
				lines.setLastPosition (gameObject.transform.position + new Vector3 (0, 10, 0));
		}
	}

	void CardToCard() {
		deffense.stamina = deffense.stamina - attack.shock;
		attack.stamina = attack.stamina - deffense.resistance;
		Sorting tempSortScript;
		tempSortScript = GameObject.Find ("FieldManager").GetComponent ("Sorting") as Sorting;

		if (attack.stamina <= 0) {
			tempSortScript.removeObj (attackObj);
			Destroy (attackObj);
		}
		if (deffense.stamina <= 0) {
			tempSortScript.removeObj (deffenseObj);
			Destroy (deffenseObj);
		}
	}

	void CardToPlayer() {

	}
}
