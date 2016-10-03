/*
  파일명       : GameMgr.cs
  작성자       : 방준선
  목적         : 게임 매니저(턴 관리)
  최종 수정 날 : 
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    //8월 6일 손황호 추가
    CardCtrl createcard;
    //
    public Text TextTurn; //현재 턴을 보여주기 위한 텍스트
    public Text TextShow; //승자를 보여주기 위한 텍스트
    public Text Player1HPText; //P1의 체력 텍스트
    public Text Player2HPText; //P2의 체력 텍스트
    public Text Player1CPText; //P1의 체력 텍스트
    public Text Player2CPText; //P2의 체력 텍스트
    public PlayerInfo player1, player2;
    public static int showturn; //턴
    public turnend turnEnd; //턴종료 스크립트
    public GameObject turnButton; //턴종료버튼
    public GameObject turnButtonSpawnPoint; //턴종료버튼 생성위치
	public GameObject resultPanel; //0827 LSJ : when Game is overed
	public GameObject resultWin; //0827 LSJ
	public GameObject resultLose; //0827 LSJ
	public GameObject p_Hit;

    //public static List<GameObject> fieldCard;
    bool mousemove; //마우스 드래그 확인용

    CubeScript test; //필드카드
    MakeGame makegame; //카드 불러오기 위해 Card Deck 오브젝트의 스크립트 참조용

    private WaitForSeconds m_StartDrawWait; //게임 처음 시작 시 덱에서 카드로 보내는 딜레이
    private int tempturn;
	private Object p_temp;

    // 게임 진행 상황.
    private enum GameProgress
    {
        None = 0,       // 시합 시작 전.
        Start,          // 시합 시작.
        Turn,           // 시합 중.
        Result,         // 결과 표시.
        GameOver,       // 게임 종료.
        Disconnect,     // 연결 끊기. 서버연동시
    };

    // 턴 종류.
    private enum Turn
    {
        Player1 = 0,        // 자신의 턴.
        Player2,       // 상대의 턴.
    };

    // 시합 결과.
    private enum Winner
    {
        None = 0,       // 시합 중.
        Player1,         // P1승리.
        Player2          // P2승리.
    };

    public static bool myTurn;

    // 시합 시작 전의 대기 시간.
    private const float waitTime = 1.0f;
    private float enemyTurnTime = 3.0f;
    private float DrawTime = 5.0f;

    // 진행 상황.
    private GameProgress progress;

    // 현재의 턴.
    private Turn turn;

    // 로컬 기호.
    //private Turn myTurn;

    // 리모트 기호.
    //private Turn enemyTurn;

    // 승자.
    private Winner winner;

    // 게임 종료 플래그.
    private bool isGameOver;

    // 대기 시간.
    private float currentTime;
    RaycastHit hit;

    Ray ray;

    GameObject GetCardEmptyObject; //카드를 호출해서 레이캐스트를 쏘기 위한 빈 게임 오브젝트

    void Awake()
    {
        player1 = GameObject.Find("P1").GetComponent<PlayerInfo>();
        player2 = GameObject.Find("P2").GetComponent<PlayerInfo>();
    }

    // Use this for initialization
    void Start()
    {
        //Screen.autorotateToLandscapeLeft = true;
		resultPanel.SetActive (false); //0827 LSJ : reset result panel
        //Screen.orientation = ScreenOrientation.Landscape;
        //Screen.SetResolution(1280, 720, true);
        // 게임을 초기화합니다.
        Reset();
        isGameOver = false;
        GameStart();
        GameObject temp = Instantiate(turnButton, turnButtonSpawnPoint.transform.position, turnButtonSpawnPoint.transform.rotation) as GameObject;
        turnEnd = temp.GetComponent<turnend>();

        GetCardEmptyObject = new GameObject();
        GetCardEmptyObject.transform.position = new Vector3(62f, 3f, -10f);
        makegame = GameObject.Find("CardDeck").GetComponent<MakeGame>();
        //fieldCard = new List<GameObject>(5);

        //8월 6일 손황호추가
        createcard = FindObjectOfType<CardCtrl>();

        m_StartDrawWait = new WaitForSeconds(0.25f);

        StartCoroutine(this.GameLoop());

        Player1HPText.text = System.Convert.ToString(player1.HP);
        Player2HPText.text = System.Convert.ToString(player2.HP);
        Player1CPText.text = System.Convert.ToString(player1.Cost);
        Player2CPText.text = System.Convert.ToString(player2.Cost);

        tempturn = showturn;
        TextTurn.text = showturn + "턴";

		//Set Particle's PlayBack Speed 
		p_Hit.GetComponent<ParticleSystem>().playbackSpeed = 3.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Turn") //클릭한 대상이 턴버튼
                    {
                        //myTurn = (turn == Turn.Player1) ? false : true;
                        Player1HPText.text = System.Convert.ToString(player1.HP);
                        Player2HPText.text = System.Convert.ToString(player2.HP);
                        turnEnd.myturn = false;
                        //turnEnd.test();
                        //Debug.Log(turnEnd.flip);

						// **Audio Clip** : Turn Button Click
						this.GetComponent<AudioSource>().Play();
                        myTurn = false;
                    }
                   // if (Input.mousePosition.y < Screen.height / 2)
                    //{
                        //8월 6일 손황호
                        if (hit.collider.tag == "Player_Field_Card_S")
                        {
                            test = hit.transform.gameObject.GetComponent<CubeScript>();
                            GameObject temp = hit.transform.gameObject;
                            test.FieldMouseDown(temp);

                            mousemove = true;
                            // test.hitPosition = hit.point;
                            //test.isMoveState = true;
                        }
                   // }

                }
            }
            else
            {
                if (mousemove)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        GameObject temp = hit.collider.gameObject;
                        test.FieldUpdate(temp);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                mousemove = false;
                if (Physics.Raycast(ray, out hit))
                {
                    //8월 6일 손황호
					if (hit.collider.tag == "Enemy_Field_Card_S" || hit.collider.tag == "Enemy_Char") {
						Debug.Log ("얏타1");
						//test = GameObject.FindWithTag("Field_Card").GetComponent<CubeScript>();
						//8월 17일 손황호 수정
						GameObject temp = hit.transform.gameObject;

						// **Particle System** : Hit Particle
						p_temp = Instantiate(p_Hit, new Vector3(temp.transform.position.x, 1.2f, temp.transform.position.z), Quaternion.identity);
						Invoke ("DestroyParticle", 1.0f);

						// **Audio Source** : Hit Sound
						//p_Hit.GetComponent<AudioSource>().Play();

						//끝
						test.FieldMouseUp (temp);

						Debug.Log ("Good");

						// test.hitPosition = hit.point;
						//test.isMoveState = true;
					} else {

					}
                }
            }
        }
    }

    /*switch (progress)
    {
        case GameProgress.Start:
            //Debug.Log("업데이트 레디");
            UpdateStart();
            break;

        case GameProgress.Turn:
            //Debug.Log("업데이트 턴");
            UpdateTurn();
            break;

        case GameProgress.GameOver:
            UpdateGameOver();
            break;
    }*/


    // 
    void OnGUI()
    {
        switch (progress)
        {
            case GameProgress.Start:
                //();
                break;

            case GameProgress.Turn:
                // 남은 시간을 그립니다.

                /*if (turn == myTurn)
                {
                    GUISkin skin = GUI.skin;
                    GUIStyle style = new GUIStyle(GUI.skin.GetStyle("button"));
                    style.normal.textColor = Color.white;
                    style.fontSize = 25;

                    if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 100), "턴종료", style))
                    {
                        bool setMark = false;
                        setMark = DoOwnTurn();
                    }
                }*/
                break;

            case GameProgress.Result:
                // 승자를 표시합니다.
                DrawWinner();
                break;

            case GameProgress.GameOver:
                // 승자를 표시합니다.
                DrawWinner();
                break;

            default:
                break;
        }

    }

    void UpdateStart()
    {
        // 시합 시작 신호 표시를 기다립니다.
        TextShow.text = "마음의 준비를 하는 중";
        currentTime += Time.deltaTime;
        showturn = 1;

        //0815 PJS
        //makegame.CallCard(GetCardEmptyObject.transform);
        if (currentTime > waitTime)
        {
            // 표시가 끝나면 게임 시작입니다.
            progress = GameProgress.Turn;
            TextShow.text = string.Empty;
            currentTime = 0f;
            TextTurn.text = showturn + "턴";

        }

    }

    void UpdateTurn()
    {

        if (myTurn) //내턴일 경우
        {
            DoOwnTurn();// setMark = DoOwnTurn();
        }
        if (!myTurn) //상대방 턴
        {
            DoOppnentTurn();
        }

        // 승자 체크
        winner = CheckHP();
        if (winner != Winner.None)
        {
            // 게임 종료.
            progress = GameProgress.Result;
        }

    }

    // 게임 종료 처리
    void UpdateGameOver()
    {
        Reset();
        isGameOver = true;
    }

    // 자신의 턴일 때의 처리.
    IEnumerator DoOwnTurn()
    { //RaycastHit 3D
      //Debug.Log("내턴");

        TextTurn.text = showturn + "턴";
        if (myTurn)
        {
            if (tempturn != showturn)
            {
                while (CardCtrl.chkOn < 4)
                {
                    makegame.CallCard(GetCardEmptyObject.transform);
                    yield return new WaitForSeconds(0.25f);
                }
                tempturn = showturn;
            }
        }

        yield return null;
    }

    // 상대의 턴일 때의 처리. 현재는 5초 대기
    IEnumerator DoOppnentTurn()
    {

        //currentTime += Time.deltaTime;

        //if (currentTime > enemyTurnTime)
        //{
        if (!myTurn)
        {

            showturn++;
            TextTurn.text = showturn + "턴";
            //myTurn = (turn == Turn.Player2) ? true : false;
            //currentTime = 0f;

            yield return GameObject.Find("AI").GetComponent<AITurn>().Attack();
            //8월 6일 손황호 추가
            if (Sorting.Enemy_fieldCard.Count < 5)
            {
                GameObject.Find("AI").GetComponent<AITurn>().Throw();
            }
            yield return new WaitForSeconds(3f);
            turnEnd.myturn = true;
            myTurn = true;
            showturn++;
            //끝
            //0815 BJS
        }
        //}
    }

    // 체력 체크.
    Winner CheckHP()
    {
        if (player1.HP <= 0)
        {
            return Winner.Player2;
        }
        else if (player2.HP <= 0)
        {
            return Winner.Player1;
        }
        return Winner.None;
    }

    // 게임 리셋.
    void Reset()
    {
        //turn = Turn.Own;
        showturn = 1;
        turn = Turn.Player1;
        progress = GameProgress.None;
    }

	// 결과 표시. 0827 LSJ : Set Result; Win or Lose
    void DrawWinner()
    {
		resultPanel.SetActive (true);
		if (player1.HP <= 0) {
			resultLose.SetActive (true);
			resultWin.SetActive (false);

			// **Audio Clip** : Losing Sound
			resultLose.GetComponent<AudioSource>().Play();
		} else {
			resultLose.SetActive (false);
			resultWin.SetActive (true);

			// **Audio Clip** : Victory Sound
			resultWin.GetComponent<AudioSource>().Play();
		}
        //"승리하였습니다."
        //TextShow.text = winner + "가 승리하였습니다.";
    }

	//0828 LSJ : Back to Open Scene 
	public void BackToOpenScene(string preScene)
	{
		SceneManager.LoadScene(preScene, LoadSceneMode.Single);
	}

    // 게임 시작.
    public void GameStart()
    {
        // 게임 시작 상태로 합니다.
        progress = GameProgress.Start;

        // 서버가 먼저 하게 설정합니다.
        turn = Turn.Player1;
        myTurn = true;
        // 자신과 상대의 기호를 설정합니다.
        //if (m_transport.IsServer() == true) //서버용
        //myTurn = Turn.Player1;
        //enemyTurn = Turn.Player2;

        // 이전 설정을 클리어합니다.
        isGameOver = false;

    }

    // 게임 종료 체크.
    public bool IsGameOver()
    {
        return isGameOver;
    }

    //0815 BJS
    public void CostTextChange()
    {
        Player1CPText.text = System.Convert.ToString(player1.Cost);
        Player2CPText.text = System.Convert.ToString(player2.Cost);
    }

    //0817 BJS
    IEnumerator GameLoop()
    {
        for (int i = 0; i <= 4; i++)
        {
            makegame.CallCard(GetCardEmptyObject.transform);
            yield return new WaitForSeconds(0.25f);
        }

        Debug.Log(myTurn);
        while (winner == Winner.None)
        {
            yield return StartCoroutine(DoOwnTurn());
            yield return StartCoroutine(DoOppnentTurn());
            // Debug.Log(winner);
            winner = CheckHP();
            if (winner != Winner.None)
            {
                // 게임 종료.
                DrawWinner();
            }
        }
    }

	private void DestroyParticle()
	{
		Destroy (p_temp);
	}
}