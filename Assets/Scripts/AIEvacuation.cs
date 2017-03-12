using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
using System.Linq;
using System.Collections.Generic;
public class AIEvacuation : AIPath {
	private GameObject sorties;
	private AstarPath pathEngine;
	private NavGraph grille;
	private List<GraphNode> dangersConnus;
	private Seeker chercheur;
	private Path cheminEnCours;
	private bool signalEvacuation;
	private bool signalAucuneIssue;
	private GameObject cibleIdle;
	private bool changementComportement;
	private float originalSpeed;

	public int distanceVision = 5;


	protected override void Start () {
		base.Start ();
		sorties = GameObject.Find ("Sorties de secours");
		pathEngine = GameObject.Find ("A*").GetComponent<AstarPath> ();
		grille = pathEngine.graphs[0];
		dangersConnus = new List<GraphNode> ();
		chercheur = this.gameObject.AddComponent<Seeker> ();
		cheminEnCours = null;
		signalEvacuation = false;
		signalAucuneIssue = false;
		cibleIdle = new GameObject ();
		changementComportement = true;
		this.originalSpeed = speed;
	}

	protected void ComportementSortiePlusProche() {
		if (sorties.transform.childCount > 0) {
			Debug.Log ("Comportement Sortie la plus proche.");
			List<GameObject> lesSorties = new List<GameObject> ();
			for (int i = 0, n = sorties.transform.childCount; i < n; ++i) {
				lesSorties.Add (sorties.transform.GetChild (i).gameObject);
			}
			AllerVersSortieLaPlusProche (lesSorties);
		} else {
			signalAucuneIssue = true;
			changementComportement = true;
		}
	}

	protected void ComportementAucuneIssue() {
		Debug.Log ("Comportement Aucune issue.");
		this.target = this.gameObject.transform;
	}

	protected void ComportementEvacuation() {
		this.speed = this.originalSpeed;
		if (signalAucuneIssue) {
			ComportementAucuneIssue ();
		} else {
			ComportementSortiePlusProche ();
		}
	}

	protected void ComportementIdle() {
		Debug.Log ("Comportement Idle.");
		List<GraphNode> nodes = PathUtilities.BFS (DonneNodeCourant (), this.distanceVision);
		int index = Random.Range (0, nodes.Count);
		cibleIdle.transform.position = (Vector3)nodes [index].position;
		this.target = cibleIdle.transform;
		this.speed = Random.Range (1, this.originalSpeed);
	}

	protected void ChercherDuDanger() {
		IEnumerable<GraphNode> dangers = DonneListeDangerVisible();
		foreach (GraphNode danger in dangers) { 
			if (!this.dangersConnus.Contains (danger)) {
				Debug.Log ("OMG, un nouveau danger!!!");
				signalEvacuation = true;
				changementComportement = true;
				dangersConnus.Add (danger);
			}
		}
	}

	public override void Update () {
		if (changementComportement) {
			changementComportement = false;
			if (signalEvacuation) {
				ComportementEvacuation ();
			} else {
				ComportementIdle ();
			}
		}
		ChercherDuDanger ();
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

	public override void OnTargetReached () {
		base.OnTargetReached ();
		if (!signalAucuneIssue) {
			changementComportement = true;
		}
	}

	protected void AllerVersSortieLaPlusProche(IEnumerable<GameObject> sorties) {
		IEnumerable<Vector3> positions = sorties.Select (x => (Vector3)this.DonneNodeAvecPosition (x.transform.position).position);
		IEnumerator<GameObject> eSorties = sorties.GetEnumerator ();
		IEnumerator<Vector3> ePositions = positions.GetEnumerator ();
		Path bestPath = cheminEnCours;
		Transform bestSortie = null;
		bool tousLesCheminsImpossibles = true;
		while (eSorties.MoveNext() && ePositions.MoveNext()) {
			Vector3 position = ePositions.Current;
			GameObject sortie = eSorties.Current;
			ABPathAvecExclusions p = ABPathAvecExclusions.Construct(GetFeetPosition(), position, this.dangersConnus);
			Path path = chercheur.StartPath (p);
			while (path.GetState() != PathState.ReturnQueue) path.WaitForPath();
			if (path.CompleteState == PathCompleteState.Complete) {
				tousLesCheminsImpossibles = false;
			}
			if (bestPath == null || path.GetTotalLength() < bestPath.GetTotalLength()) {
				bestPath = path;
				bestSortie = sortie.transform;
			}
		}
		if (tousLesCheminsImpossibles) {
			Debug.Log ("OMG!!! Il n'y a plus d'issues!!!");
			signalAucuneIssue = true;
			changementComportement = true;
		} else if (bestPath != this.cheminEnCours) {
			this.target = bestSortie;
			this.cheminEnCours = bestPath;
		}
	}

	protected GraphNode DonneNodeAvecPosition(Vector3 pos) {
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

	protected GraphNode DonneNodeCourant() {
		return DonneNodeAvecPosition (transform.position);
	}

	protected IEnumerable<GraphNode> DonneListeDangerVisible()
	{
		List<GraphNode> nodes = PathUtilities.BFS (DonneNodeCourant (), this.distanceVision);
		IEnumerable<GraphNode> filteredNodes;
		if (path != null) {
			filteredNodes = nodes.Intersect (this.path.path);
		} else {
			filteredNodes = nodes;
		}
		return filteredNodes.Where(x => (x.Tag & 0x1) == 0x1);
	}
} 
// Comportement en absence de dangers
// Comportement en présence de dangers et connaissance d'une issue potentielle
// Comportement en présence de dangers et élimination d'une issue potentielle
// Comportement en présence de dangers et absence d'issues
// Comportement lorsque victime d'un danger