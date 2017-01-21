using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSurface : MonoBehaviour {

	const float MESH_ELEMENT_SIZE = 0.5f;
	public float viscosity = 2.0f;
	public float dampening = 0.8f;
	public float waveSpread = 5.0f;
	private Mesh myMesh;

	private ModelSurface myModel;

	private float meshSizeX;
	private float meshSizeZ;

	private int gridSizeX;
	private int gridSizeZ;

	public GameObject[,] gridPanels;
    public GameObject PanelPool;

    // Use this for initialization
	void Start () {
		myMesh = GetComponent<MeshFilter>().mesh;

		meshSizeX = myMesh.bounds.extents.x * 2f;
		meshSizeZ = myMesh.bounds.extents.z * 2f;

		gridSizeX = (int)Mathf.Floor(meshSizeX / MESH_ELEMENT_SIZE) + 1;
		gridSizeZ = (int)Mathf.Floor(meshSizeZ / MESH_ELEMENT_SIZE) + 1;

		// Create Panels
		gridPanels = new GameObject[gridSizeX, gridSizeZ];
		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				gridPanels[x, z] = makePanel(x, z);
			}
		}

		SetPanelTexture("PanelTextures/default");

		// Create model
		myModel = new ModelSurface(gridSizeX, gridSizeZ); 
	}

	void Update() {
		if(myModel == null) { return; }

		if(Input.GetButton("Submit")) {
			myModel.reset();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(myModel == null) { return; }

		// Model
		myModel.update(Time.fixedDeltaTime, viscosity, dampening, waveSpread);
		// Graphic
		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				float scaleFactor = 0.3f + myModel.vertPos[x, z] / 2f;
				gridPanels[x, z].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

				Vector3 gradient = myModel.getGradientAtPoint (x, z);

				Quaternion q1 = Quaternion.AngleAxis (90f, new Vector3 (1f, 0f, 0f));
				Quaternion q2 = Quaternion.FromToRotation (new Vector3 (1f, 0f, 0f), new Vector3(gradient.x, gradient.z, 0f));
				gridPanels [x, z].transform.localRotation = q1 * q2;
			}
		}
	}

	private GameObject makePanel(int x, int z) {
		float xPos = (x - Mathf.Ceil(gridSizeX / 2)) * MESH_ELEMENT_SIZE;
		float zPos = (z - Mathf.Ceil(gridSizeZ / 2)) * MESH_ELEMENT_SIZE;
		Vector3 originPoint = new Vector3(xPos, 0.1f, zPos);

		GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Quad);
		panel.name = "Panel[" + x.ToString() + "][" + z.ToString() + "]";
		panel.transform.parent = PanelPool.transform;
		panel.transform.localPosition = originPoint;
		panel.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		 
		Quaternion q1 = Quaternion.AngleAxis (90f, new Vector3 (1f, 0f, 0f));
		panel.transform.localRotation = q1;


		panel.tag = "Panel";

		return panel;
	}

    public void SetPanelTexture(string texturePath)
    {
        var texture = Resources.Load(texturePath) as Texture2D;

		foreach (var panel in gridPanels)
        {  
			panel.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Transparent");
			panel.GetComponent<Renderer>().material.SetColor("_Transparent", Color.clear);
			panel.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }


    public Vector3 getGradientAtPosition(Vector3 transformPos) {
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt(xPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeX / 2);
		int zGrid = Mathf.RoundToInt(zPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeZ / 2);

		return myModel.getGradientAtPoint(xGrid, zGrid);
	}

	public void setPulseAtPosition(Vector3 transformPos, float pulseForce) {
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt(xPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeX / 2);
		int zGrid = Mathf.RoundToInt(zPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeZ / 2);

		myModel.setPulseAtPoint(xGrid, zGrid, pulseForce);
	}

	public void toggleOscillatorAtPosition(Vector3 transformPos, float pulseForce) {
		float xPos = transformPos.x;
		float zPos = transformPos.z;

		int xGrid = Mathf.RoundToInt(xPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeX / 2);
		int zGrid = Mathf.RoundToInt(zPos / MESH_ELEMENT_SIZE) + Mathf.FloorToInt(gridSizeZ / 2);

		myModel.toggleOscillatorAtPosition(xGrid, zGrid, pulseForce);
	}
}