//This static class stores information that is relevant application-wide.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData {

	public enum AvailableGameStates {
		Menu,	//Consulting the menu
		Starting,	//Game is starting
		Playing,	//Game is playing
		Paused	//Game is paused
	};

	public enum AvailableFireState {
		Intact,	//GameObject is un-burned
		Burning,	//GameObject is burning
		Burnt	//GameObject has burned already
	};
}
