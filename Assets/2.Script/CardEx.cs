/* 파일명      : CardEx.cs
   작성자      : 안태규
   목적        : 게임내 데이터베이스 및 덱데이터 활용
   최종 수정 날 : 7월 29일
  */
using UnityEngine;
using System.Collections;

[System.Serializable]
public class CardEx {
    
    
    [System.NonSerialized]
	public byte id;
    public string name;
    public int cost;
    public int attackpoint;
    public int counterpoint;
    public int healthpoint;
    public CARD_TYPE card_type;

}
