/* 파일명      : UI_MainMGR.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI_MainMGR : MonoBehaviour {

	// Use this for initialization
	public void OnclickStartBtn()
	{ 
		Debug.Log("Click Button"); 
		//씬 읽어 오기 
		SceneManager.LoadScene("scMain");
		//SceneManager.LoadScene("scMain", LoadSceneMode.Additive); 
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
