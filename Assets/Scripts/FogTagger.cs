using UnityEngine;
using System.Collections;

public class FogTagger: MonoBehaviour {

	GameObject[,] panels;


	void Start() {
		panels = GetComponent<ControlSurface> ().gridPanels;
		foreach (var panel in panels) {
			panel.tag = "Undiscovered";
		}
	}


	void Update() {
		//GameObject particle = GetComponent<GameObject> ("Particle(Clone)");
	}
}