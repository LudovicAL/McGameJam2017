using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Pathfinding;
public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AstarPath pathEngine = GameObject.Find ("A*").GetComponent<AstarPath> ();
		NavGraph graph = pathEngine.graphs [0];
		Vector3 v = new Vector3 (-5f, 0f, 4f);
		NNInfo info = graph.GetNearest (v);
		if (Mathf.Sqrt (VectorMath.SqrDistanceXZ ((Vector3)info.node.position, v)) < 1) {
			info.node.Tag = 0x1;
		}
		v = new Vector3 (0f, 0f, -5f);
		info = graph.GetNearest (v);
		if (Mathf.Sqrt (VectorMath.SqrDistanceXZ ((Vector3)info.node.position, v)) < 1) {
			info.node.Tag = 0x1;
		}
	}
}
