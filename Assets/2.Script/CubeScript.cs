//작성자 : 서범석
//목적 : 필드카드 이벤트 처리
//최근 수정일 : 2016-07-28
//수정 내용 
// - 2016-07-14 : 스크립트 병합
// - 2016-07-28 : 공격 애니메이션 제작
using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour
{
    //8월24일 손황호 추가
    GameObject LeftText;
    GameObject RightText;
    //8월24일 손황호 추가 끝
    public int flag = 0; // 공격용 플래그
    private int movingFlag = 0;  // 움직임 애니메이션용 플래그
    private int moveCount = 0; //움직임 애니메이션 흐름용 카운트

    public int shock;
    public int resistance;
    public int stamina;

    GameObject attackObj;
    GameObject deffenseObj;

    CubeScript attack;
    CubeScript deffense;

    private FieldScript fields;
    private LineScript lines;
    //8월 15일 손황호 추가
    public GameMgr GM;

    private Vector3 oldVector;
    private Vector3 newVector;
    /// <summary>
    /// 제약조건을 위해 추가하였습니다 필드로 카드를 냈을 때의 턴을 가져와서 공격을 할 수 있는 지를 확인합니다.
    /// </summary>
    private int turn;  //0817 BJS

    // Use this for initialization
    void Start()
    {
        //0817 Data value
        this.shock = this.GetComponent<Field_CardCtrl>().SetCardData.attackpoint;
        this.resistance = this.GetComponent<Field_CardCtrl>().SetCardData.counterpoint;
        this.stamina = this.GetComponent<Field_CardCtrl>().SetCardData.healthpoint;

        turn = GameMgr.showturn;
        fields = GameObject.Find("FieldManager").GetComponent("FieldScript") as FieldScript;
        lines = GameObject.Find("Line").GetComponent("LineScript") as LineScript;
        //8월 15일 손황호 추가
        GM = GameObject.Find("GameManager").GetComponent("GameMgr") as GameMgr;
        //8월 24일 손황호 추가
        Transform[] childrentemp = this.gameObject.GetComponentsInChildren<Transform>();
        LeftText = childrentemp[4].gameObject;
        RightText = childrentemp[6].gameObject;
        //8월 24일 손황호 추가 끝
    }

    // Update is called once per frame
    void Update()
    { //8월 24일 손황호 추가
        RightText.GetComponent<TextMesh>().text = System.Convert.ToString(stamina);
        //My player
        if (GameMgr.myTurn == true && this.tag == "Player_Field_Card")
            LeftText.GetComponent<TextMesh>().text = System.Convert.ToString(shock);
        else if (GameMgr.myTurn == false && this.tag == "Player_Field_Card")
            LeftText.GetComponent<TextMesh>().text = System.Convert.ToString(resistance);
        //Enemy player
        if (GameMgr.myTurn == false && this.tag == "Enemy_Field_Card")
            LeftText.GetComponent<TextMesh>().text = System.Convert.ToString(shock);
        else if (GameMgr.myTurn == true && this.tag == "Enemy_Field_Card")
            LeftText.GetComponent<TextMesh>().text = System.Convert.ToString(resistance);
        //8월 24일 손황호 추가 끝
        //움직이는 애니메이션
        if (movingFlag == 1)
        {
            this.gameObject.transform.Translate(
                new Vector3((oldVector.x - newVector.x) / 10,
                    0,
                    (newVector.z - oldVector.z) / 10
                )
            );
            moveCount++;
            if (moveCount >= 10)
                movingFlag = 2;
        }
        if (movingFlag == 2)
        {
            this.gameObject.transform.Translate(
                new Vector3((newVector.x - oldVector.x) / 10,
                    0,
                    (oldVector.z - newVector.z) / 10
                )
            );
            moveCount--;
            //8월 15일 손황호 추가
            if (moveCount <= 0)
            {
                movingFlag = 0;
                if (deffenseObj.transform.gameObject.tag == "Enemy_Field_Card")
                    CardToCard();
                else if (deffenseObj.transform.gameObject.tag == "Enemy_Char")
                    CardToPlayer();
                //끝
            }
        }
    }

    public void FieldMouseDown(GameObject gameObject)
    {
        if (turn != GameMgr.showturn) //0817 BJS
        {
            flag = 1;
            attack = gameObject.GetComponent<CubeScript>();
            attackObj = gameObject;
            //Debug.Log (hit.transform.gameObject.name);
            //fields.addBattleCard ();

            oldVector = gameObject.transform.position;
            lines.setStartPosition(oldVector + new Vector3(0, 10, 0));

			// **Audio Clip** : Drawing Bow
			this.GetComponent<AudioSource>().Play();
            turn = GameMgr.showturn;
        }
    }

    public void FieldMouseUp(GameObject gameObject)
    {
        if (flag == 1)
        {
            deffense = gameObject.GetComponent<CubeScript>();
            deffenseObj = gameObject;
            newVector = gameObject.transform.position;
            movingFlag = 1;
        }
        //끝

        lines.setStartPosition(new Vector3(0, 10, 0));
        lines.setLastPosition(new Vector3(0, 10, 0));
        flag = 0;
    }

    public void FieldUpdate(GameObject gameObject)
    {
        if (flag == 1)
        {
            //라인 그리기
            lines.setLastPosition(oldVector);
            //8월 15일 손황호 수정
            if (gameObject.transform.gameObject.tag == "Enemy_Field_Card" || gameObject.transform.gameObject.tag == "Enemy_Field_Card_S" || gameObject.transform.gameObject.tag == "Enemy_Char")
                lines.setLastPosition(gameObject.transform.position + new Vector3(0, 10, 0));
        }
    }

    void CardToCard()
    {
        deffense.stamina = deffense.stamina - attack.shock;
        attack.stamina = attack.stamina - deffense.resistance;
        Sorting tempSortScript;
        tempSortScript = GameObject.Find("FieldManager").GetComponent("Sorting") as Sorting;

        if (attack.stamina <= 0)
        {
            tempSortScript.removeObj(attackObj);
            Destroy(attackObj);
        }
        if (deffense.stamina <= 0)
        {
            tempSortScript.removeObj(deffenseObj);
            Destroy(deffenseObj);
        }
    }

    //8월 15일 손황호 추가
    void CardToPlayer()
    {
        GM.player2.HP -= attack.shock;
        GM.Player2HPText.text = System.Convert.ToString(GM.player2.HP);
    }
    //끝
}
