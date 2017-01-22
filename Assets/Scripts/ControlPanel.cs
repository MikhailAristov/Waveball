using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour {

	private PanelFogState fogState;
	private PanelForceActionState forceActionState;

	public ColorPalette colorPalette;
	private Texture2D[] texturePalette;

	public Transform Model;
	public Renderer ModelRenderer;

	private float alpha;
	private float alphaTaget;

	public PanelForceActionState ActionState {
		get { return forceActionState; }
		set { 
			var renderer = ModelRenderer;
			switch (value) {
			case PanelForceActionState.None:
				//renderer.material.shader = Shader.Find ( "Unlit/Transparent" );
				//renderer.material.color = Color.white;
				renderer.material.mainTexture = texturePalette[0];
				break;
			case PanelForceActionState.Pulse:
				//renderer.material.color = Color.clear;
				renderer.material.mainTexture = texturePalette[1];
				break;
			case PanelForceActionState.Oscillator:
				//renderer.material.color = Color.white;
				renderer.material.mainTexture = texturePalette[2];
				break;
			}
			forceActionState = value;
		} 
	}

	public PanelFogState FogState {
		get { return fogState; }
		set { 
			var renderer = ModelRenderer;
			switch (value) {
			case PanelFogState.Undiscovered:
				renderer.material.color = Color.clear;
					alphaTaget = 0;
				break;
			case PanelFogState.Discovered:
				renderer.material.color = colorPalette.Palette[0];
					alphaTaget = 1;
					break;
			case PanelFogState.InSight:
				renderer.material.color = colorPalette.Palette[2];
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
		FogState = PanelFogState.Undiscovered;


		texturePalette = new Texture2D[3];
		texturePalette[0] = Resources.Load ("PanelTextures/default") as Texture2D;
		texturePalette[1] = Resources.Load ("PanelTextures/pulse") as Texture2D;
		texturePalette[2] = Resources.Load ("PanelTextures/oscillator") as Texture2D;
		ActionState = PanelForceActionState.None;

	}

	public void SetColor(Color color)
	{

	}

	void Update()
	{
		var c = ModelRenderer.material.color;
		alpha = Mathf.Lerp( alpha, alphaTaget, 3f *Time.deltaTime);
		c.a = alpha;
		ModelRenderer.material.color = c;
	}
}
