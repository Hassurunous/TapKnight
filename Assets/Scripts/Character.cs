using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Sometimes we'll want to disable this object, such as in the endless mode we will disable it for the enemy.
	[SerializeField]
	GameObject hitpointObject;

	// HP Bar is modified as the HP get lower, so we store a reference here for easy access.
	[SerializeField]
	GameObject hitpointBar;

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

    //Storing the audio file references for easy callback in the gameplay.
    [SerializeField]
    private AudioClip hurtSound;

    [SerializeField]
    private AudioClip clashSound;

    [SerializeField]
    private AudioClip missSound;

    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioClip perfectSound;

    [SerializeField]
    private AudioClip attackSound;
    // TODO: Create logic for playing individual sounds. Diverge the "Player Character" from the "Enemy Character" class,
    // so that we can eliminate unnecessary code. Possibly just need a setting to detect which it is, but it might be
    // better to simply make another class.


    //Having a separate audio source for the weapons and the character sounds means we can easily play the "miss" sound
    // and also play the "hurt" sound. Might end up separating these in a different way later (perhaps a different source
    // for each sound), but, for now, this is appropriate.
    [SerializeField]
    private AudioSource weaponSounds;

    [SerializeField]
    private AudioSource characterSounds;

	// Methods for updating the values on the character. 

	// Call this method when the character takes some damage. 
	// If the damage exceeds their armor, deal that much damage, otherwise deal 1 damage. Return how much damage was dealt.
	public void Hurt() {
		animator.SetTrigger ("takeDamage");
        if (missSound)
        {
            weaponSounds.clip = missSound;
            weaponSounds.Play();
        }
        if (hurtSound)
        {
            characterSounds.clip = hurtSound;
            characterSounds.Play();
        }
	}

    // Call this method when the character gets a "Close!". "Close!" hits will deal no damage, and will instead count as a 
    // clash, where both combatants have struck each other's weapon. Both characters will then recoil with the impact.
    public void Clash(float attackSpeed) {
        animator.SetTrigger("attack");
        animator.SetFloat("attackSpeed", attackSpeed);
        if (clashSound)
        {
            weaponSounds.clip = clashSound;
            weaponSounds.Play();
        }    
    }


	// Attack method triggers the appropriate animations and sounds. Optional clash parameter for "close" hits. 
	public void Attack(float attackSpeed) {
		animator.SetFloat ("attackSpeed", attackSpeed);
		animator.SetTrigger ("attack");
        if (attackSound)
        {
            weaponSounds.clip = attackSound;
            weaponSounds.Play();
        }
    }

    public void Attack(float attackSpeed, bool clash)
    {
        animator.SetFloat("attackSpeed", attackSpeed);
        animator.SetTrigger("attack");
        if (clashSound && clash)
        {
            weaponSounds.clip = clashSound;
            weaponSounds.Play();
        }
        else if (attackSound)
        {
            weaponSounds.clip = attackSound;
            weaponSounds.Play();
        }
    }

    // Call this method when the character takes AND receives damage
    private void AttackAndHurt(float attackSpeed) {
		animator.SetFloat ("attackSpeed", attackSpeed);
		animator.SetTrigger ("attack");
		animator.SetTrigger ("badAttack");
	}

	// Reduce the current hit points based on the amount of damage dealt
	public void TakeDamage(bool attackToo, int damage, float attackSpeed = 1.0f) {
		int damageDealt = 0;
		if (damage > armor) {
			damageDealt = damage - armor;
		} else {
			damageDealt = 1;
		}
		int newHitpoints = _hitpoints - damageDealt > 0 ? _hitpoints - damageDealt : 0;
		_hitpoints = newHitpoints;
		animator.SetInteger ("currentHP", newHitpoints);
		if (attackToo) {
			AttackAndHurt (attackSpeed);
		} else {
			Hurt ();
		}
        if (animator.GetInteger("currentHP") <= 0)
        {
            characterSounds.clip = deathSound;
            characterSounds.Play();
        }
		UpdateHPBar ();
	}

	// Increase the current hit points by a value, up to a maximum of _hitpointsMax
	public void GainHP(int percentage) {
		int newHitpoints = _hitpoints + (int)(_hitpointsMax * (percentage/100f)) < _hitpointsMax ? _hitpoints + (int)(_hitpointsMax * (percentage/100f))  : _hitpointsMax;
		_hitpoints = newHitpoints;
		animator.SetInteger ("currentHP", newHitpoints);
		UpdateHPBar ();
        if (perfectSound)
        {
            characterSounds.clip = perfectSound;
            characterSounds.Play();
        }
	}
		
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
		_hitpoints = _hitpointsMax;
	}

	// Call this method to change the sprite
	public void UpdateSprite(Sprite newSprite) {
		sprite = newSprite;
		spriteRenderer.sprite = sprite;
	}

	// Call this method to change the animator controller
	public void UpdateAnimatorController(RuntimeAnimatorController animController) {
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

	// Change the scale of the HP bar based on how much HP the character has
	void UpdateHPBar() {
		float hpScale = (_hitpoints / (float)hitpointsMax);
		hitpointBar.transform.localScale = new Vector2 (hpScale, 1f);
	}

	// Set up the component given the values provided for sprite and animatorController
	public void Setup() {
		UpdateSprite (sprite);
		animator.SetInteger ("currentHP", _hitpointsMax);
	}
		
		
	void Awake() {
//		Debug.Log ("Character wakeup. Character name = " + _name);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
