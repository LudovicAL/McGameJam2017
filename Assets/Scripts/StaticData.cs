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

	public const int BASIC_GROUND = 0;
	public const int BURNING_GROUND = 1;
	public const int BURNT_GROUND = 2;
	public const int OBSTACLE = 3;
	public const int UNLOCKED_DOOR = 4;
	public const int UNLOCKED_WINDOW = 5;
	public const int LOCKED_DOOR = 6;
	public const int LOCKED_WINDOW = 7;
	public const int WALL = 8;
	public const int STAIR = 9;
	public const int EXTERIOR = 10;
}
