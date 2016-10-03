/* 파일명      : CardSuffle.cs
   작성자      : 손황호
   목적        : 게임내의 덱 데이터값 셔플 및 덱드로우 관련
   최종 수정 날 : 7월 29일
  */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CardSuffle : MonoBehaviour {
    //카드 프리팹
    public GameObject OCard;
    public List<Card> cards;
    
    public CardSuffle()
    {
        this.cards = new List<Card>();
    }

   
    public void shuffle()
    {
        System.Random ran = new System.Random();
        
        int n = cards.Count;
        while(n > 1)
        {
            n--;
            int num = ran.Next(n+1);
            Card value = cards[num];
            cards[num] = cards[n];
            cards[n] = value;
        }
    }

    // 마우스로 카드 터치시 발동
    public IEnumerator GetCard(Transform curCard)
    {
        //0.12  최정상
        Vector3 Temp_Position=curCard.localPosition;
        Temp_Position.x = 0;
        
        while (true)
        {
            //yield return new WaitForSeconds(0.01f);

            // 카드가 다 뒤졉혀 질 때까지
            if (curCard.localEulerAngles.z < 180)
            {
                curCard.localEulerAngles += new Vector3(0, 0, 1);   // z축으로 카드 뒤집기
                curCard.localPosition -= new Vector3(-1f, 0, 0);  // x축으로 카드 센터로 옮기기

                // 
                if (curCard.localEulerAngles.z < 180)
                    curCard.localPosition += new Vector3(0, 1f, 0);

            }
            else
            {

                break;
            }
			yield return new WaitForEndOfFrame();    
        }
        Temp_Position.y = ((Temp_Position.y - 0.12f ) * -1)+ 0.01f;
        curCard.localPosition = Temp_Position;
        Destroy(curCard.gameObject);

    }


}
