/* 파일명      : CardCtrl.cs
   작성자      : 이승진
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CardCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    FieldScript fieldManager;
	DrawCardManager dcManager; //0809 LSJ
    GameMgr gameManager; //0817 BJS

    public GameObject field_Card;
    public Transform field_CardTr;
    public GameObject ui_Card;
    public Transform ui_CardTr;
    public Vector3 mousePos;
	public GameObject p_Throw;
	private Object p_temp;

    public static int chkOn = 0; //0815 PJS

    Vector3 ui_CardPos;
    Vector3 ui_LocP;
    MakeGame mg;

    Vector3 scaletemp;
    Quaternion rotationtemp;
    Vector3 postemp;

    RectTransform recttemp;
    //private bool showChk;

    //연결시키기  7월14일
    /*public CardCtrl(MakeGame mg)
    {
        this.mg = mg;
    }*/
    //7월14일 끝
    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (fieldManager.FieldCardCount < 10)
        //{
        if (GameMgr.myTurn)
        {
            if (!gameManager.player1.ThrowCard(this.GetComponent<DrawCardManager>().CardData)) //코스트를 확인하기 위해서 PlayerInfo의 함수를 호출합니다. 만약 코스트에 걸리지 않으면 true를 반환합니다
            {
                recttemp.transform.localScale = scaletemp;
                recttemp.transform.localPosition = postemp;
                recttemp.transform.localRotation = rotationtemp;
                return;
            }
            //8월 6일 손황호 위에꺼와 교체
            if (Sorting.Player_fieldCard.Count < 5)
            {
                System.Threading.Thread.Sleep(200);
                ui_Card.SetActive(false);
                chkOn--;
                gameManager.CostTextChange();//0817 BJS 코스트를 보여주기 위해 추가
                //Debug.Log(playerinfo.ThrowCard(this.GetComponent<DrawCardManager>().CardData));
                CreateCard(field_Card, transform.position, new Quaternion(0f, 0f, 0f, 0f));

				// **Particle System** : Destroy Particle
				Invoke("DestroyParticle", 1.5f);

				// **Audio Clip** : PlayaCard Sound
				this.GetComponentInParent<AudioSource>().Play();

                fieldManager.FieldCardCount++;
                ui_Card = null;
                transform.localPosition = ui_CardPos;

                // 19, July new Update
                recttemp.transform.localScale = scaletemp;
                recttemp.transform.localPosition = postemp;
                recttemp.transform.localRotation = rotationtemp;
                //end
            }
            else
            {
                recttemp.transform.localScale = scaletemp;
                recttemp.transform.localPosition = postemp;
                recttemp.transform.localRotation = rotationtemp;
            }
        }
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        if (GameMgr.myTurn)
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 100.0f; //distance of the plane from the camera
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
    }

    #endregion

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        ui_Card = gameObject;
        ui_CardPos = transform.localPosition;// transform.position;
    }
    #endregion
    //8월 17일 손황호 수정
    public void CreateCard(GameObject ob, Vector3 v, Quaternion q)
    {
        var temp = Instantiate(ob, v, q) as GameObject;

		// **Particle System** : ThrowCard Particle
		p_temp = Instantiate(p_Throw, v, q);

        Transform[] childrentemp = temp.GetComponentsInChildren<Transform>();
        if (GameMgr.myTurn == true)
        {
            temp.tag = "Player_Field_Card";
            childrentemp[2].tag = "Player_Field_Card_S";
        }
        else if (GameMgr.myTurn == false)
        {
            temp.tag = "Enemy_Field_Card";
            childrentemp[2].tag = "Enemy_Field_Card_S";
        }

        temp.transform.parent = GameObject.Find("FieldManager").transform;
        //0809 LSJ
        //8월 17일 손황호 수정 1줄
        childrentemp[2].GetComponent<Renderer>().material = Resources.Load("CharMaterials/" + dcManager.HandCardMaterial) as Material;
        //0815 LSJ
        temp.GetComponent<Field_CardCtrl>().SetCardData = this.GetComponent<DrawCardManager>().CardData;

    }
    //8월 17일 끝
    //8월 17일 끝

    void Start()
    {
		ui_Card.GetComponent<CanvasRenderer>().SetAlpha(0f); //0815 PJS
		dcManager = this.GetComponent<DrawCardManager> (); //0809 LSJ
        fieldManager = GameObject.Find("FieldManager").GetComponent<FieldScript>();

        //Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        scaletemp = ui_Card.transform.localScale;
        postemp = ui_Card.transform.localPosition;
        rotationtemp = ui_Card.transform.localRotation;

        recttemp = ui_Card.transform.GetComponent<RectTransform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameMgr>();
    }

    void Update()
    {
    }

	private void DestroyParticle(){
		Destroy (p_temp);
	}
}
