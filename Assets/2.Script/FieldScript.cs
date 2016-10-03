/* 파일명      : FieldScript.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FieldScript : MonoBehaviour {
	public GameObject m_goBattleCardClone;
	public List<GameObject> m_lField;

	private int f_cardCnt;

	public int FieldCardCount{
		get{ return f_cardCnt; }
		set{ f_cardCnt = value; }
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		f_cardCnt = this.transform.childCount;
	}

	public void addBattleCard(){//Card addCard) {
		//m_lField.Add (addCard);

		GameObject goTemp = Instantiate (m_goBattleCardClone, Vector3.zero, Quaternion.identity) as GameObject;
		goTemp.transform.parent = this.transform;
	}
}
