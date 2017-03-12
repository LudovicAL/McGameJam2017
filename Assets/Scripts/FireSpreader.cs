using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

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
	private List<GraphNode> neighborFlammableNodes;
	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;

	void Start() {
		scriptsBucket = StaticData.GetScriptBucket ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		gridGraph = AstarPath.active.astarData.gridGraph;
		currentNode = gridGraph.GetNearest (transform.position).node;
		currentNode.Tag = StaticData.BURNING_GROUND;
		neighborFlammableNodes = PathUtilities.BFS (currentNode, 1);
		endTime = Time.time + maxBurningTime;
		timeSinceLastSpread = 0.0f;
		chanceOfSpreadingToAdjacentTile = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		chanceOfSpreadingToNPC = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		spreadingDelay = Mathf.Max (0.0f, spreadingDelay);
	}

	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (Time.time > endTime){
				DestroyThisComponent();
			} else {
				timeSinceLastSpread += Time.deltaTime;
				if (timeSinceLastSpread > spreadingDelay) {
					SpreadToNeighbor ();
					timeSinceLastSpread = 0.0f;
				}
			}
		}
	}

	private void SpreadToNeighbor() {
		float rndf = Random.Range (0.0f, 1.0f);
		if (rndf <= chanceOfSpreadingToAdjacentTile) {	//Fire is spreading!
			List<GraphNode> neighborBurningNodes = PathUtilities.BFS (currentNode, 1, StaticData.BURNING_GROUND);
			List<GraphNode> neighborBurntNodes = PathUtilities.BFS (currentNode, 1, StaticData.BURNT_GROUND);
			neighborFlammableNodes = neighborFlammableNodes.Except (neighborBurningNodes).ToList ().Except (neighborBurntNodes).ToList ();
			if (neighborFlammableNodes.Count > 0) {
				int rndI = Random.Range (0, neighborFlammableNodes.Count - 1);
				neighborFlammableNodes [rndI].Tag = StaticData.BURNING_GROUND;
				GameObject.Instantiate (firePrefab, (Vector3)neighborFlammableNodes [rndI].position, Quaternion.LookRotation(Vector3.down));
			} else {
				DestroyThisComponent();
			}
		}
	}

	private void DestroyThisComponent() {
		currentNode.Tag ^= StaticData.BURNING_GROUND;
		currentNode.Tag = StaticData.BURNT_GROUND;
		Destroy (this.gameObject);
	}

	protected void OnMenu() {
		SetCanvasState (StaticData.AvailableGameStates.Menu);

	}

	protected void OnStarting() {
		SetCanvasState (StaticData.AvailableGameStates.Starting);

	}

	protected void OnPlaying() {
		SetCanvasState (StaticData.AvailableGameStates.Playing);

	}

	protected void OnPausing() {
		SetCanvasState (StaticData.AvailableGameStates.Paused);
	}

	public void SetCanvasState(StaticData.AvailableGameStates state) {
		gameState = state;
	}
}
