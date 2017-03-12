using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NodeUtilities {

	private static AstarPath pathEngine = null;
	private static NavGraph grille = null;

	public static GraphNode DonneNodeAvecPosition(Vector3 pos) {
		if (pathEngine == null) {
			pathEngine = AstarPath.active;
			grille = pathEngine.astarData.gridGraph;
		}
		float maxNearestNodeDistance = pathEngine.maxNearestNodeDistance;
		NNConstraint contrainte = new NNConstraint ();
		contrainte.constrainDistance = true;
		contrainte.constrainWalkability = true;
		contrainte.walkable = true;
		pathEngine.maxNearestNodeDistance = 1;
		NNInfo info = grille.GetNearestForce (pos, contrainte);
		pathEngine.maxNearestNodeDistance = maxNearestNodeDistance;
		return info.node;
	}

}
