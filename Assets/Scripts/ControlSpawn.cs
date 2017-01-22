using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSpawn : MonoBehaviour {

	public bool spawnActive = true;

	public GameObject surface;
	public GameObject jukebox;
	public ControlParticle particle;
	public GameObject particlePrefab;
	public ColorPalette colorPalette;

	// Use this for initialization
	void Start () {
		StartCoroutine(keepSpawningParticles());
	}

	private IEnumerator keepSpawningParticles() {
		while(spawnActive) {
			if(GameObject.FindGameObjectsWithTag("Player").Length <= 0) {

				Vector3 spawnPos = transform.position + transform.up * 0.7f;
				var instantiate = Instantiate( particlePrefab, spawnPos, Quaternion.identity);
				instantiate.GetComponent<Rigidbody>().velocity = transform.up * 2.0f;
				this.particle = instantiate.GetComponent<ControlParticle> ();
				this.particle.surface = this.surface;
				this.particle.jukebox = this.jukebox;
				this.particle.SetColor ( colorPalette.Palette[2] );
			}
			// Then wait
			yield return new WaitForSeconds(1.0f);
		}
	}
}
