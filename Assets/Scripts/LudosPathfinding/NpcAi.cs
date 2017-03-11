using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class NpcAi : MonoBehaviour {

	public enum AvailableNpcStates {
		Idle,
		Moving
	};

	public float speed;
	public Tile currentTile {get; private set;}
	public Tile destinationTile {get; private set;}
	public List<Tile> currentPath {get; private set;}
	public int progressionInPath {get; private set;}
	public AvailableNpcStates state {get; private set;}
	private GridManager grid;

	// Use this for initialization
	void Start () {
		grid = GameObject.Find ("Grid").GetComponent<GridManager> ();
		currentTile = GetCurrentTile();
		destinationTile = null;
		currentPath = null;
		progressionInPath = 0;
		state = AvailableNpcStates.Idle;
	}

	void Update () {
		if (state == AvailableNpcStates.Moving) {
			Move ();
		} else {
			ChooseNextTile ();
		}
	}

	//Moves the ghost to currentTile
	public void Move() {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, currentTile.go.transform.position, step);
		if (transform.position == currentTile.go.transform.position) {
			progressionInPath++;
			if (currentPath.Count > (progressionInPath)) {
				currentTile = currentPath [progressionInPath];
			} else {
				state = AvailableNpcStates.Idle;
				currentPath = null;
			}
		}
	}

	//Chooses the next tile to go and prepares the path
	public void ChooseNextTile() {
		currentPath = AStarPathfinding.GeneratePath(currentTile, GetDestinationTile(), grid);
		progressionInPath = 0;
		if (currentPath != null) {
			destinationTile = currentPath.Last();
			state = AvailableNpcStates.Moving;
		}
	}

	public Tile GetCurrentTile() {
		return grid.FindTileByCoordinates ((int)transform.position.x, (int)transform.position.y);
	}

	public Tile GetDestinationTile() {
		return grid.walkableTileList [UnityEngine.Random.Range (0, grid.walkableTileList.Count - 1)];
	}
}
