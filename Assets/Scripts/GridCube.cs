using UnityEngine;
using System.Collections;

public class GridCube : MonoBehaviour {

	public int cubeNumber = 0;
	public bool inUse = false;

	public bool getInUse()
	{
		return inUse;
	}

	void OnMouseDown(){
		inUse = false;
	}
}
