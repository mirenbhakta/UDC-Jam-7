using System;
using System.Collections.Generic;
using UnityEngine;

namespace Miren
{
	public struct MeshData : IDisposable
	{
		public List<Vector3> vertices;
		public List<int> triangles;
		public List<Vector2> uvs;

		public static MeshData FromPool ()
		{
			MeshData md;
			md.vertices = ListPool<Vector3>.Get ();
			md.triangles = ListPool<int>.Get ();
			md.uvs = ListPool<Vector2>.Get ();
			return md;
		}

		public void ToMesh (Mesh mesh)
		{
			mesh.Clear ();
			mesh.SetVertices (vertices);
			mesh.SetTriangles (triangles, 0);
			mesh.SetUVs (0, uvs);
		}

		public Mesh ToMesh ()
		{
			Mesh mesh = new Mesh ();
			ToMesh (mesh);
			return mesh;
		}

		public void Dispose ()
		{
			ListPool<Vector3>.Add (vertices);
			ListPool<int>.Add (triangles);
			ListPool<Vector2>.Add (uvs);
		}
	}
}
