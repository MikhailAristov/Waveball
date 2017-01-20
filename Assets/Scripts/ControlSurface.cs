using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSurface : MonoBehaviour {

	const float MeshElementSize = 0.5f;

	private MeshFilter myMesh;

	// Use this for initialization
	void Start () {
		myMesh = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		Mesh mesh = myMesh.mesh; // mesh
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		int i = 0;
		while (i < vertices.Length) {
			vertices[i].y = normals[i].y * Mathf.Sin(Time.time + Mathf.Sqrt(vertices[i].x * vertices[i].x + vertices[i].z * vertices[i].z));
			i++;
		}
		mesh.vertices = vertices;
	}
}
