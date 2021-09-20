using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButtons : MonoBehaviour
{
    public Animator signAnim;
    public Animator blurAnim;

    public TitleController titleController;

    private bool inMenu = false;    //Helps prevent accidental click on Help page

    void OnMouseDown()
    {
        if (this.gameObject.tag == "Play" && !inMenu)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Title")
            {
                titleController.EndTitle();
            } else
            {
                SceneManager.LoadScene(scene.name);
            }
        } else if (this.gameObject.tag == "Title")
        {
            SceneManager.LoadScene("Title");
        } else if (this.gameObject.tag == "Help" && !inMenu)
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurNothing")
            {
                signAnim.SetTrigger("Rise");
                GameObject.Find("BGBlur").GetComponent<BlurController>().StartBlur();
            }

            inMenu = true;
        }
        else if (this.gameObject.tag == "Credits" && !inMenu)
        {

        } else     //Back
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurHold")
            {
                signAnim.SetTrigger("Fall");
                GameObject.Find("BGBlur").GetComponent<BlurController>().EndBlur();
            }

            inMenu = false;
        }
    }
}
