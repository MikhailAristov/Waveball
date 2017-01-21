using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawn : MonoBehaviour {

	public bool spawnActive = true;

	// Use this for initialization
	void Start () {
		//StartCoroutine(keepSpawningParticles());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator keepSpawningParticles() {
		while(spawnActive) {
			GameObject prefabParticle = Resources.Load("Particle") as GameObject;
			Vector3 spawnPos = transform.position + transform.up * 0.7f;
			GameObject particle = Instantiate(prefabParticle, spawnPos, Quaternion.identity);
			particle.GetComponent<Rigidbody>().velocity = transform.up * 5.0f;
			// Then wait
			yield return new WaitForSeconds(2.0f);
		}
	}
}
