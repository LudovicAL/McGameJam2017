using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	private GameObject scriptsBucket;
	private StaticData.AvailableGameStates gameState;
	//public AudioClip[] burnSFX;
	public float maxTimeSoundSFX;
	public float minTimeSoundSFX;
	private float nextSFXTime;
	public AudioClip[] idleSFX;
	public AudioClip[] afraidSFX;
	private List<AIEvacuation> allCharacters;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () 
	{
		scriptsBucket = GameObject.Find ("ScriptBucket");
		scriptsBucket.GetComponent<GameStatesManager> ().MenuGameState.AddListener(OnMenu);
		scriptsBucket.GetComponent<GameStatesManager> ().StartingGameState.AddListener(OnStarting);
		scriptsBucket.GetComponent<GameStatesManager> ().PlayingGameState.AddListener(OnPlaying);
		scriptsBucket.GetComponent<GameStatesManager> ().PausedGameState.AddListener(OnPausing);
		SetCanvasState (scriptsBucket.GetComponent<GameStatesManager> ().gameState);
		//Gamestatemanger
		nextSFXTime = 0.0f;
		allCharacters = new List<AIEvacuation> ();
		audioSource = this.gameObject.GetComponent<AudioSource> ();

		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Character");


		foreach (GameObject go in temp) 
		{
			
			
			allCharacters.Add (go.GetComponent<AIEvacuation>());	
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (gameState == StaticData.AvailableGameStates.Playing) {

			if (Time.time > nextSFXTime) 
			{
				bool isOneAfraid = false;
				foreach (AIEvacuation a in allCharacters) 
				{
					if (a.signalEvacuation) {
						isOneAfraid = true;
						break;
					}
						
				}
				if (isOneAfraid) {
					int rnd = Random.Range (0, afraidSFX.Length - 1);
					audioSource.clip = afraidSFX [rnd];
					audioSource.Play ();
				} else {
					int rnd = Random.Range (0, idleSFX.Length);
					audioSource.clip = idleSFX [rnd];
					audioSource.Play();
				}
				float audioLenght = audioSource.clip.length;
				float ranTime = Random.Range (minTimeSoundSFX, maxTimeSoundSFX);
				float nowTime = Time.time;
				nextSFXTime = ranTime + nowTime+ audioLenght;
			}
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
