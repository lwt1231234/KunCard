using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	public int StageLevel;
	public string StageType;
	bool Available;

	// Use this for initialization
	public void Init () {
		if (StageLevel == 1) {
			Available = true;
		} else {
			Available = false;
			bool[] StageClear = GameObject.Find ("GameInfo").GetComponent<GameInfo>().StageClear;
			switch (StageLevel) {
			case 2:
				if (StageClear [1-1])
					Available = true;
				break;
			case 3:
				if (StageClear [2-1])
					Available = true;
				break;
			}
		}
		if (Available) {
			Color a = new Color (255f / 255f, 255f / 255f, 255f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		} else {
			Color a = new Color (141f / 255f, 141f / 255f, 141f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		//		Debug.Log("鼠标按下时");
		if(Available)
			GameObject.Find ("GameManager").GetComponent<GameManager_InStage>().OnClickStage(this.gameObject);
	}


}
