/*
  파일명      : turnend.cs
  작성자      : 방준선
  목적        : 턴 버튼 관리(회전 등)
  최종 수정 날 : 
*/
using UnityEngine;
using System.Collections;


public class turnend : MonoBehaviour
{
    public Texture[] textures;
    public float changeInterval = 0.33F;
    public Renderer rend;
    public bool flip;
    public bool myturn;
    private Quaternion Up = Quaternion.identity;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        myturn = true;
        rend.material.mainTexture = textures[0];//내턴

        transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.2f);
        if (transform.localEulerAngles.z >= 50.0f && myturn) //90도 돌면 이미지 변경
        {
            rend.material.mainTexture = textures[1];//상대턴
        }
        else if (transform.localEulerAngles.z >= 50.0f && !myturn) //90도 돌면 이미지 변경
        {
            rend.material.mainTexture = textures[0];//내턴
        }
        if (transform.localEulerAngles.z >= 180.0f && myturn) //180도 돌면
        {
            transform.eulerAngles = new Vector3(0, 0, 180f);
            myturn = !myturn;
            flip = false;
        }
        else if (transform.localEulerAngles.z >= 358.0f && !myturn) //180도 돌면
        {
            transform.eulerAngles = new Vector3(0, 0, 0f);
            myturn = !myturn;
            flip = false;
        }
        transform.Rotate(new Vector3(0, 0, 1) * 60 * Time.deltaTime);

    }

    void Update()
    {
        //flip = true;
        //Debug.Log(flip);

            if(myturn)
        {
            Up.eulerAngles = new Vector3(0f, 180f, 0f);
            rend.material.mainTexture = textures[0];//내턴
        }
            else if (!myturn)
        {
            Up.eulerAngles = new Vector3(180f, 180f, 0f);
            rend.material.mainTexture = textures[1];//상대턴
        }
            





        transform.rotation = Quaternion.Slerp(transform.rotation, Up, Time.deltaTime* 5.0f);

            //transform.Rotate(new Vector3(0, 0, 1) * 60 * Time.deltaTime);
        
    }
}
