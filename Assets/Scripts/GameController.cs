using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameState currentGameState;

	// Track the player's current score. Useful depending on the game mode. For our initial MVP (infinite runner style) this will be the main user feedback.
	public static int Score = 0;

	// Singleton initialization of GameController static
	private static GameController _instance = null;
	public static GameController instance {
		get {
			return _instance;
		}
	}

	void Update() {
		switch (currentGameState) {
		case GameState.Menu:
//			CombatController
			break;
		}
	}

	void Awake()
	{
		//Check if instance already exists
		if (_instance == null) {
			//if not, set instance to this
			_instance = this;
		} else if (_instance != this) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (this.gameObject);
	}
}

// Controls logic of the game
public enum GameState {
	Menu,
	Map,
	Combat
}

