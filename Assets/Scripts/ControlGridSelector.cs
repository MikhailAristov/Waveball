using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGridSelector : MonoBehaviour {

	private float leftMouseButtonDown;
	private float rightMouseButtonDown;

	const float MAX_CHARGE_TIME = 2f;
	const float CHARGE_MULTIPLIER = 10f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			leftMouseButtonDown = Time.timeSinceLevelLoad;
		}
		if(Input.GetMouseButtonDown(1)) {
			rightMouseButtonDown = Time.timeSinceLevelLoad;
		}

		if(Input.GetMouseButtonUp(0)) {
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit myHit;
			if(Physics.Raycast(mouseRay, out myHit, 20f) && myHit.collider.CompareTag("Panel")) {
				float force = 10f + Mathf.Min(Time.timeSinceLevelLoad - leftMouseButtonDown, MAX_CHARGE_TIME) * CHARGE_MULTIPLIER;
				GetComponent<ControlSurface>().setPulseAtPosition(myHit.collider.transform.localPosition, force);
			}
		} else if(Input.GetMouseButtonUp(1)) {
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit myHit;
			if(Physics.Raycast(mouseRay, out myHit, 20f) && myHit.collider.CompareTag("Panel")) {
				float amplitude = 0.5f + Mathf.Min(Time.timeSinceLevelLoad - rightMouseButtonDown, MAX_CHARGE_TIME) / MAX_CHARGE_TIME;
				GetComponent<ControlSurface>().toggleOscillatorAtPosition(myHit.collider.transform.localPosition, amplitude);
			}
		}
	}
}
