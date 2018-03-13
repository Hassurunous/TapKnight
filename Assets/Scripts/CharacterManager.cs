using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

	private Dictionary<string, GameObject> _characterDict = new Dictionary<string, GameObject>();

	[SerializeField]
	string[] prefabNames;

	[SerializeField]
	GameObject[] characterPrefabs;

	// Find the appropriate character, given a name
	public GameObject GetCharacterByName(string name) {
		GameObject character = _characterDict [name];
		if (character != null) {
			return character;
		}
		return null;
	}

	// Singleton initialization of GameController static
	private static CharacterManager _instance = null;
	public static CharacterManager instance {
		get {
			return _instance;
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

		for (int i = 0; i < prefabNames.Length; i++) {
			if (i < characterPrefabs.Length) {
				_characterDict [prefabNames [i]] = characterPrefabs [i];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
