/* 파일명      : Field_CardCtrl.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Field_CardCtrl : MonoBehaviour {

	//0815 LSJ
	Card f_cardData;	//Check Field Card Data
	CardCtrl uicardManager;

	public GameObject f_Card;
	public Rigidbody cardRb; //물리 체크 
	Vector3 f_cardPos; //생성된 필드카드 좌표

	private static Renderer f_carMat; //0809 LSJ : Field Card Material
	private float force;	//카드의 운동 에너지 

	//0815 LSJ
	public Card SetCardData{
		get{ return f_cardData; }
		set{ f_cardData = value; }
	}

	void Awake(){
		//sort = GameObject.Find ("Field_Manager").GetComponent<Sorting> ();
		//0809 LSJ
		f_carMat = this.GetComponent<Renderer>();

	}	

    
	void Start () {
		force = 10.0f;
        //8월 17일 손황호 수정
        if (this.tag == "Player_Field_Card")
        {
            f_Card.transform.rotation = new Quaternion(0f, 0f, 360f, 0f);
        }
        else
        {
            f_Card.transform.rotation = new Quaternion(0f, 0f, 360f, 0f);
        }
        //8월 17일 끝
        f_cardPos = f_Card.GetComponent<Transform> ().transform.position;
		cardRb = GetComponent<Rigidbody> ();

		//GameObject.Find ("FieldManager").GetComponent<Sorting> ().insertObj(this.transform.position.x, f_Card);

		StartCoroutine(GameObject.Find ("FieldManager").GetComponent<Sorting> ().insertObj(this.transform.position.x, f_Card));
		//0815 LSJ
		//Debug.Log (f_cardData.cost);
    }
    

	void Update () {
		f_Card.transform.DOMoveY (0.1f, 0.4f);
	}

	void FixedUpdate(){
		cardRb.AddForce (transform.up * force);	//물리 운동 
	}
}
