using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	private bool clicked = false;
	private Vector3 mousePosition;
	private float moveSpeed = 1f;
	private bool onConveyor = true;
	private Rigidbody2D rigidBody;
	private bool onCrusher = false;

	private int level;

	void Start(){
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (!clicked && onConveyor) {
			transform.position = new Vector3 (8, transform.position.y - .05f, transform.position.z);

			if (transform.position.y < -5.6f)
				Destroy (gameObject);
		} else if (clicked) {
			if (Input.GetButton ("Fire1")) {	//Follow mouse position
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
				transform.position = Vector2.Lerp (transform.position, mousePosition, moveSpeed);
				Debug.Log (transform.position);
			} else {
				clicked = false;
				transform.localScale = new Vector3 (1f, 1f, 1);

				/*GameObject grid = GameObject.Find ("Crusher");
				SnapToGrid snapToGrid = grid.GetComponent<SnapToGrid> ();
				if (onCrusher) {
					int yCoordinate = snapToGrid.gridCounter;
					int xCoordinate = 0;
					while (snapToGrid.gridCounter > 3) {
						yCoordinate -= 3;
						xCoordinate++;
					}
					transform.position = snapToGrid.m_Array [xCoordinate, yCoordinate];
					snapToGrid.gridCounter++;
				}
				else
					Destroy (gameObject);*/
			}
		} else if (!onCrusher){
			Destroy (gameObject);
		}
	}

	void OnMouseDown(){
		clicked = true;
		gameObject.AddComponent<Rigidbody2D> ();
		transform.localScale = new Vector3 (1.5f,1.5f,1);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Crusher")) {
			onConveyor = false;
			onCrusher = true;
			clicked = false;
			Destroy (rigidBody);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Crusher")) {
			onCrusher = false;
		}
	}
}
