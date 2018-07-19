using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	// Use this for initialization
	public GameObject UI_Text;
	public Sprite[] images;

	GameObject actionPoint_UI;
	GameObject actionColor_UI;
	GameObject actionType_UI;
	GameObject GameManager;


	public CardInfo CardInfo_this;
	public string owner;
	public bool visiable,chosed,used;
	public string InWhere;

	void Start () {
		GameManager = GameObject.Find ("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		UpdateUI ();
	}

	public void Init(CardInfo c,bool vis,string o,string p){
		CardInfo_this = new CardInfo(c.actionPoint,c.actionColor,c.actionType);
		visiable = vis;
		owner = o;
		InWhere = p;
		chosed = false;
		used = false;

		actionPoint_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
		actionPoint_UI.transform.SetParent (GameObject.Find ("Canvas").transform);

		actionColor_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
		actionColor_UI.transform.SetParent (GameObject.Find ("Canvas").transform);

		actionType_UI= (GameObject)Instantiate (UI_Text, transform.position, Quaternion.identity);
		actionType_UI.transform.SetParent (GameObject.Find ("Canvas").transform);

		UpdateUI ();

		if (visiable) {
			if (CardInfo_this.actionColor == "剪") {
				Color a = new Color (255f / 255f, 203f / 255f, 203f / 255f, 1f);
				gameObject.GetComponent<SpriteRenderer> ().color = a;
			}
			if (CardInfo_this.actionColor == "布") {
				Color a = new Color (203 / 255f, 251 / 255f, 255 / 255f, 1f);
				gameObject.GetComponent<SpriteRenderer> ().color = a;
			}
			if (CardInfo_this.actionColor == "石") {
				Color a = new Color (255 / 255f, 254 / 255f, 203 / 255f, 1f);
				gameObject.GetComponent<SpriteRenderer> ().color = a;
			}
		}
	}

	public void UpdateUI(){
		int cardHigh = 35;
		int cardWidth = cardHigh;

		actionPoint_UI.GetComponent<Text> ().text = CardInfo_this.actionPoint.ToString ();
		actionColor_UI.GetComponent<Text> ().text = CardInfo_this.actionColor;
		actionType_UI.GetComponent<Text> ().text = CardInfo_this.actionType;

		Vector3 offsetPoint = new Vector3 (-cardWidth, cardHigh, 0);
		actionPoint_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetPoint;

		Vector3 offsetColor = new Vector3 (cardWidth, cardHigh, 0);
		actionColor_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetColor;

		Vector3 offsetType = new Vector3 (0, -cardHigh, 0);
		actionType_UI.transform.localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2, Screen.height / 2, 0) + offsetType;
	}

	public void ViewPlay(){
		gameObject.GetComponent<SpriteRenderer>().sprite = images[1];
	}

	public void WinBattle(){
		CardInfo_this.actionPoint *= 2;
		transform.localScale = new Vector3 (1.3f, 1.3f, 1f);
		UpdateUI ();
	}

	public void BeUsed(){
		actionColor_UI.SetActive (false);
		actionPoint_UI.SetActive (false);
		actionType_UI.SetActive (false);
		gameObject.SetActive (false);
	}

	public void BeDestroied(){
		Destroy(actionColor_UI);
		Destroy(actionPoint_UI);
		Destroy(actionType_UI);
		Destroy(gameObject);
	}

	void OnMouseDrag ()
	{
//		Debug.Log("鼠标拖动该模型区域时");
	}

	void OnMouseDown()
	{
//		Debug.Log("鼠标按下时");
		if (used == false) {
			if (InWhere == "Game")
				GameManager.GetComponent<GameManager_InBattle> ().OnClickCard (this.gameObject);
			if (InWhere == "UpGrade")
				GameManager.GetComponent<GameManager_InStage> ().OnClickCard (this.gameObject);
			if (InWhere == "Remove")
				GameManager.GetComponent<GameManager_InStage> ().OnClickCard (this.gameObject);
			if (InWhere == "AddCard")
				GameManager.GetComponent<GameManager_InBattle> ().OnClickAddCard (this.gameObject);
		}

	}
	void OnMouseUp()
	{
//		Debug.Log("鼠标抬起时");
	}

	void OnMouseEnter()
	{
//		Debug.Log("鼠标进入该对象区域时");
		gameObject.GetComponent<SpriteRenderer>().sprite = images[1];
	}
	void OnMouseExit()
	{
//		Debug.Log("鼠标离开该模型区域时");
		gameObject.GetComponent<SpriteRenderer>().sprite = images[0];
	}
	void OnMouseOver()
	{
//		Debug.Log("鼠标停留在该对象区域时");
	}
}
