using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawn : MonoBehaviour {

	public bool spawnActive = true;

	public GameObject surface;
	public GameObject particle;

	// Use this for initialization
	void Start () {
		StartCoroutine(keepSpawningParticles());
	}

	private IEnumerator keepSpawningParticles() {
		while(spawnActive) {
			GameObject prefabParticle = Resources.Load("Particle") as GameObject;
			Vector3 spawnPos = transform.position + transform.up * 0.7f;
			particle = Instantiate(prefabParticle, spawnPos, Quaternion.identity);
			particle.GetComponent<Rigidbody>().velocity = transform.up * 2.0f;
			particle.GetComponent<ControlParticle>().surface = this.surface;

			//particle.transform.parent = particlePool.transform;
			// Then wait
			yield return new WaitForSeconds(2.0f);
		}
	}
}
