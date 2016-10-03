/* 파일명      : Card.cs
   작성자      : 손황호
   목적        : 게임 소스 내의 구조체형 타입(데이터베이스 연동)
   최종 수정 날 : 7월 29일
  */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CARD_TYPE : byte
{
    고체,
    기체,
    액체
}
public class Card : MonoBehaviour {
    // 번호
    public byte number { get; private set; }
    //이름
    public string name { get; private set; }
    //비용
    public int cost { get; private set; }
    //충격
    public int attackpoint { get; private set;}
    //저항
    public int counterpoint { get; private set; }
    //기력
    public int healthpoint { get; private set; }
    // 타입 (고체,기체,액체)
   public CARD_TYPE card_type { get; private set; }

    public Card(byte number,string name,int cost,int attackpoint,int counterpoint,int healthpoint,CARD_TYPE card_type)
        //CARD_TYPE card_type)
    {
        this.number = number;
        this.name = name;
        this.cost = cost;
        this.attackpoint = attackpoint;
        this.counterpoint = counterpoint;
        this.healthpoint = healthpoint;
        this.card_type = card_type;
    }

}
