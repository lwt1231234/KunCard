using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_InBattle : MonoBehaviour {

	public GameObject Card_Prefab,BattleActionLabel_Prefab;

	public GameObject Player_Health_UI,Player_Armor_UI,PlayerSkillcard;
	public GameObject CPU_Health_UI,CPU_Armor_UI,CPUSkillcard;
	public GameObject UseCardButton,GameResult;

	public List<SkillInfo> SkillList=new List<SkillInfo>();

	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> Player_CardListInGame=new List<CardInfo>();
	public List<GameObject> Player_CardObjectList=new List<GameObject>();

	public List<CardInfo> CPU_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardListInGame=new List<CardInfo>();
	public List<GameObject> CPU_CardObjectList=new List<GameObject>();

	public List<string> PlayerAction=new List<string>();
	public List<string> CPUAction=new List<string>();

	public int Player_Health,CPU_Health,Player_Armor,CPU_Armor;
	public int Player_Skill,CPU_Skill;

	int CardUsedID,CPUCardUsedID,BPNumber;
	string GameStage;
	public GameObject GameInfo;

	void Start () {
		GameInfo = GameObject.Find ("GameInfo");
		if (GameInfo == null) {
			GameInfo = new GameObject ("GameInfo");
			GameInfo.AddComponent<GameInfo>();

			Invoke ("Start", 0.5f);
			return;
		}
		GameStage = "Init";

		Player_Health = GameInfo.GetComponent<GameInfo> ().Player_MaxHealth;
		CPU_Health = GameInfo.GetComponent<GameInfo> ().CPU_MaxHealth;

		SkillList = GameInfo.GetComponent<GameInfo> ().SkillList;
		Player_Skill = GameInfo.GetComponent<GameInfo> ().Player_SKill_Used;
		PlayerSkillcard.GetComponent<SkillCard> ().Init (SkillList[Player_Skill-1].Name,SkillList[Player_Skill-1].Description);

		CPU_Skill = GameInfo.GetComponent<GameInfo> ().CPU_SKill_Used;
		CPUSkillcard.GetComponent<SkillCard> ().Init (SkillList[CPU_Skill-1].Name,SkillList[CPU_Skill-1].Description);

		Player_Armor = 0;
		CPU_Armor = 0;
		GameResult.SetActive (false);

		Player_CardList = GameInfo.GetComponent<GameInfo> ().Player_CardList;
		CPU_CardList = GameInfo.GetComponent<GameInfo> ().CPU_CardList;

		for (int i = 0; i < 5; i++) {
			int j = Random.Range(0, Player_CardList.Count);
			Player_CardListInGame.Add (Player_CardList [j]);
			Player_CardList.Remove(Player_CardList [j]);
		}
			
		for (int i = 0; i < 7; i++) {
			int j = Random.Range(0, CPU_CardList.Count);
			CPU_CardListInGame.Add (CPU_CardList [j]);
			CPU_CardList.Remove(CPU_CardList [j]);
		}
		UpdateUI ();
		StartBP ();
	}

	void StartBP(){
		GameStage = "BP";
		BPNumber = 2;
		//玩家卡牌
		Vector3 PlayerCardPosition = new Vector3 (-9, -16, 0);
		for (int i = 0; i < Player_CardListInGame.Count; i++) {
			GameObject CardObject_tmp = Instantiate (Card_Prefab, PlayerCardPosition, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (Player_CardListInGame[i],true,"Player","Game");
			Player_CardObjectList.Add (CardObject_tmp);

			PlayerCardPosition += new Vector3 (5.5f, 0, 0);
		}

		//机器卡牌
		Vector3 CPUCardPosition = new Vector3 (-16, 0, 0);
		for (int i = 0; i < CPU_CardListInGame.Count; i++) {
			GameObject CardObject_tmp = Instantiate (Card_Prefab, CPUCardPosition, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (CPU_CardListInGame[i],true,"CPU","Game");
			CPU_CardObjectList.Add (CardObject_tmp);

			CPUCardPosition += new Vector3 (5.5f, 0, 0);
		}
	}

	void StartGame(){
		GameStage = "InGame";


		//机器卡牌
		Vector3 CPUCardPosition = new Vector3 (-9, 15, 0);
		for (int i = 0; i < CPU_CardObjectList.Count; i++) {
			CPU_CardObjectList [i].transform.position = CPUCardPosition;

			CPUCardPosition += new Vector3 (5.5f, 0, 0);
		}

		CPUCardUsedID = Random.Range (0, CPU_CardObjectList.Count-1);
		CPUCardUsedID = 0;
		CardUsedID = -1;
	}


	
	// Update is called once per frame
	void Update () {
		
	}
	//--------------------------------------画面效果
	void UpdateUI(){
		Player_Health_UI.GetComponent<Text> ().text = "生命：" + Player_Health.ToString ();
		CPU_Health_UI.GetComponent<Text> ().text = "生命：" + CPU_Health.ToString ();
		Player_Armor_UI.GetComponent<Text> ().text = "护甲：" + Player_Armor.ToString ();
		CPU_Armor_UI.GetComponent<Text> ().text = "护甲：" + CPU_Armor.ToString ();
	}
	//出牌
	void CardBattle(GameObject PlayerCard, GameObject CPUCard){
		PlayerCard.GetComponent<Card> ().used =true;
		CPUCard.GetComponent<Card> ().used =true;

		PlayerCard.transform.position = new Vector3 (0, -3, 0);
		PlayerCard.GetComponent<Card> ().UpdateUI ();
		CPUCard.transform.position = new Vector3 (0, 3, 0);
		CPUCard.GetComponent<Card> ().UpdateUI ();

		StartCoroutine (CardBattle2 (PlayerCard,CPUCard));
	}
	//触发克制效果
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

	//计算对战结果
	IEnumerator CardBattle3(GameObject PlayerCard, GameObject CPUCard){
		CardInfo PlayerInfo = PlayerCard.GetComponent<Card> ().CardInfo_this;
		CardInfo CPUInfo = CPUCard.GetComponent<Card> ().CardInfo_this;
		yield return new WaitForSeconds (1.0f);

		PlayerAction.Clear();
		CPUAction.Clear();

		string Action_tmp = "";

		if (PlayerInfo.actionType == "攻击") {
			Action_tmp ="受到"+PlayerInfo.actionPoint.ToString()+"点攻击";
			CPUAction.Add (Action_tmp);
			CPU_Armor -= PlayerInfo.actionPoint;
		}
		if (CPUInfo.actionType == "攻击") {
			Action_tmp ="受到"+CPUInfo.actionPoint.ToString()+"点攻击";
			PlayerAction.Add (Action_tmp);
			Player_Armor -= CPUInfo.actionPoint;
		}
		if (PlayerInfo.actionType == "防御") {
			Action_tmp ="获得"+PlayerInfo.actionPoint.ToString()+"点护甲";
			PlayerAction.Add (Action_tmp);
			Player_Armor += PlayerInfo.actionPoint;
		}
		if (CPUInfo.actionType == "防御") {
			Action_tmp ="获得"+CPUInfo.actionPoint.ToString()+"点护甲";
			CPUAction.Add (Action_tmp);
			CPU_Armor += CPUInfo.actionPoint;
		}
		if (PlayerInfo.actionType == "技能") {
			if (Player_Skill == 1)
				useSkill_chuanci ("Player",PlayerInfo.actionPoint);
			if (Player_Skill == 2)
				useSkill_huifu ("Player",PlayerInfo.actionPoint);
		}
		if (CPUInfo.actionType == "技能") {
			if (CPU_Skill == 1)
				useSkill_chuanci ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill == 2)
				useSkill_huifu ("CPU",CPUInfo.actionPoint);
		}
			
		if (Player_Armor < 0) {
			Player_Health += Player_Armor;
			Player_Armor = 0;
		}
		if (CPU_Armor < 0) {
			CPU_Health += CPU_Armor;
			CPU_Armor = 0;
		}
		if(PlayerAction.Count>0)
			StartCoroutine (ShowAction("Player"));
		if(CPUAction.Count>0)
			StartCoroutine (ShowAction("CPU"));


		for (int i = 0; i < PlayerAction.Count; i++) {
			
		}
		for (int i = 0; i < CPUAction.Count; i++) {
			
		}
		UpdateUI ();

		StartCoroutine (CardBattle4 (PlayerCard,CPUCard));
	}
	//移除卡牌
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

	IEnumerator ShowAction(string player){
		bool More = true;
		if (player == "Player") {
			if (PlayerAction.Count > 0) {
				GameObject ActionObject_tmp = Instantiate (BattleActionLabel_Prefab, Player_Health_UI.transform.position, Quaternion.identity);
				ActionObject_tmp.transform.SetParent (GameObject.Find ("Canvas").transform);
				ActionObject_tmp.GetComponent<PlayerActionLabel> ().Init (PlayerAction [0],1);
				PlayerAction.Remove (PlayerAction [0]);
			} else
				More = false;
		} else {
			if (CPUAction.Count > 0) {
				GameObject ActionObject_tmp = Instantiate (BattleActionLabel_Prefab, CPU_Armor_UI.transform.position, Quaternion.identity);
				ActionObject_tmp.transform.SetParent (GameObject.Find ("Canvas").transform);
				ActionObject_tmp.GetComponent<PlayerActionLabel> ().Init (CPUAction [0],-1);
				CPUAction.Remove (CPUAction [0]);
			} else
				More = false;
		}
		yield return new WaitForSeconds (0.5f);
		if(More)
			StartCoroutine (ShowAction (player));

	}
	//---------------------------------------技能相关
	void useSkill_chuanci (string user,int point){
		string Action_tmp = "";
		Action_tmp ="受到"+point.ToString()+"点伤害";
		if (user == "Player") {
			CPU_Health -= point;
			CPUAction.Add (Action_tmp);
		}else{
			Player_Health -= point;
			PlayerAction.Add (Action_tmp);
		}
	}

	void useSkill_huifu (string user,int point){
		string Action_tmp = "";
		Action_tmp ="恢复"+point.ToString()+"点生命";
		if (user == "Player") {
			Player_Health += point;
			int maxHealth = GameInfo.GetComponent<GameInfo> ().Player_MaxHealth;
			if (Player_Health > maxHealth)
				Player_Health = maxHealth;
			PlayerAction.Add (Action_tmp);
		}else{
			CPU_Health += point;
			int maxHealth = GameInfo.GetComponent<GameInfo> ().CPU_MaxHealth;
			if (CPU_Health > maxHealth)
				CPU_Health = maxHealth;
			CPUAction.Add (Action_tmp);
		}
	}

	//---------------------------------------玩家动作
	public void OnClickCard(GameObject c){

		if (GameStage == "InGame") {
			Vector3 CardUp = new Vector3 (0, -2f, 0);
			if (UseCardButton.GetComponent<Button> ().interactable == false)
				return;
			if (c.GetComponent<Card> ().owner != "Player")
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
		if (GameStage == "BP") {
			if (c.GetComponent<Card> ().owner != "CPU")
				return;
			CPU_CardObjectList.Remove (c);
			c.GetComponent<Card> ().BeUsed ();
			BPNumber--;
			if (BPNumber <= 0)
				StartGame ();
		}
	}

	public void OnClickPlayCard(){
		if (CardUsedID != -1) {
			CardBattle (Player_CardObjectList [CardUsedID], CPU_CardObjectList [CPUCardUsedID]);
			CardUsedID = -1;
			UseCardButton.GetComponent<Button> ().interactable = false;
		}
	}
	//----------------------------------游戏阶段进程
	void GameEnd(){
		GameResult.SetActive (true);
		if (Player_Health > CPU_Health || (Player_Health == CPU_Health && Player_Armor>CPU_Armor)) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
														+"护甲"+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"获胜！";
			GameInfo.GetComponent<GameInfo> ().WinGame ();
			Invoke ("BackToStage", 3.0f);
		} 
		if (Player_Health < CPU_Health || (Player_Health == CPU_Health && Player_Armor<CPU_Armor)) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
				+"护甲 "+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"败北！";

			GameInfo.GetComponent<GameInfo> ().WinGame ();
			Invoke ("BackToStage", 3.0f);
		} 
		if (Player_Health == CPU_Health && Player_Armor==CPU_Armor) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
				+"护甲 "+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"平局！";

			GameInfo.GetComponent<GameInfo> ().WinGame ();
			Invoke ("BackToStage", 3.0f);
		} 



	}

	void BackToStage(){
		SceneManager.LoadScene (0);
	}
}
