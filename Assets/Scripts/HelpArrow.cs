using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpArrow : MonoBehaviour
{
    public bool right;

    private void OnMouseDown()
    {
        transform.parent.gameObject.GetComponent<HelpController>().ChangePage(right);
    }
}
