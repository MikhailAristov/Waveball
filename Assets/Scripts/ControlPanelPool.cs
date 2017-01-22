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
		var panels = gameObject.GetComponentsInChildren<ControlPanel> ();


		int sumDiscovered = 0;
		foreach (var panel in panels) {
			if (panel.State == PanelFogState.Discovered || panel.State == PanelFogState.InSight) {
				sumDiscovered++;
			}
		}

		DiscoveryPercentage = (float) sumDiscovered / panels.Length;
		Debug.Log (DiscoveryPercentage);
	}
}
