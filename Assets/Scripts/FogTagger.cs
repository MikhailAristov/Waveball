using UnityEngine;
using System.Collections.Generic;

public class FogTagger: MonoBehaviour {

	public const float IN_SIGHT_RANGE = 2.5f;

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
		GameObject particle = controlSpawn.particle;
		Vector3 particleGridCoord = controlSurface.worldPosToGrid (particle.transform.position.x, particle.transform.position.z);

		// update previous visibility
		for (int x = 0; x < panels.GetLength (0); x++) {
			for (int z = 0; z < panels.GetLength (1); z++) {
				GameObject panel = panels [x, z];
				if (panel.GetComponentInParent<ControlPanel> ().State == PanelFogState.InSight) {
					panel.GetComponentInParent<ControlPanel> ().State = PanelFogState.Discovered;
				}

				if (isInRange (particleGridCoord, new Vector3 (x, 0, z))) {
					panel.GetComponentInParent<ControlPanel> ().State = PanelFogState.InSight;
				}
			}
		}
	}
	
	bool isInRange(Vector3 a, Vector3 b) {
		//return Mathf.Abs (a.x - b.x) < 3 && Mathf.Abs (a.z - b.z) < 3;
		return (a - b).magnitude < IN_SIGHT_RANGE;
	}

}