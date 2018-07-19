using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoText : MonoBehaviour {

	public bool wakeup;

	// Use this for initialization
	void Start () {
		Color a = new Color (255 / 255f, 255 / 255f, 255 / 255f, 0f);
		gameObject.GetComponent<Text> ().color = a;
		wakeup = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (wakeup) {
			Color a = gameObject.GetComponent<Text> ().color;
			a.a += 1f * Time.deltaTime;
			if (a.a > 1)
				a.a = 1;
			gameObject.GetComponent<Text> ().color = a;
		} else {
			Color a = gameObject.GetComponent<Text> ().color;
			a.a -= 1f*Time.deltaTime;
			if (a.a < 0)
				a.a = 0;
			gameObject.GetComponent<Text> ().color = a;
		}
			
	}
}
