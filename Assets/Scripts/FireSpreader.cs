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
	private FireManager fireManager;
	private GraphNode currentNode;
	private List<GraphNode> intactNeighbors;
	private AstarPath graphEngine;

	void Start(){
		fireManager = GameObject.Find ("ScriptsBucket").GetComponent<FireManager> ();
		currentNode = AstarPath.active.GetNearest (transform.position).node;
		intactNeighbors = GetIntactNeighbors ();
		endTime = Time.time + maxBurningTime;
		timeSinceLastSpread = 0.0f;
		chanceOfSpreadingToAdjacentTile = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		chanceOfSpreadingToNPC = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		spreadingDelay = Mathf.Max (0.0f, spreadingDelay);
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > endTime || intactNeighbors.Count == 0){
			Destroy (this);
		} else {
			timeSinceLastSpread += Time.deltaTime;
			if (timeSinceLastSpread < spreadingDelay) {
				SpreadToIntactNeighbor ();
				timeSinceLastSpread = 0.0f;
			}
		}
	}

	private void SpreadToIntactNeighbor(){
		float rndF = Random.Range (0.0f, 1.0f);
		if (rndF <= chanceOfSpreadingToAdjacentTile) {	//Fire is spreading!
			int rndI = Random.Range(0, intactNeighbors.Count - 1);
			GameObject.Instantiate (firePrefab, (Vector3)intactNeighbors [rndI].position, Quaternion.identity);
			fireManager.AddBurningNode (intactNeighbors [rndI]);
			intactNeighbors.RemoveAt (rndI);
		}
	}

	private List<GraphNode> GetIntactNeighbors() {
		List<GraphNode> listOfNodes = new List<GraphNode> ();
		NNConstraint contrainte = new NNConstraint();
		contrainte.constrainDistance = true;
		contrainte.constrainWalkability = true;
		contrainte.walkable = true;
		AstarPath graphEngine = AstarPath.active;
		graphEngine.maxNearestNodeDistance = 0.0f;
		NNInfo info = graphEngine.graphs[0].GetNearestForce (transform.position + Vector3.right, contrainte);
		if (info.node != null
		    && !fireManager.burningNodes.Contains(info.node)
		    && !fireManager.burntNodes.Contains(info.node)) {
			listOfNodes.Add (info.node);
		}
		info = graphEngine.graphs[0].GetNearestForce (transform.position + Vector3.left, contrainte);
		if (info.node != null
		    && !fireManager.burningNodes.Contains(info.node)
		    && !fireManager.burntNodes.Contains(info.node)) {
			listOfNodes.Add (info.node);
		}
		info = graphEngine.graphs[0].GetNearestForce (transform.position + Vector3.forward, contrainte);
		if (info.node != null
		    && !fireManager.burningNodes.Contains(info.node)
		    && !fireManager.burntNodes.Contains(info.node)) {
			listOfNodes.Add (info.node);
		}
		info = graphEngine.graphs[0].GetNearestForce (transform.position + Vector3.back, contrainte);
		if (info.node != null
		    && !fireManager.burningNodes.Contains(info.node)
		    && !fireManager.burntNodes.Contains(info.node)) {
			listOfNodes.Add (info.node);
		}
		return listOfNodes;
	}


}
