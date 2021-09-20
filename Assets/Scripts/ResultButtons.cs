using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButtons : MonoBehaviour
{
    public Animator signAnim;
    public Animator blurAnim;

    void OnMouseDown()
    {
        if (this.gameObject.tag == "Play")
        {
            SceneManager.LoadScene("Smelting16x9");
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

        } else     //Back
        {
            AnimatorClipInfo[] clipInfo = blurAnim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name == "BlurHold")
            {
                signAnim.SetTrigger("Fall");
                GameObject.Find("BGBlur").GetComponent<BlurController>().EndBlur();
            }
        }
    }
}
