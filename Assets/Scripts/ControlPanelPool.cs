using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelPool : MonoBehaviour {


	public float DiscoveryPercentage { get; set;}

	// Use this for initialization
	void Start () {		
	}
	
	// Update is called once per frame
	void Update () {


	}

	private void updateDiscoveryPercentage() {
		var panels = gameObject.GetComponentsInChildren<ControlPanel> ();


		int sumDiscovered = 0;
		foreach (var panel in panels) {
			if (panel.FogState == PanelFogState.Discovered || panel.FogState == PanelFogState.InSight) {
				sumDiscovered++;
			}
		}
		DiscoveryPercentage = (float) sumDiscovered / panels.Length;
		Debug.Log (DiscoveryPercentage);
	}
}
