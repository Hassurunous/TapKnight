using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class CharactersController : MonoBehaviour {


	// Declare the SpriteRenderer and the Animator in the Editor so they can be edited in code
	// without needing to search for those components.
	[SerializeField]
	SpriteRenderer spriteRenderer;

	[SerializeField]
	Animator animator;


	// Make the sprite and the animatorController controllable from the Editor as well, that way
	// we can run tests on these. Later, we'll use these to create several character objects
	// using this controller component by simply passing in a sprite and animator controller.
	[SerializeField]
	Sprite sprite;

	[SerializeField]
	RuntimeAnimatorController animatorController;

	// Private variables so other classes can't directly update the values here
	// but public getters so that other classes can check them.

	// How much HP does this character start with?
	private int _hitpointsMax;
	public int hitpointsMax {
		get {
			return _hitpointsMax;
		}
	}

	// How much HP does this character have?
	private int _hitpoints;
	public int hitpoints {
		get {
			return _hitpoints;
		}
	}

	// What's the name of this character?
	[SerializeField]
	private string _name;
	public string name {
		get {
			return _name;
		}
	}

	// How much damage does this character do?
	private int _attackPower;
	public int attackPower {
		get {
			return _attackPower;
		}
	}

	// How much damage does this character block with their armor?
	private int _armor;
	public int armor {
		get {
			return _armor;
		}
	}


	// Methods for updating the values on the character. 

	// Call this method when the character takes some damage. 
	// If the damage exceeds their armor, deal that much damage, otherwise deal 1 damage. Return how much damage was dealt.
	public int TakeDamage(int damage) {
		int damageDealt = 0;
		if (damage > armor) {
			damageDealt = damage - armor;
		} else {
			damageDealt = 1;
		}
		_hitpoints -= damageDealt;
		return damageDealt;
	}


	// Call this method to update persistent values of a character (i.e. attackPower, HP, or Armor)
	// Pass in the name of the property you are updating and the new value
	public void UpdateProps(string propName, int newValue) {
		switch (propName) {
		case "armor":
			UpdateArmor (newValue);
			break;
		case "attack":
			UpdateAttack (newValue);
			break;
		case "hp":
			UpdateMaxHP (newValue);
			break;
		default:
			Debug.Log ("Inappropriate propName for updateProps. Exiting without update.");
			break;
		}
	}


	// Call this method to update armor value of the character
	void UpdateArmor(int newArmor) {
		_armor = newArmor;
	}

	// Call this method to update attack value of the character
	void UpdateAttack(int newAttack) {
		_attackPower = newAttack;
	}

	// Call this method to change the max hitpoints
	void UpdateMaxHP(int newMaxHP) {
		_hitpointsMax = newMaxHP;
	}

	// Set up the component given the values provided for sprite and animatorController
	void Setup() {
//		animatorController = animatorController;
//		sprite = sprite;
		spriteRenderer.sprite = sprite;
		animator.runtimeAnimatorController = animatorController;
	}
		

	// Use this for initialization
	void Start () {
		_hitpoints = _hitpointsMax;
		Setup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
