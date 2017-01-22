using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPortalEnde : MonoBehaviour
{
	public ControlSpawn spawner;

	public float MinDistance = 1;
	public float MaxDistance = 5;

	private Material mat;

	private void Start()
	{
		mat = GetComponentInChildren<Renderer> ().material;
	}

	private void Update()
	{
		if ( spawner.particle == null )
			return;

		var color = mat.color;

		var distance = Vector3.Distance ( spawner.particle.transform.position, transform.position );
		if ( distance >= MaxDistance )
		{
			color.a = 0;
		}
		else if ( distance <= MinDistance )
		{
			color.a = 1;
		}
		else
		{
			color.a = 1f - ((distance - MinDistance) / (MaxDistance - MinDistance));
		}

		mat.color = color;
	}
}
