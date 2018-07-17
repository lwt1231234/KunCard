using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_InStage : MonoBehaviour {

	int StageLevel;
	public GameObject Card_Prefab,Skill_Prefab;


	public GameObject GameInfo;
	public GameObject Gold_UI,Title_UI;
	public GameObject ReadyToPlay,ReadyToUpGrade,ReadyToRemove,UnlockSkill,Close,StageParent;
	public GameObject ViewCard,ChooseSkill;
	public GameObject[] Stage;

	public List<GameObject> Player_CardObjectList=new List<GameObject>();
	public List<GameObject> SkillObjectList=new List<GameObject>();
	int CardSelected,SkillSelect;

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

		StageParent.SetActive(true);
		ViewCard.SetActive(true);
		ChooseSkill.SetActive(true);
	
		ReadyToPlay.SetActive (false);
		Close.SetActive (false);
		ReadyToUpGrade.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		if (GameInfo != null) {
			int gold = GameInfo.GetComponent<GameInfo> ().Gold;
			int card = GameInfo.GetComponent<GameInfo> ().Player_CardList.Count;
			Gold_UI.GetComponent<Text> ().text = "拥有金币:" + gold.ToString()+"  牌组数量:"+card.ToString();
		}
	}
	//-----------------------------------------返回按钮
	public void OnClickCancel(){
		for (int i = Player_CardObjectList.Count; i >0 ; i--) {
			Player_CardObjectList [i-1].GetComponent<Card> ().BeDestroied ();;
		}
		Player_CardObjectList.Clear ();

		StageParent.SetActive(true);
		ViewCard.SetActive(true);
		ChooseSkill.SetActive(true);

		Close.SetActive (false);
		ReadyToUpGrade.SetActive (false);
		ReadyToRemove.SetActive (false);
		UnlockSkill.SetActive (false);
		Title_UI.SetActive (false);
		for (int i = 0; i < SkillObjectList.Count; i++) {
			SkillObjectList [i].GetComponent<Skill> ().Destroied ();
		}
		SkillObjectList.Clear ();
	}
		
	//------------------------------------------------点击关卡
	public void OnClickStage(GameObject c){
		StageLevel = c.GetComponent<Stage> ().StageLevel;
		string StageType = c.GetComponent<Stage> ().StageType;
		if (StageType == "Game") {
			//开始关卡
			ReadyToPlay.SetActive (true);
			ReadyToPlay.GetComponent<Text> ().text = "开始关卡" + StageLevel.ToString();
		}
		if (StageType == "UpGrade") {
			//升级卡牌
			StageParent.SetActive(false);
			ViewCard.SetActive(false);
			ChooseSkill.SetActive(false);
			Close.SetActive (true);
			Title_UI.SetActive (true);
			Title_UI.GetComponent<Text>().text = c.GetComponent<Stage> ().Description;

			ShowPlayerCard ("UpGrade");
			GameInfo.GetComponent<GameInfo> ().LoadStage (StageLevel);
			GameInfo.GetComponent<GameInfo> ().WinGame ();
			int StageNum = GameInfo.GetComponent<GameInfo> ().StageNum;
			for (int i = 0; i < StageNum; i++)
				Stage [i].GetComponent<Stage> ().Init ();
		}
		if (StageType == "Remove") {
			//移除卡牌
			StageParent.SetActive(false);
			ViewCard.SetActive(false);
			ChooseSkill.SetActive(false);
			Close.SetActive (true);
			Title_UI.SetActive (true);
			Title_UI.GetComponent<Text>().text = c.GetComponent<Stage> ().Description;

			ShowPlayerCard ("Remove");
			GameInfo.GetComponent<GameInfo> ().LoadStage (StageLevel);
			GameInfo.GetComponent<GameInfo> ().WinGame ();
			int StageNum = GameInfo.GetComponent<GameInfo> ().StageNum;
			for (int i = 0; i < StageNum; i++)
				Stage [i].GetComponent<Stage> ().Init ();
		}
	}

	public void OnClickCancelGame(){
		ReadyToPlay.SetActive (false);
	}
		
	public void OnClickStartGame(){
		ReadyToPlay.SetActive (false);
		GameInfo.GetComponent<GameInfo> ().LoadStage (StageLevel);
	}
	//--------------------------------------查看牌组
	void ShowPlayerCard(string s){
		for (int i = Player_CardObjectList.Count; i >0 ; i--) {
			Player_CardObjectList [i-1].GetComponent<Card> ().BeDestroied ();;
		}
		Player_CardObjectList.Clear ();

		Vector3 PlayerCardPosition = new Vector3 (-22, 8, 0);
		List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
		Player_CardObjectList.Clear ();
		for (int i = 0; i < Player_CardListHere.Count; i++) {
			int xx = i % 9;
			int yy = i / 9;
			Vector3 Postion = PlayerCardPosition + new Vector3 (5.5f*xx, -6*yy, 0);
			GameObject CardObject_tmp = Instantiate (Card_Prefab, Postion, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (Player_CardListHere[i],true,"Player",s);
			Player_CardObjectList.Add (CardObject_tmp);
		}
	}

	public void OnClickViewCard(){
		StageParent.SetActive(false);
		ViewCard.SetActive(false);
		ChooseSkill.SetActive(false);
		Close.SetActive (true);
		Title_UI.SetActive (true);
		Title_UI.GetComponent<Text> ().text = "查看牌组";

		ShowPlayerCard ("View");
	}

	public void OnClickCard(GameObject c){
		string w = c.GetComponent<Card> ().InWhere;
		if (w == "UpGrade") {
			for (int i = 0; i < Player_CardObjectList.Count; i++)
				if (Player_CardObjectList[i] == c)
					CardSelected = i;
			ReadyToUpGrade.SetActive (true);

			for (int i = Player_CardObjectList.Count; i >0 ; i--) {
				Player_CardObjectList [i-1].GetComponent<Card> ().BeDestroied ();;
			}
			Player_CardObjectList.Clear ();

			List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
			string text_tmp = "升级卡牌\n";
			text_tmp += Player_CardListHere [CardSelected].actionPoint.ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionColor.ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionType.ToString()+" -> ";
			text_tmp += (Player_CardListHere [CardSelected].actionPoint+1).ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionColor.ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionType.ToString()+"\n需要";
			text_tmp += (Player_CardListHere [CardSelected].actionPoint*100).ToString()+"金币";
			ReadyToUpGrade.GetComponent<Text> ().text = text_tmp;

			if (GameInfo.GetComponent<GameInfo> ().Gold >= Player_CardListHere [CardSelected].actionPoint * 100) {
				GameObject.Find ("UpGradeOK").GetComponent<Button> ().interactable = true;
			}
			else
				GameObject.Find ("UpGradeOK").GetComponent<Button> ().interactable = false;
		}
		if (w == "Remove") {
			for (int i = 0; i < Player_CardObjectList.Count; i++)
				if (Player_CardObjectList[i] == c)
					CardSelected = i;
			ReadyToRemove.SetActive (true);

			for (int i = Player_CardObjectList.Count; i >0 ; i--) {
				Player_CardObjectList [i-1].GetComponent<Card> ().BeDestroied ();;
			}
			Player_CardObjectList.Clear ();

			List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
			string text_tmp = "移除卡牌\n";
			text_tmp += Player_CardListHere [CardSelected].actionPoint.ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionColor.ToString()+"-";
			text_tmp += Player_CardListHere [CardSelected].actionType.ToString()+"\n需要100金币";
			ReadyToRemove.GetComponent<Text> ().text = text_tmp;

			if (GameInfo.GetComponent<GameInfo> ().Gold >= 100) {
				GameObject.Find ("RemoveOK").GetComponent<Button> ().interactable = true;
			}
			else
				GameObject.Find ("RemoveOK").GetComponent<Button> ().interactable = false;
		}
	}
	//--------------------------------------------升级卡牌
	public void OnClickDoUpGrade(){
		List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
		GameInfo.GetComponent<GameInfo> ().Gold -= Player_CardListHere [CardSelected].actionPoint * 100;
		Player_CardListHere [CardSelected].actionPoint += 1;

		ReadyToUpGrade.SetActive (false);
		ShowPlayerCard ("UpGrade");
	}

	public void OnClickCancelUpGrade(){
		ReadyToUpGrade.SetActive (false);
		ShowPlayerCard ("UpGrade");
	}

	//-------------------------------------------移除卡牌
	public void OnClickDoRemove(){
		List<CardInfo> Player_CardListHere=GameInfo.GetComponent<GameInfo> ().Player_CardList;
		GameInfo.GetComponent<GameInfo> ().Gold -= Player_CardListHere [CardSelected].actionPoint * 100;
		Player_CardListHere.Remove (Player_CardListHere [CardSelected]);

		ReadyToRemove.SetActive (false);
		ShowPlayerCard ("Remove");
	}

	public void OnClickCancelRemove(){
		ReadyToRemove.SetActive (false);
		ShowPlayerCard ("Remove");
	}

	//-----------------------------------------------选择技能
	public void OnClickChooseSkill(){
		StageParent.SetActive(false);
		ViewCard.SetActive(false);
		ChooseSkill.SetActive(false);
		Close.SetActive (true);
		Title_UI.SetActive (true);
		Title_UI.GetComponent<Text> ().text = "选择技能";


		List<SkillInfo> SkillList = GameInfo.GetComponent<GameInfo> ().SkillList;
		Vector3 Position = new Vector3 (-21, 14, 0);
		SkillObjectList.Clear ();
		for (int i = 0; i < SkillList.Count; i++) {

			GameObject tmp = Instantiate (Skill_Prefab, Position, Quaternion.identity);
			tmp.GetComponent<Skill>().SkillNum = i+1;
			tmp.GetComponent<Skill> ().Init ();
			SkillObjectList.Add (tmp);
			Position += new Vector3 (0, -5, 0);
		}
	}

	public void OnClickSkill(GameObject c){
		SkillSelect = c.GetComponent<Skill> ().SkillNum;
		List<SkillInfo> SkillList = GameInfo.GetComponent<GameInfo> ().SkillList;
		if (!c.GetComponent<Skill> ().Unlocked) {
			UnlockSkill.SetActive (true);
			SkillInfo SkillThis = GameInfo.GetComponent<GameInfo> ().SkillList [SkillSelect - 1];
			for (int i = 0; i < SkillObjectList.Count; i++)
				SkillObjectList [i].GetComponent<Skill> ().Destroied ();
			UnlockSkill.GetComponent<Text> ().text = "花费" + SkillThis.GoldRequire.ToString () + "金币解锁技能\"" + SkillThis.Name+"\"";
			if (GameInfo.GetComponent<GameInfo> ().Gold >= SkillThis.GoldRequire) {
				GameObject.Find ("UnlockOK").GetComponent<Button> ().interactable = true;
			}
			else
				GameObject.Find ("UnlockOK").GetComponent<Button> ().interactable = false;
		} else {
			for (int i = 0; i < SkillList.Count; i++) {
				SkillList [i].Used = false;
			}
			SkillList [SkillSelect - 1].Used = true;
			for (int i = 0; i < SkillObjectList.Count; i++) {
				SkillObjectList[i].GetComponent<Skill> ().Init ();
			}
			GameInfo.GetComponent<GameInfo> ().Player_SKill_Used=SkillSelect;
		}
	}

	public void OnClickUnlockOK(){
		List<SkillInfo> SkillList = GameInfo.GetComponent<GameInfo> ().SkillList;
		SkillList [SkillSelect - 1].Unlocked = true;
		GameInfo.GetComponent<GameInfo> ().Gold -= SkillList [SkillSelect - 1].GoldRequire;
		for (int i = 0; i < SkillList.Count; i++) {
			SkillList [i].Used = false;
		}
		SkillList [SkillSelect - 1].Used = true;
		GameInfo.GetComponent<GameInfo> ().Player_SKill_Used=SkillSelect;

		OnClickChooseSkill ();
		UnlockSkill.SetActive (false);
	}

	public void OnClickUnlockCancel(){
		OnClickChooseSkill ();
		UnlockSkill.SetActive (false);
	}
}
