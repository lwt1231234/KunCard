using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	public int PathNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject GameInfo;
		GameInfo = GameObject.Find ("GameInfo");
		if (GameInfo == null)
			return;

		bool Available = false;
		bool[] StageClear = GameInfo.GetComponent<GameInfo>().StageClear;
		switch (PathNum) {
		case 0:
		case 1:
		case 2:
		case 4:
			if (StageClear [PathNum+1-1])
				Available = true;
			break;
		case 3:
		case 9:
		case 11:
			if (StageClear [PathNum-1])
				Available = true;
			break;
		case 5:
		case 8:
		case 10:
		case 13:
			if (StageClear [PathNum-2-1])
				Available = true;
			break;
		case 6:
		case 14:
			if (StageClear [PathNum-1-1])
				Available = true;
			break;
		case 7:
		case 12:
			if (StageClear [PathNum-3-1])
				Available = true;
			break;
		}

		if (Available) {
			Color a = new Color (255f / 255f, 255f / 255f, 255f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		} else {
			Color a = new Color (141f / 255f, 141f / 255f, 141f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		}
			
			
	}
}
