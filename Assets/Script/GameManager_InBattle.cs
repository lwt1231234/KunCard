using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardInfo{
	public int actionPoint;
	public string actionType,actionColor;
	public bool inUse;

	public CardInfo(int a,string b, string c){
		actionPoint = a;
		actionType = c;
		actionColor = b;
	}
}

public class GameManager_InBattle : MonoBehaviour {

	public GameObject Card_Prefab;

	public GameObject Player_Health_UI,Player_Armor_UI,PlayerAttackedAction_UI,PlayerArmorAction_UI;
	public GameObject CPU_Health_UI,CPU_Armor_UI,CPUAttackedAction_UI,CPUArmorAction_UI;
	public GameObject UseCardButton,GameResult;


	public List<CardInfo> Player_CardListInGame=new List<CardInfo>();
	public List<GameObject> Player_CardObjectList=new List<GameObject>();

	public List<CardInfo> CPU_CardListInGame=new List<CardInfo>();
	public List<GameObject> CPU_CardObjectList=new List<GameObject>();

	public int Player_Health,CPU_Health,Player_Armor,CPU_Armor;
	int CardUsedID,CPUCardUsedID;
	GameObject GameInfo;

	void Start () {
		GameInfo = GameObject.Find ("GameInfo");
		if (GameInfo == null) {
			GameInfo = new GameObject ("GameInfo");
			GameInfo.AddComponent<GameInfo>();
		}

		Player_Health = GameInfo.GetComponent<GameInfo> ().Player_MaxHealth;
		CPU_Health = GameInfo.GetComponent<GameInfo> ().Player_MaxHealth;
		Player_Armor = 0;
		CPU_Armor = 0;
		GameResult.SetActive (false);

		Player_CardListInGame.Add (new CardInfo(1,"剪","攻击"));
		Player_CardListInGame.Add (new CardInfo(1,"布","防御"));
		Player_CardListInGame.Add (new CardInfo(1,"石","攻击"));
		Player_CardListInGame.Add (new CardInfo(1,"剪","防御"));
		Player_CardListInGame.Add (new CardInfo(1,"石","技能"));

		CPU_CardListInGame.Add (new CardInfo(1,"剪","攻击"));
		CPU_CardListInGame.Add (new CardInfo(1,"布","防御"));
		CPU_CardListInGame.Add (new CardInfo(1,"石","攻击"));
		CPU_CardListInGame.Add (new CardInfo(1,"剪","防御"));
		CPU_CardListInGame.Add (new CardInfo(1,"石","技能"));

		//玩家卡牌
		Vector3 PlayerCardPosition = new Vector3 (-9, -15, 0);
		for (int i = 0; i < Player_CardListInGame.Count; i++) {
			GameObject CardObject_tmp = Instantiate (Card_Prefab, PlayerCardPosition, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (Player_CardListInGame[i],true,"Player");
			Player_CardObjectList.Add (CardObject_tmp);

			PlayerCardPosition += new Vector3 (5.5f, 0, 0);
		}

		//机器卡牌
		Vector3 CPUCardPosition = new Vector3 (-9, 15, 0);
		for (int i = 0; i < CPU_CardListInGame.Count; i++) {
			GameObject CardObject_tmp = Instantiate (Card_Prefab, CPUCardPosition, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (CPU_CardListInGame[i],true,"CPU");
			CPU_CardObjectList.Add (CardObject_tmp);

			CPUCardPosition += new Vector3 (5.5f, 0, 0);
		}

		CPUCardUsedID = Random.Range (0, CPU_CardObjectList.Count-1);
		CPUCardUsedID = 0;
		CardUsedID = -1;

		UpdateUI ();
	}

	void UpdateUI(){
		Player_Health_UI.GetComponent<Text> ().text = "生命：" + Player_Health.ToString ();
		CPU_Health_UI.GetComponent<Text> ().text = "生命：" + CPU_Health.ToString ();
		Player_Armor_UI.GetComponent<Text> ().text = "护甲：" + Player_Armor.ToString ();
		CPU_Armor_UI.GetComponent<Text> ().text = "护甲：" + CPU_Armor.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CardBattle(GameObject PlayerCard, GameObject CPUCard){
		PlayerCard.GetComponent<Card> ().used =true;
		CPUCard.GetComponent<Card> ().used =true;

		PlayerCard.transform.position = new Vector3 (0, -3, 0);
		PlayerCard.GetComponent<Card> ().UpdateUI ();
		CPUCard.transform.position = new Vector3 (0, 3, 0);
		CPUCard.GetComponent<Card> ().UpdateUI ();

		StartCoroutine (CardBattle2 (PlayerCard,CPUCard));
	}

	IEnumerator CardBattle2(GameObject PlayerCard, GameObject CPUCard){
		CardInfo PlayerInfo = PlayerCard.GetComponent<Card> ().CardInfo_this;
		CardInfo CPUInfo = CPUCard.GetComponent<Card> ().CardInfo_this;

		yield return new WaitForSeconds (1.0f);

		if (PlayerInfo.actionColor == "剪") {
			if (CPUInfo.actionColor == "布") {
				PlayerCard.GetComponent<Card> ().WinBattle ();
			}
			if (CPUInfo.actionColor == "石") {
				CPUCard.GetComponent<Card> ().WinBattle ();
			}
		}
		if (PlayerInfo.actionColor == "布") {
			if (CPUInfo.actionColor == "石") {
				PlayerCard.GetComponent<Card> ().WinBattle ();
			}
			if (CPUInfo.actionColor == "剪") {
				CPUCard.GetComponent<Card> ().WinBattle ();
			}
		}
		if (PlayerInfo.actionColor == "石") {
			if (CPUInfo.actionColor == "剪") {
				PlayerCard.GetComponent<Card> ().WinBattle ();
			}
			if (CPUInfo.actionColor == "布") {
				CPUCard.GetComponent<Card> ().WinBattle ();
			}
		}

		StartCoroutine (CardBattle3 (PlayerCard,CPUCard));
	}

	IEnumerator CardBattle3(GameObject PlayerCard, GameObject CPUCard){
		CardInfo PlayerInfo = PlayerCard.GetComponent<Card> ().CardInfo_this;
		CardInfo CPUInfo = CPUCard.GetComponent<Card> ().CardInfo_this;
		yield return new WaitForSeconds (1.0f);

		PlayerAttackedAction_UI.GetComponent<Text> ().text = "";
		PlayerArmorAction_UI.GetComponent<Text> ().text = "";
		CPUAttackedAction_UI.GetComponent<Text> ().text = "";
		CPUArmorAction_UI.GetComponent<Text> ().text = "";

		string CPUAction = "";
		string PlayerAction = "";
		int PlayerLifeChange = 0;
		int CPULifeChange = 0;

		if (PlayerInfo.actionType == "攻击") {
			CPUAttackedAction_UI.GetComponent<Text> ().text ="受到"+PlayerInfo.actionPoint.ToString()+"点攻击";
			CPULifeChange -= PlayerInfo.actionPoint;
		}
		if (CPUInfo.actionType == "攻击") {
			PlayerAttackedAction_UI.GetComponent<Text> ().text ="受到"+CPUInfo.actionPoint.ToString()+"点攻击";
			PlayerLifeChange -= CPUInfo.actionPoint;
		}
		if (PlayerInfo.actionType == "防御") {
			PlayerArmorAction_UI.GetComponent<Text> ().text ="获得"+PlayerInfo.actionPoint.ToString()+"点护甲";
			PlayerLifeChange += PlayerInfo.actionPoint;
		}
		if (CPUInfo.actionType == "防御") {
			CPUArmorAction_UI.GetComponent<Text> ().text ="获得"+CPUInfo.actionPoint.ToString()+"点护甲";
			CPULifeChange += CPUInfo.actionPoint;
		}
			
		Player_Armor += PlayerLifeChange;
		if (Player_Armor < 0) {
			Player_Health += Player_Armor;
			Player_Armor = 0;
		}
		CPU_Armor += CPULifeChange;
		if (CPU_Armor < 0) {
			CPU_Health += CPU_Armor;
			CPU_Armor = 0;
		}
		UpdateUI ();

		StartCoroutine (CardBattle4 (PlayerCard,CPUCard));
	}

	IEnumerator CardBattle4(GameObject PlayerCard, GameObject CPUCard){
		yield return new WaitForSeconds (1.0f);

		Player_CardObjectList.Remove (PlayerCard);
		CPU_CardObjectList.Remove (CPUCard);

		PlayerCard.GetComponent<Card> ().BeUsed ();
		CPUCard.GetComponent<Card> ().BeUsed ();

		if (Player_CardObjectList.Count <= 0) {
			GameEnd ();
		} else {
			UseCardButton.GetComponent<Button> ().interactable = true;
			CPUCardUsedID = Random.Range (0, CPU_CardObjectList.Count-1);
		}
	}

	public void OnClickCard(GameObject c){
		Vector3 CardUp = new Vector3 (0, -2f, 0);
		if (UseCardButton.GetComponent<Button> ().interactable == false)
			return;

		for (int i = 0; i < Player_CardObjectList.Count; i++) {
			//Debug.Log(c.GetComponent<Card> ().chosed);
			if (c != Player_CardObjectList [i]) {
				if (Player_CardObjectList [i].GetComponent<Card> ().chosed && Player_CardObjectList [i].GetComponent<Card> ().used==false) {
					Player_CardObjectList [i].transform.position += CardUp;
					Player_CardObjectList [i].GetComponent<Card> ().chosed = false;
					Player_CardObjectList [i].GetComponent<Card> ().UpdateUI ();
				}
			} else {
				CardUsedID = i;
				if (!c.GetComponent<Card> ().chosed&&c.GetComponent<Card> ().used == false) {
					c.transform.position -= CardUp;
					c.GetComponent<Card> ().chosed = true;
					c.GetComponent<Card> ().UpdateUI ();
				}
			} 
		}
	}

	public void OnClickPlayCard(){
		if (CardUsedID != -1) {
			CardBattle (Player_CardObjectList [CardUsedID], CPU_CardObjectList [CPUCardUsedID]);
			CardUsedID = -1;
			UseCardButton.GetComponent<Button> ().interactable = false;
		}

	}

	void GameEnd(){
		GameResult.SetActive (true);
		if (Player_Health > CPU_Health || (Player_Health == CPU_Health && Player_Armor>CPU_Armor)) {
			GameResult.GetComponent<Text> ().text = "获胜！";
		} 
		if (Player_Health < CPU_Health || (Player_Health == CPU_Health && Player_Armor<CPU_Armor)) {
			GameResult.GetComponent<Text> ().text = "败北！";
		} 
		if (Player_Health == CPU_Health && Player_Armor==CPU_Armor) {
			GameResult.GetComponent<Text> ().text = "平局！";
		} 

		//SceneManager.LoadScene (1);
	}
}
