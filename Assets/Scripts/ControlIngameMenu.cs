using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlIngameMenu : MonoBehaviour
{
	public void OnClickBack()
	{
		SceneManager.LoadScene ( "Start" );

	}
}
