using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

	public int StageLevel;
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
			Vector3 offsetColor = new Vector3 (0,55, 0);
			Description_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
			Description_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
			Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;
			Description_UI.GetComponent<Text> ().text = Description;
			Description_UI.SetActive(false);
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
