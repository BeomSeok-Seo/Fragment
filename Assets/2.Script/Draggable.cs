/*  파일명     : Draggable.cs
 *  작성자     : 배동연
 *  목적       : UI의 드래그 가능하게 하는 기능
 *  최종 수정날: 8월 10일
 * */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private GameObject draggingObject;
    private RectTransform canvasRectTransform;

    public int indexnum;

    private void UpdateDraggingObjectPos(PointerEventData pointerEventData)
    {
        if (draggingObject != null)
        {
            Vector3 screenPos = pointerEventData.position;

            Camera camera = pointerEventData.pressEventCamera;
            Vector3 newPos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, screenPos, camera, out newPos))
            {
                //드래그 중인 아이콘의 위치를 월드 좌표로 설정한다.
                draggingObject.transform.position = newPos;
                
                draggingObject.transform.rotation = canvasRectTransform.rotation;
            }
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (draggingObject != null)
        {
            Destroy(draggingObject);
        }

        // 본래의 아이콘의 컴포넌트를 가져온다.
        Image source = GetComponent<Image>();

        //드래그 조작중인 아이콘의 게임 오브젝트를 생성한다.
        draggingObject = new GameObject("Dragging Object");
       // Debug.Log(source.rectTransform.sizeDelta);
        //본래의 아이콘의 캔버스의 자식요소로 종속시켜 가장 바깥면에 표시.
        draggingObject.transform.SetParent(source.canvas.transform);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;
        

        //Canvas Group 컴포넌트의 Block Raycasts 속성을 사용해 레이캐스트가 블록되지 않도록 한다.
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        //드래그 조작중인 아이콘의 게임 오브젝트에 컴포넌트 추가
        Image draggingText = draggingObject.AddComponent<Image>();

        //텍스트 수정
        draggingText.sprite = source.sprite;
        draggingText.rectTransform.sizeDelta = source.rectTransform.sizeDelta;
        draggingText.color = source.color;
        draggingText.material = source.material;

        //캔버스의 Rect Transform을 저장해 둔다.
        canvasRectTransform = draggingText.canvas.transform as RectTransform;

        //드래그 조작중인 아이콘의 크기 변경
        (draggingObject.transform as RectTransform).sizeDelta = source.rectTransform.sizeDelta;

        draggingObject.tag = "CLONE";

        //드래그를 조작중인 아이콘의 위치를 갱신한다.
        UpdateDraggingObjectPos(pointerEventData);
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        UpdateDraggingObjectPos(pointerEventData);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        
        Destroy(draggingObject);
    }
}
