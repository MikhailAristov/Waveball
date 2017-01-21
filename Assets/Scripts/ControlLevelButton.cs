using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLevelButton : MonoBehaviour
{
	public ControlMainMenu control;

	public string LevelName;


	private void OnMouseDown()
	{
		control.Hover = this;
	}

	private void OnMouseEnter()
	{
	}

	private void OnMouseExit()
	{
	}
}
