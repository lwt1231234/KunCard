using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour {

	public int Player_MaxHealth,Gold;

	public int StageLevel;
	public int StageNum = 3;

	public string Player_SKill_Used,CPU_SKill_Used;
	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();

	public bool[] StageClear = new bool[10];


	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);

		for (int i = 0; i < 10; i++) {
			StageClear [i] = false;
		}

		Player_MaxHealth = 20;
		Gold = 100;
		StageLevel = 0;

		Player_SKill_Used = "穿刺";

		Player_CardList.Add (new CardInfo(1,"剪","攻击"));
		Player_CardList.Add (new CardInfo(1,"剪","防御"));
		Player_CardList.Add (new CardInfo(1,"剪","技能"));
		Player_CardList.Add (new CardInfo(1,"布","攻击"));
		Player_CardList.Add (new CardInfo(1,"布","防御"));
		Player_CardList.Add (new CardInfo(1,"布","技能"));
		Player_CardList.Add (new CardInfo(1,"石","攻击"));
		Player_CardList.Add (new CardInfo(1,"石","防御"));
		Player_CardList.Add (new CardInfo(1,"石","技能"));

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

			CPU_SKill_Used = "穿刺";
			SceneManager.LoadScene (1);
		}



	}

	public void WinGame(){
		if (StageLevel <= 0)
			return;
		else
			StageClear [StageLevel - 1] = true;
	}
}
