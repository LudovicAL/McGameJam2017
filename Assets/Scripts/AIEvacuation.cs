using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AIEvacuation : AIPath {
	private GameObject sorties;
	private GameObject dangers;


	public void Start () {
		base.Start ();
		sorties = GameObject.Find ("Sorties de secours");
		dangers = GameObject.Find ("Danger");
	}

	public void Update () {
		if (sorties.transform.childCount > 0) {
			Transform newTarget = sorties.transform.GetChild (0).transform;
			if (newTarget != base.target) {
				base.target = newTarget.transform;
				RepeatTrySearchPath ();
			}
		}
		base.Update ();
	}
} 
