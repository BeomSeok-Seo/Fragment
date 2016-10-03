using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// 파일명 : Transition.cs
/// 작성자 : 안태규
/// 목  적 : 씬간 이동
/// </summary>
public class Transition : MonoBehaviour {
	
    public string nextScene;

    public void OnLoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
