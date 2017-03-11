using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class ABPathAvecExclusions : ABPath {

	private List<GraphNode> exclusions;

	public static ABPathAvecExclusions Construct (Vector3 start, Vector3 end, List<GraphNode> exclusions) {
		var p = PathPool.GetPath<ABPathAvecExclusions> ();
		p.Setup (start, end, null);
		p.exclusions = exclusions;
		return p;
	}

	public override bool CanTraverse(GraphNode node) {
		return (exclusions == null || !exclusions.Contains (node)) && base.CanTraverse(node);
	}

}
