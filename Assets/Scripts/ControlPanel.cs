using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour {

	public PanelFogState State{ get; set;}

	void Start () {
		State = PanelFogState.Undiscovered;
	}
	
	void Update () {
		
	}
}
