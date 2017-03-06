public class Player {
	public string name {get; private set;}
	public Controller controller {get; private set;}

	public Player(string name, Controller controller) {
		this.name = name;
		this.controller = controller;
	}
}
