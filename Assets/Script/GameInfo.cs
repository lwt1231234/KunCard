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

	public int Comparer(CardInfo y){
		if (actionColor != y.actionColor) {
			if (actionColor == "剪")
				return -1;
			if (y.actionColor == "剪")
				return 1;
			if (actionColor == "石")
				return -1;
			return 1;
		}
		else{
			if(actionType!=y.actionType){
				if (actionType == "攻击")
					return -1;
				if (y.actionType == "攻击")
					return 1;
				if (actionType == "防御")
					return -1;
				return 1;
			}
			else{
				if(actionPoint>=y.actionPoint)
					return -1;
				else
					return 1;
			}
		}
	}
}

public class StageInfo{
	public string StageType,Description;
	public int GoldReword,AddCardLevel;

	public StageInfo(string a, string b,int c,int d){
		StageType = a;
		Description = b;
		GoldReword = c;
		AddCardLevel = d;
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

	public int StageLevel,AddCardLevel;
	public int StageNum = 14;

	public int Player_SKill_Used,CPU_SKill_Used;
	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();
	public List<StageInfo> StageList=new List<StageInfo>();
	public List<SkillInfo> SkillList=new List<SkillInfo>();

	public bool[] StageClear = new bool[14];


	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);

		for (int i = 0; i < 14; i++) {
			StageClear [i] = false;
		}
		StageClear [0] = true;
		StageList.Add(new StageInfo("Game","初入鲲湖",100,1));//1
		StageList.Add(new StageInfo("UpGrade","商店-升级卡牌",-100,1));//2
		StageList.Add(new StageInfo("Game","牛刀小试",100,1));//3
		StageList.Add(new StageInfo("Remove","商店-移除卡牌",-100,1));//4
		StageList.Add(new StageInfo("Game","？？？",100,1));//5
		StageList.Add(new StageInfo("Game","鲲之力-1",100,1));//6
		StageList.Add(new StageInfo("Game","？？？",100,1));//7
		StageList.Add(new StageInfo("Game","鲲之",100,1));//8
		StageList.Add(new StageInfo("Game","？？？",100,1));//9
		StageList.Add(new StageInfo("UpGrade","商店-升级卡牌",-100,1));//10
		StageList.Add(new StageInfo("Game","？？？",100,1));//11
		StageList.Add(new StageInfo("Remove","商店-移除卡牌",-100,1));//12
		StageList.Add(new StageInfo("Game","？？？",100,1));//13
		StageList.Add(new StageInfo("Game","鲲之力-3",100,1));//14
	
		Player_MaxHealth = 20;
		Player_SKill_Used = 1;
		Gold = 200;
		StageLevel = 0;

		SkillList.Add (new SkillInfo("无","没有效果",0,true,true));	//0
		SkillList.Add (new SkillInfo("穿刺","对敌人造成无视护甲的伤害",0,true,true));	//1
		SkillList.Add (new SkillInfo("恢复","回复自身生命值",100,false,false));		//2

		Player_CardList.Add (new CardInfo(1,"剪","攻击"));
		Player_CardList.Add (new CardInfo(1,"石","防御"));
		Player_CardList.Add (new CardInfo(1,"剪","技能"));
		Player_CardList.Add (new CardInfo(1,"布","防御"));
		Player_CardList.Add (new CardInfo(1,"石","攻击"));
	
		Player_CardList.Add (new CardInfo(1,"石","技能"));
		Player_CardList.Add (new CardInfo(1,"剪","防御"));
		Player_CardList.Add (new CardInfo(1,"布","攻击"));
	
		Player_CardList.Add (new CardInfo(1,"布","技能"));
	}

	public void LoadStage (int stage){
		StageLevel = stage;
		if (StageLevel <= 0)
			return;
		switch (StageLevel) {
		case 1:
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "防御"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "防御"));

			CPU_SKill_Used = 0;
			CPU_MaxHealth = 20;
			AddCardLevel = StageList [StageLevel - 1].AddCardLevel;
			SceneManager.LoadScene (1);
			break;
		case 2:
		case 4:
		case 10:
		case 12:
			break;
		default:
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (1, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "防御"));
			CPU_CardList.Add (new CardInfo (1, "布", "技能"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "防御"));
			CPU_CardList.Add (new CardInfo (1, "石", "技能"));

			CPU_SKill_Used = 1;
			CPU_MaxHealth = 20;
			AddCardLevel = StageList [StageLevel - 1].AddCardLevel;
			SceneManager.LoadScene (1);
			break;
		}
	}

	public void WinGame(){
		if (StageLevel <= 0)
			return;
		else {
			StageClear [StageLevel - 1] = true;
			if(StageList [StageLevel - 1].GoldReword>0)
				Gold += StageList [StageLevel - 1].GoldReword;
		}
	}

	public void Shooping(){
		if (StageLevel <= 0)
			return;
		else {
			if (StageList [StageLevel - 1].GoldReword < 0) {
				Gold += StageList [StageLevel - 1].GoldReword;
				StageList [StageLevel - 1].GoldReword-=50;
			}
		}
	}
}
