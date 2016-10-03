using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 파일명 : DBCardHolder.cs
/// 작성자 : 안태규
/// 목  적 : 카드 정보 저장용 데이터베이스
/// </summary>
public class DBCardHolder : MonoBehaviour 
{
	private static DBCardHolder holder;

	public static DBCardHolder Instance
	{
		get { return holder; }
	}

    public CardEx[] deck;

    void Awake()
	{
		holder = this;
	}

	void Start()
	{
		for (byte i = 0; i < deck.Length; i++)
			deck [i].id = i;
	}

	virtual public CardEx GetItem(byte id)
	{
		CardEx item = new CardEx ();

        item.id = deck[id].id;
        item.name = deck[id].name;
        item.cost = deck[id].cost;
        item.attackpoint = deck[id].attackpoint;
        item.counterpoint = deck[id].counterpoint;
        item.healthpoint = deck[id].healthpoint;
        item.card_type = deck[id].card_type;

        return item;
	}

	virtual public CardEx GetItem(string name)
	{
		CardEx item = new CardEx ();
		byte i = 0;

		for (i = 0; i < deck.Length; i++) 
		{
			if (deck [i].name == name)
				break;
		}

        item.id = deck[i].id;
        item.name = deck[i].name;
        item.cost = deck[i].cost;
        item.attackpoint = deck[i].attackpoint;
        item.counterpoint = deck[i].counterpoint;
        item.healthpoint = deck[i].healthpoint;
        item.card_type = deck[i].card_type;

        return item;
	}

	public CardEx[] GetAllItem()
	{
		CardEx[] items = new CardEx[deck.Length];

		for (int i = 0; i < deck.Length; i++) 
		{
			items [i] = new CardEx ();
			items [i].id = deck [i].id;
			items [i].name = deck [i].name;
            items [i].cost = deck [i].cost;
            items [i].attackpoint = deck[i].attackpoint;
            items [i].counterpoint = deck[i].counterpoint;
            items [i].healthpoint = deck[i].healthpoint;
            items [i].card_type = deck[i].card_type;
        }

		return items;
	}
}
