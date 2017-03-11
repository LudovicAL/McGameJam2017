using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpreader : MonoBehaviour {

	public float chanceOfSpreadingToAdjacentTile; //A number between 0 and 1
	public float chanceOfSpreadingToNPC; //A number between 0 and 1
	public float spreadingDelay;	//A number superior to 0
	private float timeSinceLastSpread;

	void Start(){
		timeSinceLastSpread = 0.0f;
		chanceOfSpreadingToAdjacentTile = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		chanceOfSpreadingToNPC = Mathf.Clamp (chanceOfSpreadingToAdjacentTile, 0.0f, 1.0f);
		spreadingDelay = Mathf.Max (0.0f, spreadingDelay);
	}

	// Update is called once per frame
	void Update () {
		if (HasIntactAdjacentTiles()) {
			timeSinceLastSpread += Time.deltaTime;
			if (timeSinceLastSpread < spreadingDelay) {
				SpreadToAdjacentTile ();
				timeSinceLastSpread = 0.0f;
			}
		}
	}

	private void SpreadToAdjacentTile(){
		float rnd = Random.Range (0.0f, 1.0f);
		if (rnd <= chanceOfSpreadingToAdjacentTile) {	//Fire is spreading!

		}
	}

	private bool HasIntactAdjacentTiles() {
		return false;
	}
}
