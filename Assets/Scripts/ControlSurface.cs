using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSurface : MonoBehaviour
{
	const float PANEL_SIZE = 0.5f;
	public float viscosity = 2.0f;
	public float dampening = 0.8f;
	public float waveSpread = 5.0f;

	private ModelSurface myModel;

	private float meshSizeX;
	private float meshSizeZ;

	private int gridSizeX;
	private int gridSizeZ;

	public float PanelTextureSize = 0.9f;

	public ControlPanel[,] gridPanels;
	public GameObject PanelPool;
	public GameObject PrefabPanel;

	// Use this for initialization
	void Start()
	{
		meshSizeX = 10f * 2f * transform.localScale.x;
		meshSizeZ = 10f * 2f * transform.localScale.z;

		Debug.Log ( "SurfaceSize: X:" + meshSizeX + " Y:" + meshSizeZ + " " + meshSizeX * meshSizeZ );

		gridSizeX = (int)Mathf.Floor ( meshSizeX / PANEL_SIZE ) + 1;
		gridSizeZ = (int)Mathf.Floor ( meshSizeZ / PANEL_SIZE ) + 1;

		// Create Panels
		gridPanels = new ControlPanel[gridSizeX, gridSizeZ];
		for ( int x = 0; x < gridSizeX; x++ )
		{
			for ( int z = 0; z < gridSizeZ; z++ )
			{
				gridPanels[x, z] = makePanel ( x, z );
			}
		}

		//SetPanelTexture ( "PanelTextures/default" );

		// Create model
		myModel = new ModelSurface ( gridSizeX, gridSizeZ );
	}

	void Update()
	{
		if ( Input.GetButton ( "Reset" ) )
		{
			myModel.reset ();
		}
		// Graphic
		Quaternion q1 = Quaternion.LookRotation ( -Vector3.up );


		//q1 = Quaternion.AngleAxis(90, new Vector3(1f, 0f, 0f));
		for ( int x = 0; x < gridSizeX; x++ )
		{
			for ( int z = 0; z < gridSizeZ; z++ )
			{
				var trans = gridPanels[x, z].Model;
				float scaleFactor = Mathf.Max ( 0f, PanelTextureSize + myModel.vertPos[x, z] / 2f );
				trans.localScale = new Vector3 ( scaleFactor, scaleFactor, 1f );

				Vector3 gradient = myModel.getGradientAtPoint ( x, z );

				Quaternion q2 = Quaternion.FromToRotation ( new Vector3 ( 1f, 0f, 0f ), new Vector3 ( gradient.x, gradient.z, 0f ) );
				Quaternion q = q1 * q2;

				// Quaternion gimbal lock invert prevent
				if (Mathf.Abs(q.eulerAngles.x) > 200f) {
					Debug.Log (q);
					q = new Quaternion (-q.x, q.y, -q.z, q.w);
				}
				trans.localRotation = q;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// Model
		myModel.update ( Time.fixedDeltaTime, viscosity, dampening, waveSpread );
	}

	private ControlPanel makePanel(int x, int z)
	{
		float xPos = (x - Mathf.Ceil ( gridSizeX / 2 )) * PANEL_SIZE;
		float zPos = (z - Mathf.Ceil ( gridSizeZ / 2 )) * PANEL_SIZE;
		Vector3 originPoint = new Vector3 ( xPos, 0.1f, zPos );

		var panel = Instantiate<GameObject> ( PrefabPanel );
		panel.name = "Panel[" + x + "][" + z + "]";
		panel.transform.parent = PanelPool.transform;
		panel.transform.localPosition = originPoint;

		panel.transform.localScale = new Vector3 ( PANEL_SIZE, 1f, PANEL_SIZE );
		panel.transform.localRotation = Quaternion.identity;

		var renderer = gameObject.GetComponentInChildren<Renderer> ();
		renderer.material.shader = Shader.Find ( "Standard" );

		return panel.GetComponent<ControlPanel> ();
	}

	public void SetPanelTexture(string texturePath)
	{
		var texture = Resources.Load ( texturePath ) as Texture2D;
		foreach ( var panel in gridPanels )
		{
			var renderer = panel.ModelRenderer;
			renderer.material.shader = Shader.Find ( "Unlit/Transparent" );
			renderer.material.SetColor ( "_Transparent", Color.clear );
			renderer.material.mainTexture = texture;
		}
	}

	public Vector3 worldPosToGrid(float xPos, float zPos)
	{
		int xGrid = Mathf.RoundToInt ( xPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeX / 2 );
		int zGrid = Mathf.RoundToInt ( zPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeZ / 2 );
		return new Vector3 ( xGrid, 0, zGrid );
	}

	public Vector3 getGradientAtPosition(Vector3 transformPos)
	{
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt ( xPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeX / 2 );
		int zGrid = Mathf.RoundToInt ( zPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeZ / 2 );

		return myModel.getGradientAtPoint ( xGrid, zGrid );
	}

	public void setPulseAtPosition(Vector3 transformPos, float pulseForce)
	{
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt ( xPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeX / 2 );
		int zGrid = Mathf.RoundToInt ( zPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeZ / 2 );

		myModel.setPulseAtPoint ( xGrid, zGrid, pulseForce );
	}

	public void toggleOscillatorAtPosition(Vector3 transformPos, float pulseForce)
	{
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt ( xPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeX / 2 );
		int zGrid = Mathf.RoundToInt ( zPos / PANEL_SIZE ) + Mathf.FloorToInt ( gridSizeZ / 2 );

		myModel.toggleOscillatorAtPosition ( xGrid, zGrid, pulseForce );

		bool isOscil = myModel.oscillatorMap [xGrid, zGrid];

		if (isOscil) {
			//Debug.Log (gridPanels [xGrid, zGrid].GetComponentInParent<ControlPanel>());
			gridPanels [xGrid, zGrid].GetComponentInParent<ControlPanel> ().ActionState = PanelForceActionState.Oscillator;
		} else {
			gridPanels [xGrid, zGrid].GetComponentInParent<ControlPanel> ().ActionState = PanelForceActionState.None;
		}
	
	}
}