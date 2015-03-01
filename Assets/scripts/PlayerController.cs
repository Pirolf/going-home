using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float forwardSpeed = 1.5f;
	public float sideSpeed = 0.5f;
	public bool disableMove = false;
	public GameObject playerCam;
	void Awake(){
	}
	void Start () {
		EnableDisableComponents();
	}
	void EnableDisableComponents(){
		
		string currSceneName = Application.loadedLevelName;
		if(currSceneName.Equals("fastfoodShop")){
			//disable mouselook
			Debug.Log("disable mouse look at fastfoodShop");
			gameObject.GetComponent<MouseLook>().enabled = false;
			playerCam.GetComponent<MouseLook>().enabled = false;
		}else{

			gameObject.GetComponent<MouseLook>().enabled = true;
			playerCam.GetComponent<MouseLook>().enabled = true;
		}
	}
	public void EnableMouseLook(bool enable){
		gameObject.GetComponent<MouseLook>().enabled = enable;
		playerCam.GetComponent<MouseLook>().enabled = enable;
	}
	// Update is called once per frame
	void Update () {
		if(disableMove)return;
		if(GameControl.gameState == (int)GameControl.GameState.PlayerNavigating){
			float fb = Input.GetAxis("Vertical");
			float lr = Input.GetAxis("Horizontal");

			float deltaFB = forwardSpeed * fb * Time.deltaTime;
			float deltaLR = sideSpeed * lr * Time.deltaTime;
			transform.position += transform.forward * deltaFB;
			transform.position += transform.right * deltaLR;
		}else if(GameControl.gameState == (int)GameControl.GameState.HumanInteraction){
			//check dialogue state
		}
		
		
	}
}
