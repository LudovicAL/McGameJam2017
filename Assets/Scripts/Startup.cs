using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Pathfinding;
public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject dangers = GameObject.Find ("Dangers");
		for (int i = 0, n = dangers.transform.childCount; i < n; ++i) {
			Transform danger = dangers.transform.GetChild (i);
			if (danger.gameObject.activeInHierarchy) {
				GraphNode node = NodeUtilities.DonneNodeAvecPosition (danger.position);
				node.Tag |= StaticData.BURNING_GROUND;
			}
		}
	}
}
