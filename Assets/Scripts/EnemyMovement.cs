using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float speed;
	public float viewDistance;
	[Range(0,360)]
	public float viewAngle;

	public GameObject player;
	public LayerMask blocksMask;

	private bool chasing;
	private int moveForIterations;
	private Vector3 newDirection;

	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		chasing = false;
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindWithTag("Player");
		StartCoroutine("FindPlayer");
		StartCoroutine("Move");
	}

	IEnumerator Move() {
		while (true) {
			yield return new WaitForSeconds(.2f);
			if (chasing) {
				transform.LookAt(player.transform.position);
				rb.velocity = (player.transform.position - transform.position).normalized * speed;
			}
			else {
				if (moveForIterations==0) {
					newDirection = new Vector3(Random.Range(-1f,1f),.0f,Random.Range(-1f,1f));
					float angle = Quaternion.FromToRotation(Vector3.forward, newDirection).eulerAngles.y;
					transform.rotation = Quaternion.Euler(0,angle,0);
					moveForIterations = Random.Range(1, 8);
					
				}
				rb.velocity = newDirection*speed;
				moveForIterations--;
			}
		}		
	}

	IEnumerator FindPlayer() {
		while (true) {
			yield return new WaitForSeconds(.2f);
			CheckPlayerInView();
		}
	}

	void CheckPlayerInView() {
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance);
		bool playerInArea = false;
		foreach (var target in targetsInViewRadius) {
			if (target.gameObject.CompareTag("Player")) {
				playerInArea = true;
				Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

				if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2) {
					//float distance = Vector3.Distance(transform.position, target.transform.position);
					chasing = !Physics.Raycast(transform.position, target.transform.position, viewDistance, blocksMask);					
				}
				else {
					chasing = false;
				}
			}
		}
		if (!playerInArea) {
			chasing = false;
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees* Mathf.Deg2Rad));
	}
}
