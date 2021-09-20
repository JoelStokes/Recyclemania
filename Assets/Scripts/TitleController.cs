using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Animator cameraAnim;
    public Animator buttonAnim;
    public Animator doorAnim;

    private bool ending = false;

    // Update is called once per frame
    void Update()
    {
        if (ending)
        {
            if (cameraAnim.GetCurrentAnimatorStateInfo(0).IsName("Done"))
            {
                SceneManager.LoadScene("Smelting16x9");
            }
        }
    }

    public void EndTitle()
    {
        buttonAnim.SetTrigger("Fade");
        doorAnim.SetTrigger("Rise");
        cameraAnim.SetTrigger("Zoom");

        ending = true;
    }
}
