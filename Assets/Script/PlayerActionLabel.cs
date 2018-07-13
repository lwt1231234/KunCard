using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionLabel : MonoBehaviour {
	int MoveSpeed;
	bool move;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (move==true) {
			gameObject.transform.localPosition += new Vector3 (0, MoveSpeed * Time.deltaTime, 0);
		}
	}

	public void Init(string s,int direction){
		gameObject.GetComponent<Text> ().text = s;
		MoveSpeed = 100*direction;
		move = true;

		Invoke ("TimeOut", 2.0f);
	}

	void TimeOut(){
		Destroy (gameObject);
	}
}
