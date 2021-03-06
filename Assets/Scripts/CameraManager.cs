﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CameraManager : MonoBehaviour {

	public float cameraTranslationSpeed;
	public int cameraZoomSpeed;
	public int maxZoom;
	public int minZoom;
	private Camera cam;
	private float minX;
	private float maxX;
	private float minZ;
	private float maxZ;
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
		cam = this.gameObject.GetComponent<Camera> ();
		GridGraph gridGraph = AstarPath.active.astarData.gridGraph;
		GridNode[] gn = gridGraph.nodes;
		minX = float.MaxValue;
		maxX = float.MinValue;
		minZ = float.MaxValue;
		maxZ = float.MinValue;
		foreach (GridNode n in gn) {
			if (((Vector3)n.position).x < minX){
				minX = ((Vector3)n.position).x;
			} else if (((Vector3)n.position).x > maxX){
				maxX = ((Vector3)n.position).x;
			}
			if (((Vector3)n.position).z < minZ){
				minZ = ((Vector3)n.position).z;
			} else if (((Vector3)n.position).z > maxZ){
				maxZ = ((Vector3)n.position).z;
			}
		}
		cameraTranslationSpeed *= gridGraph.nodeSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == StaticData.AvailableGameStates.Playing) {
			if (Input.GetAxis("Vertical") != 0) {
				MoveVertical ();
			}
			if (Input.GetAxis("Horizontal") != 0) {
				MoveHorizontal ();
			}
			if (Input.GetAxis("Mouse ScrollWheel") != 0){
				Zoom ();
			}
		}
	}

	private void MoveVertical() {
		if (Input.GetAxis("Vertical") < 0 && this.transform.position.z > minZ) {
			this.transform.position = this.transform.position + (Vector3.back * cameraTranslationSpeed);
		} else if (Input.GetAxis("Vertical") > 0 && this.transform.position.z < maxZ) {
			this.transform.position = this.transform.position + (Vector3.forward * cameraTranslationSpeed);
		}
	}

	private void MoveHorizontal() {
		if (Input.GetAxis("Horizontal") < 0 && this.transform.position.x > minX) {
			this.transform.position = this.transform.position + (Vector3.left * cameraTranslationSpeed);
		} else if (Input.GetAxis("Horizontal") > 0 && this.transform.position.x < maxX) {
			this.transform.position = this.transform.position + (Vector3.right * cameraTranslationSpeed);
		}
	}

	private void Zoom() {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 && cam.orthographicSize > minZoom) {
			cam.orthographicSize = Mathf.Max (1, cam.orthographicSize - cameraZoomSpeed);

		} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && cam.orthographicSize < maxZoom) {
			cam.orthographicSize = Mathf.Min(maxZoom, cam.orthographicSize + cameraZoomSpeed);
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
