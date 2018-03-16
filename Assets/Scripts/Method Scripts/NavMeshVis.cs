using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class NavMeshVis : MonoBehaviour {
	public MeshFilter meshfilter;

	void Start() {
		Vector3[] verts;
		int[] ids;

		NavMesh.Triangulate (out verts, out ids);

		Mesh mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.triangles = ids;
		meshfilter.mesh = mesh;
	}
}