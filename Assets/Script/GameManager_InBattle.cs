using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_InBattle : MonoBehaviour {

	public GameObject Card_Prefab,BattleActionLabel_Prefab;

	public GameObject Player_Health_UI,Player_Armor_UI,PlayerSkillcard;
	public GameObject CPU_Health_UI,CPU_Armor_UI,CPUSkillcard;
	public GameObject UseCardButton,CancelAddCardButton,GameResult,GetGold;

	public List<SkillInfo> SkillList=new List<SkillInfo>();

	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> Player_CardListInGame=new List<CardInfo>();
	public List<GameObject> Player_CardObjectList=new List<GameObject>();

	public List<SkillInfo> CPUSkillList=new List<SkillInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardListInGame=new List<CardInfo>();
	public List<GameObject> CPU_CardObjectList=new List<GameObject>();

	public List<CardInfo> AddCardList=new List<CardInfo>();
	public List<GameObject> AddCardObjectList=new List<GameObject>();

	public List<string> PlayerAction=new List<string>();
	public List<string> CPUAction=new List<string>();

	public int Player_Health,CPU_Health,Player_Armor,CPU_Armor;
	int Player_Get_Attack,CPU_Get_Attack;
	public SkillInfo Player_Skill,CPU_Skill;
	int AddCardLevel;

	int CardUsedID,CPUCardUsedID,BPNumber;
	string GameStage;
	public GameObject GameInfo;

	int Bust_player,Bust_CPU,Bust_player_last,Bust_CPU_last;

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
		AddCardLevel = GameInfo.GetComponent<GameInfo> ().AddCardLevel;


		SkillList = GameInfo.GetComponent<GameInfo> ().SkillList;
		Player_Skill = SkillList[GameInfo.GetComponent<GameInfo> ().Player_SKill_Used];
		PlayerSkillcard.GetComponent<SkillCard> ().Init (Player_Skill.Name,Player_Skill.Description);

		CPU_Skill = GameInfo.GetComponent<GameInfo> ().CPU_SKill_Used;
		CPUSkillcard.GetComponent<SkillCard> ().Init (CPU_Skill.Name,CPU_Skill.Description);

		Player_Armor = 0;
		CPU_Armor = 0;
		GameResult.SetActive (false);
		GetGold.SetActive (false);
		CancelAddCardButton.SetActive (false);

		for (int i = 0; i < GameInfo.GetComponent<GameInfo> ().Player_CardList.Count; i++)
			Player_CardList.Add (GameInfo.GetComponent<GameInfo> ().Player_CardList [i]);

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

		CardInfo tmp;
		for (int i = 0; i < Player_CardListInGame.Count - 1; i++)
			for (int j = Player_CardListInGame.Count - 1; j > i; j--) {
				if (Player_CardListInGame [j].Comparer (Player_CardListInGame [j - 1]) < 0) {
					tmp = Player_CardListInGame [j];
					Player_CardListInGame [j] = Player_CardListInGame [j - 1];
					Player_CardListInGame [j - 1] = tmp;
				}
			}
				
		for (int i = 0; i < CPU_CardListInGame.Count - 1; i++)
			for (int j = CPU_CardListInGame.Count - 1; j > i; j--) {
				if (CPU_CardListInGame [j].Comparer (CPU_CardListInGame [j - 1]) < 0) {
					tmp = CPU_CardListInGame [j];
					CPU_CardListInGame [j] = CPU_CardListInGame [j - 1];
					CPU_CardListInGame [j - 1] = tmp;
				}
			}
		Bust_player = 0;
		Bust_player_last = 0;
		Bust_CPU = 0;
		Bust_CPU_last = 0;
		UpdateUI ();
		StartBP ();
	}

	void StartBP(){
		GameStage = "BP";
		BPNumber = 2;
		GetGold.SetActive (true);
		GetGold.GetComponent<Text> ().text = "移除对手的2张牌";

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
		GetGold.SetActive (false);

		//机器卡牌
		Vector3 CPUCardPosition = new Vector3 (-9, 15, 0);
		for (int i = 0; i < CPU_CardObjectList.Count; i++) {
			CPU_CardObjectList [i].transform.position = CPUCardPosition;

			CPUCardPosition += new Vector3 (5.5f, 0, 0);
		}

		CPUCardUsedID = Random.Range (0, CPU_CardObjectList.Count-1);
		if (GameInfo.GetComponent<GameInfo> ().Cheat)
			CPU_CardObjectList [CPUCardUsedID].GetComponent<Card> ().ViewPlay ();
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
		Player_Get_Attack = 0;
		CPU_Get_Attack = 0;

		string Action_tmp = "";

		if (PlayerInfo.actionType == "攻击") {
			Action_tmp ="受到"+PlayerInfo.actionPoint.ToString()+"点攻击";
			CPUAction.Add (Action_tmp);
			CPU_Get_Attack += PlayerInfo.actionPoint;
		}
		if (CPUInfo.actionType == "攻击") {
			Action_tmp ="受到"+CPUInfo.actionPoint.ToString()+"点攻击";
			PlayerAction.Add (Action_tmp);
			Player_Get_Attack += CPUInfo.actionPoint;
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
			if (Player_Skill.Name == "穿刺")
				useSkill_chuanci ("Player",PlayerInfo.actionPoint);
			if (Player_Skill .Name == "恢复")
				useSkill_huifu ("Player",PlayerInfo.actionPoint);
			if (Player_Skill .Name == "蓄力")
				useSkill_xuli ("Player",PlayerInfo.actionPoint);
			if (Player_Skill.Name == "加固")
				useSkill_jiagu ("Player",PlayerInfo.actionPoint);
			if (Player_Skill.Name == "盾击")
				useSkill_dunji ("Player",PlayerInfo.actionPoint);
			if (Player_Skill.Name == "格挡")
				useSkill_gedang ("Player",PlayerInfo.actionPoint);
			if (Player_Skill.Name == "重击")
				useSkill_zhongji ("Player",PlayerInfo.actionPoint);
			if (Player_Skill.Name == "反击")
				useSkill_fanji ("Player",PlayerInfo.actionPoint);
		}
		if (CPUInfo.actionType == "技能") {
			if (CPU_Skill.Name == "穿刺")
				useSkill_chuanci ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "恢复")
				useSkill_huifu ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "蓄力")
				useSkill_xuli ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "加固")
				useSkill_jiagu ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "盾击")
				useSkill_dunji ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "格挡")
				useSkill_gedang ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "反击")
				useSkill_fanji ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "重击")
				useSkill_zhongji ("CPU",CPUInfo.actionPoint);
			if (CPU_Skill.Name == "洗礼")
				useSkill_xili ("CPU",CPUInfo.actionPoint);
		}

		Player_Armor -= Player_Get_Attack;
		Player_Get_Attack = 0;
		Player_Get_Attack = 0;
		if (Player_Armor < 0) {
			Player_Health += Player_Armor;
			Player_Armor = 0;
		}
		CPU_Armor -= CPU_Get_Attack;
		CPU_Get_Attack = 0;
		if (CPU_Armor < 0) {
			CPU_Health += CPU_Armor;
			CPU_Armor = 0;
		}

		if(PlayerAction.Count>0)
			StartCoroutine (ShowAction("Player"));
		if(CPUAction.Count>0)
			StartCoroutine (ShowAction("CPU"));


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

		if (Player_CardObjectList.Count <= 0||Player_Health <= 0 || CPU_Health <= 0) {
			GameEnd ();
		} else {
			UseCardButton.GetComponent<Button> ().interactable = true;
			CPUCardUsedID = Random.Range (0, CPU_CardObjectList.Count-1);
			if (GameInfo.GetComponent<GameInfo> ().Cheat)
				CPU_CardObjectList [CPUCardUsedID].GetComponent<Card> ().ViewPlay ();
		}

		if (Bust_CPU_last > 0) {
			for (int i = 0; i < CPU_CardObjectList.Count; i++)
				CPU_CardObjectList [i].GetComponent<Card> ().CardInfo_this.actionPoint -= Bust_CPU_last;
			Bust_CPU_last = 0;
		}

		if (Bust_CPU > 0) {
			for (int i = 0; i < CPU_CardObjectList.Count; i++)
				CPU_CardObjectList [i].GetComponent<Card> ().CardInfo_this.actionPoint += Bust_CPU;
			Bust_CPU_last = Bust_CPU;
			Bust_CPU = 0;
		}

		if (Bust_player_last > 0) {
			for (int i = 0; i < Player_CardObjectList.Count; i++)
				Player_CardObjectList [i].GetComponent<Card> ().CardInfo_this.actionPoint -= Bust_player_last;
			Bust_player_last = 0;
		}
			
		if (Bust_player > 0) {
			for (int i = 0; i < Player_CardObjectList.Count; i++)
				Player_CardObjectList [i].GetComponent<Card> ().CardInfo_this.actionPoint += Bust_player;
			Bust_player_last = Bust_player;
			Bust_player = 0;
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

	void useSkill_xuli (string user,int point){
		string Action_tmp = "";
		Action_tmp ="获得"+point.ToString()+"点强化";
		if (user == "Player") {
			Bust_player += point;
			PlayerAction.Add (Action_tmp);
		}else{
			Bust_CPU += point;
			CPUAction.Add (Action_tmp);
		}
	}

	void useSkill_jiagu (string user,int point){
		string Action_tmp = "";

		if (user == "Player") {
			int tmp = point * 2 + Player_Armor;
			Action_tmp ="护甲+"+tmp.ToString();
			Player_Armor += tmp;
			PlayerAction.Add (Action_tmp);
		}else{
			int tmp = point * 2 + CPU_Armor;
			Action_tmp ="护甲+"+tmp.ToString();
			CPU_Armor += tmp;
			CPUAction.Add (Action_tmp);
		}
	}
	void useSkill_dunji (string user,int point){
		string Action_tmp = "";
		if (user == "Player") {
			Action_tmp ="获得"+point.ToString()+"点护甲";
			Player_Armor += point;
			PlayerAction.Add (Action_tmp);

			Action_tmp = "受到" + Player_Armor.ToString () + "点攻击";
			CPU_Get_Attack +=Player_Armor;
			CPUAction.Add (Action_tmp);
		}else{
			Action_tmp ="获得"+point.ToString()+"点护甲";
			CPU_Armor += point;
			CPUAction.Add (Action_tmp);

			Action_tmp = "受到" + CPU_Armor.ToString () + "点攻击";
			Player_Get_Attack +=CPU_Armor;
			PlayerAction.Add (Action_tmp);
		}
	}
	void useSkill_gedang (string user,int point){
		string Action_tmp = "";
		Action_tmp ="获得"+(point*2).ToString()+"点护甲";
		if (user == "Player") {
			Player_Armor += point*2;
			PlayerAction.Add (Action_tmp);
		}else{
			CPU_Armor += point*2;
			CPUAction.Add (Action_tmp);
		}
	}
	void useSkill_fanji (string user,int point){
		string Action_tmp = "";

		if (user == "Player") {
			Action_tmp ="受到"+(Player_Get_Attack+point).ToString()+"点攻击";
			CPU_Armor -= Player_Get_Attack+point;
			PlayerAction.Add (Action_tmp);
		}else{
			Action_tmp ="受到"+(CPU_Get_Attack+point).ToString()+"点攻击";
			Player_Armor -= CPU_Get_Attack+point;
			PlayerAction.Add (Action_tmp);
		}
	}
	void useSkill_zhongji (string user,int point){
		string Action_tmp = "";
		Action_tmp ="受到"+(point*2).ToString()+"点攻击";
		if (user == "Player") {
			CPU_Get_Attack += point*2;
			CPUAction.Add (Action_tmp);
		}else{
			Player_Get_Attack += point*2;
			PlayerAction.Add (Action_tmp);
		}
	}

	void useSkill_xili (string user,int point){
		string Action_tmp = "";
		if (user == "Player") {
			int health_before = Player_Health;
			int maxHealth = GameInfo.GetComponent<GameInfo> ().Player_MaxHealth;
			Player_Health += point;
			if (Player_Health > maxHealth)
				Player_Health = maxHealth;
			int tmp = Player_Health - health_before;
			Action_tmp ="回复"+tmp.ToString()+"点生命";
			PlayerAction.Add (Action_tmp);

			Action_tmp ="受到"+tmp.ToString()+"点攻击";
			CPU_Get_Attack += tmp;
			CPUAction.Add (Action_tmp);
		}else{
			int health_before = CPU_Health;
			int maxHealth = GameInfo.GetComponent<GameInfo> ().CPU_MaxHealth;
			CPU_Health += point;
			if (CPU_Health > maxHealth)
				CPU_Health = maxHealth;
			int tmp = CPU_Health - health_before;
			Action_tmp ="回复"+tmp.ToString()+"点生命";
			CPUAction.Add (Action_tmp);

			Action_tmp ="受到"+tmp.ToString()+"点攻击";
			Player_Get_Attack += tmp;
			PlayerAction.Add (Action_tmp);
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
		if (Player_Health > CPU_Health || (Player_Health == CPU_Health && Player_Armor>CPU_Armor)||(Player_Health >0 && CPU_Health<=0)) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
														+"护甲"+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"获胜！";
			Invoke ("Win", 3.0f);
		} 
		if (Player_Health < CPU_Health || (Player_Health == CPU_Health && Player_Armor<CPU_Armor)||(Player_Health <=0 && CPU_Health>0)) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
				+"护甲 "+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"败北！";

			Invoke ("Lose", 3.0f);
		} 
		if ((Player_Health == CPU_Health && Player_Armor==CPU_Armor )|| (Player_Health <=0 && CPU_Health<=0)) {
			GameResult.GetComponent<Text> ().text = "生命 "+Player_Health.ToString()+":"+CPU_Health.ToString()+"\n"
				+"护甲 "+Player_Armor.ToString()+":"+CPU_Armor.ToString()+"\n"+"平局！";

			Invoke ("Lose", 3.0f);
		} 
	}

	void Win(){
		GameInfo.GetComponent<GameInfo> ().WinGame ();
		GameResult.SetActive (false);

		GetGold.SetActive (true);
		int GoldReword = GameInfo.GetComponent<GameInfo> ().StageList[GameInfo.GetComponent<GameInfo> ().StageLevel-1].GoldReword;
		GetGold.GetComponent<Text> ().text = "获得" + GoldReword.ToString () + "金币\n请选择一张牌添加到牌库中";

		string[] t = { "攻击", "防御", "技能" };
		int point = Random.Range (1, AddCardLevel+1);
		string type = t [Random.Range (0, 3)];
		AddCardList.Add (new CardInfo (point, "剪", type));

		point = Random.Range (1, AddCardLevel+1);
		type = t [Random.Range (0, 3)];
		AddCardList.Add (new CardInfo (point, "石", type));

		point = Random.Range (1, AddCardLevel+1);
		type = t [Random.Range (0, 3)];
		AddCardList.Add (new CardInfo (point, "布", type));

		Vector3 CPUCardPosition = new Vector3 (-10, 0, 0);
		for (int i = 0; i < AddCardList.Count; i++) {
			GameObject CardObject_tmp = Instantiate (Card_Prefab, CPUCardPosition, Quaternion.identity);
			CardObject_tmp.GetComponent<Card> ().Init (AddCardList[i],true,"CPU","AddCard");
			AddCardObjectList.Add (CardObject_tmp);

			CPUCardPosition += new Vector3 (10, 0, 0);
		}
		CancelAddCardButton.SetActive (true);
	}

	void Lose(){
		GameInfo.GetComponent<GameInfo> ().LostGame ();
		GameResult.SetActive (false);

		GetGold.SetActive (true);
		int GoldReword = GameInfo.GetComponent<GameInfo> ().StageList[GameInfo.GetComponent<GameInfo> ().StageLevel-1].GoldReword;
		GetGold.GetComponent<Text> ().text = "获得" + (GoldReword/2).ToString () + "金币";

		Invoke ("BackToStage", 2.0f);
	}

	public void OnClickAddCard(GameObject c){
		int i;
		for (i = 0; i < AddCardObjectList.Count; i++)
			if (AddCardObjectList [i] == c)
				GameInfo.GetComponent<GameInfo> ().Player_CardList.Add (AddCardList [i]);
		BackToStage ();
		//Invoke ("BackToStage", 3.0f);
	}

	public void OnClickCancelAddCard(){
		BackToStage ();
	}

	void BackToStage(){
		SceneManager.LoadScene (0);
	}
}
