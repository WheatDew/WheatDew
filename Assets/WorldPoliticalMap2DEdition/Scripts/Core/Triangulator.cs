using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WPMF {
	public class Triangulator {
		Vector3[] m_points;
		int m_numPoints;
		static List<int> indices;
		static int[] V;

		public static int[]  GetPoints (Vector3[] points) {
			Triangulator triangulator = new Triangulator (points);
			return triangulator.Triangulate ();
		}

		public Triangulator (Vector3[] points) {
			this.m_points = points;
			m_numPoints = points.Length;
		}

		public int[] Triangulate () {
			int n = m_numPoints;
			if (n < 3)
				return new int[0];
			if (indices == null) {
				indices = new List<int> (n * 3);
			} else {
				indices.Clear ();
			}
			if (V == null || V.Length < n) {
				V = new int[n];
			}
			if (Area () > 0) {
				for (int v = 0; v < n; v++)
					V [v] = v;
			} else {
				for (int v = 0; v < n; v++)
					V [v] = (n - 1) - v;
			}
			int nv = n;
			int count = 2 * nv;
			int sizeofInt = sizeof(int);

			for (int v = nv - 1; nv > 2;) {
				if ((count--) <= 0)
					return indices.ToArray ();
			
				int u = v;
				if (nv <= u)
					u = 0;
				v = u + 1;
				if (nv <= v)
					v = 0;
				int w = v + 1;
				if (nv <= w)
					w = 0;
			
				if (Snip (u, v, w, nv, V)) {
					int a, b, c;
					a = V [u];
					b = V [v];
					c = V [w];
					indices.Add (a);
					indices.Add (b);
					indices.Add (c);
					Buffer.BlockCopy (V, (v + 1) * sizeofInt, V, v * sizeofInt, (nv - v - 1) * sizeofInt); // fast shift array to the left one position
					nv--;
					count = 2 * nv;
				}
			}
			indices.Reverse ();
			return indices.ToArray ();
		}

		private bool Snip (int u, int v, int w, int n, int[] V) {
			Vector3 A = m_points [V [u]];
			Vector3 B = m_points [V [v]];
			Vector3 C = m_points [V [w]];
			if (Mathf.Epsilon > (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x))
				return false;
			float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
			float cCROSSap, bCROSScp, aCROSSbp;
			ax = C.x - B.x;
			ay = C.y - B.y;
			bx = A.x - C.x;
			by = A.y - C.y;
			cx = B.x - A.x;
			cy = B.y - A.y;

			for (int p = 0; p < n; p++) {
				Vector3 P = m_points [V [p]];

				bpy = P.y - B.y;
				bpx = P.x - B.x;
				aCROSSbp = ax * bpy - ay * bpx;
				if (aCROSSbp < 0.0f)
					continue;

				cpx = P.x - C.x;
				cpy = P.y - C.y;
				bCROSScp = bx * cpy - by * cpx;
				if (bCROSScp < 0.0f)
					continue;

				apx = P.x - A.x;
				apy = P.y - A.y;
				cCROSSap = cx * apy - cy * apx;
				if (cCROSSap >= 0.0f) { // inside triangle
					if (p == u || p == v || p == w)
						continue;
					return false;
				}
			}
			return true;
		}

		private float Area () {
			int n = m_numPoints;
			float A = 0.0f;
			for (int p = n - 1, q = 0; q < n; p = q++) {
				Vector3 pval = m_points [p];
				Vector3 qval = m_points [q];
				A += pval.x * qval.y - qval.x * pval.y;
			}
			return (A * 0.5f);
		}
	

	}


}