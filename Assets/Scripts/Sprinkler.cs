using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Sprinkler : MonoBehaviour {

	// Use this for initialization
	private GameObject sprinkler;

	GameObject partSys;

	void Start () {
		sprinkler = this.gameObject;
		partSys =  sprinkler.transform.Find("Particle System").gameObject.GetComponent<Transform>().gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ExtinguishFire()
	{
		
	}
}
