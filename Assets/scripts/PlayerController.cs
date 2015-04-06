using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public static PlayerController self;
	public float forwardSpeed = 1.5f;
	public float sideSpeed = 0.5f;
	public bool disableMove = false;
	public GameObject playerCam;
	public GameObject playerBody;//the child with mesh renderer
	private Camera camera;
	private float maxNPCRaycastDistance = 2.5f;
	public GameObject playerRaycastHitObj = null;
	public GameObject talkOrNotCanvas;
	//read only
	/*
	public GameObject PlayerRaycastHitObj{
		get{return playerRaycastHitObj;}
	}
	*/
	public float MaxNPCRaycastDistance{
		get{return maxNPCRaycastDistance;}
	}
	void Awake(){
		camera = playerCam.GetComponent<Camera>();
		self = this;
	}
	void Start () {
		//EnableDisableComponents();
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
	//callback
	public void ProceedToTalk(bool talk){
		talkOrNotCanvas.SetActive(false);
		if(!talk){
			GameControl.gameState = (int)GameControl.GameState.PlayerNavigating;
			return;
		}
		if(playerRaycastHitObj == null){
			Debug.Log("npc null");
			return;
		}
		NPCScript npc = playerRaycastHitObj.GetComponent<NPCScript>();
		if(npc == null)return;

		GameControl.gameState = (int)GameControl.GameState.HumanInteraction;
		EnableMouseLook(false);

		DialogueSystem.dialogueSystem.StartInteraction(npc);



	}

	public void Navigate(){
		float fb = Input.GetAxis("Vertical");
		float lr = Input.GetAxis("Horizontal");

		float deltaFB = forwardSpeed * fb * Time.deltaTime;
		float deltaLR = sideSpeed * lr * Time.deltaTime;
		transform.position += transform.forward * deltaFB;
		transform.position += transform.right * deltaLR;
	}
	public void LookAtObject(){
		//raycast
		Vector3 facingDirection = playerCam.transform.forward;
		facingDirection.Normalize();

		Debug.DrawRay(transform.position, facingDirection * 2.5f, Color.red );

		RaycastHit[] hitInfo;
		hitInfo = Physics.RaycastAll(
			playerCam.transform.position,
			facingDirection,
			maxNPCRaycastDistance,
			LayerMask.NameToLayer("Player")
		);
		GameObject closestHit = null;
		//find the closest hit
		float minDist = maxNPCRaycastDistance;
		foreach(RaycastHit rh in hitInfo){
			GameObject hit = rh.collider.gameObject;
			float d = Vector3.Distance(hit.transform.position, transform.position);
			if(d < minDist){
				minDist = d;
				closestHit = hit; 
			}
		}
		playerRaycastHitObj = closestHit;
	}
	// Update is called once per frame
	void Update () {
		Debug.Log("state: " +GameControl.gameState);
		if(disableMove)return;
		if(GameControl.gameState == (int)GameControl.GameState.PlayerNavigating){
			Navigate();
			
			LookAtObject();
			
			if(playerRaycastHitObj != null){
				//Debug.Log("hit: " + playerRaycastHitObj.name);
				if(playerRaycastHitObj.tag == "NPC" && playerRaycastHitObj.name == "NPC_rigged"){
					//show a talk sign
					//npc turn around, look at player
					NPCScript npc = playerRaycastHitObj.GetComponent<NPCScript>();
					if(!npc.beggedToday){
						EnableMouseLook(false);
						GameControl.gameState = (int)GameControl.GameState.BrowsingMenu;
						
						//playerRaycastHitObj.transform.parent.gameObject.GetComponent<NavMeshAgent>().Stop();
						playerRaycastHitObj.transform.parent.LookAt(gameObject.transform);
						playerRaycastHitObj.GetComponent<Animation>().Stop();
						playerRaycastHitObj.transform.parent.gameObject.GetComponent<NavMeshAgent>().enabled = false;
						//Destroy(playerRaycastHitObj.GetComponent<Rigidbody>());
						talkOrNotCanvas.SetActive(true);
					}else{
						playerRaycastHitObj = null;
					}
					

				}
			}
			

		}else if(GameControl.gameState == (int)GameControl.GameState.HumanInteraction){
			//check dialogue state
			Debug.Log("having conversation");
		}else if(GameControl.gameState == (int)GameControl.GameState.BrowsingMenu){
			Debug.Log("BrowsingMenu");
		}
		
		
	}

}
