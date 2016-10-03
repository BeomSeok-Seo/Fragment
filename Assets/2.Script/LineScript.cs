/* 파일명      : LineScript.cs
   작성자      : 
   목적        : 
   최종 수정 날 : 
  */
using UnityEngine;
using System.Collections;


public class LineScript : MonoBehaviour {
	private LineRenderer m_lrLineRenderer;

	// Use this for initialization
	void Start () {
		m_lrLineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 setStartPosition(Vector3 vector) {
		m_lrLineRenderer.SetPosition (0, vector);
		return vector;
	}

	public Vector3 setLastPosition(Vector3 vector) {
		m_lrLineRenderer.SetPosition (1, vector);
		return vector;
	}
}
