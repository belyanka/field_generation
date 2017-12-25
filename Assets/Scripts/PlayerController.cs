using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		initialPosition = transform.position;
	}
	
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.velocity = movement * speed;
	}

	private void OnCollisionEnter(Collision other) {
		//enemy collision
		if (other.gameObject.CompareTag("Enemy")) {
			//die
			transform.position = initialPosition;
		}
		else if(other.gameObject.CompareTag("Finish")) {
			//win
		}
	}
}
