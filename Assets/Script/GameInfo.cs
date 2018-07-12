using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour {

	public int Player_MaxHealth;
	public List<Card> Player_CardList=new List<Card>();

	public int LevelStage;

	void Start () {
		GameObject.DontDestroyOnLoad(gameObject);
		Debug.Log("DontDestroyOnLoad");

		Player_MaxHealth = 20;
		LevelStage = 1;
	}

	// Update is called once per frame
	void Update () {

	}
}
