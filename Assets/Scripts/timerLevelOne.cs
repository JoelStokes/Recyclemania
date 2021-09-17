﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timerLevelOne : MonoBehaviour
{

    public float timeLeft;
    public Text timerText;
    public int scene;

	public SpriteRenderer Black;
	public bool gameStart = false;
	private int startCounter = 0;
	private int startLimit = 120;

	public bool gameEnd = false;
	public int endCounter = 0;	//How long to show "Time Up!" before transfering to next scene

	/*public GameObject[] SetToFalse;	//REMOVE THESE LATER! FOR INDIVIDUAL TESTING BUILDS!!!
	public GameObject ScoreDisplay;
	bool moveToFront = false;*/

    private int minutes;
    private int seconds;
    private int elapedSeconds;
    private float elapsedTime;
	private int colorCounter=0;

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
		} else if (gameStart) {
			timerText.text = "Times Up!";
			LoadScene (scene);
		} else {
			if (startCounter > startLimit)
				gameStart = true;
			else
				startCounter++;

			if (Black.color.a > 0 && startCounter > 1)
				Black.color = new Vector4 (1, 1, 1, Black.color.a - .015f);
				
		}

        elapsedTime += Time.deltaTime;

		if (timeLeft < 16) {	//Flash color from red to white when 15 seconds or less left
			colorCounter++;
			if (colorCounter <=30)
				timerText.color = Color.red;
			else if (colorCounter<=60)
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
        Application.LoadLevel(scene);

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