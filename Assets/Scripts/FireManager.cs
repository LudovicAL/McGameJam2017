using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FireManager : MonoBehaviour {

	public List<GraphNode> burningNodes { get; private set; }
	public List<GraphNode> burntNodes { get; private set; }

	// Use this for initialization
	void Start () {
		burningNodes = new List<GraphNode> ();
		burntNodes = new List<GraphNode> ();
	}

	public void AddBurningNode(GraphNode node) {
		if (!burningNodes.Contains(node)) {
			burningNodes.Add (node);
		}
	}

	public void AddBurntNode(GraphNode node) {
		burntNodes.Remove (node);
		if (!burntNodes.Contains(node)) {
			burntNodes.Add (node);
		}
	}
}
