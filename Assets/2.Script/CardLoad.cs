using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardLoad : MonoBehaviour {

    

	// Use this for initialization
	void Awake () {
        DBCardHolder db = DBCardHolder.Instance;
        int i = 0;
        foreach (Transform child in transform)
        {
            //child is your child transform
            CardEx tem = db.GetItem((byte)i);
            string cardname = tem.name;
            Debug.Log(cardname);
            Sprite Cardimage =  Resources.Load <Sprite>("UiCharImage\\"+cardname);
          //  Debug.Log(child);
            child.GetComponent<Image>().sprite = Cardimage;
            i++;
        }
	}
	
}
