using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tile {

	public enum AvailableTileTypes {
		Ground,
		Wall,
		Roof,
		Stair,
		Door,
		Window,
		Alarm,
		Sprinkler,
		PropaneCanister,
		Furniture
	};

	public int coordX { get; private set; }
	public int coordY { get; private set; }
	public AvailableTileTypes tileType { get; private set; }
	public GameObject go { get; private set; }
	public List<Tile> neighborTiles;
	public List<Tile> walkableNeighborTiles;

	//Constructors
	public Tile(GameObject go) {
		this.go = go;
		this.coordX = (int)go.transform.position.x;
		this.coordY = (int)go.transform.position.y;
		this.tileType = GetTileType (go.tag);
		this.neighborTiles = new List<Tile> ();
		this.walkableNeighborTiles = new List<Tile> ();
	}

	public Tile(int coordX, int coordY) {
		this.coordX = coordX;
		this.coordY = coordY;
	}

	//Redefines the equality test (in order for the test to examine coordinates only)
	public override bool Equals(object other) {
		if (other != null && this.GetType() == other.GetType()) {
			Tile tempo = (Tile) other;
			if (this.coordX == tempo.coordX && this.coordY == tempo.coordY) {
				return true;
			}
		}
		return false;
	}

	public AvailableTileTypes GetTileType(String tag) {
		switch (tag) {
			case "Ground":
				return AvailableTileTypes.Ground;
				/*
			case "Wall":
				return AvailableTileTypes.Wall;
			case "Roof":
				return AvailableTileTypes.Roof;
			case "Stair":
				return AvailableTileTypes.Stair;
			case "Door":
				return AvailableTileTypes.Door;
			case "Window":
				return AvailableTileTypes.Window;
			case "Alarm":
				return AvailableTileTypes.Alarm;
			case "Sprinkler":
				return AvailableTileTypes.Sprinkler;
			case "PropaneCanister":
				return AvailableTileTypes.PropaneCanister;
			case "Furniture":
				return AvailableTileTypes.Furniture;
				*/
			default:
				return AvailableTileTypes.Ground;
		}
	}

	//Mandatory when redefining Equals
	public override int GetHashCode() {
		return base.GetHashCode ();
	}

	//Calculates the distance from this tile to another one
	public float DistanceTo(Tile other) {
		return Vector2.Distance(
			new Vector2(this.coordX, this.coordY),
			new Vector2(other.coordX, other.coordY)
		);
	}
}
