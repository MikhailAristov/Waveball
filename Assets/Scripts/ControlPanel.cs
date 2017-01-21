using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour {

	private PanelFogState state;

	public PanelFogState State {
		get { return state; }
		set { 
			var renderer = gameObject.GetComponentInChildren<Renderer> ();
			//Debug.Log (gameObject);
			switch (value) {
			case PanelFogState.Undiscovered:
				renderer.material.shader = Shader.Find ( "UI/Default" );
				break;
			case PanelFogState.Discovered:
				renderer.material.shader = Shader.Find ( "Unlit/Transparent" );
				break;
			case PanelFogState.InSight:
				renderer.material.shader = Shader.Find ( "Standard" );
				break;
			}
			state = value;
		}
		 
	}

	void Start () {
		//State = PanelFogState.Undiscovered;
	}
	
	void Update () {
		
	}
}
