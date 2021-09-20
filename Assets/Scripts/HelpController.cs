using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    public Animator RightAnim;
    public Animator LeftAnim;
    public GameObject HelpPages;

    private int helpPage = 0;
    private int helpPageTotal = 3;  //How many pages can be cycled through
    private float helpPageDistance = 15; //x interval between pages

    // Start is called before the first frame update
    void Start()
    {
        LeftAnim.SetTrigger("Hide");    //Pages start at 0, should start hidden
    }

    public void Update()
    {
        if (helpPage == helpPageTotal)
        {
            RightAnim.SetTrigger("Hide");
        } else
        {
            RightAnim.SetTrigger("Grow");
        }

        if (helpPage == 0)
        {
            LeftAnim.SetTrigger("Hide");
        } else
        {
            LeftAnim.SetTrigger("Grow");
        }
    }

        public void ChangePage(bool right)
    {
        if (!right && helpPage > 0)
        {
            helpPage--;

            HelpPages.transform.position = new Vector3(HelpPages.transform.position.x + helpPageDistance, HelpPages.transform.position.y, HelpPages.transform.position.z);
        } else if (right && helpPage < helpPageTotal)
        {
            helpPage++;

            HelpPages.transform.position = new Vector3(HelpPages.transform.position.x - helpPageDistance, HelpPages.transform.position.y, HelpPages.transform.position.z);
        }
    }
}
