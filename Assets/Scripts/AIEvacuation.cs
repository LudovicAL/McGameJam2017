using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
using System.Linq;
using System.Collections.Generic;
public class AIEvacuation : AIPath {
	private GameObject sorties;
	private List<GraphNode> dangersConnus;
	private Seeker chercheur;
	private Path cheminEnCours;
	public bool signalEvacuation { get; private set; }
	public bool signalAucuneIssue { get; private set; }
	private GameObject cibleIdle;
	private bool changementComportement;
	private float originalSpeed;
	private StaticData.AvailableGameStates gameState;
	private GameObject scriptBucket;

	public int distanceVision = 5;

	public void SetSignalEvacuation(bool value) {
		signalEvacuation = value;
		changementComportement = true;
	}

	protected override void Start () {
		base.Start ();
		sorties = GameObject.Find ("Sorties de secours");
		dangersConnus = new List<GraphNode> ();
		chercheur = this.gameObject.AddComponent<Seeker> ();
		cheminEnCours = null;
		signalEvacuation = false;
		signalAucuneIssue = false;
		cibleIdle = new GameObject ();
		cibleIdle.transform.name = transform.name + " (Target)";
		changementComportement = true;
		this.originalSpeed = speed;
		scriptBucket = StaticData.GetScriptBucket ();
		GameStatesManager mgr = scriptBucket.GetComponent<GameStatesManager> ();
		mgr.MenuGameState.AddListener (OnMenu);
		mgr.StartingGameState.AddListener (OnStart);
		mgr.PlayingGameState.AddListener (OnPlay);
		mgr.PausedGameState.AddListener (OnPause);
	}

	protected void ComportementSortiePlusProche() {
		if (sorties.transform.childCount > 0) {
			Debug.Log ("Comportement Sortie la plus proche.");
			AllerVersSortieLaPlusProche (this.DonneObjetsSorties());
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
		List<GraphNode> nodes = PathUtilities.BFS (DonneNodeCourant (), this.distanceVision * 2);
		int index = Random.Range (0, nodes.Count);
		cibleIdle.transform.position = (Vector3)nodes [index].position;
		this.target = cibleIdle.transform;
		this.speed = Random.Range (Mathf.Max(1, (int)(this.originalSpeed * 0.25)), Mathf.Max(1, (int)(this.originalSpeed * 0.75)));
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
		if (gameState == StaticData.AvailableGameStates.Playing) {
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
			if (DonneNodesSorties ().Contains (this.DonneNodeCourant ())) {
				//GameObject.Destroy (this.gameObject);
				this.gameObject.SetActive(false);
				Debug.Log ("JE SUIS SAUVÉ!!!");
			} else {
				changementComportement = true;
			}
		}
	}

	protected void AllerVersSortieLaPlusProche(IEnumerable<GameObject> sorties) {
		IEnumerable<Vector3> positions = sorties.Select (x => (Vector3)NodeUtilities.DonneNodeAvecPosition (x.transform.position).position);
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

	protected IEnumerable<GraphNode> DonneNodesSorties() {
		return DonneObjetsSorties().Select (x => NodeUtilities.DonneNodeAvecPosition (x.transform.position));
	}

	protected List<GameObject> DonneObjetsSorties() {
		List<GameObject> lesSorties = new List<GameObject> ();
		for (int i = 0, n = sorties.transform.childCount; i < n; ++i) {
			lesSorties.Add (sorties.transform.GetChild (i).gameObject);
		}
		return lesSorties;
	}

	public GraphNode DonneNodeCourant() {
		return NodeUtilities.DonneNodeAvecPosition (transform.position);
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
		return filteredNodes.Where(x => (x.Tag & StaticData.BURNING_GROUND) == StaticData.BURNING_GROUND);
	}

	public void OnMenu() {
		SetCanvasState (StaticData.AvailableGameStates.Menu);
	}

	public void OnStart() {
		SetCanvasState (StaticData.AvailableGameStates.Starting);
	}

	public void OnPlay() {
		SetCanvasState (StaticData.AvailableGameStates.Playing);
	}

	public void OnPause() {
		SetCanvasState (StaticData.AvailableGameStates.Paused);
	}

	public void SetCanvasState(StaticData.AvailableGameStates state) {
		gameState = state;
	}
} 
// Comportement en absence de dangers
// Comportement en présence de dangers et connaissance d'une issue potentielle
// Comportement en présence de dangers et élimination d'une issue potentielle
// Comportement en présence de dangers et absence d'issues
// Comportement lorsque victime d'un danger