using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {

	public GameObject UI_Text_Black,UI_Text_White;
	GameObject Name_UI,Description_UI;
	public int SkillNum,GoldRequire;
	public bool Unlocked,Used;
	string Name,Description;
	public Sprite[] images;
	// Use this for initialization
	public void Init () {
		GameObject GameInfo = GameObject.Find ("GameInfo");
		if (GameInfo == null) {
			Invoke ("Start", 0.5f);
			return;
		}
		SkillInfo SkillThis = GameInfo.GetComponent<GameInfo> ().SkillList [SkillNum];
		Name = SkillThis.Name;
		Description = SkillThis.Description;
		GoldRequire = SkillThis.GoldRequire;
		Unlocked = SkillThis.Unlocked;
		Used = SkillThis.Used;

		if (Name_UI == null) {
			Name_UI= (GameObject)Instantiate (UI_Text_Black, transform.position, Quaternion.identity);
			Name_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
			Name_UI.GetComponent<Text> ().text = Name;
			Name_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
			Name_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
		}

		if (Description_UI == null) {
			Description_UI= (GameObject)Instantiate (UI_Text_White, transform.position, Quaternion.identity);
			Description_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
			Description_UI.GetComponent<Text> ().text = Description;
			Vector3 offsetColor = new Vector3 (55,0, 0);
			Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;
			Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;
			if (Unlocked == false)
				Description_UI.GetComponent<Text> ().text += "(花费" + GoldRequire.ToString () + "解锁)";
		}
			
		if(Used)
			gameObject.GetComponent<SpriteRenderer>().sprite = images[1];
		else
			gameObject.GetComponent<SpriteRenderer>().sprite = images[0];

		if (Unlocked) {
			Color a = new Color (255f / 255f, 255f / 255f, 255f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		} else {
			Color a = new Color (141f / 255f, 141f / 255f, 141f / 255f, 1f);
			gameObject.GetComponent<SpriteRenderer> ().color = a;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Name_UI!=null)
			Name_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
		Vector3 offsetColor = new Vector3 (55,0, 0);
		if(Description_UI!=null)
		Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;
	}

	void OnMouseDown()
	{
		//		Debug.Log("鼠标按下时");
		GameObject GameManager = GameObject.Find ("GameManager");
		GameManager.GetComponent<GameManager_InStage>().OnClickSkill(this.gameObject);

	}

	public void Destroied(){
		Destroy (Name_UI);
		Destroy (Description_UI);
		Destroy (gameObject);
	}
}
