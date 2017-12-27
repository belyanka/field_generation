using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public int life;
	
	private Rigidbody rb;
	private GameController gameController;
	private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		
		GameObject gameControllerObj = GameObject.FindWithTag("GameController");
		if (gameControllerObj!=null)
		{
			gameController = gameControllerObj.GetComponent<GameController>();
		}
		rb = GetComponent<Rigidbody>();
		initialPosition = transform.position;
		gameController.UpdateLifeCount(life);
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
			if (life == 0)
			{
				gameController.GameOver();
			}
			else
			{
				life--;
				gameController.UpdateLifeCount(life);
			}
			transform.position = initialPosition;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Finish")) {
			//win
			if ((Mathf.Abs(other.gameObject.transform.position.x-transform.position.x) < 0.1f) && 
				(Mathf.Abs(other.gameObject.transform.position.z-transform.position.z) < 0.1f))
			{
				gameController.Win();
			}
		}
	}
	
}
