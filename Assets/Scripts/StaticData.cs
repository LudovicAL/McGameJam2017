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
		
	public const int BURNING_GROUND = 0x1 << 0;
	public const int BURNT_GROUND = 0x1 << 1;
	public const int OBSTACLE = 0x1 << 2;
	public const int UNLOCKED_DOOR = 0x1 << 3;
	public const int UNLOCKED_WINDOW = 0x1 << 4;
	public const int LOCKED_DOOR = 0x1 << 5;
	public const int LOCKED_WINDOW = 0x1 << 6;
	public const int WALL = 0x1 << 7;
	public const int STAIR = 0x1 << 8;
	public const int EXTERIOR = 0x1 << 9;
}
