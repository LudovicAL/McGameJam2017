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
		info.node.Tag = 0x1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
