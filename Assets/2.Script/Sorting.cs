/* 파일명      : Sorting.cs
   작성자      : 배동연
   목적        : 필드 정렬
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Sorting : MonoBehaviour {

    float[] posX = { -40, -30, -20, -10, 0, 10, 20, 30, 40 };
    int PosY = 1;
    //8월 17일 손황호
    int Player_posZ = -13;
    int Enemy_PosZ = 15;
    //8월 17일 끝
    int DownY = 80;
    //8월 6일 손황호 Player_positions, Enemy_positions 수정  player_fieldcard,enemy_fieldcard 수정
    Vector3[] Player_positions = new Vector3[9];
    Vector3[] Enemy_positions = new Vector3[9];
    public static List<GameObject> Player_fieldCard;
    public static List<GameObject> Enemy_fieldCard;

    //끝
    // Use this for initialization
    void Start()
    {
        Player_fieldCard = new List<GameObject>(5);
        Enemy_fieldCard = new List<GameObject>(5);
        //8월 6일 손황호 Player_positions, Enemy_positions 수정
        for (int i = 0; i < 9; i++)
        {
            Player_positions[i] = new Vector3(posX[i], PosY, Player_posZ);
            Enemy_positions[i] = new Vector3(posX[i], PosY, Enemy_PosZ);
        }
        //끝
    }
	// Update is called once per frame
	void Update () {

	}
    //8월 6일 수정 손황호
	public IEnumerator insertObj(float mouseX, GameObject ins)
    {
        if (ins.tag == "Player_Field_Card")
        {
            if (Player_fieldCard.Count == 0)
                Player_fieldCard.Add(ins);
            else
            {
                Player_insert(mouseX, ins);
            }
            yield return new WaitForSeconds(1.0f);
            Player_sort();
        }
        else if (ins.tag == "Enemy_Field_Card")
        {
            if (Enemy_fieldCard.Count == 0)
                Enemy_fieldCard.Add(ins);
            else
            {
                Enemy_insert(mouseX, ins);
            }
            yield return new WaitForSeconds(1.0f);
            Enemy_sort();
        
        }
    }
    //끝

    //player_insert,enemy_insert 추가 8월 6일 손황호
    public void Player_insert(float mouseX, GameObject ins)
    {
        
        switch (Player_fieldCard.Count) // 카드 갯수
        { 
            case 1:
                if (mouseX < posX[4])
                    Player_fieldCard.Insert(0, ins);
                else
                    Player_fieldCard.Add(ins);
                break;
            case 2:
                if (mouseX < posX[3])
                {
                    Player_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[5])
                {
                    Player_fieldCard.Insert(1, ins);
                }
                else
                    Player_fieldCard.Add(ins);
                break;
            case 3:
                if (mouseX < posX[2])
                {
                    Player_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[4])
                {
                    Player_fieldCard.Insert(1, ins);
                }
                else if (mouseX < posX[6])
                {
                    Player_fieldCard.Insert(2, ins);
                }
                else
                    Player_fieldCard.Add(ins);
                break;
            case 4:
                if (mouseX < posX[1])
                {
                    Player_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[3])
                {
                    Player_fieldCard.Insert(1, ins);
                }
                else if (mouseX < posX[5])
                {
                    Player_fieldCard.Insert(2, ins);
                }
                else if (mouseX < posX[7])
                {
                    Player_fieldCard.Insert(3, ins);
                }
                else
                    Player_fieldCard.Add(ins);
                break;
            case 5: // 생성 안해야지 이미 꽉참
                Destroy(ins);
                break;
        }
    }

    public void Enemy_insert(float mouseX, GameObject ins)
    {
        
        switch (Enemy_fieldCard.Count) // 카드 갯수
        {
            case 1:
                if (mouseX < posX[4])
                    Enemy_fieldCard.Insert(0, ins);
                else
                    Enemy_fieldCard.Add(ins);
                break;
            case 2:
                if (mouseX < posX[3])
                {
                    Enemy_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[5])
                {
                    Enemy_fieldCard.Insert(1, ins);
                }
                else
                    Enemy_fieldCard.Add(ins);
                break;
            case 3:
                if (mouseX < posX[2])
                {
                    Enemy_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[4])
                {
                    Enemy_fieldCard.Insert(1, ins);
                }
                else if (mouseX < posX[6])
                {
                    Enemy_fieldCard.Insert(2, ins);
                }
                else
                    Enemy_fieldCard.Add(ins);
                break;
            case 4:
                if (mouseX < posX[1])
                {
                    Enemy_fieldCard.Insert(0, ins);
                }
                else if (mouseX < posX[3])
                {
                    Enemy_fieldCard.Insert(1, ins);
                }
                else if (mouseX < posX[5])
                {
                    Enemy_fieldCard.Insert(2, ins);
                }
                else if (mouseX < posX[7])
                {
                    Enemy_fieldCard.Insert(3, ins);
                }
                else
                    Enemy_fieldCard.Add(ins);
                break;
            case 5: // 생성 안해야지 이미 꽉참
                Debug.Log("적 필드 꽉참");
                break;
        }

    }
    //끝
    public void removeObj(GameObject obj)
    {
        if (obj.tag == "Player_Field_Card")
        {
            for (int i = 0; i < Player_fieldCard.Count; i++)
            {
                if (obj.transform == Player_fieldCard[i].transform)
                {
                    Player_fieldCard.RemoveAt(i);
                    Player_sort();
                }
            }
        }
        if (obj.tag == "Enemy_Field_Card")
        {
            for (int i = 0; i < Enemy_fieldCard.Count; i++)
            {
                if (obj.transform == Enemy_fieldCard[i].transform)
                {
                    Enemy_fieldCard.RemoveAt(i);
                    Enemy_sort();
                }
            }
        }
    }

    void Player_sort()
    {
        int even = -1;
        //player
        switch (Player_fieldCard.Count) // 카드 갯수
        {
            case 1:
                Player_fieldCard[0].transform.DOMove(Player_positions[4], 0.4f);
                break;
            case 2:

                for (int i = 0; i < 2; i++)
                {
                    Player_fieldCard[i].transform.DOMove(Player_positions[4 + even], 0.4f);
                    even *= -1;
                }
                break;
            case 3:
                even *= 2;
                for (int i = 0; i < 3; i++)
                {
                    Player_fieldCard[i].transform.DOMove(Player_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
            case 4:
                even *= 3;
                for (int i = 0; i < 4; i++)
                {
                    Player_fieldCard[i].transform.DOMove(Player_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
            case 5:
                even *= 4;
                for (int i = 0; i < 5; i++)
                {
                    Player_fieldCard[i].transform.DOMove(Player_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
        }
      
    }
    void Enemy_sort()
    {
        int even = -1;
        //enemy
        switch (Enemy_fieldCard.Count) // 카드 갯수
        {

            case 1:
                Enemy_fieldCard[0].transform.DOMove(Enemy_positions[4], 0.4f);
                break;
            case 2:

                for (int i = 0; i < 2; i++)
                {
                    Enemy_fieldCard[i].transform.DOMove(Enemy_positions[4 + even], 0.4f);
                    even *= -1;
                }
                break;
            case 3:
                even *= 2;
                for (int i = 0; i < 3; i++)
                {
                    Enemy_fieldCard[i].transform.DOMove(Enemy_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
            case 4:
                even *= 3;
                for (int i = 0; i < 4; i++)
                {
                    Enemy_fieldCard[i].transform.DOMove(Enemy_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
            case 5:
                even *= 4;
                for (int i = 0; i < 5; i++)
                {
                    Enemy_fieldCard[i].transform.DOMove(Enemy_positions[4 + even], 0.4f);
                    even += 2;
                }
                break;
        }
    }
}
