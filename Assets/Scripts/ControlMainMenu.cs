using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMainMenu : MonoBehaviour
{
	public ControlLevelButton Hover;
	public Transform Camera;
	public Image screenBlend;


	public float BlendTime = 1f;

	private float time;
	private Quaternion rotation;
	private Vector3 position;

	public void Start()
	{
		rotation = Camera.rotation;
		position = Camera.position;
	}

	public void OnClickExit()
	{
		Application.Quit ();
	}

	public void OnClickOptions()
	{

	}


	private void Update()
	{
		if ( Hover != null )
		{
			var lookDir = Hover.transform.position - Camera.position;
			lookDir.Normalize ();
			var lookRotation = Quaternion.LookRotation ( lookDir );

			time += Time.deltaTime;
			var blend = Mathf.Min ( 1f, time / BlendTime );
			blend = Mathf.SmoothStep ( 0, 1, blend );

			Camera.rotation = Quaternion.Slerp ( rotation, lookRotation, blend );
			Camera.position = Vector3.Lerp ( position, Hover.transform.position, blend );

			var color = screenBlend.color;
			color.a = blend;
			screenBlend.color = color;

			if ( blend >= 1f )
			{
				SceneManager.LoadScene ( Hover.LevelName, LoadSceneMode.Single );
				SceneManager.LoadScene ( "IngameMenu", LoadSceneMode.Additive );
			}
		}
	}
}
