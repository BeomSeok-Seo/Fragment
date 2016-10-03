
/* 파일명      : PlayerInfo.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
// string 을 Card 객체로 바꿔주세요!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public sealed class PlayerInfo : MonoBehaviour
{
    private List<Card> Cards; // 소지 카드 리스트.
    private int playerHP; // 체력.
    private int playerCost; // 코스트.

    /// <summary>
    /// 소지한 카드 리스트 반환.
    /// </summary>
    public List<Card> HandCard
    {
        get { return Cards; }
    }

    /// <summary>
    /// 플레이어 체력.
    /// </summary>
    public int HP
    {
        get { return playerHP; }
        set { playerHP = value; }
    }

    /// <summary>
    /// 플레이어 코스트.
    /// </summary>
    public int Cost
    {
        get { return playerCost; }
        set { playerCost = value; }
    }

    void Awake()
    {
        // 아래 코드의 주석을 해제해주세요!

        // 카드더미.OnDrawCard += this.DrawCard;

        Cards = new List<Card>();
        playerHP = 30;
        playerCost = 5;
    }

    /// <summary>
    /// 카드더미에서 카드를 가져옴.
    /// </summary>
    /// <param name="item"></param>
    public void DrawCard(Card item)
    {
        // 아래 코드를 카드더미 스크립트에 작성해주세요! (카드더미의 맨 위에 있는 카드를 넘겨줄 수 있는 곳.)

        // public delegate void DrawCardHandler(카드 item);
        // public static event DrawCardHandler OnDrawCard;
        // OnDrawCard(item);

        Cards.Add(item);
        Debug.Log("Draw : " + item);
    }

    /// <summary>
    /// 핸드에서 선택된 카드를 필드에 전달. (소지한 카드의 정보를 플레이어가 가지고 있으므로 핸드에서 선택한 카드를 빼주기 위함)
    /// </summary>
    /// <param name="item"></param>
    public bool ThrowCard(Card item)
    {
        // 선택된 카드의 코스트와 playerCost 비교.
        int currentCost = (playerCost + item.cost);
        Debug.Log("카드 코스트" + item.cost);
        Debug.Log("코스트" + currentCost);
        if (currentCost < 0) return false;
        else if (currentCost > 10) currentCost = 10;
        playerCost = currentCost;

        //int index = Cards.IndexOf(item);
        //Cards.RemoveAt(index);

        return true;
    }

    /// <summary>
    /// 카드 버리기.
    /// </summary>
    /// <param name="item"></param>
    public Card ReleaseCard(Card item)
    {
        int index = Cards.IndexOf(item);
        Cards.RemoveAt(index);

        return item;
    }
}
