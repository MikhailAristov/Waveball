using UnityEngine;

public class ControlParticle : MonoBehaviour
{
	// TODO: Objecte poolen

	private Rigidbody myRigidBody;

	public float minimalVelocityBeforeDeath = 0.1f;
	public float gradientForceMultiplier = 100f;

	public GameObject surface;
	private ControlSurface surfaceControl;

	// Use this for initialization
	void Start()
	{
		myRigidBody = GetComponent<Rigidbody> ();
		surfaceControl = surface.GetComponent<ControlSurface> ();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if ( !myRigidBody.IsSleeping ()
			&& myRigidBody.velocity.magnitude < minimalVelocityBeforeDeath )
		{
			Destroy ( gameObject );
			return;
		}

		Vector3 currentGradient = surfaceControl.getGradientAtPosition ( transform.position );
		if ( currentGradient.sqrMagnitude > 0 )
			myRigidBody.AddForce ( currentGradient * gradientForceMultiplier );
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
			Destroy ( gameObject );
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ( collision.collider.CompareTag ( "Obstacle" ) )
		{
			Destroy ( gameObject );
		}
	}
}
