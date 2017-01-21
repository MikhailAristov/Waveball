using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawn : MonoBehaviour {

	public bool spawnActive = true;

	public GameObject surface;

	// Use this for initialization
	void Start () {
		StartCoroutine(keepSpawningParticles());
	}

	private IEnumerator keepSpawningParticles() {
		while(spawnActive) {
			GameObject prefabParticle = Resources.Load("Particle") as GameObject;
			Vector3 spawnPos = transform.position + transform.up * 0.7f;
			GameObject particle = Instantiate(prefabParticle, spawnPos, Quaternion.identity);
			particle.GetComponent<Rigidbody>().velocity = transform.up * 5.0f;
			particle.GetComponent<ControlParticle>().surface = this.surface;
			// Then wait
			yield return new WaitForSeconds(2.0f);
		}
	}
}
