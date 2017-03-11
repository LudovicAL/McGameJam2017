using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Pathfinding;
public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AstarPath pathEngine = GameObject.Find ("A*").GetComponent<AstarPath> ();
		NavGraph graph = pathEngine.graphs [0];
		NNInfo info = graph.GetNearest (new Vector3 (-49, 0, 15));
		if (Mathf.Sqrt (VectorMath.SqrDistanceXZ ((Vector3)info.node.position, new Vector3 (-49, 0, 15))) < 1) {
			info.node.Tag = 0x1;
			Debug.Log ("Startup node : " + info.node.GetHashCode ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
