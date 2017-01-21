using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSurface : MonoBehaviour {

	const float MESH_ELEMENT_SIZE = 0.2f;

	private Mesh myMesh;

	private ModelSurface myModel;

	private float meshSizeX;
	private float meshSizeZ;

	private int gridSizeX;
	private int gridSizeZ;

	private GameObject[,] gridNeedles;
	public GameObject needlePool;

	// Use this for initialization
	void Start () {
		myMesh = GetComponent<MeshFilter>().mesh;

		Debug.Log(myMesh.bounds);

		meshSizeX = myMesh.bounds.extents.x * 2f;
		meshSizeZ = myMesh.bounds.extents.z * 2f;

		gridSizeX = (int)Mathf.Floor(meshSizeX / MESH_ELEMENT_SIZE) + 1;
		gridSizeZ = (int)Mathf.Floor(meshSizeZ / MESH_ELEMENT_SIZE) + 1;

		Debug.Log(gridSizeX);
		Debug.Log(gridSizeZ);

		// Create needles
		gridNeedles = new GameObject[gridSizeX, gridSizeZ];
		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				gridNeedles[x, z] = makeNeedle(x, z);
			}
		}

		// Create model
		myModel = new ModelSurface(gridSizeX, gridSizeZ); 
	}
	
	// Update is called once per frame
	void Update () {
		// Model
		myModel.update(Time.deltaTime);
		// Graphic
		for(int x = 0; x < gridSizeX; x++) {
			for(int z = 0; z < gridSizeZ; z++) {
				//Vector3 myPos = gridNeedles[x, z].transform.position;
				//Vector3 newPos = new Vector3(myPos.x, myModel.vertPos[x, z], myPos.z);
				//gridNeedles[x, z].transform.position = newPos;
				float scaleFactor = 0.1f + myModel.vertPos[x, z] / 2f;
				gridNeedles[x, z].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			}
		}
	}

	private GameObject makeNeedle(int x, int z) {
		float xPos = (x - Mathf.Ceil(gridSizeX / 2)) * MESH_ELEMENT_SIZE;
		float zPos = (z - Mathf.Ceil(gridSizeZ / 2)) * MESH_ELEMENT_SIZE;
		Vector3 originPoint = new Vector3(xPos, 0f, zPos);
		GameObject needle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		needle.name = "needle[" + x.ToString() + "][" + z.ToString() + "]";
		needle.transform.parent = needlePool.transform;
		needle.transform.localPosition = originPoint;
		needle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		needle.GetComponent<SphereCollider>().enabled = false;
		return needle;
	}
}
