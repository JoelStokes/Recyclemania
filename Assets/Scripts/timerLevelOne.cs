using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timerLevelOne : MonoBehaviour
{

    public float timeLeft;
    public Text timerText;
    public int scene;

	public SpriteRenderer Black;
	public bool gameStart = false;
	private float startCounter = 0;
	private float startLimit = 30;

	public bool gameEnd = false;
	public int endCounter = 0;	//How long to show "Time Up!" before transfering to next scene

    public ResultsManager resultsManager;
    public CrusherController crusherController;

	/*public GameObject[] SetToFalse;	//REMOVE THESE LATER! FOR INDIVIDUAL TESTING BUILDS!!!
	public GameObject ScoreDisplay;
	bool moveToFront = false;*/

    private int minutes;
    private int seconds;
    private int elapedSeconds;
    private float elapsedTime;
	private float colorCounter=0;

	void Start()
	{
		CalculateMath ();
		if (seconds > 9) {
			timerText.text = minutes.ToString () + ":" + seconds.ToString ();
		} else {
			timerText.text = minutes.ToString () + ":0" + seconds.ToString ();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (timeLeft > 0 && gameStart) {
			CalculateMath ();
			if (seconds > 9) {
				timerText.text = minutes.ToString () + ":" + seconds.ToString ();
			} else {
				timerText.text = minutes.ToString () + ":0" + seconds.ToString ();
			}
			timeLeft -= Time.deltaTime;
		} else if (gameStart && !gameEnd) {
			timerText.text = "";
            resultsManager.StartResults();
            crusherController.EndGame();
            gameEnd = true;
		} else if (!gameEnd) {
			if (startCounter > startLimit * (Time.deltaTime * 60))
				gameStart = true;
			else
				startCounter += Time.deltaTime;

			if (Black.color.a > 0 && startCounter > 1 * (Time.deltaTime * 60))
				Black.color = new Vector4 (1, 1, 1, Black.color.a - .015f * (Time.deltaTime * 60));
				
		}

        elapsedTime += Time.deltaTime;

		if (timeLeft < 16) {    //Flash color from red to white when 15 seconds or less left
            colorCounter += Time.deltaTime;
            if (colorCounter < 30 * (Time.deltaTime * 60))
                timerText.color = new Vector4(1,.1f,.1f,1);
            else if (colorCounter < 60 * (Time.deltaTime * 60))
                timerText.color = Color.white;
            else
                colorCounter = 0;
		}
    }

    void CalculateMath()
    {
        minutes = Mathf.FloorToInt(timeLeft / 60);
        seconds = Mathf.FloorToInt(timeLeft % 60);

        if (seconds == 60)
        {
            minutes += 1;
        }
    }

    public void LoadScene(int scene)
    {
        //Application.LoadLevel(scene);

		/*for (int i = 0; i < 4; i++) {		//FOR INDIVIDUAL BUILD! REMOVE!!
			SetToFalse[i].SetActive (false);
		}
		if (!moveToFront) {
			moveToFront = true;
			SetToFalse [9].transform.position = new Vector3 (SetToFalse [9].transform.position.x, SetToFalse [9].transform.position.y, 
				SetToFalse [9].transform.position.z - 19);
			SetToFalse [28].transform.position = new Vector3 (SetToFalse [28].transform.position.x, SetToFalse [28].transform.position.y, 
				SetToFalse [28].transform.position.z - 19);
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 6);

		}*/
    }

    public int timeElapsed()
    {
        elapsedTime += Time.deltaTime;
        elapedSeconds = Mathf.CeilToInt(elapsedTime % 60);
        return elapedSeconds;
    }
}
