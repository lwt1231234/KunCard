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
	public int GoldReword,AddCardLevel,SkillUsed;

	public StageInfo(string a, string b,int c,int d,int e){
		StageType = a;
		Description = b;
		GoldReword = c;
		AddCardLevel = d;
		SkillUsed = e;
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
	public bool Cheat;

	public int StageLevel,AddCardLevel;
	public int StageNum = 14;

	public SkillInfo CPU_SKill_Used;
	public int Player_SKill_Used;
	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();
	public List<StageInfo> StageList=new List<StageInfo>();
	public List<SkillInfo> SkillList=new List<SkillInfo>();
	public List<SkillInfo> CPUSkillList=new List<SkillInfo>();

	public bool[] StageClear = new bool[14];


	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);
		Cheat = false;

		for (int i = 0; i < StageNum; i++) {
			//StageClear [i] = false;
			StageClear [i] = true;
		}
		//StageClear [0] = true;
		StageList.Add(new StageInfo("Game","初入鲲湖",100,1,0));				//1
		StageList.Add(new StageInfo("UpGrade","商店-升级卡牌",-50,1,0));		//2
		StageList.Add(new StageInfo("Game","牛刀小试",150,2,1));				//3
		StageList.Add(new StageInfo("Remove","商店-移除卡牌",-50,1,0));		//4
		StageList.Add(new StageInfo("Game","海底巨人",200,2,2));				//5
		StageList.Add(new StageInfo("Game","鲲之力-力",300,3,2));				//6
		StageList.Add(new StageInfo("Game","巨型海龟",200,2,3));				//7
		StageList.Add(new StageInfo("Game","鲲之力-甲",300,3,4));				//8
		StageList.Add(new StageInfo("Game","商店守卫",400,4,5));				//9
		StageList.Add(new StageInfo("UpGrade","商店-升级卡牌",-50,1,0));		//10
		StageList.Add(new StageInfo("Game","商店守卫",400,4,1));				//11
		StageList.Add(new StageInfo("Remove","商店-移除卡牌",-50,1,0));		//12
		StageList.Add(new StageInfo("Game","暗影牧师",400,4,6));				//13
		StageList.Add(new StageInfo("Game","鲲之力-灵",500,5,0));				//14
	
		Player_MaxHealth = 20;
		Player_SKill_Used = 1;
		Gold = 1000;
		StageLevel = 0;

		SkillList.Add (new SkillInfo("无","没有效果",0,true,true));					//0
		SkillList.Add (new SkillInfo("穿刺","造成X点无视护甲的伤害",0,true,true));	//1
		SkillList.Add (new SkillInfo("恢复","回复自身X点生命值",100,false,false));		//2
		SkillList.Add (new SkillInfo("蓄力","下一张卡牌的基础点数+X",500,false,false));		//3
		SkillList.Add (new SkillInfo("加固","护甲+X然后翻倍",500,false,false));			//4
		SkillList.Add (new SkillInfo("反击","造成X+本回合受到非穿刺伤害",500,false,false));

		CPUSkillList.Add (new SkillInfo("无","没有效果",0,true,true));					//0
		CPUSkillList.Add (new SkillInfo("穿刺","造成X点无视护甲的伤害",0,true,true));	//1
		CPUSkillList.Add (new SkillInfo("重击","造成2X点伤害",100,false,false));		//2
		CPUSkillList.Add (new SkillInfo("格挡","获得2X点护甲",100,false,false));			//3
		CPUSkillList.Add (new SkillInfo("盾击","护甲+X并造成等同于护甲的伤害",500,false,false));			//4
		CPUSkillList.Add (new SkillInfo("反击","造成X+本回合受到非穿刺伤害",500,false,false));			//5
		CPUSkillList.Add (new SkillInfo("洗礼","回复X点生命并造成回复量的伤害",500,false,false));			//6


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

		CPU_CardList.Clear ();
		CPU_SKill_Used = CPUSkillList [StageList [StageLevel - 1].SkillUsed];
		CPU_MaxHealth = 20;
		AddCardLevel = StageList [StageLevel - 1].AddCardLevel;

		switch (StageLevel) {
		case 2:
		case 4:
		case 10:
		case 12:
			break;
		case 1:
			CPU_MaxHealth = 20;
			Player_MaxHealth = 20;
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "防御"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "防御"));
			SceneManager.LoadScene (1);
			break;
		case 3:
			CPU_MaxHealth = 20;
			Player_MaxHealth = 20;
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (1, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "防御"));
			CPU_CardList.Add (new CardInfo (1, "布", "技能"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "防御"));
			CPU_CardList.Add (new CardInfo (1, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 5:
			CPU_MaxHealth = 25;
			Player_MaxHealth = 25;
			CPU_CardList.Add (new CardInfo (2, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (2, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "布", "技能"));
			CPU_CardList.Add (new CardInfo (2, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 6:
			CPU_MaxHealth = 25;
			Player_MaxHealth = 25;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (3, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (3, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "布", "技能"));
			CPU_CardList.Add (new CardInfo (3, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (3, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 7:
			CPU_MaxHealth = 25;
			Player_MaxHealth = 25;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (2, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (2, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (2, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "布", "防御"));
			CPU_CardList.Add (new CardInfo (2, "布", "技能"));
			CPU_CardList.Add (new CardInfo (2, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (2, "石", "防御"));
			CPU_CardList.Add (new CardInfo (2, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 8:
			CPU_MaxHealth = 25;
			Player_MaxHealth = 25;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (3, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (3, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (3, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (3, "布", "防御"));
			CPU_CardList.Add (new CardInfo (3, "布", "防御"));
			CPU_CardList.Add (new CardInfo (3, "布", "技能"));
			CPU_CardList.Add (new CardInfo (3, "石", "防御"));
			CPU_CardList.Add (new CardInfo (3, "石", "防御"));
			CPU_CardList.Add (new CardInfo (3, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 9:
			CPU_MaxHealth = 30;
			Player_MaxHealth = 30;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (4, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (4, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (4, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "布", "防御"));
			CPU_CardList.Add (new CardInfo (4, "布", "技能"));
			CPU_CardList.Add (new CardInfo (4, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "石", "防御"));
			CPU_CardList.Add (new CardInfo (4, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 11:
			CPU_MaxHealth = 30;
			Player_MaxHealth = 30;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (4, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (4, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (4, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "布", "防御"));
			CPU_CardList.Add (new CardInfo (4, "布", "技能"));
			CPU_CardList.Add (new CardInfo (4, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "石", "防御"));
			CPU_CardList.Add (new CardInfo (4, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 13:
			CPU_MaxHealth = 30;
			Player_MaxHealth = 30;
			CPU_CardList.Clear ();
			CPU_CardList.Add (new CardInfo (4, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (4, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (4, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "布", "技能"));
			CPU_CardList.Add (new CardInfo (4, "布", "技能"));
			CPU_CardList.Add (new CardInfo (4, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (4, "石", "技能"));
			CPU_CardList.Add (new CardInfo (4, "石", "技能"));
			SceneManager.LoadScene (1);
			break;
		case 14:
			CPU_MaxHealth = 40;
			Player_MaxHealth = 40;
			for (int i = 0; i < Player_CardList.Count; i++)
				CPU_CardList.Add (new CardInfo (Player_CardList [i].actionPoint + 1, Player_CardList [i].actionColor, Player_CardList [i].actionType));
			CPU_SKill_Used = SkillList [Player_SKill_Used];
			SceneManager.LoadScene (1);
			break;
		default:
			CPU_CardList.Add (new CardInfo (1, "剪", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "剪", "防御"));
			CPU_CardList.Add (new CardInfo (1, "剪", "技能"));
			CPU_CardList.Add (new CardInfo (1, "布", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "布", "防御"));
			CPU_CardList.Add (new CardInfo (1, "布", "技能"));
			CPU_CardList.Add (new CardInfo (1, "石", "攻击"));
			CPU_CardList.Add (new CardInfo (1, "石", "防御"));
			CPU_CardList.Add (new CardInfo (1, "石", "技能"));
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
	public void LostGame(){
		if (StageLevel <= 0)
			return;
		else {
			if(StageList [StageLevel - 1].GoldReword>0)
				Gold += StageList [StageLevel - 1].GoldReword/2;
		}
	}
		
	public void Shooping(){
		if (StageLevel <= 0)
			return;
		else {
			if (StageList [StageLevel - 1].GoldReword < 0) {
				Gold += StageList [StageLevel - 1].GoldReword;
				StageList [StageLevel - 1].GoldReword-=10;
			}
		}
	}
}
