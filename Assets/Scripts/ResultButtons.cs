using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButtons : MonoBehaviour
{
    public Animator signAnim;
    public Animator creditsAnim;
    public Animator blurAnim;

    public TitleController titleController;

    void OnMouseDown()
    {
        if (this.gameObject.tag == "Play")
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
        } else if (this.gameObject.tag == "Help")
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurNothing")
            {
                signAnim.SetTrigger("Rise");
                GameObject.Find("BGBlur").GetComponent<BlurController>().StartBlur();
            }
        }
        else if (this.gameObject.tag == "Credits")
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurNothing")
            {
                creditsAnim.SetTrigger("Rise");
                GameObject.Find("BGBlur").GetComponent<BlurController>().StartBlur();
            }
        }
        else     //Back
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurHold")
            {
                if (creditsAnim)
                {
                    creditsAnim.SetTrigger("Fall");
                } else
                {
                    signAnim.SetTrigger("Fall");
                }
                GameObject.Find("BGBlur").GetComponent<BlurController>().EndBlur();
            }
        }
    }
}
