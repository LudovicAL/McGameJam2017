using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FireAlarm : MonoBehaviour {

	public float alarmEffectRadiusInNodeCount;
	public bool alarmIsOn { get; private set; }
	private AudioSource audioSource;
	private GridGraph gridGraph;
	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	private List<AIEvacuation> characterList;
	private float alarmEffectRadiusInWorldSpace;
	private ParticleSystem particleSystem;

	void Start() {
		scriptsBucket = GameObject.Find ("ScriptsBucket");
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		gridGraph = AstarPath.active.astarData.gridGraph;
		characterList = new List<AIEvacuation> ();
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Character");
		foreach (GameObject go in temp) {
			characterList.Add (go.GetComponent<AIEvacuation> ());
		}
		particleSystem = this.GetComponent<ParticleSystem> ();
		alarmIsOn = false;
		audioSource = this.gameObject.GetComponent<AudioSource> ();
		alarmEffectRadiusInWorldSpace = alarmEffectRadiusInNodeCount * gridGraph.nodeSize;
	}

	void Update() {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (alarmIsOn) {
				foreach(AIEvacuation c in characterList) {
					if (Vector3.Distance(c.transform.position, this.transform.position) < alarmEffectRadiusInWorldSpace) {
						if (!c.signalEvacuation) {
							c.SetSignalEvacuation (true);
						}
					}
				}
			} else {
				foreach(AIEvacuation c in characterList) {
					if (Vector3.Distance(c.transform.position, this.transform.position) < alarmEffectRadiusInWorldSpace) {
						if (c.signalEvacuation) {
							alarmIsOn = true;
							StartCoroutine(PlayAlarm ());
						}
					}
				}
			}
		}
	}

	private IEnumerator PlayAlarm() {
		audioSource.Play ();
		particleSystem.Emit (10);
		yield return new WaitForSeconds (Mathf.CeilToInt(audioSource.clip.length));
		audioSource.Play ();
		particleSystem.Emit (10);
		yield return new WaitForSeconds (Mathf.CeilToInt(audioSource.clip.length));
		audioSource.Play ();
		particleSystem.Emit (10);
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
