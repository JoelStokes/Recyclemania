using UnityEngine;
using System.Collections;

public class CreationMovement : MonoBehaviour {

	// All it does is move it, then destroys when off-screen
	void Update () {
		if (transform.position.x < 9.5f)
			transform.position = new Vector3 (transform.position.x + .02f * (Time.deltaTime * 60), transform.position.y, transform.position.z);
		else
			Destroy (gameObject);
	}
}
