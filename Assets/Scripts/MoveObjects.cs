using UnityEngine;
using System.Collections;

public class MoveObjects : MonoBehaviour {

	GameObject GridObject;
	CrusherController gridScript;

	GameObject LeverObject;
	LeverController leverScript;

	private bool shrinkAndDestroy = false;

	private bool clicked = false;
	private Vector3 mousePosition;
	private float moveSpeed = 1f;
	private float speed;
	private bool onConveyor = true;
	private float defaultY;

	private bool lerp = false;		//Conveyor Belt Lerp
	private float startTime;
	private float journeyLength;
	private float fracJourney;
	private float distCovered;
	private float sizeCounter = 1.5f;
	private float lerpSpeed = 8f;	//Always the same on any lerp object

	private timerLevelOne timerScript;
	private GameObject ScoreObject;
	private bool gameStart = false;

    public bool gameEnd = false;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (.9f, .9f, 1);

		defaultY = transform.position.y;	//Get default for putting item back on conveyor if accidental pick-up

		LeverObject = GameObject.Find("Lever");
		GridObject = GameObject.Find ("Grid");
		ScoreObject = GameObject.Find ("Score");
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameStart || !gameEnd) {
			timerScript = ScoreObject.GetComponent<timerLevelOne> ();
			gameStart = timerScript.gameStart;
            gameEnd = timerScript.gameEnd;
		}

        if (lerp) {
			distCovered = (Time.time - startTime) * lerpSpeed;
			fracJourney = distCovered / journeyLength;
			if (sizeCounter > 1.1f) {
				sizeCounter -= .05f * (Time.deltaTime * 60);
				transform.localScale = new Vector3 (sizeCounter, sizeCounter, 1);
			}
			transform.position = Vector3.Lerp (transform.position, 
				new Vector3 (transform.position.x, defaultY, transform.position.z), fracJourney);
			if (transform.position.y == defaultY) {
				lerp = false;
				transform.localScale = new Vector3 (1.05f, 1.05f, 1);
			}
		}

		gridScript = GridObject.GetComponent<CrusherController> ();
		speed = gridScript.speed;

		if (shrinkAndDestroy) {
			if (transform.localScale.x > .2f)
				transform.localScale = new Vector3 (transform.localScale.x - .1f * (Time.deltaTime * 60), transform.localScale.y - .1f * (Time.deltaTime * 60), 1);
			else
				Destroy(gameObject);
		}
			
		if (!clicked && onConveyor && (transform.position.y == defaultY)) {
			transform.position = new Vector3 (
				transform.position.x+speed * (Time.deltaTime * 60), defaultY, transform.position.z);
			if (transform.position.x > 9.5f)
				Destroy (gameObject);
		} else if (!clicked && onConveyor && !lerp) {
			lerp = true;
			journeyLength = Vector3.Distance (transform.position,new Vector3(transform.position.x, defaultY, transform.position.z));
			fracJourney = lerpSpeed / journeyLength;
			startTime = Time.time;
			sizeCounter = 1.5f;
		} else {
			if (Input.GetButton ("Fire1") && onConveyor && !gameEnd) {	//Fix later on
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
				transform.position = Vector2.Lerp (transform.position, mousePosition, moveSpeed);
			} else if (onConveyor) {
				clicked = false;
				transform.localScale = new Vector3 (.9f, .9f, 1);
			}
		}
	}

	void OnMouseDown(){
		if (!onConveyor && !clicked && gameStart) {
			gridScript = GridObject.GetComponent<CrusherController> ();

			if (tag == "Plastic")	//Removes from count before destroying
				gridScript.plasticCount--;
			else if (tag == "Cardboard")
				gridScript.cardboardCount--;
			else if (tag == "Glass")
				gridScript.glassCount--;
			else if (tag == "Metal")
				gridScript.metalCount--;
				
			shrinkAndDestroy = true;
		} else if (gameStart) {
			clicked = true;
			lerp = false;
			transform.localScale = new Vector3 (1.5f,1.5f,1);	//Enlarges object slightly for easier viewing
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Crusher")) {
				clicked = false;
				onConveyor = false;
		} else if (other.gameObject.CompareTag ("CrusherTop")) {
			leverScript = LeverObject.GetComponent<LeverController>();
			if (leverScript.getCrushing ()) {
				gridScript = GridObject.GetComponent<CrusherController> ();
				Destroy (gameObject);
			}
		}
	}
}