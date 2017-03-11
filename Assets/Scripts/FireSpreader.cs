using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FireSpreader : MonoBehaviour {

	public GameObject firePrefab;
	public float chanceOfSpreadingToAdjacentTile; //A number between 0 and 1
	public float chanceOfSpreadingToNPC; //A number between 0 and 1
	public float spreadingDelay;	//A number superior to 0
	public float maxBurningTime;
	private float endTime;
	private float timeSinceLastSpread;
	private GraphNode currentNode;
	private GridGraph gridGraph;
	private List<GraphNode> neighborNodes;

	void Start() {
		gridGraph = AstarPath.active.astarData.gridGraph;
		currentNode = gridGraph.GetNearest (transform.position).node;
		int[] neighborNodesOffsets = gridGraph.neighbourOffsets;
		neighborNodes = new List<GraphNode> ();
		foreach (int nno in neighborNodesOffsets) {
			neighborNodes.Add(gridGraph.nodes[currentNode.NodeIndex + nno]);
		}
		endTime = Time.time + maxBurningTime;
		timeSinceLastSpread = 0.0f;
		chanceOfSpreadingToAdjacentTile = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		chanceOfSpreadingToNPC = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		spreadingDelay = Mathf.Max (0.0f, spreadingDelay);
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > endTime){
			Destroy (this);
		} else {
			timeSinceLastSpread += Time.deltaTime;
			if (timeSinceLastSpread < spreadingDelay) {
				SpreadToNeighbor ();
				timeSinceLastSpread = 0.0f;
			}
		}
	}

	private void SpreadToNeighbor(){
		float rndF = Random.Range (0.0f, 1.0f);
		if (rndF <= chanceOfSpreadingToAdjacentTile) {	//Fire is spreading!
			
		}
	}
}
