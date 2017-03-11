using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AI : MonoBehaviour {

	// Use this for initialization
	Transform currentNPC;
	bool isAlerted;
	float fireDetetectRadius;
	float hearingRadius;
	float screamingRadius;

	Transform t;
	void Start () {
		isAlerted = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SeekExit()
	{
		
	}

	void Scream()
	{
		//emit particles going outward from t
	}
		
}
