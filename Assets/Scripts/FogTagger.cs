using UnityEngine;
using System.Collections.Generic;

public class FogTagger: MonoBehaviour {

	public ControlSurface controlSurface;
	public GameObject[,] panels;

	public ControlSpawn controlSpawn;


	void Start() {
//		Debug.Log (controlSurface);
//		foreach (var panel in panels) {
//			panel.tag = "Undiscovered";
//		}

	}


	void Update() {
		panels = controlSurface.gridPanels;

//		Debug.Log (controlSpawn.particlePool.Count);
//		for (int i = 0; i < controlSpawn.particlePool.Count; i++) {
//			Debug.Log (controlSpawn.particlePool [i]);
//		}
//		foreach (var particle in controlSpawn.particlePool) {
//			Debug.Log(particle);
//		}

		GameObject particle = controlSpawn.particle;
		Debug.Log (particle);
		Vector3 particleGridCoord = controlSurface.worldPosToGrid (particle.transform.position.x, particle.transform.position.z);

		// update previous visibility
		for (int x = 0; x < panels.GetLength(0); x++) {
			for (int z = 0; z < panels.GetLength(1); z++) {
				GameObject panel = panels [x, z];
				if (panel.GetComponentInParent<ControlPanel> ().State == PanelFogState.InSight) {
					panel.GetComponentInParent<ControlPanel> ().State = PanelFogState.Discovered;
				}
//				Debug.Log (particleGridCoord);
//				Debug.Log (new Vector3(x, 0, z));
//				Debug.Log ("-------");


				if (isInRange(particleGridCoord, new Vector3(x, 0, z))) {
					panel.GetComponentInParent<ControlPanel> ().State = PanelFogState.InSight;
				}
			}
		}
	
		/*
		GameObject particle = controlSpawn.particle;
		Vector3 gridCoord = controlSurface.worldPosToGrid (particle.transform.position.x, particle.transform.position.z);

		int range = 2;
		Debug.Log("Particle");
		for (int x = (int) gridCoord.x - range; x < (int) gridCoord.x + range; x++) {
			for (int z = (int) gridCoord.z - range; z < (int) gridCoord.z + range; z++) {
				if (x < 0 || x > panels.GetLength (0) || z < 0 || z > panels.GetLength (1)) {
					continue;
				}
				panels [x, z].tag = "InSight";
			}
		}
		*/
	}
	

	bool isInRange(Vector3 a, Vector3 b) {
		return Mathf.Abs (a.x - b.x) < 3 && Mathf.Abs (a.z - b.z) < 3;
	}

}