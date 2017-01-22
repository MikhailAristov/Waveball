using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour {

	private PanelFogState state;

	private Color[] colorPalette;


	public PanelFogState State {
		get { return state; }
		set { 
			var renderer = gameObject.GetComponentInChildren<Renderer> ();
			//Debug.Log (gameObject);
			switch (value) {
			case PanelFogState.Undiscovered:
				renderer.material.color = colorPalette [0];
				break;
			case PanelFogState.Discovered:
				renderer.material.color = colorPalette [1];
				break;
			case PanelFogState.InSight:
				renderer.material.color = colorPalette [2];
				break;
			}
			state = value;
		}
		 
	}

	void Start () {
		colorPalette = new Color[3];
		colorPalette [0] = Color.black;
		colorPalette [1] = Color.gray;
		colorPalette [2] = Color.blue;

		State = PanelFogState.Undiscovered;
	}
	
	void Update () {
		
	}
}
