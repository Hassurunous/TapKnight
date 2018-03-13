using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class Character : MonoBehaviour {


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
	[SerializeField]
	private int _hitpointsMax = 1;
	public int hitpointsMax {
		get {
			return _hitpointsMax;
		}
	}

	// How much HP does this character have?
	[SerializeField]
	private int _hitpoints = 0;
	public int hitpoints {
		get {
			return _hitpoints;
		}
	}

	// What's the name of this character?
	[SerializeField]
	private string _name = "";
	public string charName {
		get {
			return _name;
		}
	}

	// How much damage does this character do?
	[SerializeField]
	private int _attackPower = 0;
	public int attackPower {
		get {
			return _attackPower;
		}
	}

	// How much damage does this character block with their armor?
	[SerializeField]
	private int _armor = 0;
	public int armor {
		get {
			return _armor;
		}
	}

	// Is this an enemy or a player character?
	// Enemies face to the left, players face to the right. 
	[SerializeField]
	private bool _enemy = false;
	public bool enemy {
		get {
			return _enemy;
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
//	public void UpdateProps<T>(string propName, T newData) {
//		switch (propName) {
//		case "armor":
//			UpdateArmor ((int)newData);
//			break;
//		case "attack":
//			UpdateAttack ((int)newData);
//			break;
//		case "hp":
//			UpdateMaxHP ((int)newData);
//			break;
//		case "sprite":
//			UpdateSprite ((Sprite)newData);
//			break;
//		case "anim":
//			UpdateAnimatorController ((AnimatorController)newData);
//			break;
//		default:
//			Debug.Log ("Inappropriate propName for updateProps. Exiting without update.");
//			break;
//		}
//	}


	// Call this method to update armor value of the character
	public void UpdateArmor(int newArmor) {
		_armor = newArmor;
	}

	// Call this method to update attack value of the character
	public void UpdateAttack(int newAttack) {
		_attackPower = newAttack;
	}

	// Call this method to change the max hitpoints
	public void UpdateMaxHP(int newMaxHP) {
		_hitpointsMax = newMaxHP;
	}

	// Call this method to change the sprite
	public void UpdateSprite(Sprite newSprite) {
		sprite = newSprite;
		spriteRenderer.sprite = sprite;
	}

	// Call this method to change the animator controller
	public void UpdateAnimatorController(AnimatorController animController) {
		animatorController = animController;
		animator.runtimeAnimatorController = animatorController;
	}

	public void CopyOtherCharacter(Character character) {
		sprite = character.sprite;
		spriteRenderer.sprite = sprite;
		animatorController = character.animatorController;
		animator.runtimeAnimatorController = animatorController;

		_hitpointsMax = character.hitpointsMax;
		_hitpoints = character.hitpoints;
		_name = character.name;
		_armor = character.armor;
		_attackPower = character.attackPower;
		_enemy = character.enemy;
	}

	// Set up the component given the values provided for sprite and animatorController
	void Setup() {
	}
		
		
	void Awake() {
		Debug.Log ("Character wakeup. Character name = " + _name);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
