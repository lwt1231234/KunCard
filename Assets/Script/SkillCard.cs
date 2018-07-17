using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour {

	public GameObject UI_Text;
	GameObject Name_UI,Description_UI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(string name,string description){
		Name_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
		Name_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
		Name_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
		Name_UI.GetComponent<Text> ().text = name;

		Vector3 offsetColor = new Vector3 (0,55, 0);
		Description_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
		Description_UI.transform.SetParent (GameObject.Find ("Canvas").transform);
		Description_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;
		Description_UI.GetComponent<Text> ().text = "description";
		Description_UI.SetActive(false);
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
