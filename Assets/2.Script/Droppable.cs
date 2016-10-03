/*  파일명     : Droppable.cs
 *  작성자     : 배동연
 *  목적       : UI의 드롭영역을 위한 코딩
 *  최종 수정날: 8월 17일
 * */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Droppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	//드롭영역에 들어온 아이콘
    [SerializeField]  private Text icon;

    private GameObject temp; // 드래그한 본체 넣어 놓을 곳

    public int booling;
    
    public List<int> myDeck= new List<int>(40); //이 리스트 이용해서 가져가면 됩니다

    private List<GameObject> Deck = new List<GameObject>(40);
    private float[] pos_y = new float[40];

    private int[] countCheck = new int[43];

    public int[] Count
    {
        get { return countCheck; }
    }

    private int card_Num ;

    public GameObject deck_card;

    public GameObject instans; // 드래그하고 있는 오브젝트를 넣을곳

    void Start()
    {
        for (int i = 0; i < 40; i++)
        {
            pos_y[i] = -(60 * i);
        }
        for (int i = 0; i < 43; i++)
        {
            countCheck[i] = 0;
        }

        booling = 0;

        DeckLoader loader = DeckLoader.Instance;
        loader.Read();
        
        loadData();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)   //영역안에 넣었을때 그 순간에 크기 변경
    {
        if (pointerEventData.dragging)
        {
            instans = GameObject.Find("Dragging Object"); //드래그 중인 오브젝트가 앤터되었을 때 instans에 넣어준다.

            if (myDeck.Count < 40)
            {
                if (instans != null)
                {
                    if (instans.tag == "CLONE")
                    {
                        temp = pointerEventData.pointerDrag.gameObject;
                        card_Num = temp.GetComponent<Draggable>().indexnum;
                
                       //instans.GetComponent<Text>().fontSize = 50;
                       // (instans.transform as RectTransform).sizeDelta = new Vector2(266, 100);
                      //  instans.GetComponent<Text>().color = Color.gray;

                    }
                  
                }
            }
            Debug.Log(instans);
            if (instans != null && instans.tag == "DECK_OUT")
            {
                booling = 2;
                Debug.Log("enter: " + booling);
            }
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)    //드래그 한 상태에서 영역 밖으로 나갈때 다시 크기 변경
    {
        if (pointerEventData.dragging)
        {
            //instans = GameObject.Find("Dragging Object");
            if (myDeck.Count < 40)
            {
                if (instans != null)
                {
                    if (instans.tag == "CLONE")
                    {
                       // instans.GetComponent<Text>().fontSize = temp.GetComponent<Text>().fontSize;
                       // (instans.transform as RectTransform).sizeDelta = new Vector2(150, 250);
                    }
               
                }
            }

            Debug.Log(instans);
            if (instans != null && instans.tag == "DECK_OUT")
            {
                booling = 1;
                Debug.Log("enter: " + booling);
            }
        }
    }

    public void OnDrop(PointerEventData pointerEventData)
    {
        DBCardHolder db = DBCardHolder.Instance;

        if (myDeck.Count < 40)
        {
          if(countCheck[card_Num] < 2)
                if (instans != null)
                {
                    if (instans.tag == "CLONE")
                    {
                        //   Debug.Log(myDeck);

                        myDeck.Add(card_Num);
                        myDeck.Sort();

                        instans = Instantiate(deck_card);

                        (instans.transform as RectTransform).sizeDelta = new Vector2(200, 50);
                        (instans.transform.FindChild("deck_card").transform as RectTransform).sizeDelta = new Vector2(200, 50);

                        instans.transform.SetParent(GameObject.Find("deckContent").transform);

                        (instans.transform as RectTransform).localPosition = new Vector3(95.4999847f, -50, 0); //위치 기본 세팅

                        string id = db.GetItem(byte.Parse((temp.GetComponent<Draggable>().indexnum - 1).ToString())).name;
                        instans.transform.FindChild("deck_card").GetComponent<Text>().text = id;
                        MyExtensions.RectTransformExtensions.SetDefaultScale(instans.transform as RectTransform);
                        instans.name = card_Num + "card";

                        countCheck[card_Num]++;

                        if (Deck.Count == 0)
                        {
                            Deck.Add(instans);
                        }
                        else
                        {
                            int k = myDeck.FindIndex(x => x == card_Num);
                            //Debug.Log(k);
                            Deck.Insert(k, instans);
                        }

                        //덱 컨텍스트 크기 업
                        set_context_sizeup();

                        //덱 순서 대로 위치 정렬
                        position_set();
                        instans = null;
                        //   Debug.Log(myDeck.Count);
                    }
                 
                }
            
            Debug.Log(instans);
            if (instans != null && instans.tag == "DECK_OUT")
            {
                booling = 3;
                Debug.Log("ondrap: " + booling);
            }
        }
    }

    public void deckOutCard(int inputnum)
    {
        int k = myDeck.FindIndex(x => x == inputnum);
        Debug.Log("outCard index : " +k);
        myDeck.Remove(inputnum);
        GameObject sourse = Deck[k];
        Deck.RemoveAt(k);
        Destroy(sourse);
        

        if(MyExtensions.RectTransformExtensions.GetHeight((GameObject.Find("deckContent").transform as RectTransform))>130)
            set_context_sizedown();

        countCheck[inputnum]--;
        position_set();

        Debug.Log(myDeck.Count);
    }

    private void set_context_sizeup()
    {
        MyExtensions.RectTransformExtensions.SetHeight((GameObject.Find("deckContent").transform as RectTransform), MyExtensions.RectTransformExtensions.GetHeight((GameObject.Find("deckContent").transform as RectTransform)) + 60);
    }

    private void set_context_sizedown()
    {
        MyExtensions.RectTransformExtensions.SetHeight((GameObject.Find("deckContent").transform as RectTransform), MyExtensions.RectTransformExtensions.GetHeight((GameObject.Find("deckContent").transform as RectTransform)) - 60);
    }

    private void position_set()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            (Deck[i].transform as RectTransform).localPosition = new Vector3(95.4999847f, -50, 0);
            (Deck[i].transform as RectTransform).localPosition += new Vector3(0, pos_y[i], 0);
        }

        GameObject.Find("countText").GetComponent<Text>().text = myDeck.Count.ToString();
    }

    public void loadData()
    {
        DBCardHolder db = DBCardHolder.Instance;

        for (int i = 0; i < myDeck.Count; i++)
        {
            instans = Instantiate(deck_card);

            (instans.transform as RectTransform).sizeDelta = new Vector2(200, 50);
            (instans.transform.FindChild("deck_card").transform as RectTransform).sizeDelta = new Vector2(200, 50);

            instans.transform.SetParent(GameObject.Find("deckContent").transform);

            string id = db.GetItem(byte.Parse((myDeck[i] - 1).ToString())).name;
            instans.transform.FindChild("deck_card").GetComponent<Text>().text = id;
            MyExtensions.RectTransformExtensions.SetDefaultScale(instans.transform as RectTransform);

            instans.name = myDeck[i].ToString() + "card";

            Deck.Add(instans);

            set_context_sizeup();
        }

        position_set();
        /*
        for (int i = 0; i < Deck.Count; i++)
        {
            (Deck[i].transform as RectTransform).localPosition += new Vector3(-126, 0, 0);
        }
        */
    }

    public void clear()
    {
        GameObject[] go = new GameObject[40];

        go = Deck.ToArray();

        
        Deck.Clear();

        for (int i = 0; i < myDeck.Count; i++)
        {
            Destroy(go[i]);
        }

        myDeck.Clear();

        for (int i = 0; i < 43; i++)
        {
            countCheck[i] = 0;
        }

        MyExtensions.RectTransformExtensions.SetHeight((GameObject.Find("deckContent").transform as RectTransform), 116);

        GameObject.Find("countText").GetComponent<Text>().text = myDeck.Count.ToString();

    }
}
