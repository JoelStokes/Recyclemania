using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    public Animator blurAnim;
    public GameObject BlurObj;

    private bool blurring = false;
    private float blurTimer = 0;
    private float blurValue = 0;
    private float blurMax = 1;
    private float blurLim = .25f;

    // Start is called before the first frame update
    void Start()
    {
        BlurObj.GetComponent<Renderer>().sharedMaterial.SetFloat("Radius", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (blurring && blurValue < blurMax)   //Add Blur
        {
            blurTimer += Time.deltaTime;
            if (blurTimer > blurLim)
            {
                if (!BlurObj.activeSelf)
                {
                    BlurObj.GetComponent<Renderer>().sharedMaterial.SetFloat("Radius", 0);
                    BlurObj.SetActive(true);
                }
                else
                {
                    blurValue++;
                    BlurObj.GetComponent<Renderer>().sharedMaterial.SetFloat("Radius", blurValue);
                }

                blurTimer = 0;
            }
        } else if (!blurring && BlurObj.activeSelf)  //Remove Blur
        {
            blurTimer += Time.deltaTime;
            if (blurTimer > blurLim)
            {
                if (blurValue > 0)
                {
                    blurValue--;
                    BlurObj.GetComponent<Renderer>().sharedMaterial.SetFloat("Radius", blurValue);
                }
                else
                {
                    BlurObj.SetActive(false);
                }

                blurTimer = 0;
            }
        }
    }

    public void StartBlur()
    {
        blurring = true;
        blurAnim.SetTrigger("Blur");
    }

    public void EndBlur()
    {
        blurring = false;
        blurAnim.SetTrigger("Clear");
    }
}
