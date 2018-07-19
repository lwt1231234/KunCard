using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoManager_Opening : MonoBehaviour {

	public GameObject[] Text;
	public int TextNum;
	// Use this for initialization
	void Start () {
		float []time_es =new float[TextNum];
		time_es [0] = 1f;
		time_es [1] = 4f;
		time_es [2] = 3f;
		time_es [3] = 2f;
		time_es [4] = 2f;
		time_es [5] = 2f;
		time_es [6] = 4f;
		time_es [7] = 4f;
		time_es [8] = 4f;

		float time_tmp = 0;
		for (int i = 0; i < TextNum-1; i++) {
			//Text [i].SetActive (false);
			time_tmp += time_es [i];
			StartCoroutine (WakeUp (i,time_tmp));
		}
		time_tmp += time_es [8];
		StartCoroutine (Sleep (time_tmp));
		StartCoroutine (WakeUp (TextNum-1,time_tmp+2));

		Invoke ("Jump", time_tmp + 2 + 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator WakeUp(int TextID,float Time_to_Wake){
		yield return new WaitForSeconds (Time_to_Wake);
		Text [TextID].GetComponent<VideoText> ().wakeup = true;
	}

	IEnumerator Sleep(float Time_to_Wake){
		yield return new WaitForSeconds (Time_to_Wake);
		for (int i = 0; i < TextNum - 1; i++) {
			Text [i].GetComponent<VideoText> ().wakeup = false;
		}
	}

	public void Jump(){
		SceneManager.LoadScene (3);
	}
}
