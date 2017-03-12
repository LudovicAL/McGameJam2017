using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FireIgniter : MonoBehaviour {

	public GameObject firePrefab;
	public Sprite matche;
	public Sprite ignitedMatche;
	public int maxNumberOfIgnitions;
	private GridGraph gridGraph;
	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	private int numberOfIgnitions;

	// Use this for initialization
	void Start () {
		numberOfIgnitions = 0;
		gridGraph = AstarPath.active.astarData.gridGraph;
		scriptsBucket = GameObject.Find ("ScriptsBucket");
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}
	
	// Update is called once per frame
	void Update () {
		//if (gameState == StaticData.AvailableGameStates.Playing) {
		if (numberOfIgnitions < maxNumberOfIgnitions) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray, out hit)) {
				NNConstraint contrainte = new NNConstraint();
				contrainte.constrainWalkability = true;
				contrainte.walkable = true;
				NNInfo info = gridGraph.GetNearestForce (hit.point, contrainte);
				if (info.node != null) {
					this.transform.position = (Vector3)info.node.position;
					if (Input.GetMouseButtonDown(0)) {
						this.GetComponent<SpriteRenderer> ().sprite = ignitedMatche;
					} else if (Input.GetMouseButtonUp(0)) {
						this.GetComponent<SpriteRenderer> ().sprite = matche;
						GameObject.Instantiate(firePrefab, this.transform.position, Quaternion.LookRotation(Vector3.down));
						numberOfIgnitions++;
					}
				}
			}
		} else {
			Destroy (this.gameObject);
		}
		//}
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
