using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticle : MonoBehaviour {

	// TODO: Objecte poolen

	private Rigidbody myRigidBody;

	public float minimalVelocityBeforeDeath = 0.1f;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(myRigidBody.velocity.magnitude < minimalVelocityBeforeDeath) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Finish")) {
			Debug.LogError("YOU HAVE WON");
			Destroy(gameObject);
		}
	}
}
