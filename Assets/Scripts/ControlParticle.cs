using System;
using UnityEngine;

public class ControlParticle : MonoBehaviour
{
	// TODO: Objecte poolen

	private Rigidbody myRigidBody;

	public float minimalVelocityBeforeDeath = 0.001f;
	public float gradientForceMultiplier = 100f;

	public GameObject surface;
	private ControlSurface surfaceControl;
	public GameObject jukebox;
	private ControlJukebox jukeboxControl;

	// Use this for initialization
	void Start()
	{
		myRigidBody = GetComponent<Rigidbody> ();
		surfaceControl = surface.GetComponent<ControlSurface> ();
		jukeboxControl = jukebox.GetComponent<ControlJukebox> ();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 currentGradient = surfaceControl.getGradientAtPosition ( transform.position );
		if(currentGradient.sqrMagnitude > 0) {
			myRigidBody.AddForce ( currentGradient * gradientForceMultiplier );
		}

		if(myRigidBody.velocity.sqrMagnitude < minimalVelocityBeforeDeath) {
			jukeboxControl.playDeathSound();
			Destroy(gameObject);
		} 
	}

	public void SetColor(Color color)
	{

	}

	void Update()
	{
		Vector3 currentGradient = surfaceControl.getGradientAtPosition ( transform.position );
		Debug.DrawRay ( transform.position, currentGradient.normalized );
		Debug.DrawRay ( transform.position, myRigidBody.velocity, Color.red );
	}

	void OnTriggerEnter(Collider other)
	{
		if ( other.CompareTag ( "Finish" ) )
		{
			//Debug.LogError ( "YOU HAVE WON" );
			jukeboxControl.playWinSound();
			Destroy ( gameObject );
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ( collision.collider.CompareTag ( "Obstacle" ) )
		{
			jukeboxControl.playDeathSound();
			Destroy ( gameObject );
		}
	}
}
