using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public Animator blurAnim;
    public Animator clipboardAnim;
    public Animator letterAnim;
    public Animator pinAnim;

    public GameObject Accuracy;
    public GameObject CrushCount;
    public GameObject Score;
    public GameObject Seal;
    public GameObject Grade;
    public GameObject Handwritten;

    public Sprite badSeal;
    public Sprite okSeal;
    public Sprite goodSeal;
    public Sprite starSeal;

    private bool clipboardStarted = false;
    private float clipboardTimer = 0;
    private float clipboardLim = 1;
    private float letterLim = 2.5f;
    private bool letterStarted = false;

    private int finalScore = 0;
    private float finalAccuracy = 0;
    private int finalCrushes = 0;

    private bool gameEnd = false;

    private void Start()
    {
        Score.GetComponent<TextMesh>().text = "";
        Accuracy.GetComponent<TextMesh>().text = "";
        CrushCount.GetComponent<TextMesh>().text = "";
        Grade.GetComponent<TextMesh>().text = "";
        Handwritten.GetComponent<TextMesh>().text = "";

        Seal.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd)
        {
            clipboardTimer += Time.deltaTime;
            if (clipboardTimer > clipboardLim && !clipboardStarted)
            {
                clipboardAnim.SetTrigger("Rise");
                clipboardStarted = true;
            } else if (clipboardTimer > letterLim && !letterStarted)
            {
                SetGrade();
                letterStarted = true;
                letterAnim.SetTrigger("Spin");
                pinAnim.SetTrigger("Drop");
                Debug.Log("anim called");
            }
        }
    }

    public void StartResults(int score, int crushes, int correct)
    {
        blurAnim.SetTrigger("Blur");
        gameEnd = true;

        finalScore = score;
        finalCrushes = crushes;
        if (crushes != 0)
        {
            finalAccuracy = Mathf.Round((correct / crushes) * 100);
        } else
        {
            finalAccuracy = 0;
        }
    }

    private void RandomizeValues()
    {
        Score.GetComponent<TextMesh>().text = "";
        Accuracy.GetComponent<TextMesh>().text = "";
        CrushCount.GetComponent<TextMesh>().text = "";
    }

    private void SetGrade()
    {
        if (finalScore < 100)
        {
            Grade.GetComponent<TextMesh>().text = "F";
            Seal.GetComponent<SpriteRenderer>().sprite = badSeal;
            Handwritten.GetComponent<TextMesh>().text = "I don't think you \n understand the game...";
        } else if (finalScore < 500)
        {
            Grade.GetComponent<TextMesh>().text = "D";
            Seal.GetComponent<SpriteRenderer>().sprite = okSeal;
            Handwritten.GetComponent<TextMesh>().text = "Well... \n The job's done.";
        }
        else if (finalScore < 1000)
        {
            Grade.GetComponent<TextMesh>().text = "C";
            Seal.GetComponent<SpriteRenderer>().sprite = okSeal;
            Handwritten.GetComponent<TextMesh>().text = "Pretty good work.";
        }
        else if (finalScore < 2000)
        {
            Grade.GetComponent<TextMesh>().text = "B";
            Seal.GetComponent<SpriteRenderer>().sprite = goodSeal;
            Handwritten.GetComponent<TextMesh>().text = "Nice job!";
        }
        else if (finalScore < 3000)
        {
            Grade.GetComponent<TextMesh>().text = "A";
            Seal.GetComponent<SpriteRenderer>().sprite = goodSeal;
            Handwritten.GetComponent<TextMesh>().text = "Wow! \n Incredible!";
        }
        else
        {
            Grade.GetComponent<TextMesh>().text = "S";
            Seal.GetComponent<SpriteRenderer>().sprite = starSeal;
            Handwritten.GetComponent<TextMesh>().text = "You beat Joel's best! \n Spectacular!";
        }
    }
}
