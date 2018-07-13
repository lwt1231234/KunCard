using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour {

	public int Player_MaxHealth;

	public int LevelStage;

	public string Player_SKill_Used,CPU_SKill_Used;
	public List<string> StageSkillList=new List<string>();
	public List<CardInfo> Player_CardList=new List<CardInfo>();
	public List<CardInfo> CPU_CardList=new List<CardInfo>();


	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);
		Debug.Log("DontDestroyOnLoad");

		StageSkillList.Add ("穿刺");

		Player_MaxHealth = 20;
		LevelStage = 1;

		Player_SKill_Used = "穿刺";
		CPU_SKill_Used = StageSkillList [LevelStage - 1];

		Player_CardList.Add (new CardInfo(1,"剪","攻击"));
		Player_CardList.Add (new CardInfo(1,"剪","防御"));
		Player_CardList.Add (new CardInfo(1,"剪","技能"));
		Player_CardList.Add (new CardInfo(1,"布","攻击"));
		Player_CardList.Add (new CardInfo(1,"布","防御"));
		Player_CardList.Add (new CardInfo(1,"布","技能"));
		Player_CardList.Add (new CardInfo(1,"石","攻击"));
		Player_CardList.Add (new CardInfo(1,"石","防御"));
		Player_CardList.Add (new CardInfo(1,"石","技能"));


		if (LevelStage == 1) {
			CPU_CardList.Add (new CardInfo(1,"剪","攻击"));
			CPU_CardList.Add (new CardInfo(1,"剪","攻击"));
			CPU_CardList.Add (new CardInfo(1,"剪","防御"));
			CPU_CardList.Add (new CardInfo(1,"布","攻击"));
			CPU_CardList.Add (new CardInfo(1,"布","攻击"));
			CPU_CardList.Add (new CardInfo(1,"布","防御"));
			CPU_CardList.Add (new CardInfo(1,"石","攻击"));
			CPU_CardList.Add (new CardInfo(1,"石","攻击"));
			CPU_CardList.Add (new CardInfo(1,"石","防御"));
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
