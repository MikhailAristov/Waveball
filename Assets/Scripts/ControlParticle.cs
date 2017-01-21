using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticle : MonoBehaviour {

	// TODO: Objecte poolen

	private Rigidbody myRigidBody;

	public float minimalVelocityBeforeDeath = 0.1f;

	public GameObject surface;
	private ControlSurface surfaceControl;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody>();
		surfaceControl = surface.GetComponent<ControlSurface>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(myRigidBody.velocity.magnitude < minimalVelocityBeforeDeath) {
			Destroy(gameObject);
			return;
		}

		Vector3 currentGradient = surfaceControl.getGradientAtPosition(transform.position);
		//Debug.Log(currentGradient);
		myRigidBody.AddForce(currentGradient * 1000);
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Finish")) {
			Debug.LogError("YOU HAVE WON");
			Destroy(gameObject);
		}
	}
}
