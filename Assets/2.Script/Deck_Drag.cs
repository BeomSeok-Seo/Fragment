/*  파일명     : Deck_Drag.cs
 *  작성자     : 배동연
 *  목적       : Deck 안에 이미 수립된 카드들의 드래그를 위한 코드
 *  최종 수정날: 8월 16일
 * */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Deck_Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject deckdraggingObject;
    private RectTransform canvasRectTransform;

    public GameObject deckcard;

    int tem;
    Image imageSource;
    Text source;

    private void UpdateDraggingObjectPos(PointerEventData pointerEventData)
    {
        if (deckdraggingObject != null)
        {
            Vector3 screenPos = pointerEventData.position;

            Camera camera = pointerEventData.pressEventCamera;
            Vector3 newPos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, camera, out newPos))
            {
                //드래그 중인 아이콘의 위치를 월드 좌표로 설정한다.
                deckdraggingObject.transform.position = newPos;

                deckdraggingObject.transform.rotation = canvasRectTransform.rotation;
            }
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (deckdraggingObject != null)
        {
            Destroy(deckdraggingObject);
        }
       

        // 본래의 아이콘의 컴포넌트를 가져온다.
        source = transform.FindChild("deck_card").GetComponent<Text>();
       
        imageSource = GetComponent<Image>();

        

        //드래그 조작중인 아이콘의 게임 오브젝트를 생성한다.
        deckdraggingObject = Instantiate(deckcard);
        Debug.Log(deckdraggingObject);
        //본래의 아이콘의 캔버스의 자식요소로 종속시켜 가장 바깥면에 표시.
        deckdraggingObject.transform.SetParent(source.canvas.transform);
        deckdraggingObject.transform.SetAsLastSibling();
        deckdraggingObject.transform.localScale = Vector3.one;


        //Canvas Group 컴포넌트의 Block Raycasts 속성을 사용해 레이캐스트가 블록되지 않도록 한다.
        CanvasGroup canvasGroup = deckdraggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Text draggingText = deckdraggingObject.transform.FindChild("deck_card").GetComponent<Text>();

        //텍스트 수정
        draggingText.text = source.text;
        draggingText.font = source.font;
        draggingText.fontSize = source.fontSize;
        draggingText.color = source.color;
        draggingText.alignment = source.alignment;

        DBCardHolder db = DBCardHolder.Instance;

        byte id = db.GetItem(draggingText.text.ToString()).id;
        tem = id + 1;

        //캔버스의 Rect Transform을 저장해 둔다.
        canvasRectTransform = draggingText.canvas.transform as RectTransform;

        //드래그 조작중인 아이콘의 크기 변경
        (deckdraggingObject.transform as RectTransform).sizeDelta = source.rectTransform.sizeDelta;

        deckdraggingObject.tag = "DECK_OUT";

        //덱에서 제거
        

        //원본 삭제
       // Destroy(source);


        deckdraggingObject.name = "Dragging Object";

        GameObject.Find("deckScroll").GetComponent<Droppable>().instans = deckdraggingObject;
        
        //드래그를 조작중인 아이콘의 위치를 갱신한다.
        UpdateDraggingObjectPos(pointerEventData);

    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        UpdateDraggingObjectPos(pointerEventData);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        
        Destroy(deckdraggingObject);
        Debug.Log("enddrag: "+GameObject.Find("deckScroll").GetComponent<Droppable>().booling);
        if (GameObject.Find("deckScroll").GetComponent<Droppable>().booling == 1)
        {
            GameObject.Find("deckScroll").GetComponent<Droppable>().deckOutCard(tem);
        }
      
    }
	
}
