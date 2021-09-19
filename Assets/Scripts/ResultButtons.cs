using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButtons : MonoBehaviour
{
    public bool retry;

    void OnMouseDown()
    {
        if (retry)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        } else
        {
            SceneManager.LoadScene("Title");
        }
    }
}
