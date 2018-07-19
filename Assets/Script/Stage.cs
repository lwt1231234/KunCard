using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

	public int StageLevel,GoldReword;
	public string StageType;
	int Available;
	public string Description;
	public GameObject UI_Text;
	GameObject Description_UI;

	// Use this for initialization
	public void Init () {
		StageInfo t = GameObject.Find ("GameInfo").GetComponent<GameInfo>().StageList[StageLevel-1];
		StageType = t.StageType;
		Description = t.Description;
		GoldReword = t.GoldReword;

		bool[] StageClear = GameObject.Find ("GameInfo").GetComponent<GameInfo>().StageClear;
		Available = 0;
		if (StageClear [StageLevel - 1])
			Available = 2;
		else {
			if (StageLevel == 1) {
				Available = 1;
			}
			switch (StageLevel) {
			case 2:
				if (StageClear [1-1])
					Available = 1;
				break;
			case 3:
				if (StageClear [2-1])
					Available = 1;
				break;
			case 4:
				if (StageClear [3-1])
					Available = 1;
				break;
			case 5:
				if (StageClear [3-1])
					Available = 1;
				break;
			case 6:
				if (StageClear [5-1])
					Available = 1;
				break;
			case 7:
				if (StageClear [3-1])
					Available = 1;
				break;
			case 8:
				if (StageClear [7-1])
					Available = 1;
				break;
			case 9:
				if (StageClear [6-1])
					Available = 1;
				break;
			case 10:
				if (StageClear [9-1])
					Available = 1;
				break;
			case 11:
				if (StageClear [8-1])
					Available = 1;
				break;
			case 12:
				if (StageClear [11-1])
					Available = 1;
				break;
			case 13:
				if (StageClear [4-1]||StageClear [9-1]||StageClear [11-1])
					Available = 1;
				break;
			case 14:
				if (StageClear [13-1])
					Available = 1;
				break;
			}
		}

		if (Available ==1) {
			Color a = new Color (255f / 255f, 255f / 255f, 255f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		} 
		if(Available ==0){
			Color a = new Color (141f / 255f, 141f / 255f, 141f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		}
		if(Available ==2){
			Color a = new Color (162f / 255f, 255f / 255f, 144f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		}
		if (Description_UI == null) {
			Vector3 offsetColor = new Vector3 (0, 35, 0);
			Description_UI = (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
			Description_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
			Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint (transform.position) - new Vector3 (Screen.width / 2, Screen.height / 2, 0) + offsetColor;
			if (Available > 0)
				Description_UI.GetComponent<Text> ().text = Description;
			else
				Description_UI.GetComponent<Text> ().text = "???";
			
			Description_UI.SetActive (false);
		} else {
			if (Available > 0)
				Description_UI.GetComponent<Text> ().text = Description;
			else
				Description_UI.GetComponent<Text> ().text = "???";
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		//		Debug.Log("鼠标按下时");
		if (Available > 0) {
			GameObject.Find ("GameManager").GetComponent<GameManager_InStage> ().OnClickStage (this.gameObject);
			Description_UI.SetActive (false);
		}
	}

	void OnMouseEnter()
	{
		//		Debug.Log("鼠标进入该对象区域时");
		Description_UI.SetActive(true);
	}
	void OnMouseExit()
	{
		//		Debug.Log("鼠标离开该模型区域时");
		Description_UI.SetActive(false);
	}


}
