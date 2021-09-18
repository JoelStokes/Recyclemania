using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Required for text manipulation

public class CrusherController : MonoBehaviour {

	const int BASE = 25;

	public int level = 1;	//Controls Conveyor Speed & complexity of smelting blueprints (Public for moveObjects.cs)`
	private int score = 0;	//Determines the level
	private int multiplier = 1;
	private int matchCounter = 0;
	private float counter = 0;	//Controls how long until next object spawn
	private const int COUNTER_DEFAULT = 23;		//20 in old
	private const int POST_METAL_COUNTER = 43;	//40 in old
	private float respawnRate;
	private int levelMetalStart = 4;
	public float speed;

	public int glassCount = 0;	//Public so MoveObject can properly delete
	public int plasticCount = 0;
	public int cardboardCount = 0;
	public int metalCount = 0;
	private bool crushCheck = false;
	private float crushCounter = 0;	//Creates delay after lever press and before crusher movement downwards

	private bool lerp1 = false;	//Grid Lerp 1
	private int lerpArray1;
	private GameObject lerpingObject1;
	private float startTime1;
	private float journeyLength1;
	private float fracJourney1;
	private float distCovered1;
	private float sizeCounter1 = 1.5f;
	private bool lerp2 = false;	//Grid Lerp 2
	private int lerpArray2;
	private GameObject lerpingObject2;
	private float startTime2;
	private float journeyLength2;
	private float fracJourney2;
	private float distCovered2;
	private float sizeCounter2 = 1.5f;
	private float lerpSpeed = 8f;	//Always the same on any lerp object

	public bool shrinkAndDestroy = false;
	private GameObject shrinkObject;

	public GameObject Plastic;
	public GameObject Cardboard;
	public GameObject Glass;
	public GameObject Metal;
	public GameObject Conveyor;
	private GameObject LeverObject;

	public GameObject Junk;	//Used for crusher creation after compressing raw materials
	public GameObject Bear;
	public GameObject Backpack;
	public GameObject License;
	public GameObject Newspaper;
	public GameObject Wheel;
	public GameObject BlueprintConsole; //Used for spawn position of creations
	public GameObject BackgroundConveyor;
	public float backgroundConveyorCounter = 0;

	public int plasticNumber;
	public int cardboardNumber;
	public int glassNumber;
	public int metalNumber;
	public GameObject PlasticNumberText;
	public GameObject CardboardNumberText;
	public GameObject GlassNumberText;
	public GameObject MetalNumberText;
	public GameObject PlasticNumberBox;
	public GameObject CardboardNumberBox;
	public GameObject GlassNumberBox;
	public GameObject MetalNumberBox;
	private float[] numberTextX;
	private float numberBoxAddendum = .3f;

	public GameObject ScoreObject;
	public GameObject MultiplierObject;

	public SpriteRenderer SpeedUp;
	public SpriteRenderer Warning;
	public TextMesh GetReady;
	public TextMesh Go;
	private float speedUpStartY;
	private int speedUpCounter = 0;
	private bool speedUpWait = false;
	public SpriteRenderer X;
	public SpriteRenderer Checkmark;
	public int checkmarkCounter = 0;

	private LeverController leverScript;
	private GridCube gridCubeScript;
	private timerLevelOne timerScript;

	private float blueprintCounter = 0;	//Used at start to randomize screen before game has begun
	private bool gameStart = false;

	public GameObject[] GridCubeArray;	//Default 12, can change if decided not difficult enough/too difficult

    private bool gameEnd = false;

	public void MoveConveyor()	//Animates conveyor belt movement
	{
		Conveyor.transform.position = new Vector3 (Conveyor.transform.position.x+speed*(Time.deltaTime * 60), Conveyor.transform.position.y, 
			Conveyor.transform.position.z);

		if (Conveyor.transform.position.x >=2.3398)
			Conveyor.transform.position = new Vector3 (-2.7f, Conveyor.transform.position.y, Conveyor.transform.position.z);
	}

	public void LevelUp()	//Occurs every level up, adds level, gets new values
	{
		level++;

		if (score > 0){
			speedUpWait = true;
		}
			
		if (level < levelMetalStart+1)
			speed = .03f + (level * .015f);
		else
			speed = .03f + ((level-4) * .015f);

		if (level == levelMetalStart+2)
			respawnRate -=2;
		else
			respawnRate -= 3;
		
		if (level == 2)
			respawnRate = COUNTER_DEFAULT + 6;
		if (level == levelMetalStart + 1)
			respawnRate = POST_METAL_COUNTER;
	}

	public void newBlueprint()	//Sets new numbers for blueprint
	{
		do {
			plasticNumber = UnityEngine.Random.Range (0, (Mathf.CeilToInt (level / 2)) + 2);
			cardboardNumber = UnityEngine.Random.Range (0, (Mathf.CeilToInt (level / 2)) + 2);
			glassNumber = UnityEngine.Random.Range (0, (Mathf.CeilToInt (level / 2)) + 2);
			if (level > levelMetalStart)
				metalNumber = UnityEngine.Random.Range (0, (Mathf.CeilToInt (level / 2)) + 2);
			else
				metalNumber = 0;
		} while (((plasticNumber + cardboardNumber + glassNumber+metalNumber < (level/2)+1) ||
			(plasticNumber+cardboardNumber+glassNumber+metalNumber >12)) ||
			(plasticNumber + cardboardNumber + glassNumber+metalNumber > (level/2)+2));

		if (level == levelMetalStart) {	//Random 3 material positions on level before Metal
			int random = UnityEngine.Random.Range (4, 7);
			PlasticNumberText.transform.position = new Vector3 (numberTextX [random],
				PlasticNumberText.transform.position.y, PlasticNumberText.transform.position.z);
			int random2 = random;
			while (random == random2) {
				random2 = UnityEngine.Random.Range (4, 7);
			}
			CardboardNumberText.transform.position = new Vector3 (numberTextX [random2],
				CardboardNumberText.transform.position.y, CardboardNumberText.transform.position.z);
			int random3 = random2;
			while (random == random3 || random2 == random3) {
				random3 = UnityEngine.Random.Range (4, 7);
			}
			GlassNumberText.transform.position = new Vector3 (numberTextX [random3],
				GlassNumberText.transform.position.y, GlassNumberText.transform.position.z);
		} else if (level > levelMetalStart + 1) {	//Random for all 4 after metal start
			int random = UnityEngine.Random.Range (0, 4);
			PlasticNumberText.transform.position = new Vector3 (numberTextX [random],
				PlasticNumberText.transform.position.y, PlasticNumberText.transform.position.z);
			int random2 = random;
			while (random == random2) {
				random2 = UnityEngine.Random.Range (0, 4);
			}
			CardboardNumberText.transform.position = new Vector3 (numberTextX [random2],
				CardboardNumberText.transform.position.y, CardboardNumberText.transform.position.z);
			int random3 = random2;
			while (random == random3 || random2 == random3) {
				random3 = UnityEngine.Random.Range (0, 4);
			}
			GlassNumberText.transform.position = new Vector3 (numberTextX [random3],
				GlassNumberText.transform.position.y, GlassNumberText.transform.position.z);
			int random4 = random3;
			while (random == random4 || random2 == random4 || random3 == random4) {
				random4 = UnityEngine.Random.Range (0, 4);
			}
			MetalNumberText.transform.position = new Vector3 (numberTextX [random4],
				MetalNumberText.transform.position.y, MetalNumberText.transform.position.z);
		} else if (level<levelMetalStart){									//Only 3, no randomization
			PlasticNumberText.transform.position = new Vector3 (numberTextX [4],
				PlasticNumberText.transform.position.y, PlasticNumberText.transform.position.z);
			CardboardNumberText.transform.position = new Vector3 (numberTextX [5],
				CardboardNumberText.transform.position.y, CardboardNumberText.transform.position.z);
			GlassNumberText.transform.position = new Vector3 (numberTextX [6],
				GlassNumberText.transform.position.y, GlassNumberText.transform.position.z);
		}else {																//All 4, no randomization
			PlasticNumberText.transform.position = new Vector3 (numberTextX [0],
				PlasticNumberText.transform.position.y, PlasticNumberText.transform.position.z);
			CardboardNumberText.transform.position = new Vector3 (numberTextX [1],
				CardboardNumberText.transform.position.y, CardboardNumberText.transform.position.z);
			GlassNumberText.transform.position = new Vector3 (numberTextX [2],
				GlassNumberText.transform.position.y, GlassNumberText.transform.position.z);
			MetalNumberText.transform.position = new Vector3 (numberTextX [3],
				MetalNumberText.transform.position.y, MetalNumberText.transform.position.z);
		}

		if (level <= levelMetalStart) {
			PlasticNumberBox.transform.position = new Vector3 (numberTextX [4]+numberBoxAddendum,
				PlasticNumberBox.transform.position.y, PlasticNumberBox.transform.position.z);
			CardboardNumberBox.transform.position = new Vector3 (numberTextX [5]+numberBoxAddendum,
				CardboardNumberBox.transform.position.y, CardboardNumberBox.transform.position.z);
			GlassNumberBox.transform.position = new Vector3 (numberTextX [6]+numberBoxAddendum,
				GlassNumberBox.transform.position.y, GlassNumberBox.transform.position.z);
		} else {
			PlasticNumberBox.transform.position = new Vector3 (numberTextX [0]+numberBoxAddendum,
				PlasticNumberBox.transform.position.y, PlasticNumberBox.transform.position.z);
			CardboardNumberBox.transform.position = new Vector3 (numberTextX [1]+numberBoxAddendum,
				CardboardNumberBox.transform.position.y, CardboardNumberBox.transform.position.z);
			GlassNumberBox.transform.position = new Vector3 (numberTextX [2]+numberBoxAddendum,
				GlassNumberBox.transform.position.y, GlassNumberBox.transform.position.z);
			MetalNumberBox.SetActive (true);
			MetalNumberBox.transform.position = new Vector3 (numberTextX [3]+numberBoxAddendum,
				MetalNumberBox.transform.position.y, MetalNumberBox.transform.position.z);
		}

		TextMesh text = PlasticNumberText.GetComponent<TextMesh>();	//Set Current Energy
		text.text = plasticNumber.ToString();
		TextMesh text2 = CardboardNumberText.GetComponent<TextMesh>();	//Set Q Ability Cost (Determined by current animal)
		text2.text = cardboardNumber.ToString();
		TextMesh text3 = GlassNumberText.GetComponent<TextMesh> ();	//Set W Ability Cost
		text3.text = glassNumber.ToString();
		TextMesh text4 = MetalNumberText.GetComponent<TextMesh> ();	//Set W Ability Cost

		if (level > levelMetalStart) {
			text4.text = metalNumber.ToString ();	//If 0, nothing is displayed
		} else {
			text4.text = " ";
		}
	}

	void Start()
	{
		LeverObject = GameObject.Find("Lever");
		leverScript = LeverObject.GetComponent<LeverController> ();
		LevelUp ();

		Checkmark.color = new Vector4 (1, 1, 1, 0);	//Set pop-up graphics to 0 alpha
		X.color = new Vector4 (1, 1, 1, 0);
		SpeedUp.color = new Vector4 (1, 1, 1, 0);
		Warning.color = new Vector4 (1, 1, 1, 0);
		Go.color = new Vector4 (1, 1, 1, 0);
		speedUpStartY = SpeedUp.transform.position.y;

		numberTextX = new float[7];				//Get default x positions of components
		numberTextX[0] = PlasticNumberText.transform.position.x;	//0-3 values for all 4 in play
		numberTextX[1] = CardboardNumberText.transform.position.x;
		numberTextX[2] = GlassNumberText.transform.position.x;
		numberTextX[3] = MetalNumberText.transform.position.x;
		float addendum = (CardboardNumberText.transform.position.x - numberTextX [0]) / 2;
		numberTextX[4] = numberTextX[0] - addendum;	//4-6 values for only 3 in play
		numberTextX[5] = numberTextX[1] - addendum;
		numberTextX[6] = numberTextX[2] - addendum;
		MetalNumberBox.SetActive (false);

		respawnRate = COUNTER_DEFAULT + 6;

		//level = levelMetalStart;	//FOR DEBUGGING PURPOSES TO JUMP TO SPECIFIC LEVEL! REMOVE OTHERWISE!!!

		newBlueprint ();
	}

    private void FixedUpdate()
    {
		if (backgroundConveyorCounter < 20.4f * Time.fixedTime)   //Create background conveyor objects
			backgroundConveyorCounter += Time.fixedTime;
		else
		{
			backgroundConveyorCounter = 0;
			Instantiate(BackgroundConveyor, new Vector3(BlueprintConsole.transform.position.x + .05f,
				BlueprintConsole.transform.position.y - .22f, 2.56f), Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () {

		if (!gameStart) {
			if (blueprintCounter > 1 * (Time.deltaTime * 60))
			{
				newBlueprint();
				blueprintCounter = 0;
			}
			else
				blueprintCounter += Time.deltaTime;

			if (GetReady.color.a > 0) {
				GetReady.color = new Vector4 (1, 1, 0, GetReady.color.a - (.01f * (Time.deltaTime * 60)));
				GetReady.transform.position = new Vector3 (GetReady.transform.position.x,
					GetReady.transform.position.y - (.01f * (Time.deltaTime * 60)), GetReady.transform.position.z);
			}

			timerScript = ScoreObject.GetComponent<timerLevelOne> ();
			gameStart = timerScript.gameStart;

			if (gameStart)
				Go.color = new Vector4 (0, 1, 0, 1);
		}

		//Debug.Log ("Plastic: " + plasticCount + ", Cardboard: " + cardboardCount + ", Glass: " + glassCount + ", Metal: " + metalCount);
		//DEBUGGING - to show 
		if (speedUpWait) {
			if (!leverScript.getCrushing ()) {
				speedUpCounter = 10;
				if (level == (levelMetalStart + 1)) {
					Warning.color = new Vector4 (1, 0, 0, 1);
					Warning.transform.position = new Vector3 (Warning.transform.position.x, speedUpStartY, Warning.transform.position.z);
				} else {
					SpeedUp.color = new Vector4 (0, 1, 1, 1);
					SpeedUp.transform.position = new Vector3 (SpeedUp.transform.position.x, speedUpStartY, SpeedUp.transform.position.z);
				}
				speedUpWait = false;
			}
		}

		if (matchCounter == level && level < levelMetalStart) {
			LevelUp ();
			matchCounter = 0;
		} else if (matchCounter == 4 && level >= levelMetalStart) {
			LevelUp ();
			matchCounter = 0;
		}
			
		if (shrinkAndDestroy) {
			if (shrinkObject != null) {
				if (shrinkObject.transform.localScale.x > .2f)
					shrinkObject.transform.localScale = new Vector3 (shrinkObject.transform.localScale.x - .15f * (Time.deltaTime * 60), 
						shrinkObject.transform.localScale.y - .15f, 1);
				else
					Destroy (shrinkObject.gameObject);
			}
		}

		MoveConveyor ();	//Updates conveyor belt movement

		if (Checkmark.color.a > 0) {	//Fade X / Checkmark / SpeedUp / Warning
			if (checkmarkCounter > 0) {
				checkmarkCounter--;
			} else {
				Checkmark.color = new Vector4 (1,1,1,Checkmark.color.a-.05f * (Time.deltaTime * 60));
			}
			X.color = new Vector4 (1, 1, 1, 0);
		}
		if (X.color.a > 0) {
			if (checkmarkCounter > 0) {
				checkmarkCounter--;
			} else {
				X.color = new Vector4 (1, 1, 1, X.color.a - .05f * (Time.deltaTime * 60));
			}
			Checkmark.color = new Vector4 (1, 1, 1, 0);
		}
		if (Go.color.a > 0) {
			Go.color = new Vector4 (0, 1, 0, Go.color.a - .025f * (Time.deltaTime * 60));
		}

		if (SpeedUp.color.a > 0) {
			SpeedUp.transform.position = new Vector3 (SpeedUp.transform.position.x, SpeedUp.transform.position.y - .01f * (Time.deltaTime * 60), 
				SpeedUp.transform.position.z);
			if (speedUpCounter > 0) {
				speedUpCounter--;
				SpeedUp.color = new Vector4 (SpeedUp.color.r, SpeedUp.color.b, 1, SpeedUp.color.a);
			} else {
				SpeedUp.color = new Vector4 (SpeedUp.color.r, SpeedUp.color.b, 1, SpeedUp.color.a - .02f * (Time.deltaTime * 60));
			}
		} else if (Warning.color.a > 0) {
			Warning.transform.position = new Vector3 (Warning.transform.position.x, Warning.transform.position.y - .01f * (Time.deltaTime * 60), 
				Warning.transform.position.z);
			if (speedUpCounter > 0) {
				speedUpCounter--;
				Warning.color = new Vector4 (Warning.color.r, Warning.color.b, 0, Warning.color.a);
			} else {
				Warning.color = new Vector4 (Warning.color.r, Warning.color.b, 0, Warning.color.a - .02f * (Time.deltaTime * 60));
				Warning.color = new Vector4 (Warning.color.r, Warning.color.b, 0, Warning.color.a - .02f * (Time.deltaTime * 60));
			}
		}

		if (!leverScript.getCrushing ())	//To avoid double checking crushing process
			crushCheck = false;

		if (leverScript.getCrushing () && crushCounter < 8*(Time.deltaTime*60) && !crushCheck)
			crushCounter+=Time.deltaTime*60;

		if (leverScript.getCrushing () && !crushCheck && crushCounter > 7 * (Time.deltaTime * 60) && gameStart) {	//
			crushCheck = true;
			for (int i=0; i<12; i++)
			{
				gridCubeScript = GridCubeArray[i].GetComponent<GridCube>();
				gridCubeScript.inUse = false;
			}

            /*Debug.Log("Glass: " + glassCount + ", Plastic: " + plasticCount + ", Cardboard: " + cardboardCount);
            Debug.Log("Actual Values, Glass: " + glassNumber + ", Plastic: " + plasticNumber + ", Cardboard: " + cardboardCount);*/

			if ((glassCount == glassNumber) && (plasticCount == plasticNumber) && (cardboardCount == cardboardNumber)
				&& (metalCount == metalNumber)) {
				/*int result1 = Mathf.CeilToInt (level / 4) * BASE;
				int result2 = metalNumber + glassNumber + plasticNumber + cardboardNumber;
				int result3 = 1+(multiplier/4);
					//Debug.Log("Result1: " + result1 + ", Result2: " + result2 + ", Result3: " + result3);*/
				score += ((glassNumber + plasticNumber + cardboardNumber + metalNumber) * level) * multiplier;
				Checkmark.color = new Vector4 (1, 1, 1, 1);
				if (multiplier < 9)	//Multiplier can't be more than 9
					multiplier++;
				matchCounter++;

				int randomNumber = UnityEngine.Random.Range (0, 5);	//Instantiate crusher creation
				switch (randomNumber) {
				case 0:
					Instantiate (Bear, new Vector3 (BlueprintConsole.transform.position.x+.8f, 
						BlueprintConsole.transform.position.y+.24f, 4f), Quaternion.identity);
					break;
				case 1:
					Instantiate (License, new Vector3 (BlueprintConsole.transform.position.x+.8f, 
						BlueprintConsole.transform.position.y, 4f), Quaternion.identity);
					break;
				case 2:
					Instantiate (Wheel, new Vector3 (BlueprintConsole.transform.position.x+.8f, 
						BlueprintConsole.transform.position.y+.24f, 4f), Quaternion.identity);
					break;
				case 3:
					Instantiate (Newspaper, new Vector3 (BlueprintConsole.transform.position.x+.8f, 
						BlueprintConsole.transform.position.y+.15f, 4f), Quaternion.identity);
					break;
				case 4:
					Instantiate (Backpack, new Vector3 (BlueprintConsole.transform.position.x+.8f, 
						BlueprintConsole.transform.position.y+.24f, 4f), Quaternion.identity);
					break;
				}

			} else {
				X.color = new Vector4 (1, 1, 1, 1);
				multiplier = 1;
				if (glassCount+plasticCount+metalCount+cardboardCount > 0)	//Only create junk if stuff in compressor
					Instantiate (Junk, new Vector3 (BlueprintConsole.transform.position.x+.8f, BlueprintConsole.transform.position.y+.24f,
						4f), Quaternion.identity);
			}

			glassCount = 0;
			plasticCount = 0;
			cardboardCount = 0;
			metalCount = 0;
			checkmarkCounter = 10;

			newBlueprint ();

			Text text = ScoreObject.GetComponent<Text>();	//Set Current Score
			text.text = score.ToString();
			Text text2 = MultiplierObject.GetComponent<Text>();	//Set Current Multiplier
			text2.text = multiplier.ToString();

			crushCounter = 0;
			}

		//Spawn Objects
		if (counter > (respawnRate * (Time.deltaTime * 15)) - (level * 2f * (Time.deltaTime * 15)))
		{
			if (level > levelMetalStart)
			{
				int randomNumber = UnityEngine.Random.Range(0, 4);
				switch (randomNumber)
				{
					case 0:
						Instantiate(Plastic, new Vector3(-9.5f, Conveyor.transform.position.y + .6f, -1), Quaternion.identity);
						break;
					case 1:
						Instantiate(Cardboard, new Vector3(-9.5f, Conveyor.transform.position.y + .6f, -1), Quaternion.identity);
						break;
					case 2:
						Instantiate(Glass, new Vector3(-9.5f, Conveyor.transform.position.y + .6f, -1), Quaternion.identity);
						break;
					case 3:
						Instantiate(Metal, new Vector3(-9.5f, Conveyor.transform.position.y + .6f, -1), Quaternion.identity);
						break;
				}

				randomNumber = UnityEngine.Random.Range(0, 4);
				switch (randomNumber)
				{
					case 0:
						Instantiate(Plastic, new Vector3(-9.5f, Conveyor.transform.position.y - .6f, -1), Quaternion.identity);
						break;
					case 1:
						Instantiate(Cardboard, new Vector3(-9.5f, Conveyor.transform.position.y - .6f, -1), Quaternion.identity);
						break;
					case 2:
						Instantiate(Glass, new Vector3(-9.5f, Conveyor.transform.position.y - .6f, -1), Quaternion.identity);
						break;
					case 3:
						Instantiate(Metal, new Vector3(-9.5f, Conveyor.transform.position.y - .6f, -1), Quaternion.identity);
						break;
				}
			}
			else
			{
				int randomNumber = UnityEngine.Random.Range(0, 3);
				switch (randomNumber)
				{
					case 0:
						Instantiate(Plastic, new Vector3(-9.5f, Conveyor.transform.position.y, -1), Quaternion.identity);
						break;
					case 1:
						Instantiate(Cardboard, new Vector3(-9.5f, Conveyor.transform.position.y, -1), Quaternion.identity);
						break;
					case 2:
						Instantiate(Glass, new Vector3(-9.5f, Conveyor.transform.position.y, -1), Quaternion.identity);
						break;
				}
			}
			counter = 0;
		}
        else
        {
			counter += Time.deltaTime;
		}

		//Lerp management
		if (lerp1) {
			if (lerpingObject1 != null) {	//Make sure it's not lerping after deletion
				distCovered1 = (Time.time - startTime1) * lerpSpeed;
				fracJourney1 = distCovered1 / journeyLength1;
				if (sizeCounter1 > 1.05f) {
					sizeCounter1 -= .05f;
					lerpingObject1.transform.localScale = new Vector3 (sizeCounter1, sizeCounter1, 1);
				}
				lerpingObject1.transform.position = Vector3.Lerp (lerpingObject1.transform.position, 
					GridCubeArray [lerpArray1].transform.position, fracJourney1);
				if (lerpingObject1.transform.position == GridCubeArray [lerpArray1].transform.position) {
					lerp1 = false;
					lerpingObject1.transform.localScale = new Vector3 (1f, 1f, 1);
				}
			}
		}

		if (lerp2) {
			if (lerpingObject2 != null) {
				distCovered2 = (Time.time - startTime2) * lerpSpeed;
				fracJourney2 = distCovered1 / journeyLength2;
				lerpingObject2.transform.position = Vector3.Lerp (lerpingObject2.transform.position, 
					GridCubeArray [lerpArray2].transform.position, fracJourney2);
				if (sizeCounter2 > 1.05f) {
					sizeCounter2 -= .05f;
					lerpingObject2.transform.localScale = new Vector3 (sizeCounter2, sizeCounter2, 1);
				}
				if (lerpingObject2.transform.position == GridCubeArray [lerpArray2].transform.position) {
					lerpingObject2.transform.localScale = new Vector3 (1f, 1f, 1);	//Failsafe
					lerp2 = false;
				}
			}
		}	
	}

    public void EndGame()
    {
        gameEnd = true;
        ScoreObject.SetActive(false);
        MultiplierObject.SetActive(false);

        //Need to put in code to prevent player from playing after time end
    }

	void OnTriggerEnter2D(Collider2D other)	//To count what items are currently there and attach to grid
	{
		if (!leverScript.getCrushing ()) {	//Makes sure nothing can be added while crushing
			int result = -1;	//If still -1 at end, no free space
			for (int i = 0; i < 12; i++) {
				gridCubeScript = GridCubeArray [i].GetComponent<GridCube> ();	//Set script to correct cube
				if (!gridCubeScript.getInUse ()) {
					gridCubeScript.inUse = true;
					result = i;
					break;
				}
			}
			
			if (result >= 0 && result < 12) {	//If there's free space
				if (other.gameObject.CompareTag ("Glass"))
					glassCount++;
				else if (other.gameObject.CompareTag ("Plastic"))
					plasticCount++;
				else if (other.gameObject.CompareTag ("Cardboard"))
					cardboardCount++;
				else if (other.gameObject.CompareTag ("Metal"))
					metalCount++;

				if (!lerp1) {	//Helps manage more than one object lerping at once
					lerp1 = true;
					lerpingObject1 = other.gameObject;
					journeyLength1 = Vector3.Distance (other.gameObject.transform.position, GridCubeArray [result].transform.position);
					fracJourney1 = lerpSpeed / journeyLength1;
					startTime1 = Time.time;
					lerpArray1 = result;
					sizeCounter1 = 1.5f;
				} else {
					lerp2 = true;
					lerpingObject2 = other.gameObject;
					journeyLength2 = Vector3.Distance (other.gameObject.transform.position, GridCubeArray [result].transform.position);
					fracJourney2 = lerpSpeed / journeyLength2;
					startTime2 = Time.time;
					lerpArray2 = result;
					sizeCounter2 = 1.5f;
				}
			} else {
				shrinkAndDestroy = true;
				shrinkObject = other.gameObject;
			}
		}
	}

    /*
	void OnTriggerExit2D(Collider2D other)	//To subtract if items removed
	{
		if (other.gameObject != shrinkObject && !crushCheck) {
			if (other.gameObject.CompareTag ("Glass"))
				glassCount--;
			else if (other.gameObject.CompareTag ("Plastic"))
				plasticCount--;
			else if (other.gameObject.CompareTag ("Cardboard"))
				cardboardCount--;
			else if (other.gameObject.CompareTag ("Metal"))
				metalCount--;
		}
	}*/
}
