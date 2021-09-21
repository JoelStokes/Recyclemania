using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGConveyorMove : MonoBehaviour
{
    private float endX = 3.42f;
    private float startX;
    private float speed = .02f;

    private void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < endX)
        {
            transform.position = new Vector3(transform.position.x + speed * (Time.deltaTime * 60), transform.position.y, transform.position.z);
        } else
        {
            transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        }
    }
}
