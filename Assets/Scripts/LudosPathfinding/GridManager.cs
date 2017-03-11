using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour {

	public List<Tile> tileList { get; private set; }
	public List<Tile> walkableTileList { get; private set; }

	// Use this for initialization
	void Awake () {
		tileList = new List<Tile> ();
		walkableTileList = new List<Tile> ();
		foreach (Transform child in this.transform) {
			tileList.Add (new Tile(child.gameObject));
		}
		walkableTileList = GetTilesOfType (Tile.AvailableTileTypes.Ground);
		foreach(Tile tile in tileList) {
			FindNeighbors(tile);
		}
		PositionMainCamera ();
	}

	//Places the main camera on the game field and scales it
	public void PositionMainCamera() {
		Vector3 firstTilePosition = tileList.ElementAt(0).go.transform.position;
		Vector3 lastTilePosition = tileList.ElementAt(tileList.Count - 1).go.transform.position;
		Camera.main.transform.position = ((firstTilePosition - lastTilePosition) * 0.5f) + lastTilePosition;
		Camera.main.transform.Translate (new Vector3 (0, 0, -10));
		Camera.main.orthographicSize = 1;
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		while (!GeometryUtility.TestPlanesAABB (planes, tileList.ElementAt (0).go.GetComponent<SpriteRenderer> ().bounds)) {
			planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
			Camera.main.orthographicSize += 0.5f;
		}
	}

	//Finds a tile neighbors and store the data in an array that is copied to the tile reference object
	private void FindNeighbors(Tile tile) {
		//Horizontal
		FindNeighborAtCoordinates (tile.coordX + 1, tile.coordY, tile.neighborTiles);
		FindNeighborAtCoordinates (tile.coordX - 1, tile.coordY, tile.neighborTiles);
		//Vertical
		FindNeighborAtCoordinates (tile.coordX, tile.coordY + 1, tile.neighborTiles);
		FindNeighborAtCoordinates (tile.coordX, tile.coordY - 1, tile.neighborTiles);
		//Upper-Left
		FindNeighborAtCoordinates (tile.coordX - 1, tile.coordY + 1, tile.neighborTiles);
		//Upper-Right
		FindNeighborAtCoordinates (tile.coordX + 1, tile.coordY + 1, tile.neighborTiles);
		//Lower-Left
		FindNeighborAtCoordinates (tile.coordX - 1, tile.coordY - 1, tile.neighborTiles);
		//Lower-Right
		FindNeighborAtCoordinates (tile.coordX + 1, tile.coordY - 1, tile.neighborTiles);
	}

	private void FindNeighborAtCoordinates(int coordX, int coordY, List<Tile> neighborTiles) {
		Tile tile = FindTileByCoordinates (coordX, coordY);
		if (tile != null) {
			neighborTiles.Add (tile);
		}
	}
	
	//Finds a tile by its coordinates
	public Tile FindTileByCoordinates(int coordX, int coordY) {
		Tile tile = tileList.Find(t => t.Equals(new Tile(coordX, coordY)));
		return tile;
	}

	//Returns a list of all tiles of the specified type
	public List<Tile> GetTilesOfType(Tile.AvailableTileTypes type) {
		List<Tile> tilesOfType = new List<Tile> ();
		foreach (Tile tile in tileList) {
			if (tile.tileType == type) {
				tilesOfType.Add (tile);
			}		
		}
		return tilesOfType;
	}
}
