using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour {

	public GameObject TopCrusher;
	public GameObject BottomCrusher;

	private float topDefaultY;
	private bool crushing = false;
	private bool bottomHit = false;
	public Animator anim;
	private int crusherCounter = 0;

	private timerLevelOne timerScript;
	private GameObject ScoreObject;
	private bool gameStart = false;

    public bool gameEnd = false;

	public bool getCrushing()
	{
		return crushing;
	}

	// Use this for initialization
	void Start () {
		topDefaultY = TopCrusher.transform.position.y;

		ScoreObject = GameObject.Find ("Score");
	}

	// Update is called once per frame
	void Update () {
		if (!gameStart) {
			timerScript = ScoreObject.GetComponent<timerLevelOne> ();
			gameStart = timerScript.gameStart;
		}

		if (crushing && !bottomHit && crusherCounter > 7 && gameStart) {
			TopCrusher.transform.position = new Vector3 (TopCrusher.transform.position.x, TopCrusher.transform.position.y - .4f * (Time.deltaTime * 60),
				TopCrusher.transform.position.z);
			if (BottomCrusher.transform.position.y+4f >= TopCrusher.transform.position.y)
				bottomHit = true;
		} else if (bottomHit) {
			TopCrusher.transform.position = new Vector3 (TopCrusher.transform.position.x, TopCrusher.transform.position.y + .4f * (Time.deltaTime * 60),
				TopCrusher.transform.position.z);
			if (TopCrusher.transform.position.y >= topDefaultY)
			{
				bottomHit = false;
				crushing = false;
			}
			crusherCounter = 0;
		}

		if (crusherCounter < 8 && crushing)
			crusherCounter++;
	}

	void OnMouseDown(){
		if (!crushing && gameStart && !gameEnd)
		{
			crushing = true;
			anim.SetTrigger ("Crush");	//Play lever animation
		}
	}

}
