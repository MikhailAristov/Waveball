using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour {

	private PanelFogState fogState;
	private PanelForceActionState forceActionState;

	private Color[] colorPalette;
	private Texture2D[] texturePalette;

	public Transform Model;
	public Renderer ModelRenderer;


	public PanelForceActionState ActionState {
		get { return forceActionState; }
		set { 
			var renderer = gameObject.GetComponentInChildren<Renderer> ();
			switch (value) {
			case PanelForceActionState.None:
				//renderer.material.shader = Shader.Find ( "Unlit/Transparent" );
				renderer.material.color = Color.white;
				renderer.material.mainTexture = texturePalette[0];
				break;
			case PanelForceActionState.Pulse:
				//renderer.material.color = Color.clear;
				renderer.material.mainTexture = texturePalette[1];
				break;
			case PanelForceActionState.Oscillator:
				Debug.Log ("oscil set");
				renderer.material.color = Color.white;
				renderer.material.mainTexture = texturePalette[2];
				break;
			}
			forceActionState = value;
		} 
	}

	public PanelFogState FogState {
		get { return fogState; }
		set { 
			var renderer = gameObject.GetComponentInChildren<Renderer> ();
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
			fogState = value;
		} 
	}

	private void Awake()
	{
		if(Model == null)
			Model = transform.Find ("Model");
		ModelRenderer = Model.GetComponent<Renderer> ();
	}

	void Start () {
		colorPalette = new Color[3];
		colorPalette [0] = Color.black;
		colorPalette [1] = Color.white;
		colorPalette [2] = Color.blue;
		FogState = PanelFogState.Undiscovered;


		texturePalette = new Texture2D[3];
		texturePalette[0] = Resources.Load ("PanelTextures/default") as Texture2D;
		texturePalette[1] = Resources.Load ("PanelTextures/pulse") as Texture2D;
		texturePalette[2] = Resources.Load ("PanelTextures/oscillator") as Texture2D;
		ActionState = PanelForceActionState.None;

	}
	
	//void Update () {
	//}
}
