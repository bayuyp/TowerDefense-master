using UnityEngine;
using System.Collections;

public class OnClickButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnChange(int index){
		if(index == 0){
			Application.Quit();
		}else {
			Application.LoadLevel(index);
		}
	}

	public void LoadLevel(int index){
		//load level tergantung index
		//GameManager.Instance.FinalRoundFinished;
		Application.LoadLevel(index+1);
	}
}
