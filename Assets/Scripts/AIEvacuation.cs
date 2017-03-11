using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
using System.Linq;
using System.Collections.Generic;
public class AIEvacuation : AIPath {
	private GameObject sorties;
	private GameObject dangers;
	private AstarPath pathEngine;
	private NavGraph grille;
	private List<GraphNode> dangersConnus;

	public int distanceVision = 5;


	protected override void Start () {
		base.Start ();
		sorties = GameObject.Find ("Sorties de secours");
		dangers = GameObject.Find ("Danger");
		pathEngine = GameObject.Find ("A*").GetComponent<AstarPath> ();
		grille = pathEngine.graphs[0];
		dangersConnus = new List<GraphNode> ();
	}

	public override void Update () {
		if (sorties.transform.childCount > 0) {
			Transform newTarget = sorties.transform.GetChild (0).transform;
			if (newTarget != base.target) {
				base.target = newTarget.transform;
			}
		}
		GraphNode danger = DangerVisibleSurChemin();
		if (danger != null) {
			dangersConnus.Add (danger);
		}
		base.Update ();
	}

	/** Requests a path to the target */
	public override void SearchPath () {
		if (target == null) throw new System.InvalidOperationException("Target is null");

		lastRepath = Time.time;
		//This is where we should search to
		Vector3 targetPosition = target.position;

		canSearchAgain = false;

		//Alternative way of requesting the path
		//ABPath p = ABPath.Construct (GetFeetPosition(),targetPosition,null);
		//seeker.StartPath (p);

		//We should search from the current position
		ABPathAvecExclusions p = ABPathAvecExclusions.Construct(GetFeetPosition(), targetPosition, this.dangersConnus);
		seeker.StartPath(p);
	}

	protected GraphNode DonneNodeAvecPosition(Vector3 pos) {
		NNConstraint contrainte = new NNConstraint ();
		contrainte.constrainDistance = true;
		contrainte.constrainWalkability = true;
		contrainte.walkable = true;
		pathEngine.maxNearestNodeDistance = 1;
		NNInfo info = grille.GetNearestForce (pos, contrainte);
		return info.node;
	}

	protected GraphNode DonneNodeCourant() {
		return DonneNodeAvecPosition (transform.position);
	}

	protected GraphNode DangerVisibleSurChemin()
	{
		if (path != null && path.vectorPath != null) {
			Vector3 pos = this.transform.position;
			int i = currentWaypointIndex, n = path.vectorPath.Count;
			for (; i < n; ++i) {
				if (Mathf.Sqrt(VectorMath.SqrDistanceXZ (pos, path.vectorPath [i])) < distanceVision) {
					GraphNode node = DonneNodeAvecPosition(path.vectorPath[i]);
					if (node.Tag == 0x1) {
						Debug.Log ("OMG, du danger!!! " + node.GetHashCode());
						return node;
					}
				} else {
					break;
				}
			}
		}
		return null;
	}
} 
// Comportement en absence de dangers
// Comportement en présence de dangers et connaissance d'une issue potentielle
// Comportement en présence de dangers et élimination d'une issue potentielle
// Comportement en présence de dangers et absence d'issues
// Comportement lorsque victime d'un danger