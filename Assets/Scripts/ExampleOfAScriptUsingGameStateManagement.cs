using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleOfAScriptUsingGameStateManagement : MonoBehaviour {

	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;

	// Use this for initialization
	void Start () {
		scriptsBucket = StaticData.GetScriptBucket ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			scriptsBucket.GetComponent<GameStatesManager> ().ChangeGameState (StaticData.AvailableGameStates.Playing);
		}
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
