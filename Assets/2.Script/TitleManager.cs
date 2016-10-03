using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// 파일명 : TitleManager.cs
/// 작성자 : 안태규
/// 목  적 : 타이틀 제어
/// </summary>
public class TitleManager : MonoBehaviour {
	//public AudioClip ac_ButtonClick;

	public void Quit()
    {
        Application.Quit();
    }

    public void Load(string nextScene)
    {
        if (CountCheck() && false)
        {
            Debug.Log("40장 아니야!");
            return;
        }

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    private bool CountCheck()
    {
        int countCheck = 0;

        DeckLoader loader = new DeckLoader();
        string path = loader.GetPath("deck.dat");

        using (StreamReader sr = new StreamReader(path, Encoding.UTF32, false))
        {
            while (sr.Peek() > -1)
            {
                sr.ReadLine();
                countCheck++;
            }
        }

        if (countCheck == 40)
            return false;

        return true;
    }

	//0907 LSJ : Button Click Sound Effect
	/*
	public void ButtonClickSound()
	{
		this.GetComponent<AudioSource> ().PlayOneShot (ac_ButtonClick);
	}
	*/
}
