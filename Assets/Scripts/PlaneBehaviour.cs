using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour {

	public Camera cam;
	public Transform plane;
	public GameObject brokenPlane, canvas;
	public Transform leftPoint, rightPoint, forwardPoint;
	Rigidbody2D rb;
	public float speed = 5f, rotateSpeed = 50f;
	Vector3 mousePosition;
	bool isGameOver;

	// Use this for initialization
	void Start () {
		if(cam == null) cam = Camera.main;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!isGameOver){
			movePlane();
			rotatePlane(Input.GetAxis("Horizontal"));
		}
	}

	void movePlane(){
		rb.velocity = transform.up * speed;
	}

	void rotatePlane(float x){	
		float angle;
		Vector2 direction = new Vector2(0,0);

		if(x < 0) direction = (Vector2) leftPoint.position - rb.position;
		if(x > 0) direction = (Vector2) rightPoint.position - rb.position;
		
		direction.Normalize();
		angle = Vector3.Cross(direction, transform.up).z;
		if(x != 0) rb.angularVelocity = -rotateSpeed * angle;
		else rb . angularVelocity = 0;
		angle = Mathf.Atan2(forwardPoint.position.y - plane.transform.position.y, forwardPoint.position.x - plane.transform.position.x) * Mathf.Rad2Deg;
		plane.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
	}

	public void gameOver(Transform missile){
		GetComponentInChildren<SpriteRenderer>().enabled = false;
		isGameOver = true;
		rb.velocity = new Vector2(0,0);
		cam.gameObject.GetComponent<CameraBehaviour>().gameOver = true;
		GameObject planeTemp = Instantiate(brokenPlane, transform.position, transform.rotation);
		for(int i =0; i < planeTemp.transform.childCount; i++){
			Rigidbody2D rbTemp = planeTemp.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
			rbTemp.AddForce(((Vector2) missile.position - rbTemp.position) * -5f,ForceMode2D.Impulse);
		}
		StartCoroutine(canvasStuff());
	}

	IEnumerator canvasStuff(){
		yield return new WaitForSeconds(1f);
		canvas.SetActive(true);
		for(int i = 0; i <= 10; i++){
			float k = (float) i /10;
			canvas.transform.localScale = new Vector3(k,k,k);
			yield return new WaitForSeconds(.01f);
		}
		Destroy(gameObject);
	}
}
