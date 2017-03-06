using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	private List<Controller> listOfAvailableContollers;
	public List<Player> listOfPlayers;

	// Use this for initialization
	void Start () {
		listOfPlayers = new List<Player> ();
		listOfAvailableContollers = new List<Controller>() {
			new Controller("KL"),	//Keyboard Left
			new Controller("KR"),	//Keyboard Right
			new Controller("C1"),	//Controller 1
			new Controller("C2"),	//Controller 2
			new Controller("C3"),	//Controller 3
			new Controller("C4"),	//Controller 4
			new Controller("C5"),	//Controller 5
			new Controller("C6"),	//Controller 6
			new Controller("C7"),	//Controller 7
			new Controller("C8"),	//Controller 8
			new Controller("C9"),	//Controller 9
			new Controller("C10"),	//Controller 10
			new Controller("C11")	//Controller 11
		};
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Controller controller in listOfAvailableContollers) {
			if (Input.GetButton(controller.buttonA)) {
				AddPlayer (controller);
				break;
			}
		}
		foreach (Player player in listOfPlayers) {
			if (Input.GetButton(player.controller.buttonB)) {
				RemovePlayer (player);
				break;
			}
		}
	}

	private void AddPlayer(Controller cm) {

	}

	private void RemovePlayer(Player p) {

	}
}
