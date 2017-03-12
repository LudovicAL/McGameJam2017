using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandlers : MonoBehaviour {

	const int PANEL_MENU = 0;
	const int PANEL_PAUSE = 1;
	const int PANEL_GAME = 2;

	public GameObject[] panelArray;
	private GameStatesManager stateMan;
	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;

	void Start ()  {
		scriptsBucket = StaticData.GetScriptBucket ();
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		ActivatePanel (PANEL_MENU);
		stateMan = scriptsBucket.GetComponent<GameStatesManager>();
	}

	public void Exit(){
		//Debug.Log ("yay");
		Application.Quit ();
	}
	public void StartGame() {
		//Debug.Log ("nay");
		ActivatePanel (PANEL_GAME);
		stateMan.ChangeGameState(StaticData.AvailableGameStates.Playing);
	}

	public void Pause() {
		//Debug.Log ("oy");
		ActivatePanel (PANEL_PAUSE);
		stateMan.ChangeGameState (StaticData.AvailableGameStates.Paused);
	}

	public void ReturnMM() {
		//Debug.Log ("si");
		ActivatePanel (PANEL_MENU);
		stateMan.ChangeGameState (StaticData.AvailableGameStates.Menu);
	}

	public void Resume(){
		//Debug.Log ("nonono");
		ActivatePanel (PANEL_GAME);
		stateMan.ChangeGameState (StaticData.AvailableGameStates.Playing);
	}

	public void ActivatePanel(int index) {
		for (int i = 0, max = panelArray.Length; i < max; i++) {
			panelArray [i].SetActive (i == index);
		}
	}

	protected void OnMenu() {
		SetCanvasState (StaticData.AvailableGameStates.Menu);
		ActivatePanel (PANEL_MENU);
	}

	protected void OnStarting() {
		SetCanvasState (StaticData.AvailableGameStates.Starting);
		ActivatePanel (PANEL_GAME);
	}

	protected void OnPlaying() {
		SetCanvasState (StaticData.AvailableGameStates.Playing);
		ActivatePanel (PANEL_GAME);
	}

	protected void OnPausing() {
		SetCanvasState (StaticData.AvailableGameStates.Paused);
		ActivatePanel (PANEL_PAUSE);
	}

	public void SetCanvasState(StaticData.AvailableGameStates state) {
		gameState = state;
	}
}