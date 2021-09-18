using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    public Animator blurAnim;

    private bool gameEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (gameEnd)
        {

        }
    }

    public void StartResults()
    {
        blurAnim.SetTrigger("Blur");
        gameEnd = true;
    }
}
