using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject prefabParticle = Resources.Load("Particle") as GameObject;
		Vector3 spawnPos = transform.position + transform.up * 0.6f;
		GameObject particle = Instantiate(prefabParticle, spawnPos, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
