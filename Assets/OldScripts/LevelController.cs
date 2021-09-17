using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	private int level = 1;	//Controls Conveyor Speed & complexity of smelting blueprints
	private int score = 0;	//Determines the level
	private int counter = 0;

	public GameObject Plastic;
	public GameObject Cardboard;
	public GameObject Glass;

	public int getLevel()
	{
		return level;
	}

	public void LevelUp()	//Changes Conveyor Speed & complexity of smelting blueprints
	{
		level++;
	}	

	public void MoveConveyor()
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y-.05f, transform.position.z);

		if (transform.position.y < -5)
			transform.position = new Vector3 (transform.position.x, 5, transform.position.z);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (score > level)	//This is crap, needs to be replaced eventually
		{
			LevelUp ();
		}

		MoveConveyor ();

		if (counter > 30) {
			int randomNumber = UnityEngine.Random.Range (0, 3);
			switch(randomNumber)
			{
			case 0:
				Instantiate (Plastic, new Vector3 (8, 5.6f, -1), Quaternion.identity);
				break;
			case 1:
				Instantiate (Cardboard, new Vector3 (8, 5.6f, -1), Quaternion.identity);
				break;
			case 2:
				Instantiate (Glass, new Vector3 (8, 5.6f, -1), Quaternion.identity);
				break;
			}
			//Instantiate (Conveyor, new Vector3 (0, Screen.height/2,1), Quaternion.identity);
			counter = 0;
		}
		else
			counter++;
	}

}
