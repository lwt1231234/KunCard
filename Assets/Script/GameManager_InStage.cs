using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_InStage : MonoBehaviour {

	int StageLevel;
	public GameObject Card_Prefab;


	public GameObject GameInfo;
	public GameObject Gold_UI;
	public GameObject ReadyToPlay,UpGrade,StageParent;
	public GameObject ViewCard;
	public GameObject[] Stage;

	public List<GameObject> Player_CardObjectList=new List<GameObject>();

	// Use this for initialization
	void Start () {
		GameInfo = GameObject.Find ("GameInfo");
		if (GameInfo == null) {
			GameInfo = new GameObject ("GameInfo");
			GameInfo.AddComponent<GameInfo>();

			Invoke ("Start", 0.5f);
			return;
		}

		int StageNum = GameInfo.GetComponent<GameInfo> ().StageNum;
		for (int i = 0; i < StageNum; i++)
			Stage [i].GetComponent<Stage> ().Init ();

		ReadyToPlay.SetActive (false);
		UpGrade.SetActive (false);
		StageParent.SetActive(true);
		ViewCard.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameInfo != null) {
			int gold = GameInfo.GetComponent<GameInfo> ().Gold;
			int card = GameInfo.GetComponent<GameInfo> ().Player_CardList.Count;
			Gold_UI.GetComponent<Text> ().text = "拥有金币:" + gold.ToString()+"  牌组数量:"+card.ToString();
		}
	}

	public void OnClickStage(GameObject c){
		StageLevel = c.GetComponent<Stage> ().StageLevel;
		string StageType = c.GetComponent<Stage> ().StageType;
		if (StageType == "UpGrade") {
			//升级卡牌
			StageParent.SetActive(false);
			ViewCard.SetActive(false);
			UpGrade.SetActive (true);

			Vector3 PlayerCardPosition = new Vector3 (-22, 8, 0);
			List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
			Player_CardObjectList.Clear ();
			for (int i = 0; i < Player_CardListHere.Count; i++) {
				int xx = i % 9;
				int yy = i / 9;
				Vector3 Postion = PlayerCardPosition + new Vector3 (5.5f*xx, -6*yy, 0);
				GameObject CardObject_tmp = Instantiate (Card_Prefab, Postion, Quaternion.identity);
				CardObject_tmp.GetComponent<Card> ().Init (Player_CardListHere[i],true,"Player","UpGrade");
				Player_CardObjectList.Add (CardObject_tmp);
			}

		}
		if (StageType == "Game") {
			//开始关卡
			ReadyToPlay.SetActive (true);
			ReadyToPlay.GetComponent<Text> ().text = "开始关卡" + StageLevel.ToString();
		}

	}

	public void OnClickViewCard(){
		StageParent.SetActive(false);
		ViewCard.SetActive(false);
		UpGrade.SetActive (true);

		Vector3 PlayerCardPosition = new Vector3 (-22, 8, 0);
		List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
		Player_CardObjectList.Clear ();
		for (int i = 0; i < Player_CardListHere.Count; i++) {
			int xx = i % 9;
			int yy = i / 9;
			Vector3 Postion = PlayerCardPosition + new Vector3 (5.5f*xx, -6*yy, 0);
			GameObject CardObject_tmp = Instantiate (Card_Prefab, Postion, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (Player_CardListHere[i],true,"Player","View");
			Player_CardObjectList.Add (CardObject_tmp);
		}
	}

	public void OnClickStartGame(){
		ReadyToPlay.SetActive (false);
		GameInfo.GetComponent<GameInfo> ().LoadStage (StageLevel);
	}

	public void OnClickCancelGame(){
		ReadyToPlay.SetActive (false);
	}

	public void OnClickCard(GameObject c){
		print (1);
	}

	public void OnClickCancelUpGrade(){
		for (int i = Player_CardObjectList.Count; i >0 ; i--) {
			Player_CardObjectList [i-1].GetComponent<Card> ().BeDestroied ();;
		}
		StageParent.SetActive(true);
		ViewCard.SetActive(true);
		UpGrade.SetActive (false);

	}
}
