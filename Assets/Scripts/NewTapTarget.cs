using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewTapTarget : MonoBehaviour, IPointerClickHandler { 

	// Reference for the TimerRing
    [SerializeField]
	GameObject TimerRing;

	// Timer Ring Sprite Renderer. Rather than look up every time, just store it initially.
	[SerializeField]
	SpriteRenderer timerRenderer;

	// Reference for the TargetRing
    [SerializeField]
	GameObject TargetRing;

    // How big is the outer ring (TimerRing) currently, compared to the size of the Target Ring
    float timerRingScale = 3.0f;

    // Interpolation parameter tells the percentage of the transformation that has taken place in the Lerp function
    float tParam = 0f;

	// We want to delay slightly after the rings align to destroy the ring to give users a bit of a chance to get the 100% click
	bool aligned = false;
	float alignTime = 0.0f;
    float delayTime;

    // TapTarget lifetime. Smaller number = faster target.
    [SerializeField]
    private float _lifetime = 0.0f;
    public float lifetime
    {
        get
        {
            return _lifetime;
        }
    }

	Color red = Color.red;
	Color green = Color.green;

	void Update()
	{
		// Scale the TimerRing down over time
		if (tParam < 1) {
            tParam += Time.deltaTime / _lifetime;//* decaySpeed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
			timerRingScale = Mathf.Lerp (3f, 1f, tParam);
			timerRenderer.color = Color.Lerp (red, green, tParam);
			TimerRing.transform.localScale = new Vector3 (timerRingScale, timerRingScale, 1f);
		} else if (aligned == false) {
			alignTime = Time.time;
			aligned = true;
		} else if (aligned && Time.time >= alignTime + delayTime) {
            //CombatController.Instance.TargetOutcome(this, false);
            TapTargetEventManager.TapTargetCall(transform.position, Accuracy(false));
        }

	}

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");
        TapTargetEventManager.TapTargetCall(transform.position, Accuracy(true));
    }

    // Use this for initialization
    void Start () {
	}

	void Awake() {
	}

    // Calculate the accuracy of the user and destroy the TapTarget. "hit" is false if the function is 
    // called for a missed target and will automatically return 0.
	public int Accuracy(bool hit) {
		Kill ();
		if (hit) {
			return (int) (100.0f / Mathf.Pow (2, timerRingScale - 1));
		} else {
			return 0;
		}
	}

	void Kill() {
        Destroy (this.gameObject);
	}

    public void TapTargetSetup(float newLifetime)
    {
        _lifetime = newLifetime;
        delayTime = newLifetime * 0.05f;
    }
}
