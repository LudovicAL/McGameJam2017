using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Pathfinding;
public class Startup : MonoBehaviour {

	public GameObject prefabPerso;

	// Use this for initialization
	void Start () {
		GameObject dangers = GameObject.Find ("Dangers");
		for (int i = 0, n = dangers.transform.childCount; i < n; ++i) {
			Transform danger = dangers.transform.GetChild (i);
			if (danger.gameObject.activeInHierarchy) {
				GraphNode node = NodeUtilities.DonneNodeAvecPosition (danger.position);
				node.Tag |= StaticData.BURNING_GROUND;
			}
		}

		GridGraph grid = AstarPath.active.astarData.gridGraph;
		List<GraphNode> graphNodes = new List<GraphNode> ();
		grid.GetNodes(delegate(GraphNode node) {
			if (node.Walkable) {
				graphNodes.Add(node);
			}
			return true;
		});
		int nbPersonnages = Mathf.CeilToInt(graphNodes.Count / (20 * grid.nodeSize));
		for (int i = 0; i < nbPersonnages; ++i) {
			int nodeIndex = Random.Range (0, graphNodes.Count);
			GraphNode node = graphNodes [nodeIndex];
			GameObject obj = GameObject.Instantiate (prefabPerso, (Vector3)node.position, Quaternion.LookRotation(Vector3.forward));
		}
	}
}
