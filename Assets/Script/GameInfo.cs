using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class StageInfo{
	public string StageType,Description;
	public int GoldReword;

	public StageInfo(string a, string b,int c){
		StageType = a;
		Description = b;
		GoldReword = c;
	}
}

public class SkillInfo{
	public string Name,Description;
	public int GoldRequire;
	public bool Unlocked,Used;

	public SkillInfo(string a, string b,int c,bool d,bool e){
		Name = a;
		Description = b;
		GoldRequire = c;
		Unlocked = d;
		Used = e;
	}
}

public class GameInfo : MonoBehaviour {

	public int Player_MaxHealth,Gold;
	public int CPU_MaxHealth;

	public int StageLevel;
	public int StageNum = 3;

	public int Player_SKill_Used,CPU_SKill_Used;
	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();
	public List<StageInfo> StageList=new List<StageInfo>();
	public List<SkillInfo> SkillList=new List<SkillInfo>();

	public bool[] StageClear = new bool[10];


	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);

		for (int i = 0; i < 10; i++) {
			StageClear [i] = false;
		}
		StageClear [0] = true;
		StageList.Add(new StageInfo("Game","起点",100));
		StageList.Add(new StageInfo("UpGrade","商店-升级卡牌",0));
		StageList.Add(new StageInfo("Remove","商店-移除卡牌",0));

		Player_MaxHealth = 20;
		Gold = 200;
		StageLevel = 0;

		SkillList.Add (new SkillInfo("穿刺","对敌人造成无视护甲的伤害",0,true,true));	//1
		SkillList.Add (new SkillInfo("恢复","回复自身生命值",100,false,false));		//2

		Player_CardList.Add (new CardInfo(1,"剪","攻击"));
		Player_CardList.Add (new CardInfo(1,"剪","防御"));
		Player_CardList.Add (new CardInfo(1,"剪","技能"));
		Player_CardList.Add (new CardInfo(1,"布","攻击"));
		Player_CardList.Add (new CardInfo(1,"布","防御"));
		Player_CardList.Add (new CardInfo(1,"布","技能"));
		Player_CardList.Add (new CardInfo(1,"石","攻击"));
		Player_CardList.Add (new CardInfo(1,"石","防御"));
		Player_CardList.Add (new CardInfo(1,"石","技能"));
	}

	public void LoadStage (int stage){
		StageLevel = stage;
		if (StageLevel <= 0)
			return;
		if (StageLevel == 1) {
			CPU_CardList.Clear();
			CPU_CardList.Add (new CardInfo(1,"剪","攻击"));
			CPU_CardList.Add (new CardInfo(1,"剪","攻击"));
			CPU_CardList.Add (new CardInfo(1,"剪","防御"));
			CPU_CardList.Add (new CardInfo(1,"布","攻击"));
			CPU_CardList.Add (new CardInfo(1,"布","攻击"));
			CPU_CardList.Add (new CardInfo(1,"布","防御"));
			CPU_CardList.Add (new CardInfo(1,"石","攻击"));
			CPU_CardList.Add (new CardInfo(1,"石","攻击"));
			CPU_CardList.Add (new CardInfo(1,"石","防御"));

			CPU_SKill_Used = 1;
			CPU_MaxHealth = 20;
			SceneManager.LoadScene (1);
		}



	}

	public void WinGame(){
		if (StageLevel <= 0)
			return;
		else {
			StageClear [StageLevel - 1] = true;
			Gold += StageList [StageLevel - 1].GoldReword;
		}
		
	}
}
