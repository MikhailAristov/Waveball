using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlGoal : MonoBehaviour {

	public string nextLevelName;
	public int hitCounter;
	public int targetHitCount = 1;

	// Use this for initialization
	void Start () {
		hitCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(hitCounter >= targetHitCount || Input.GetButton("NextLevel")) {
			hitCounter = 0;
			SceneManager.LoadScene( nextLevelName, LoadSceneMode.Single );
			SceneManager.LoadScene ( "IngameMenu", LoadSceneMode.Additive );
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if ( other.CompareTag ( "Player" ) )
		{
			hitCounter += 1;
		}
	}
}
