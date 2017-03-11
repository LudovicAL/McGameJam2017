using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_extra : NpcAi {

	// Use this for initialization
	Transform currentNPC;
	bool isAlerted;
	float fireDetetectRadius;
	float hearingRadius;
	float screamingRadius;


	private GridManager meinGrid;
	public List<Tile> doors = null;
	public List<Tile> windows = null;
	//Stuff commented out are inaccessible due to access rights.
	//code here is under the assumption that they can be accessed correctly
	Transform t;


	void Start () {
	//base.Start();

		doors = meinGrid.GetTilesOfType (Tile.AvailableTileTypes.Door);
		windows = meinGrid.GetTilesOfType (Tile.AvailableTileTypes.Window);

	}
	
	// Update is called once per frame
	void Update () {
		/*if(state = AvailableNpcStates.Idle)
		{
			if (isAlerted) {
			Move (1.5f);
			} else {
			Move ();
		}
			
		}
		else{
			ChooseNextTile ();
		}*/
	}

	//Prametized Move, I don't want to rewrite on Ludo's code so I'll overload it for now, until an opinion is given
	void Move(float speedMultiplier)
	{
		float step = speedMultiplier*speed * Time.deltaTime;
		/*
		 * Exact same code as base.Move()
		 * 
		 * */

	}

	void SeekExit()
	{
		
	}

	void Scream()
	{
		//emit particles going outward from t
	}
	// WIP, meant to make the units prefer doors, then windows
	// Planning to add stairs in order to move up, which would simply require reloading the current floor's doors and windows
	public Tile GetDestinationTile()
	{
		Tile temp = null;
		if (isAlerted)
		{
			if (doors.Count > 0) {   
				
				temp = doors [UnityEngine.Random.Range (0, doors.Count - 1)];
				doors.Remove (temp);

			} else
				if(windows.Count > 0)
				{

					temp = windows [UnityEngine.Random.Range (0, windows.Count - 1)];
					windows.Remove (temp);

				}
			return temp;

		} else
			return base.GetDestinationTile ();
	}
}
