using UnityEngine;
using System.Collections;

public class SnapToGrid : MonoBehaviour {
	
	public Vector3[,] m_Array;
	public bool[,] slotFree;

	private int glassCount = 0;
	private int plasticCount = 0;
	private int cardboardCount = 0;

	// Use this for initialization
	void Start () {
		m_Array = new Vector3[4,3];

		for (int i = 0; i < 4; i++) {	//Set vector to proper x and y positions for array
			for (int j = 0; j < 3; j++) {
				m_Array [i,j] = new Vector3 (i + (-3), j + (-1.5f), 0);
			}
		}

		slotFree = new bool[4, 3];

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 3; j++)
				slotFree [i, j] = false;
			}
		}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)	//Set proper scores
	{
		if (other.gameObject.CompareTag ("Glass"))
			glassCount++;
		else if (other.gameObject.CompareTag ("Plastic"))
			plasticCount--;
		else if (other.gameObject.CompareTag ("Cardboard"))
			cardboardCount--;

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 3; j++){
				if (slotFree [i, j] == false) {
					slotFree [i, j] = true;
					other.transform.localScale = new Vector3 (1, 1, 1);
					other.transform.position = m_Array [i, j];
					break;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Glass"))
			glassCount--;
		else if (other.gameObject.CompareTag ("Plastic"))
			plasticCount--;
		else if (other.gameObject.CompareTag ("Cardboard"))
			cardboardCount--;
	}
}
