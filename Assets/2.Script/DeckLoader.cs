using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// 파일명 : DeckLoader.cs
/// 작성자 : 안태규
/// 목  적 : 구성 덱 입출력
/// </summary>
public class DeckLoader : MonoBehaviour
{
    private static DeckLoader loader;

    public static DeckLoader Instance
    {
        get { return loader; }
    }

    List<int> deck;
    
    int[] deckCount;

    void Awake()
    {
        loader = this;
        deck = GameObject.Find("deckScroll").GetComponent<Droppable>().myDeck;
        deckCount = GameObject.Find("deckScroll").GetComponent<Droppable>().Count;
    }

    public void Read()
    {
        string name = "deck.dat";

        using (StreamReader sr = new StreamReader(GetPath(name), Encoding.UTF32, false))
        {
            while (sr.Peek() > -1)
            {
                int id = byte.Parse(sr.ReadLine());
                deckCount[id]++;
                deck.Add(id);
            }
        }

        /*for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i]);
        }
        Debug.Log(deck.Count);*/
    }

    public void Write()
    {
        int[] selectedItem = deck.ToArray();
        string name = "deck.dat";

        using (StreamWriter sw = new StreamWriter(GetPath(name), false, Encoding.UTF32))
        {
            for (int i = 0; i < selectedItem.Length; i++)
            {
                sw.WriteLine(selectedItem[i].ToString());
                ///Debug.Log(selectedItem[i]);
            }
            ///Debug.Log(selectedItem.Length);
        }

        SceneManager.LoadScene("scOpen");
    }

    public string GetPath(string fileName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, fileName);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, fileName);
        }
    }
}
