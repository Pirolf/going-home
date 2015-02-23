using UnityEngine;
using System.Collections;

//forward: +x
//left: +z
public class PlayerController : MonoBehaviour {
	public float forwardSpeed = 0.5f;
	public float sideSpeed = 0.5f;
	public float rotYSpeed = 100f;
	private Vector3 deltaTranslate;
	//look around
	// Use this for initialization
	void Awake(){
		deltaTranslate = Vector3.zero;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float fb = Input.GetAxis("Vertical");
		float lr = Input.GetAxis("Horizontal");
		float deltaFB = forwardSpeed * fb * Time.deltaTime;
		float deltaLR = sideSpeed * lr * Time.deltaTime;
		transform.position += transform.forward * deltaFB;
		transform.position += transform.right * deltaLR;
		//transform.Translate(deltaFB, 0, deltaLR);

		
	}
}
