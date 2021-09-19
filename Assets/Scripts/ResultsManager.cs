using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public Animator blurAnim;
    public Animator clipboardAnim;
    public Animator letterAnim;
    public Animator pinAnim;
    public Animator textAnim;
    public Animator buttonAnim;

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
    private float textLim = 3.5f;
    private bool textStarted = false;
    private float buttonLim = 5.5f;
    private bool buttonStarted = false;

    private int finalScore = 0;
    private float finalAccuracy = 0;
    private int finalCrushes = 0;

    public GameObject blurBox;
    private float blur = 0;
    private float blurMax = 30;
    private float blurTimer = 0;
    private float blurLim = .15f;

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
            blurTimer += Time.deltaTime;
            if (blurTimer > blurLim && blur < blurMax)
            {
                if (!blurBox.activeSelf)
                {
                    blurBox.SetActive(true);
                    blurTimer = 0;
                }
                else
                {
                    blurBox.GetComponent<Renderer>().sharedMaterial.SetFloat("Radius", blur);
                    blur++;
                    blurTimer = 0;
                }
            }
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
            } else if (clipboardTimer > textLim && !textStarted)
            {
                textAnim.SetTrigger("Show");
                textStarted = true;
            } else if (clipboardTimer > buttonLim && !buttonStarted)
            {
                buttonAnim.SetTrigger("Rise");
                buttonStarted = true;
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

        Score.GetComponent<TextMesh>().text = finalScore.ToString();
        Accuracy.GetComponent<TextMesh>().text = (Mathf.Ceil(finalAccuracy)) + "%";
        CrushCount.GetComponent<TextMesh>().text = finalCrushes.ToString();
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
